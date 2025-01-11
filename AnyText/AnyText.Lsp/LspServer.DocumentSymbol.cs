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
                Range = MapToLspTypesRange(documentSymbol.Range),
                SelectionRange = MapToLspTypesRange(documentSymbol.SelectionRange),
                Children = documentSymbol.Children.Select(child => MapToLspTypesDocumentSymbol(child)).ToArray()
            };
        }

        private LspTypes.Range MapToLspTypesRange(ParseRange range)
        {
            return new LspTypes.Range()
            {
                Start = new Position
                {
                    Line = (uint)range.Start.Line,
                    Character = (uint)range.Start.Col
                },
                End = new Position
                {
                    Line = (uint)range.End.Line,
                    Character = (uint)range.End.Col
                }
            };
        }
    }
}
