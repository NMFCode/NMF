using LspTypes;
using Newtonsoft.Json.Linq;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        public CompletionList HandleCompletion(JToken arg)
        {
            try
            {
                var completionParams = arg.ToObject<CompletionParams>();
                var uri = completionParams.TextDocument.Uri;

                if (!_documents.TryGetValue(uri, out var document))
                {
                    return new CompletionList { };
                }

                var position = new ParsePosition
                {
                    Col = Convert.ToInt32(completionParams.Position.Character),
                    Line = Convert.ToInt32(completionParams.Position.Line)
                };
                var completionItems = document.SuggestCompletions(position) ?? Enumerable.Repeat("", 1);
                var kindItems = FindSymbolKinds(completionItems, document) ?? Enumerable.Repeat(CompletionItemKind.Text, 1);

                var zipped = completionItems.Zip(kindItems, (completion, kind) => new { completion, kind });



                return new CompletionList
                {
                    Items = zipped.Select(suggestion => new CompletionItem { Label = suggestion.completion, Kind = suggestion.kind, }).ToArray()
                };
            }
            catch (Exception ex)
            {
                SendLogMessage(MessageType.Error, ex.ToString());
                return new CompletionList 
                {
                    Items = new[] { new CompletionItem { Label = "", Kind = CompletionItemKind.Text } }
                };
            }
        }

        public IEnumerable<CompletionItemKind> FindSymbolKinds(IEnumerable<string> completionItems, Parser document)
        {
            if (completionItems.IsNullOrEmpty()) return null;
            List<CompletionItemKind> completionItemKinds = new List<CompletionItemKind>();
            var documentSymbols = document.GetDocumentSymbolsForNonValidDocuments();
            foreach (var item in completionItems)
            {
                var kind = FindSymbolKind(documentSymbols, item);
                completionItemKinds.Add(kind);
            }
            return completionItemKinds;
        }

        public static CompletionItemKind FindSymbolKind(IEnumerable<DocumentSymbol> symbols, string searchName)
        {
            foreach (var symbol in symbols)
            {
                if (symbol.Name == searchName)
                    return LspTypesMapper.KindMapper[symbol.Kind];

                var kindInChildren = FindSymbolKind(symbol.Children, searchName);
                if (kindInChildren != null)
                    return kindInChildren;
            }
            return CompletionItemKind.Text;
        }
    }
}
