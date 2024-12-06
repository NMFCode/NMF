using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes that another rule can occur an arbitrary number of times
    /// </summary>
    public class ZeroOrMoreRule : Rule
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ZeroOrMoreRule() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="innerRule">the inner rule</param>
        public ZeroOrMoreRule(Rule innerRule)
        {
            InnerRule = innerRule;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="innerRule">the inner rule</param>
        /// <param name="formattingInstructions">formatting instructions</param>
        public ZeroOrMoreRule(Rule innerRule, params FormattingInstruction[] formattingInstructions)
        {
            InnerRule = innerRule;
            FormattingInstructions = formattingInstructions;
        }

        /// <inheritdoc />
        protected internal override bool CanStartWith(Rule rule, List<Rule> trace)
        {
            if (trace.Contains(this))
            {
                return false;
            }
            trace.Add(this);
            return rule == InnerRule || InnerRule.CanStartWith(rule, trace);
        }

        /// <inheritdoc />
        protected internal override bool IsEpsilonAllowed(List<Rule> trace)
        {
            return true;
        }

        /// <summary>
        /// Gets or sets the inner rule
        /// </summary>
        public Rule InnerRule { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var applications = new List<RuleApplication>();
            var examined = new ParsePositionDelta();
            RuleHelper.Star(context, InnerRule, applications, savedPosition, ref position, ref examined);
            return new MultiRuleApplication(this, savedPosition, applications, position - savedPosition, examined);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            return true;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var applications = new List<RuleApplication>();
            var length = RuleHelper.SynthesizeStar(semanticElement, InnerRule, applications, position, context);
            return new MultiRuleApplication(this, position, applications, length, default);
        }
    }
}
