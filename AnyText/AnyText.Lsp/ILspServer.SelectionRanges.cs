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
        ///     Handles the <c>textDocument/selectionRange</c> request from the client. This is used to retrieve selection ranges
        ///     at an array of given positions in a document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (SelectionRangeParams)</param>
        /// <returns>An array of <see cref="SelectionRange" /> objects, each containing details on a selection range in the document
        /// corresponding to the received positions.</returns>
        [JsonRpcMethod("textDocument/selectionRange")]
        LspTypes.SelectionRange[] QuerySelectionRanges(JToken arg);
    }
}
