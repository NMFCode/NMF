using NMF.Glsp.Contracts;
using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Notation;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Layout;
using NMF.Glsp.Protocol.ModelData;
using NMF.Glsp.Protocol.Notification;
using NMF.Glsp.Server.UndoRedo;
using NMF.Models;
using NMF.Models.Changes;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Models.Services;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace NMF.Glsp.Server
{
    internal class ClientSession : IGlspSession, IGlspClientSession
    {
        private readonly GlspUndoStack _undoStack = new GlspUndoStack();
        private IModelSession _modelSession;
        private IDiagram _diagram;
        private readonly IModelServer _modelServer;
        private readonly ModelChangeRecorder _layoutRecorder = new ModelChangeRecorder();

        private Action<ActionMessage> _sendToClient;
        private string _sessionId;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<ResponseAction>> _openRequests = new();
        private int _requestCounter;
        private GElement[] _selected;

        public ClientSession(GraphicalLanguage language, IModelServer modelServer)
        {
            _modelServer = modelServer;
            Language = language;

            language.Initialize();
        }

        public GGraph Root { get; private set; }

        public GElement[] SelectedElements
        {
            get => _selected;
            set
            {
                _selected = value;
                if (_modelSession != null)
                {
                    if (value != null && value.Length > 0)
                    {
                        _modelSession.SelectedElements = value.SelectMany(el => el.Skeleton.CalculateSelection(el));
                    }
                    else
                    {
                        _modelSession.SelectedElements = Root?.Skeleton.CalculateSelection(Root);
                    }
                }
            }
        }

        public bool IsDirty { get; set; }


        public bool CanUndo => _undoStack.CanUndo;

        public bool CanRedo => _undoStack.CanRedo;

        public ISkeletonTrace Trace { get; } = new SkeletonTrace();

        public GraphicalLanguage Language { get; }

        public async Task InitializeAsync(Uri uri)
        {
            if (Root != null) { return; }

            _modelSession = _modelServer?.GetOrCreateSession(uri) ?? new ModelSession(null, new Model() { ModelUri = uri });

            var sourceModel = _modelSession.Root;
            if (sourceModel == null)
            {
                sourceModel = Language.StartRule.GetRootSkeleton().CreateInstance(null, null) as IModelElement;
                _modelSession.Root = sourceModel;
            }
            _diagram = CreateDiagram(uri, out var needsLayout);

            _modelSession.PerformedOperation += ForwardOperation;
            _modelSession.IsDirtyChanged += ForwardDirtyFlag;
            _layoutRecorder.Attach(_diagram, false);
            Root = Language.Create(sourceModel, _diagram, Trace);
            var layoutRequest = new RequestBoundsAction
            {
                NewRoot = Root
            };
            var layoutResponse = await RequestAsync(layoutRequest);
            if (layoutResponse is ComputedBoundsAction computedBounds)
            {
                computedBounds.UpdateBounds(this);
            }

            if (needsLayout)
            {
                Language.DefaultLayoutEngine.CalculateLayout(Root);
            }
        }

        private void ForwardDirtyFlag(object sender, EventArgs e)
        {
            SendToClient(new SetDirtyAction
            {
                IsDirty = _modelSession.IsDirty,
                Reason = "operation"
            });
        }

        private void ForwardOperation(object sender, ModelChangeSet e)
        {
            _undoStack.NotifyModelOperation();
            SendUpdateToClient();
        }

        private IDiagram CreateDiagram(Uri sourceUri, out bool needsLayout)
        {
            var path = GetDiagramUri(sourceUri, _modelSession.LocalPath);
            if (path == null)
            {
                needsLayout = true;
                return null;
            }
            Uri diagramUri = new Uri(path, UriKind.RelativeOrAbsolute);
            var resolvedDiagram = FindDiagram(_modelServer.Repository.Resolve(diagramUri));
            if (resolvedDiagram != null)
            {
                needsLayout = false;
                return resolvedDiagram;
            }
            needsLayout = true;
            var result = new Diagram();
            var model = new Model
            {
                ModelUri = diagramUri,
                RootElements = { result }
            };
            if (!_modelServer.Repository.Models.ContainsKey(diagramUri))
            {
                _modelServer.Repository.Models.Add(diagramUri, model);
            }
            return result;
        }

        private IDiagram FindDiagram(IModelElement modelElement)
        {
            if (modelElement is IDiagram diagram)
            {
                return diagram; 
            }
            if (modelElement is Model model && model.RootElements.Count > 0 && model.RootElements[0] is IDiagram diagram1)
            {
                return diagram1;
            }
            return null;
        }

        private static string GetDiagramUri(Uri sourceUri, string hintPath)
        {
            var path = sourceUri.IsAbsoluteUri && sourceUri.IsFile ? sourceUri.AbsolutePath : hintPath;
            return path + ".layout.xmi";
        }

        public void Redo() => _undoStack.Redo(_modelSession);

        public void SendToClient(BaseAction action)
        {
            _sendToClient?.Invoke(new ActionMessage
            {
                Action = action,
                ClientId = _sessionId
            });
        }

        public void Undo() => _undoStack.Undo(_modelSession);

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
                }
                else if (message is ExecutableAction executable)
                {
                    await executable.ExecuteAsync(this);
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

        private void SendUpdateToClient()
        {
            SendToClient(new UpdateModelAction
            {
                Animate = true,
                NewRoot = Root
            });
        }

#pragma warning disable VSTHRD103 // Call async methods when in an async method
        private async Task PerformOperationAsync(Protocol.BaseProtocol.Operation op)
        {
            try
            {
                if (!_layoutRecorder.IsRecording)
                {
                    _layoutRecorder.Start();
                }
                var isModelTransaction = await _modelSession.PerformOperationAsync(() => op.ExecuteAsync(this));
                _layoutRecorder.Stop(detachAll: false);
                var layoutTransaction = _layoutRecorder.GetModelChanges();
                if (isModelTransaction)
                {
                    _undoStack.NotifyModelOperation();
                    SendUpdateToClient();
                }
                else if (layoutTransaction.Changes.Count > 0)
                {
                    _undoStack.Notify(layoutTransaction);
                }
                SendToClient(new SetDirtyAction { IsDirty = true, Reason = "operation" });
            }
            catch
            {
                if (_layoutRecorder.IsRecording)
                {
                    _layoutRecorder.Stop(detachAll: false);
                    var changes = _layoutRecorder.GetModelChanges();
                    changes.Invert();
                }
                throw;
            }
            finally
            {
                _layoutRecorder.Reset();
            }
        }
#pragma warning restore VSTHRD103 // Call async methods when in an async method

        public async Task<ResponseAction> RequestAsync(RequestAction request)
        {
            var nextRequest = Interlocked.Increment(ref _requestCounter);
            // for some reason, the request ID must always be empty
            request.RequestId = ""; // "_" + nextRequest.ToString();
            var completionSource = new TaskCompletionSource<ResponseAction>();
            if (!_openRequests.TryAdd(request.RequestId, completionSource))
            {
                await Task.Delay(50);
                return await RequestAsync(request);
            }
            SendToClient(request);
            return await completionSource.Task;
        }

        public void Save(Uri uri)
        {
            if (uri == null)
            {
                _modelSession.Save();
                SaveDiagram(_modelSession.Root?.Model?.ModelUri);
            }
            else
            {
                _modelSession.Save(uri);
                SaveDiagram(uri);
            }
            SendToClient(new SetDirtyAction { IsDirty = false, Reason = "save" });
        }

        private void SaveDiagram(Uri uri)
        {
            var path = GetDiagramUri(uri, _modelSession.LocalPath);
            if (path != null)
            {
                _modelServer.Repository.Save(_diagram, path, true);
            }
        }

        public void Synchronize()
        {
            SendToClient(new UpdateModelAction
            {
                Animate = true,
                NewRoot = Root
            });
        }
    }
}
