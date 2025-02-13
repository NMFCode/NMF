using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Gets the locations for references of a symbol
        /// </summary>
        /// <param name="position">The position of the symbol in the document</param>
        /// <returns>An IEnumerable of <see cref="ParseRange"/> objects, each denoting a range between parse positions.</returns>
        public IEnumerable<ParseRange> GetReferences(ParsePosition position)
        {
            var ruleApplication = Context.RootRuleApplication.GetLiteralAt(position)?.GetFirstReferenceOrDefinition();

            if (ruleApplication?.SemanticElement != null && _context.TryGetReferences(ruleApplication.SemanticElement, out var references))
            {
                return references.Select(reference =>
                {
                    return new ParseRange(reference.CurrentPosition, reference.CurrentPosition + reference.Length);
                });
            }

            return null;
        }
    }
}
