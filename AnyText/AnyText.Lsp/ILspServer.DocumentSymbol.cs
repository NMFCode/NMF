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
        ///     Handles the <c>textDocument/documentSymbol/full</c> request from the client. This is used to retrieve all document symbols for a document, 
        ///     which can be used to show the outline of and jump to notable parts of the document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (DocumentSymbolParams)</param>
        /// <returns>An array of <see cref="DocumentSymbol" /> objects, each containing details on a document symbol with subsequent children symbols.</returns>
        [JsonRpcMethod(Methods.TextDocumentDocumentSymbolName)]
        DocumentSymbol[] QueryDocumentSymbols(JToken arg);
    }
}
