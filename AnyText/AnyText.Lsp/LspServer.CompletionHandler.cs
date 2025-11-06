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

                var completionItems = document.SuggestCompletions(position, out var fragment);
                var sortedSuggestions = SortSuggestions(completionItems, fragment);

                var items = sortedSuggestions.Select(suggestion => new CompletionItem
                {
                    Label = suggestion.Label,
                    Detail = suggestion.Detail,
                    Documentation = suggestion.Documentation,
                    Preselect = suggestion.Completion.StartsWith(fragment),
                    Kind = LspTypesMapper.SymbolKindMappings[suggestion.Kind],
                    TextEdit = GetTextEdit(suggestion, position, document)
                });
                items = PostProcessCompletions(document, items);
                
                return new CompletionList
                {
                    Items = items.ToArray(),
                };
            }
            catch (Exception ex)
            {
                _ = SendLogMessageAsync(MessageType.Error, ex.ToString());
                return new CompletionList
                {
                    Items = new[] { new CompletionItem { Label = "", Kind = CompletionItemKind.Text } }
                };
            }
        }

        /// <summary>
        /// Performs a post-processing on the given completion collection
        /// </summary>
        /// <param name="document">the document for which the completion list is requested</param>
        /// <param name="completions">the current list of completions</param>
        /// <returns>a potentially modified list of completions</returns>
        protected virtual IEnumerable<CompletionItem> PostProcessCompletions(Parser document, IEnumerable<CompletionItem> completions) => completions;

        private static IEnumerable<CompletionEntry> SortSuggestions(IEnumerable<CompletionEntry> completionItems, string fragment)
        {
            if (completionItems == null)
            {
                return Enumerable.Empty<CompletionEntry>();
            }

            var sortedSuggestions = completionItems
                    .Where(suggestion => suggestion.Completion != null)
                    .OrderByDescending(suggestion => CalculateScore(suggestion.Completion, fragment))
                    .ThenBy(suggestion => suggestion.SortText ?? suggestion.Label)
                    .ToList();

            return sortedSuggestions;


            static int CalculateScore(string completion, string lastToken)
            {
                if (completion.StartsWith(lastToken)) return 2;
                if (completion.Contains(lastToken)) return 1;
                return 0;
            }
        }

        private SumType<LspTypes.TextEdit, InsertReplaceEdit> GetTextEdit(CompletionEntry completionEntry, ParsePosition position, Parser document)
        {
            var startPosition = AsPosition(completionEntry.StartPosition);

            if (position.Line >= document.Context.Input.Length)
            {
                return new LspTypes.TextEdit
                {
                    Range = new LspTypes.Range
                    {
                        Start = startPosition,
                        End = startPosition
                    },
                    NewText = completionEntry.Completion
                };
            }

            return new InsertReplaceEdit
            {
                Replace = new LspTypes.Range
                {
                    Start = startPosition,
                    End = AsPosition(position)
                },
                Insert = new LspTypes.Range
                {
                    Start = startPosition,
                    End = AsPosition(completionEntry.StartPosition + completionEntry.Length)
                },
                NewText = completionEntry.Completion
            };
        }
    }
}