using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a class for a rule that applies escaping
    /// </summary>
    public abstract class EscapedRegexRule : RegexRule
    {
        /// <summary>
        /// Escapes the given string
        /// </summary>
        /// <param name="value">the unescaped string</param>
        /// <returns>the escaped string</returns>
        public abstract string Escape(string value);

        /// <summary>
        /// Unescapes the given string
        /// </summary>
        /// <param name="value">the escaped string</param>
        /// <returns>the unescaped string</returns>
        public abstract string Unescape(string value);

        /// <inheritdoc />
        public override RuleApplication CreateRuleApplication(string matched, ParsePosition position, ParsePositionDelta examined, ParseContext context)
        {
            return new EscapedRuleApplication(this, matched, position, examined);
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            if (string.IsNullOrEmpty(semanticElement?.ToString()))
            {
                return new FailedRuleApplication(this, position, default, position, string.Empty);
            }
            return CreateRuleApplication(Escape(semanticElement.ToString()), position, default, context);
        }

        private sealed class EscapedRuleApplication : LiteralRuleApplication
        {
            public EscapedRuleApplication(Rule rule, string literal, ParsePosition currentPosition, ParsePositionDelta examinedTo) : base(rule, literal, currentPosition, examinedTo)
            {
            }

            public override object GetValue(ParseContext context)
            {
                var escapeRule = (EscapedRegexRule)Rule;
                return escapeRule.Unescape(Literal);
            }

        }
    }
}
