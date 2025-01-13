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
        ///     Handles the <c>textDocument/references</c> request from the client. This is used to retrieve the locations
        ///     of all references to a symbol in a document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (ReferenceParams)</param>
        /// <returns>An array of <see cref="Location" /> objects containing the documents and locations of the references to a symbol.</returns>
        [JsonRpcMethod(Methods.TextDocumentReferencesName)]
        Location[] QueryFindReferences(JToken arg);
    }
}
