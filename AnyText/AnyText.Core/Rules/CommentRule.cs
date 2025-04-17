using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule for a (single-line) comment
    /// </summary>
    public class CommentRule : Rule
    {
        /// <summary>
        /// Gets or sets the beginning 
        /// </summary>
        public string CommentStart { get; set; }

        /// <inheritdoc />
        protected internal override bool CanStartWith(Rule rule, List<Rule> trace)
        {
            return false;
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context)
        {
            return false;
        }

        /// <inheritdoc />
        protected internal override bool IsEpsilonAllowed(List<Rule> trace)
        {
            return false;
        }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            if (position.Line >= context.Input.Length)
            {
                return null;
            }
            var line = context.Input[position.Line];
            if (line.Length < position.Col + CommentStart.Length)
            {
                return null;
            }

            if (MemoryExtensions.Equals(CommentStart, line.AsSpan(position.Col, CommentStart.Length), context.StringComparison))
            {
                var nextLine = new ParsePositionDelta(1, 0);
                var res = new LiteralRuleApplication(this, line.Substring(position.Col), nextLine);
                position += nextLine;
                return res;
            }

            return null;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            throw new NotSupportedException("Cannot synthetisize comments");
        }

        /// <inheritdoc />
        public override bool IsLiteral => true;

        /// <inheritdoc />
        public override bool IsComment => true;

        /// <inheritdoc />
        public override string TokenType => "comment";
    }
}
