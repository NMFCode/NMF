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
        /// Gets the location for the definition of a symbol
        /// </summary>
        /// <param name="position">The position of the symbol in the document</param>
        /// <returns>A <see cref="ParseRange"/> object, denoting a range between parse positions.</returns>
        public ParseRange GetDefinition(ParsePosition position)
        {
            var ruleApplications = _matcher.GetRuleApplicationsAt(position);

            var ruleApplication = ruleApplications.Aggregate(ruleApplications.First(), (largest, next) => {
                var largestDelta = ParsePositionDelta.Larger(largest.Length, next.Length);
                if (largest.Length == largestDelta) return largest;
                return next;
            });

            var definitionRuleApplication = _context.GetDefinition(ruleApplication.GetIdentifier(_context));

            return new ParseRange(definitionRuleApplication.CurrentPosition, definitionRuleApplication.CurrentPosition + definitionRuleApplication.Length);
        }
    }
}
