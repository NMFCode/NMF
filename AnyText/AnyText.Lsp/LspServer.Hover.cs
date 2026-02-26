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
            string hoverText = GetHoverText(document, position);

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

        private string GetHoverText(Parser document, ParsePosition position)
        {
            _readWriteLock.EnterReadLock();
            try
            {
                var ruleApplication = document.Context.RootRuleApplication.GetLiteralAt(position, true);

                string hoverText = ruleApplication?.Rule?.GetHoverText(ruleApplication, document, position);
                return hoverText;
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }
    }
}