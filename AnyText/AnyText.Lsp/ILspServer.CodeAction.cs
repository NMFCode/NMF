using LspTypes;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace NMF.AnyText
{
    public partial interface ILspServer
    {
        /// <summary>
        ///     Handles the <c>textDocument/codeAction</c> request from the client.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (CodeActionParams)</param>
        [JsonRpcMethod(Methods.TextDocumentCodeActionName)]
        CodeAction[] CodeAction(JToken arg);
    }
}