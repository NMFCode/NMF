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
        /// Parses the selection range for given positions
        /// </summary>
        /// <param name="positions">The positions in the document</param>
        /// <returns>An IEnumerable of <see cref="SelectionRange"/> objects, each containing details on a selection range in the document.</returns>
        public IEnumerable<SelectionRange> GetSelectionRanges(IEnumerable<ParsePosition> positions)
        {
            return positions.Select(position => GetSelectionRange(position));
        }

        private SelectionRange GetSelectionRange(ParsePosition position)
        {
            var ruleApplications = _matcher.GetRuleApplicationsAt(position);
            var ruleApplication = ruleApplications.Aggregate(ruleApplications.First(), (smallest, next) => {
                var largestDelta = ParsePositionDelta.Larger(smallest.Length, next.Length);
                if (smallest.Length == largestDelta) return next;
                return smallest;
            });
            
            return GetSelectionRange(ruleApplication);
        }

        private SelectionRange GetSelectionRange(RuleApplication ruleApplication)
        {
            if (ruleApplication == null) return null;

            return new SelectionRange()
            {
                Range = new ParseRange(ruleApplication.CurrentPosition, ruleApplication.CurrentPosition + ruleApplication.Length),
                Parent = GetSelectionRange(ruleApplication.Parent)
            };
        }
    }
}
