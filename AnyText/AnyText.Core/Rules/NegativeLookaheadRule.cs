using NMF.AnyText.PrettyPrinting;
using System.Collections.Generic;

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

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="inner">the negative lookahead</param>
        public NegativeLookaheadRule(FormattedRule inner) : this(inner.Rule) { }

        /// <inheritdoc />
        protected internal override bool CanStartWith(Rule rule, List<Rule> trace)
        {
            return false;
        }

        /// <inheritdoc />
        protected internal override bool IsEpsilonAllowed(List<Rule> trace)
        {
            return true;
        }

        /// <summary>
        /// Gets or sets the negative lookahead
        /// </summary>
        public Rule Inner { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            var savedPosition = position;
            var attempt = context.Matcher.MatchCore(Inner, recursionContext, context, ref position);
            if (attempt.IsPositive)
            {
                position = savedPosition;
                return new FailedRuleApplication(this, attempt.ExaminedTo, "found negative lookahead");
            }
            return new SingleRuleApplication(this, attempt, default, attempt.ExaminedTo);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            return true;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            return new SingleRuleApplication(this, null, default, default);
        }

        internal override void Write(PrettyPrintWriter writer, ParseContext context, SingleRuleApplication ruleApplication)
        {
            // do not write anything for lookaheads
        }
    }
}
