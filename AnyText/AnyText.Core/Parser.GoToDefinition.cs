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
        public ParseRange GetDefinition(ParsePosition position)
        {
            var ruleApplications = _matcher.GetRuleApplicationsAt(position)
                .Where(ruleApplication => ruleApplication.Rule.IsReference || ruleApplication.Rule.IsDefinition);

            var ruleApplication = ruleApplications.Aggregate(ruleApplications.First(), (smallest, next) => {
                var largestDelta = ParsePositionDelta.Larger(smallest.Length, next.Length);
                if (smallest.Length == largestDelta) return next;
                return smallest;
            });

            var definitionRuleApplication = _context.GetDefinition(ruleApplication.GetValue(_context));

            var startLine = definitionRuleApplication.CurrentPosition.Line;
            var startCol = definitionRuleApplication.CurrentPosition.Col;

            var endLine = startLine + definitionRuleApplication.Length.Line;
            var endCol = startCol + definitionRuleApplication.Length.Col;

            var start = new ParsePosition(startLine, startCol);
            var end = new ParsePosition(endLine, endCol);

            return new ParseRange(start, end);
        }
    }
}
