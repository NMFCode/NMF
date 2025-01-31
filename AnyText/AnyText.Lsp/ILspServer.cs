using LspTypes;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Reflection.PortableExecutable;
using System.Threading;

namespace NMF.AnyText
{
    public partial interface ILspServer
    {
        [JsonRpcMethod(Methods.TextDocumentDidChangeName)]
        void DidChange(JToken arg);

        [JsonRpcMethod(Methods.TextDocumentDidCloseName)]
        void DidClose(JToken arg);

        [JsonRpcMethod(Methods.TextDocumentDidOpenName)]
        void DidOpen(JToken arg);

        [JsonRpcMethod(Methods.TextDocumentDidSaveName)]
        void DidSave(TextDocumentIdentifier textDocument, string text);

        [JsonRpcMethod(Methods.ExitName)]
        void Exit();

        [JsonRpcMethod(Methods.InitializeName)]
        public InitializeResult Initialize(
            int? processId
            , _InitializeParams_ClientInfo clientInfo
            , string locale
            , string rootPath
            , Uri rootUri
            , ClientCapabilities capabilities
            , TraceValue trace
            , WorkspaceFolder[] workspaceFolders
            , object InitializationOptions = null);

        [JsonRpcMethod(Methods.InitializedName)]
        void Initialized();
        
        [JsonRpcMethod(Methods.ShutdownName)]
        void Shutdown();

        /// <summary>
        ///     Handles the <c>*/setTrace</c> request from the client. This is used to set the trace setting of the server.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (SetTraceParams)</param>
        [JsonRpcMethod(MethodConstants.SetTrace)]
        public void SetTrace(JToken arg);

        [JsonRpcMethod(Methods.TextDocumentCompletionName)]
        public CompletionList HandleCompletion(JToken arg);

    }
}