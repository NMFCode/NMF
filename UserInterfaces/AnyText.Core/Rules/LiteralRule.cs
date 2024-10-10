using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule that matches a constant text
    /// </summary>
    public class LiteralRule : Rule
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="literal">the literal that should be matched</param>
        public LiteralRule(string literal)
        {
            Literal = literal;
        }


        /// <inheritdoc />
        public override bool CanStartWith(Rule rule)
        {
            return false;
        }

        /// <inheritdoc />
        public override bool IsEpsilonAllowed()
        {
            return false;
        }

        /// <summary>
        /// Gets the literal that should be matched
        /// </summary>
        public string Literal { get; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            if (position.Line >= context.Input.Length)
            {
                return new FailedRuleApplication(this, position, default, position, Literal);
            }
            var line = context.Input[position.Line];
            if (line.Length < position.Col + Literal.Length)
            {
                return new FailedRuleApplication(this, position, new ParsePositionDelta(0, Literal.Length), position, Literal);
            }

            if (MemoryExtensions.Equals(Literal, line.AsSpan(position.Col, Literal.Length), context.StringComparison))
            {
                var res = new LiteralRuleApplication(this, Literal, position, new ParsePositionDelta(0, Literal.Length));
                position = position.Proceed(Literal.Length);
                return res;
            }

            return new FailedRuleApplication(this, position, new ParsePositionDelta(0, Literal.Length), position, Literal);
        }

        /// <inheritdoc />
        public override string TokenType => "keyword";
    }
}
