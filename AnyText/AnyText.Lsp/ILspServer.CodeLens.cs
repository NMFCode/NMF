using LspTypes;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace NMF.AnyText
{
    public partial interface ILspServer
    {
        /// <summary>
        ///     Handles the <c>textDocument/codeLens</c> request from the client.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (CodeLensParams)</param>
        /// <returns>A Array of <see cref="CodeLens" /> objects containing the available CodeLenses of the document.</returns>
        [JsonRpcMethod(Methods.TextDocumentCodeLensName)]
        CodeLens[] CodeLens(JToken arg);
        /// <summary>
        ///     Handles the <c>codeLense/resolve</c> request from the client.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (CodeLens)</param>
        /// <returns>A <see cref="CodeLens" /> object containing the executed CodeLens</returns>
        [JsonRpcMethod(Methods.CodeLensResolveName)]
        CodeLens CodeLensResolve(JToken arg);
    }
}