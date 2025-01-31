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
            var ruleApplications = _matcher.GetRuleApplicationsAt(position);
            var values = ruleApplications.Select(ruleApplication => ruleApplication.GetIdentifier(_context));

            var ruleApplication = ruleApplications.Aggregate(ruleApplications.First(), (largest, next) => {
                var largestDelta = ParsePositionDelta.Larger(largest.Length, next.Length);
                if (largest.Length == largestDelta) return largest;
                return next;
            });

            var referenceRuleApplications = _context.GetReferences(ruleApplication.GetIdentifier(_context));

            return referenceRuleApplications.Select(reference =>
                new ParseRange(reference.CurrentPosition, reference.CurrentPosition + reference.Length)); 
        }
    }
}
