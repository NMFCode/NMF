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
            var ruleApplications = _matcher.GetRuleApplicationsAt(position);
            var definition = _context.GetDefinition(ruleApplications.First(ruleApplication => ruleApplication.Rule.IsReference || ruleApplication.Rule.IsDefinition).GetValue(_context));

            var startLine = definition.CurrentPosition.Line;
            var startCol = definition.CurrentPosition.Col;

            var endLine = startLine + definition.Length.Line;
            var endCol = startCol + definition.Length.Col;

            var start = new ParsePosition(startLine, startCol);
            var end = new ParsePosition(endLine, endCol);

            return new ParseRange(start, end);
        }
    }
}
