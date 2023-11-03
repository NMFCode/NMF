using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.ModelData;
using NMF.Glsp.Protocol.Notification;
using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.UndoRedo;
using NMF.Models.Changes;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NMF.Glsp.Server
{
    internal class ClientSession : IClientSession
    {
        private readonly TransactionUndoStack _undoStack = new TransactionUndoStack();
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder();
        private readonly ModelRepository _repository = new ModelRepository();

        public ClientSession(GraphicalLanguage language)
        {
            Language = language;
        }

        public GGraph Root { get; private set; }

        public string[] SelectedElements { get; set; }
        
        public bool IsDirty { get; set; }


        public bool CanUndo => _undoStack.CanUndo;

        public bool CanRedo => _undoStack.CanRedo;

        public ISkeletonTrace Trace { get; } = new SkeletonTrace();

        public GraphicalLanguage Language { get; }

        public void Initialize(Uri uri)
        {
            var sourceModel = _repository.Resolve(uri);
            if (sourceModel is IModel model)
            {
                sourceModel = model.RootElements.FirstOrDefault();
            }

            Root = Language.Create(sourceModel, null, Trace);
        }

        public void Redo() => _undoStack.Redo();

        public void SendToClient(BaseAction action)
        {
            throw new NotImplementedException();
        }

        public void Undo() => _undoStack.Undo();

        internal Task DisposeAsync()
        {
            throw new NotImplementedException();
        }

        internal Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        internal void Process(ActionMessage message)
        {
            try
            {
                if (message.Action is Operation op)
                {
                    _recorder.Start();
                    op.Execute(this);
                    _recorder.Stop(detachAll: false);
                    var transaction = _recorder.GetModelChanges();
                    if (transaction.Changes.Count > 0)
                    {
                        _undoStack.Notify(transaction);
                    }
                    SendToClient(new UpdateModelAction
                    {
                        Animate = true,
                        NewRoot = Root
                    });
                }
                else if (message.Action is ExecutableAction executable)
                {
                    executable.Execute(this);
                }
            }
            catch (Exception ex)
            {
                SendToClient(new MessageAction
                {
                    Details = ex.StackTrace,
                    Severity = SeverityLevels.Error,
                    Message = ex.Message
                });
            }
        }
    }
}
