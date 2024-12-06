using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule that is matched at least once
    /// </summary>
    public class OneOrMoreRule : Rule
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public OneOrMoreRule() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="innerRule">the inner rule</param>
        public OneOrMoreRule(Rule innerRule)
        {
            InnerRule = innerRule;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="innerRule">the inner rule</param>
        public OneOrMoreRule(FormattedRule innerRule)
        {
            InnerRule = innerRule.Rule;
            FormattingInstructions = innerRule.FormattingInstructions;
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
            if (trace.Contains(this))
            {
                return false;
            }
            trace.Add(this);
            return InnerRule.IsEpsilonAllowed(trace);
        }

        /// <summary>
        /// Gets or sets the inner rule
        /// </summary>
        public Rule InnerRule { get; set; }

        /// <summary>
        /// Gets or sets the formatting instructions
        /// </summary>
        public FormattingInstruction[] FormattingInstructions { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var attempt = context.Matcher.MatchCore(InnerRule, context, ref position);
            if (!attempt.IsPositive)
            {
                position = savedPosition;
                return new FailedRuleApplication(this, attempt.CurrentPosition, attempt.ExaminedTo, attempt.ErrorPosition, attempt.Message);
            }
            var applications = new List<RuleApplication> { attempt };
            var examined = attempt.ExaminedTo;
            RuleHelper.Star(context, InnerRule, applications, savedPosition, ref position, ref examined);
            return new MultiRuleApplication(this, attempt.CurrentPosition, applications, position - savedPosition, examined);
        }

        internal override void Write(PrettyPrintWriter writer, ParseContext context, MultiRuleApplication ruleApplication)
        {
            foreach (var inner in ruleApplication.Inner)
            {
                inner.Write(writer, context);
                RuleHelper.ApplyFormattingInstructions(FormattingInstructions, writer);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<SynthesisRequirement> CreateSynthesisRequirements()
        {
            return InnerRule.CreateSynthesisRequirements();
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            return InnerRule.CanSynthesize(semanticElement);
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var attempt = InnerRule.Synthesize(semanticElement, position, context);
            if (!attempt.IsPositive)
            {
                return new FailedRuleApplication(this, position, attempt.ExaminedTo, attempt.ErrorPosition, attempt.Message);
            }
            var applications = new List<RuleApplication>() { attempt };
            var length = RuleHelper.SynthesizeStar(semanticElement, InnerRule, applications, position + attempt.Length, context);
            return new MultiRuleApplication(this, position, applications, length, default);
        }
    }
}
