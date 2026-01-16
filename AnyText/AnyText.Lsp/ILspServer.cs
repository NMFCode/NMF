using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using LspTypes;
using System;
using System.Reflection.PortableExecutable;
using System.Threading;
using NMF.AnyText.InlayClasses;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes an interface for an LSP server
    /// </summary>
    public partial interface ILspServer
    {
        /// <summary>
        /// Sets the JSON-RPC instance used by the server for communication with the client.
        /// </summary>
        /// <param name="rpc">The <see cref="JsonRpc"/> instance to be associated with the server.</param>
        void SetRpc(JsonRpc rpc);
      
        /// <summary>
        /// Gets called when the client signals a change
        /// </summary>
        /// <param name="arg">the parameters of the request</param>
        [JsonRpcMethod(Methods.TextDocumentDidChangeName)]
        void DidChange(JToken arg);

        /// <summary>
        /// Gets called when the client closed a document
        /// </summary>
        /// <param name="arg">the parameters of the request</param>
        [JsonRpcMethod(Methods.TextDocumentDidCloseName)]
        void DidClose(JToken arg);

        /// <summary>
        /// Gets called when the client opened a document
        /// </summary>
        /// <param name="arg">the parameters of the request</param>
        [JsonRpcMethod(Methods.TextDocumentDidOpenName)]
        void DidOpen(JToken arg);

        /// <summary>
        /// Gets called when the client saved a document
        /// </summary>
        /// <param name="textDocument">the document to save</param>
        /// <param name="text">the actual text</param>
        [JsonRpcMethod(Methods.TextDocumentDidSaveName)]
        void DidSave(TextDocumentIdentifier textDocument, string text);

        /// <summary>
        /// Gets called when the client exits
        /// </summary>
        [JsonRpcMethod(Methods.ExitName)]
        void Exit();

        /// <summary>
        /// Initializes the server
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="clientInfo"></param>
        /// <param name="locale">the language</param>
        /// <param name="rootPath"></param>
        /// <param name="rootUri"></param>
        /// <param name="capabilities">capabilities of the client</param>
        /// <param name="trace"></param>
        /// <param name="workspaceFolders">workspace folders</param>
        /// <param name="InitializationOptions">options for the initialization</param>
        /// <returns></returns>
        [JsonRpcMethod(Methods.InitializeName)]
        public InitializeResult Initialize(int? processId,
                                           _InitializeParams_ClientInfo clientInfo,
                                           string locale,
                                           string rootPath,
                                           Uri rootUri,
                                           ClientCapabilities capabilities,
                                           TraceValue trace,
                                           WorkspaceFolder[] workspaceFolders,
                                           object InitializationOptions = null);

        /// <summary>
        /// Initializes the server
        /// </summary>
        [JsonRpcMethod(Methods.InitializedName)]
        void Initialized();
        
        /// <summary>
        /// Shuts down the server
        /// </summary>
        [JsonRpcMethod(Methods.ShutdownName)]
        void Shutdown();

        /// <summary>
        ///     Handles the <c>$/setTrace</c> request from the client. This is used to set the trace setting of the server.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (SetTraceParams)</param>
        [JsonRpcMethod(MethodConstants.SetTrace)]
        public void SetTrace(JToken arg);

        /// <summary>
        /// Suggests completions
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>A completion list</returns>
        [JsonRpcMethod(Methods.TextDocumentCompletionName)]
        public CompletionList HandleCompletion(JToken arg);
      
        /// <summary>
        ///     Handles the <c>workspace/ececuteCommand</c> request from the client. This is used to execute an action on the
        ///     Server.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (ExceuteCommandParams)</param>
        [JsonRpcMethod(Methods.WorkspaceExecuteCommandName)]
        public void ExecuteCommand(JToken arg);

        /// <summary>
        /// Provides Inlay hints for the document
        /// </summary>
        /// <param name="arg">The parameters of the request</param>
        /// <returns>A list of inlay hints</returns>
        [JsonRpcMethod("textDocument/inlayHint")]
        public InlayHint[] ProvideInlayHints(JToken arg);

    }
}