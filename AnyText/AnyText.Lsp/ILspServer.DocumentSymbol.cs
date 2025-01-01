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
        [JsonRpcMethod(Methods.TextDocumentDocumentSymbolName)]
        DocumentSymbol[] QueryDocumentSymbols(JToken arg);
    }
}
