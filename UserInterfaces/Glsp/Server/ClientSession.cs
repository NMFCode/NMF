using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Notation;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.ModelData;
using NMF.Glsp.Protocol.Notification;
using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.UndoRedo;
using NMF.Models;
using NMF.Models.Changes;
using NMF.Models.Meta;
using NMF.Models.Repository;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Glsp.Server
{
    internal class ClientSession : IGlspSession, IGlspClientSession
    {
        private readonly TransactionUndoStack _undoStack = new TransactionUndoStack();
        private readonly ModelChangeRecorder _recorder = new ModelChangeRecorder();
        private readonly ModelChangeRecorder _layoutRecorder = new ModelChangeRecorder();
        private readonly ModelRepository _repository = new ModelRepository();

        private Action<ActionMessage> _sendToClient;
        private string _sessionId;
        private ConcurrentDictionary<string, TaskCompletionSource<ResponseAction>> _openRequests = new();
        private int _requestCounter;

        public ClientSession(GraphicalLanguage language)
        {
            Language = language;

            language.Initialize();
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
            if (Root != null) { return; }

            var sourceModel = _repository.Resolve(uri);
            if (sourceModel is IModel model)
            {
                sourceModel = model.RootElements.FirstOrDefault();
            }
            if (sourceModel == null)
            {
                sourceModel = Language.StartRule.GetRootSkeleton().CreateInstance(null, null) as IModelElement;
            }
            _recorder.Attach(sourceModel, false);
            var diagram = CreateDiagram(uri);
            _layoutRecorder.Attach(diagram, false);
            Root = Language.Create(sourceModel, diagram, Trace);
        }

        private IDiagram CreateDiagram(Uri sourceUri)
        {
            return new Diagram();
        }

        public void Redo() => _undoStack.Redo();

        public void SendToClient(BaseAction action)
        {
            _sendToClient?.Invoke(new ActionMessage
            {
                Action = action,
                ClientId = _sessionId
            });
        }

        public void Undo() => _undoStack.Undo();

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public Task InitializeAsync(Action<ActionMessage> messageHandler, string clientSessionId)
        {
            _sendToClient = messageHandler;
            _sessionId = clientSessionId;
            return Task.CompletedTask;
        }

        public async Task ProcessAsync(BaseAction message)
        {
            try
            {
                if (message is Protocol.BaseProtocol.Operation op)
                {
                    await PerformOperationAsync(op);
                    SendToClient(new UpdateModelAction
                    {
                        Animate = true,
                        NewRoot = Root
                    });
                }
                else if (message is ExecutableAction executable)
                {
                    await executable.Execute(this);
                }
                else if (message is ResponseAction response && _openRequests.TryRemove(response.ResponseId, out var requestTask))
                {
                    requestTask.SetResult(response);
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

#pragma warning disable VSTHRD103 // Call async methods when in an async method
        private async Task PerformOperationAsync(Protocol.BaseProtocol.Operation op)
        {
            try
            {
                _recorder.Start();
                _layoutRecorder.Start();
                await op.Execute(this);
                _recorder.Stop(detachAll: false);
                _layoutRecorder.Stop(detachAll: false);
                var transaction = _recorder.GetModelChanges();
                var layoutTransaction = _layoutRecorder.GetModelChanges();
                if (transaction.Changes.Count > 0 || layoutTransaction.Changes.Count > 0)
                {
                    _undoStack.Notify(transaction, layoutTransaction);
                }
            }
            catch
            {
                if (_recorder.IsRecording)
                {
                    _recorder.Stop(detachAll: false);
                    var changes = _recorder.GetModelChanges();
                    changes.Invert();
                }
                else if (_layoutRecorder.IsRecording)
                {
                    _layoutRecorder.Stop(detachAll: false);
                    var changes = _layoutRecorder.GetModelChanges();
                    changes.Invert();
                }
                throw;
            }
            finally
            {
                _recorder.Reset();
                _layoutRecorder.Reset();
            }
        }
#pragma warning restore VSTHRD103 // Call async methods when in an async method

        public Task<ResponseAction> Request(RequestAction request)
        {
            var nextRequest = Interlocked.Increment(ref _requestCounter);
            // for some reason, the request ID must always be empty
            request.RequestId = ""; // "_" + nextRequest.ToString();
            var completionSource = new TaskCompletionSource<ResponseAction>();
            if (!_openRequests.TryAdd(request.RequestId, completionSource))
            {
                return Request(request);
            }
            SendToClient(request);
            return completionSource.Task;
        }
    }
}
