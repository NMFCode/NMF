using NMF.AnyText.Workspace;
using NMF.Models;
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

        public TextSynchronization(IModelElement rootElement, Parser document, ILspServer server)
        {
            _rootElement = rootElement;
            _document = document;

            _rootElement.BubbledChange += BubbledChange;
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

        public void Dispose()
        {
            _rootElement.BubbledChange -= BubbledChange;
        }
    }
}
