using NMF.AnyText.Workspace;
using NMF.Models;
using NMF.Models.Changes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal class TextSynchronization : IDisposable
    {
        private readonly IModelElement _rootElement;
        private readonly ILspServer _server;
        private readonly Parser _document;
        private readonly ModelChangeRecorder _recorder;

        public TextSynchronization(IModelElement rootElement, Parser document, ILspServer server)
        {
            _rootElement = rootElement;
            _document = document;
            _recorder = new ModelChangeRecorder();
            _recorder.Attach(_rootElement, false);
            _server = server;
        }

        public List<IRunningSynchronization> RunningSynchronizations { get; } = new List<IRunningSynchronization>();

        private void BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (e.ChangeType != ChangeType.PropertyChanged && e.ChangeType != ChangeType.CollectionChanged)
            {
                return;
            }
            var edit = _document.Update(e.Element);
            if (edit != null)
            {
                _ = _server.ApplyWorkspaceEditAsync(_document.Context.TrackAndCreateWorkspaceEdit(edit.ToArray(), _document.Context.FileUri), "synchronization");
            }
        }

        public void SyncAll()
        {
            _rootElement.BubbledChange += BubbledChange;
        }

        public void StartListening()
        {
            _recorder.Start();
        }

        public void Complete(bool synchronizeText)
        {
            _recorder.Stop(false);
            var changes = _recorder.GetModelChanges();
            if (changes.Changes.Count > 0)
            {
                var changedElements = new Dictionary<IModelElement, (bool identifierChanged, List<string> features)>();
                foreach (var change in changes.Descendants().OfType<IElementaryChange>())
                {
                    if (!changedElements.TryGetValue(change.AffectedElement, out var features))
                    {
                        features = (false, new List<string>());
                        changedElements[change.AffectedElement] = features;
                    }
                    features.features.Add(change.Feature.Name);
                    if (change.Feature == change.AffectedElement.GetClass().Identifier)
                    {
                        changedElements[change.AffectedElement] = (true, features.features);
                    }
                }
                var edit = _document.Update(changedElements.Select(changed => new ModelUpdate(changed.Key, changed.Value.features, changed.Value.identifierChanged)));
                if (edit != null)
                {
                    _ = _server.ApplyWorkspaceEditAsync(_document.Context.TrackAndCreateWorkspaceEdit(edit.ToArray(), _document.Context.FileUri), "synchronization");
                }
            }
            _recorder.Reset();
        }

        public void Dispose()
        {
            _rootElement.BubbledChange -= BubbledChange;
        }
    }
}
