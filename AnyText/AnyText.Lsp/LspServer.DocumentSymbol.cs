using LspTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class LspServer
    {
       /// <inheritdoc cref="ILspServer"/>
       public LspTypes.DocumentSymbol[] QueryDocumentSymbols(JToken arg)
        {
            var documentSymbolParams = arg.ToObject<DocumentSymbolParams>();
            string uri = documentSymbolParams.TextDocument.Uri;
            
            if (!_documents.TryGetValue(uri, out var document))
            {
                return Array.Empty<LspTypes.DocumentSymbol>();
            }
            
            var documentSymbols = document.GetDocumentSymbolsFromRoot();

            return documentSymbols.Select(documentSymbol => MapToLspTypesDocumentSymbol(documentSymbol)).ToArray();
        }

        private LspTypes.DocumentSymbol MapToLspTypesDocumentSymbol(DocumentSymbol documentSymbol)
        {
            return new LspTypes.DocumentSymbol()
            {
                Name = documentSymbol.Name,
                Detail = documentSymbol.Detail,
                Kind = (LspTypes.SymbolKind)documentSymbol.Kind,
                Tags = documentSymbol.Tags.Select(tag => (LspTypes.SymbolTag)tag).ToArray(),
                Range = AsRange(documentSymbol.Range),
                SelectionRange = AsRange(documentSymbol.SelectionRange),
                Children = documentSymbol.Children.Select(child => MapToLspTypesDocumentSymbol(child)).ToArray()
            };
        }
    }
}
