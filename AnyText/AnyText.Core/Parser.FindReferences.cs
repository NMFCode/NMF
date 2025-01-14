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
        public IEnumerable<ParseRange> GetReferences(ParsePosition position)
        {
            var ruleApplications = _matcher.GetRuleApplicationsAt(position)
                .Where(ruleApplication => ruleApplication.Rule.IsReference || ruleApplication.Rule.IsDefinition);

            var ruleApplication = ruleApplications.Aggregate(ruleApplications.First(), (smallest, next) => {
                var largestDelta = ParsePositionDelta.Larger(smallest.Length, next.Length);
                if (smallest.Length == largestDelta) return next;
                return smallest;
            });

            var referenceRuleApplications = _context.GetReferences(ruleApplication.GetValue(_context));

            return referenceRuleApplications.Select(reference =>
            {
                var startLine = reference.CurrentPosition.Line;
                var startCol = reference.CurrentPosition.Col;

                var endLine = startLine + reference.Length.Line;
                var endCol = startCol + reference.Length.Col;

                var start = new ParsePosition(startLine, startCol);
                var end = new ParsePosition(endLine, endCol);

                return new ParseRange(start, end);
            }); 
        }
    }
}
