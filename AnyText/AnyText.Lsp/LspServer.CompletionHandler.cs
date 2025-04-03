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
        /// <inheritdoc />
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

                var completionItems = document.SuggestCompletions(position);

                var sortedSuggestions = LspServer.SortSuggestions(completionParams, document, position, completionItems);

                return new CompletionList
                {
                    Items = sortedSuggestions.Select(suggestion => new CompletionItem
                    {
                        Label = suggestion.Completion,
                        Kind = LspTypesMapper.KindMapper[suggestion.Kind],
                        TextEdit = GetTextEdit(suggestion, document)
                    }).ToArray()
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

        private static List<CompletionEntry> SortSuggestions(CompletionParams completionParams, Parser document, ParsePosition position, IEnumerable<CompletionEntry> completionItems)
        {
            var lineText = document.Context.Input[position.Line];

            var lastToken = lineText.Substring(0, (int)completionParams.Position.Character).Split(' ').LastOrDefault() ?? "";

            var sortedSuggestions = completionItems
                .Distinct()
                .OrderByDescending(suggestion => CalculateScore(suggestion.Completion, lastToken))
                .ToList();

            return sortedSuggestions;


            static int CalculateScore(string completion, string lastToken)
            {
                if (completion.StartsWith(lastToken)) return 2;
                if (completion.Contains(lastToken)) return 1;
                return 0;
            }
        }

        private LspTypes.TextEdit GetTextEdit(CompletionEntry completionEntry, Parser document)
        {
            var position = AsPosition(completionEntry.StartPosition);

            if (position.Line >= document.Context.Input.Length)
            {
                return new LspTypes.TextEdit
                {
                    Range = new LspTypes.Range
                    {
                        Start = position,
                        End = position
                    },
                    NewText = completionEntry.Completion
                };
            }

            var lineText = document.Context.Input[position.Line];

            var prefix = ExtractExistingPrefix(lineText, completionEntry);
            var newText = completionEntry.Completion.Substring(prefix.Length);

            return new LspTypes.TextEdit
            {
                Range = new LspTypes.Range
                {
                    Start = position,
                    End = new Position(position.Line, position.Character + (uint)newText.Length)
                },
                NewText = newText
            };
        }

        private string ExtractExistingPrefix(string lineText, CompletionEntry completionEntry)
        {
            var cursorPosition = completionEntry.StartPosition.Col;
            int maxPrefixLength = Math.Min(cursorPosition, completionEntry.Completion.Length);

            for (int i = maxPrefixLength; i > 0; i--)
            {
                if (completionEntry.Completion.StartsWith(lineText.Substring(cursorPosition - i, i)))
                {
                    return lineText.Substring(cursorPosition - i, i);
                }
            }

            return "";
        }

        private CompletionItemKind FindSymbolKind(IEnumerable<DocumentSymbol> symbols, string searchName)
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