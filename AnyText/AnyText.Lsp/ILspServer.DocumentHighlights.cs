using LspTypes;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial interface ILspServer
    {
        /// <summary>
        ///     Handles the <c>textDocument/documentHighlight</c> request from the client. This is used to retrieve the locations and kinds
        ///     of all highlights for a literal at a given position in the document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (DocumentHighlightParams)</param>
        /// <returns>An array of <see cref="LspTypes.DocumentHighlight" /> objects containing the range and kind of all matching highlights in the document.</returns>
        [JsonRpcMethod(Methods.TextDocumentDocumentHighlightName)]
        LspTypes.DocumentHighlight[] QueryDocumentHighlights(JToken arg);
    }
}
