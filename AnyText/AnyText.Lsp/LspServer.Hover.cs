using LspTypes;
using Newtonsoft.Json.Linq;
using NMF.AnyText.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <inheritdoc/>
        public Hover Hover(JToken arg)
        {
            var hoverParams = arg.ToObject<HoverParams>();
            string uri = hoverParams.TextDocument.Uri;

            if (!_documents.TryGetValue(uri, out var document))
            {
                return null;
            }

            var position = AsParsePosition(hoverParams.Position);
            var ruleApplication = document.Context.RootRuleApplication.GetLiteralAt(position)?.GetFirstReferenceOrDefinition();

            string hoverText = ruleApplication?.Rule?.GetHoverText();

            if (string.IsNullOrWhiteSpace(hoverText))
            {
                var documentSymbols = document.GetDocumentSymbolsFromRoot();
                if (documentSymbols == null || !documentSymbols.Any())
                {
                    return null;
                }

                var matchingSymbol = FindSymbolAtPosition(documentSymbols, position);
                if (matchingSymbol == null)
                {
                    return null;
                }

                string languageId = document.Context.Grammar.LanguageId;
                string symbolType = matchingSymbol.Kind.ToString();

                var sb = new StringBuilder();
                sb.AppendLine($"**{matchingSymbol.Name}** ({symbolType})");

                if (!string.IsNullOrWhiteSpace(matchingSymbol.Detail))
                {
                    sb.AppendLine($"\n```{languageId}\n{matchingSymbol.Detail}\n```");
                }

                if (matchingSymbol.Tags != null && matchingSymbol.Tags.Length > 0)
                {
                    sb.AppendLine("\n**Tags:**");
                    foreach (var tag in matchingSymbol.Tags)
                    {
                        sb.AppendLine($"- {tag}");
                    }
                }

                hoverText = sb.ToString();
            }

            if (string.IsNullOrWhiteSpace(hoverText))
            {
                return null;
            }

            var hoverContent = new MarkedString
            {
                Language = document.Context.Grammar.LanguageId,
                Value = hoverText
            };

            return new Hover
            {
                Contents = new SumType<string, MarkedString, MarkedString[], MarkupContent>(hoverContent),
            };
        }

        private DocumentSymbol FindSymbolAtPosition(IEnumerable<DocumentSymbol> symbols, ParsePosition position)
        {
            foreach (var symbol in symbols)
            {
                var childSymbol = symbol.Children != null ? FindSymbolAtPosition(symbol.Children, position) : null;
                if (childSymbol != null)
                {
                    return childSymbol;
                }

                if (IsExactPosition(position, symbol.Range))
                {
                    return symbol;
                }
            }
            return null;
        }

        private bool IsExactPosition(ParsePosition position, ParseRange range)
        {
            return position.Line == range.Start.Line && position.Col >= range.Start.Col;
        }
    }
}