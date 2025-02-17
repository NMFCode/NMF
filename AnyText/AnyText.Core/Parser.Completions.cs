using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Calculates completion suggestions for the given position
        /// </summary>
        /// <param name="position">the position where completions should be calculated</param>
        /// <returns>A collection of code completions</returns>
        public IEnumerable<string> SuggestCompletions(ParsePosition position)
        {
            var nextTokenPosition = _matcher.NextTokenPosition(position);
            return Context.RootRuleApplication.SuggestCompletions(position, _context, nextTokenPosition).Distinct();
        }
    }
}
