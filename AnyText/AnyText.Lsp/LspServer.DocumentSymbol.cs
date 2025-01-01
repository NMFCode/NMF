using LspTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        DocumentSymbol[] ILspServer.QueryDocumentSymbols(JToken arg)
        {
            var documentSymbolParams = arg.ToObject<DocumentSymbolParams>();
            string uri = documentSymbolParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return Array.Empty<DocumentSymbol>();
            }
            
            var parsedDocumentSymbols = document.GetDocumentSymbolsFromRoot();

            return parsedDocumentSymbols.Select(parsedDocumentSymbol => MapToDocumentSymbol(parsedDocumentSymbol)).ToArray();
        }

        private DocumentSymbol MapToDocumentSymbol(Parser.ParsedDocumentSymbol parsedDocumentSymbol)
        {
            return new DocumentSymbol()
            {
                Name = parsedDocumentSymbol.Name,
                Detail = parsedDocumentSymbol.Detail,
                Kind = (SymbolKind)parsedDocumentSymbol.Kind,
                Tags = parsedDocumentSymbol.Tags.Select(tag => (SymbolTag)tag).ToArray(),
                Range = MapToRange(parsedDocumentSymbol.Range),
                SelectionRange = MapToRange(parsedDocumentSymbol.SelectionRange),
                Children = parsedDocumentSymbol.Children.Select(child => MapToDocumentSymbol(child)).ToArray()
            };
        }

        private LspTypes.Range MapToRange(Parser.DocumentSymbolRange range)
        {
            return new LspTypes.Range()
            {
                Start = new Position
                {
                    Line = range.Start.Line,
                    Character = range.Start.Character
                },
                End = new Position
                {
                    Line = range.End.Line,
                    Character = range.End.Character
                }
            };
        }
    }
}
