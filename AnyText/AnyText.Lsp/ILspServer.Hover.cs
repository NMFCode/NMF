using LspTypes;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System.Threading;

namespace NMF.AnyText
{
    public partial interface ILspServer
    {
        /// <summary>
        ///     Handles the <c>textDocument/hover</c> request from the client.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (TextDocumentHover)</param>
        [JsonRpcMethod(Methods.TextDocumentHoverName)]
        Hover Hover(JToken arg);
    }
}