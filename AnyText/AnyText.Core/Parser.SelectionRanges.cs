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
        public SelectionRange[] GetSelectionRanges(IEnumerable<ParsePosition> positions)
        {
            return positions.Select(position => GetSelectionRange(position)).ToArray();
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

            var startLine = ruleApplication.CurrentPosition.Line;
            var startCol = ruleApplication.CurrentPosition.Col;

            var endLine = startLine + ruleApplication.Length.Line;
            var endCol = startCol + ruleApplication.Length.Col;

            var start = new ParsePosition(startLine, startCol);
            var end = new ParsePosition(endLine, endCol);

            var parseRange = new ParseRange(start, end);

            return new SelectionRange()
            {
                Range = parseRange,
                Parent = GetSelectionRange(ruleApplication.Parent)
            };
        }
    }
}
