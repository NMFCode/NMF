using NMF.AnyText.Model;
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
        /// Gets the rule application for the definition of a symbol
        /// </summary>
        /// <param name="position">The position of the symbol in the document</param>
        /// <returns>The rule application of the symbol definition</returns>
        public RuleApplication GetDefinition(ParsePosition position)
        {
            var ruleApplication = Context.RootRuleApplication.GetLiteralAt(position).GetFirstReferenceOrDefinition();

            if (ruleApplication?.SemanticElement != null && _context.TryGetDefinition(ruleApplication.SemanticElement, out var definition))
            {
                return definition;
            }
            return null;
        }
    }
}
