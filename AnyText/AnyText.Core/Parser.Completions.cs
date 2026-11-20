using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText
{
    public partial class Parser
    {
        private static readonly char[] SpaceSeparators = { ' ', '\t' };

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
            ParsePosition nextTokenPosition;
            ParsePosition startPosition;
            ParsePosition endPosition;
            if (literalAtPosition != null)
            {
                if (literalAtPosition.CurrentPosition != position && position.Line == literalAtPosition.CurrentPosition.Line && literalAtPosition.CurrentPosition.Col < position.Col && position.Line < Context.Input.Length)
                {
                    fragment = Context.Input[position.Line].Substring(literalAtPosition.CurrentPosition.Col, position.Col - literalAtPosition.CurrentPosition.Col);
                }
                startPosition = literalAtPosition.CurrentPosition;
                endPosition = literalAtPosition.CurrentPosition + literalAtPosition.Length;
                nextTokenPosition = _matcher.NextTokenPosition(new ParsePosition(position.Line, position.Col + 1));

                // literal might be completed or we need to replace the current literal
                return Context.RootRuleApplication.SuggestCompletions(startPosition, endPosition, fragment, _context, nextTokenPosition)
                    .Concat(Context.RootRuleApplication.SuggestCompletions(endPosition, endPosition, string.Empty, Context, nextTokenPosition));
            }
            else if (position.Line < Context.Input.Length)
            {
                var line = Context.Input[position.Line];
                if (line.Length > position.Col && !char.IsWhiteSpace(line[position.Col]))
                {
                    // means we are at a token that was not understood, take the entire remaining line as fragment
                    var spaceSep = line.LastIndexOfAny(SpaceSeparators, position.Col);
                    fragment = line.Substring(spaceSep + 1, position.Col - spaceSep - 1);
                    startPosition = new ParsePosition(position.Line, spaceSep + 1);
                    var nextSpace = line.IndexOfAny(SpaceSeparators, position.Col);
                    if (nextSpace < 0) nextSpace = line.Length;
                    endPosition = new ParsePosition(position.Line, nextSpace);
                }
                else
                {
                    startPosition = position;
                    endPosition = position;
                }
                nextTokenPosition = _matcher.NextTokenPosition(position);
            }
            else
            {
                startPosition = position;
                endPosition = position;
                nextTokenPosition = position;
            }

            return Context.RootRuleApplication.SuggestCompletions(startPosition, endPosition, fragment, _context, nextTokenPosition)?.DistinctBy(e => e.Completion);
        }
    }
}
