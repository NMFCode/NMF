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

            string languageId = _languages
                .Where(kvp => uri.EndsWith(kvp.Key))
                .Select(kvp => kvp.Key)
                .FirstOrDefault() ?? "plaintext";

            string symbolType = matchingSymbol.Kind.ToString();

            var hoverText = new StringBuilder();
            hoverText.AppendLine($"**{matchingSymbol.Name}** ({symbolType})");

            if (!string.IsNullOrWhiteSpace(matchingSymbol.Detail))
            {
                hoverText.AppendLine($"\n```{languageId}\n{matchingSymbol.Detail}\n```");
            }

            if (matchingSymbol.Tags != null && matchingSymbol.Tags.Length > 0)
            {
                hoverText.AppendLine("\n**Tags:**");
                foreach (var tag in matchingSymbol.Tags)
                {
                    hoverText.AppendLine($"- {tag}");
                }
            }

            var hoverContent = new MarkedString
            {
                Language = languageId,
                Value = hoverText.ToString()
            };

            return new Hover
            {
                Contents = new SumType<string, MarkedString, MarkedString[], MarkupContent>(hoverContent),
                Range = AsRange(matchingSymbol.Range)
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