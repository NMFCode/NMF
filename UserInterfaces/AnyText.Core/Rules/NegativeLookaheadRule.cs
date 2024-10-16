using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a negative lookahead rule
    /// </summary>
    public class NegativeLookaheadRule : Rule
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public NegativeLookaheadRule() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="inner">the negative lookahead</param>
        public NegativeLookaheadRule(Rule inner)
        {
            Inner = inner;
        }

        /// <inheritdoc />
        public override bool CanStartWith(Rule rule)
        {
            return false;
        }

        /// <inheritdoc />
        public override bool IsEpsilonAllowed()
        {
            return true;
        }

        /// <summary>
        /// Gets or sets the negative lookahead
        /// </summary>
        public Rule Inner { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var attempt = context.Matcher.MatchCore(Inner, context, ref position);
            if (attempt.IsPositive)
            {
                return new FailedRuleApplication(this, savedPosition, attempt.ExaminedTo, savedPosition, "found negative lookahead");
            }
            return new SingleRuleApplication(this, attempt, default, attempt.ExaminedTo);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            return true;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            return new SingleRuleApplication(this, null, default, default);
        }
    }
}
