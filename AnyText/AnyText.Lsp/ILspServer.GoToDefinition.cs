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
        ///     Handles the <c>textDocument/definition</c> request from the client. This is used to retrieve the location
        ///     of the definition of a symbol in a document.
        /// </summary>
        /// <param name="arg">The JSON token containing the parameters of the request. (TextDocumentPositionParams)</param>
        /// <returns>A <see cref="LocationLink" /> object containing the document and position of the definition of a symbol.</returns>
        [JsonRpcMethod(Methods.TextDocumentDefinitionName)]
        LocationLink QueryDefinition(JToken arg);
    }
}
