using LspTypes;
using StreamJsonRpc;
using System.Threading;

namespace NMF.AnyText
{
    public interface ILspServer
    {
        [JsonRpcMethod(Methods.TextDocumentDidChangeName)]
        void DidChange(DidChangeTextDocumentParams changes);

        [JsonRpcMethod(Methods.TextDocumentDidCloseName)]
        void DidClose(DidCloseTextDocumentParams closeParams);

        [JsonRpcMethod(Methods.TextDocumentDidOpenName)]
        void DidOpen(DidOpenTextDocumentParams openParams);

        [JsonRpcMethod(Methods.TextDocumentDidSaveName)]
        void DidSave(DidSaveTextDocumentParams saveParams);

        [JsonRpcMethod(Methods.ExitName)]
        void Exit();

        [JsonRpcMethod(Methods.InitializeName)]
        InitializeResult Initialize(InitializeParams @params);

        [JsonRpcMethod(Methods.InitializedName)]
        void Initialized();

        [JsonRpcMethod(Methods.TextDocumentSemanticTokensFull)]
        SemanticTokens QuerySemanticTokens(SemanticTokensParams tokenParams);

        [JsonRpcMethod(Methods.ShutdownName)]
        void Shutdown();
    }
}