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
        /// <param name="fragment">the fragment of the token at the given position</param>
        public IEnumerable<CompletionEntry> SuggestCompletions(ParsePosition position, out string fragment)
        {
            fragment = string.Empty;
            var literalAtPosition = Context.RootRuleApplication.GetLiteralAt(position);
            if (literalAtPosition != null && literalAtPosition.CurrentPosition != position && position.Line == literalAtPosition.CurrentPosition.Line && literalAtPosition.CurrentPosition.Col < position.Col && position.Line < Context.Input.Length)
            {
                fragment = Context.Input[position.Line].Substring(literalAtPosition.CurrentPosition.Col, position.Col - literalAtPosition.CurrentPosition.Col);
            }

            var nextTokenPosition = _matcher.NextTokenPosition(position);
            return Context.RootRuleApplication.SuggestCompletions(position, fragment, _context, nextTokenPosition)?.DistinctBy(e => e.Completion);
        }
    }
}
