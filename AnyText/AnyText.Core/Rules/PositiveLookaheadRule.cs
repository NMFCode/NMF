using NMF.AnyText.PrettyPrinting;
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
    public class PositiveLookaheadRule : Rule
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public PositiveLookaheadRule() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="inner">the negative lookahead</param>
        public PositiveLookaheadRule(Rule inner)
        {
            Inner = inner;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="inner">the negative lookahead</param>
        public PositiveLookaheadRule(FormattedRule inner) : this(inner.Rule) { }

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
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var attempt = context.Matcher.MatchCore(Inner, context, ref position);
            if (!attempt.IsPositive)
            {
                return new InheritedFailRuleApplication(this, attempt, attempt.ExaminedTo);
            }
            position = savedPosition;
            return new LookaheadRuleApplication(this, attempt, default, attempt.ExaminedTo);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context)
        {
            return true;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            return new LookaheadRuleApplication(this, null, default, default);
        }

        internal override void Write(PrettyPrintWriter writer, ParseContext context, SingleRuleApplication ruleApplication)
        {
            // do not write anything for lookaheads
        }

        private sealed class LookaheadRuleApplication : SingleRuleApplication
        {
            public LookaheadRuleApplication(Rule rule, RuleApplication inner, ParsePositionDelta endsAt, ParsePositionDelta examinedTo) : base(rule, inner, endsAt, examinedTo)
            {
            }

            public override void IterateLiterals(Action<LiteralRuleApplication> action)
            {
                // do not iterate lookaheads
            }

            public override void IterateLiterals<T>(Action<LiteralRuleApplication, T> action, T parameter)
            {
                // do not iterate lookaheads
            }

            public override object GetValue(ParseContext context)
            {
                return null;
            }
        }
    }
}
