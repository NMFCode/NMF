using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Parses the locations and kinds of document highlights for a literal at a given position
        /// </summary>
        /// <param name="position">The position of the literal in the document</param>
        /// <returns>An IEnumerable of <see cref="DocumentHighlight"/> objects, each containing a range between parse positions and the kind of the highlight.</returns>
        public IEnumerable<DocumentHighlight> GetDocumentHighlights(ParsePosition position)
        {
            var references = GetReferences(position);
            if (references != null)
            {
                return references.Select(reference => new DocumentHighlight()
                {
                    Range = reference,
                    Kind = DocumentHighlightKind.Read
                });
            }

            var literalRuleApplication = Context.RootRuleApplication.GetLiteralAt(position) as LiteralRuleApplication;
            if (literalRuleApplication == null)
            {
                return null;
            }

            var highlights = new List<DocumentHighlight>();
            Context.RootRuleApplication.IterateLiterals(ruleApplication =>
            {
                if (ruleApplication.Literal == literalRuleApplication.Literal && ruleApplication.Rule == literalRuleApplication.Rule)
                {
                    highlights.Add(new DocumentHighlight()
                    {
                        Range = new ParseRange(ruleApplication.CurrentPosition, ruleApplication.CurrentPosition + ruleApplication.Length),
                        Kind = DocumentHighlightKind.Text
                    });
                }
            });

            return highlights;
        }
    }
}
