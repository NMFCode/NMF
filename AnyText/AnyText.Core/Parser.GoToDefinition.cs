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
        /// Gets the location for the definition of a symbol
        /// </summary>
        /// <param name="position">The position of the symbol in the document</param>
        /// <returns>A <see cref="ParseRange"/> object, denoting a range between parse positions.</returns>
        public ParseRange? GetDefinition(ParsePosition position)
        {
            var ruleApplications = _matcher.GetRuleApplicationsAt(position);

            var ruleApplication = ruleApplications.Aggregate(ruleApplications.First(), (smallest, next) => {
                var smallestDelta = ParsePositionDelta.Smaller(smallest.Length, next.Length);
                if (smallest.Length == smallestDelta) return smallest;
                return next;
            }).GetFirstReferenceOrDefinition();

            if (ruleApplication?.SemanticElement != null && _context.GetDefinition(ruleApplication.SemanticElement, out var definition))
            {
                var identifier = definition.GetIdentifier();
                return new ParseRange(identifier.CurrentPosition, identifier.CurrentPosition + identifier.Length);
            }
            return null;
        }
    }
}
