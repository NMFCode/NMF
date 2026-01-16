using NMF.AnyText.Rules;
using System.Collections.Generic;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Gets the rule application for the definition of a symbol
        /// </summary>
        /// <param name="position">The position of the symbol in the document</param>
        /// <returns>The rule applications of the symbols definitions</returns>
        public IEnumerable<RuleApplication> GetDefinitions(ParsePosition position)
        {
            var ruleApplication = Context.RootRuleApplication.GetLiteralAt(position)?.GetFirstReferenceOrDefinition();

            if (ruleApplication?.SemanticElement != null && _context.TryGetDefinitions(ruleApplication.SemanticElement, out var definitions))
            {
                return definitions;
            }
            return null;
        }
    }
}
