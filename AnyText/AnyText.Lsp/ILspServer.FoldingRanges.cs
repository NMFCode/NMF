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
        ///     Handles the <c>textDocument/foldingRange/full</c> request from the client. This is used to retrieve all folding ranges for a document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (FoldingRangeParams)</param>
        /// <returns>An array of <see cref="FoldingRange" /> objects, each containing details on a folding range in the document.</returns>
        [JsonRpcMethod(Methods.TextDocumentFoldingRangeName)]
        FoldingRange[] QueryFoldingRanges(JToken arg);
    }
}
