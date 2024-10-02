using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule that parses text based on regular expressions
    /// </summary>
    /// <remarks>Regular expressions are always restricted to a single line, only</remarks>
    public class RegexRule : Rule
    {
        /// <summary>
        /// Gets or sets the regular expression
        /// </summary>
        public Regex Regex { get; set; }

        private const string RegexFailed = "Regular expression did not match";

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            if (position.Line >= context.Input.Length)
            {
                return new FailedRuleApplication(this, default, position, RegexFailed);
            }
            var line = context.Input[position.Line];
            var match = Regex.Match(line.Substring(position.Col));
            if (match.Success)
            {
                position = position.Proceed(match.Length);
                return CreateRuleApplication(match.Value, position, new ParsePositionDelta(0, match.Length), new ParsePositionDelta(0, line.Length - position.Col + 1), context);
            }
            else
            {
                return new FailedRuleApplication(this, new ParsePositionDelta(0, line.Length - position.Col + 1), position, RegexFailed);
            }
        }

        /// <summary>
        /// Creates a new rule application
        /// </summary>
        /// <param name="matched">the matched string content</param>
        /// <param name="position">the position where the rule matched</param>
        /// <param name="length">the length of the match</param>
        /// <param name="examined">the examined length of text</param>
        /// <param name="context">the parse context</param>
        /// <returns>a rule application</returns>
        public virtual RuleApplication CreateRuleApplication(string matched, ParsePosition position, ParsePositionDelta length, ParsePositionDelta examined, ParseContext context)
        {
            return new LiteralRuleApplication(this, matched, length, examined);
        }
    }
}
