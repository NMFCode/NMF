﻿using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes that a rule that can be matched at most once
    /// </summary>
    public class ZeroOrOneRule : Rule
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ZeroOrOneRule() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="innerRule">the inner rule</param>
        public ZeroOrOneRule(Rule innerRule)
        {
            InnerRule = innerRule;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="innerRule">the inner rule</param>
        public ZeroOrOneRule(FormattedRule innerRule)
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
            return true;
        }

        /// <summary>
        /// The inner rule
        /// </summary>
        public Rule InnerRule { get; }

        /// <summary>
        /// Gets or sets the formatting instructions
        /// </summary>
        public FormattingInstruction[] FormattingInstructions { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            var savedPosition = position;
            var attempt = context.Matcher.MatchCore(InnerRule, recursionContext, context, ref position);
            if (!attempt.IsPositive)
            {
                position = savedPosition;
            }
            return new SingleRuleApplication(this, attempt, attempt.IsPositive ? attempt.Length : default, attempt.ExaminedTo);
        }

        internal override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            return new MatchOrMatchProcessor(new ZeroOrOneMatchProcessor(this));
        }

        private sealed class ZeroOrOneMatchProcessor : MatchProcessor
        {
            private readonly ZeroOrOneRule _rule;

            public ZeroOrOneMatchProcessor(ZeroOrOneRule rule)
            {
                _rule = rule;
            }

            public override Rule Rule => _rule;

            public override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RuleApplication ruleApplication, ref ParsePosition position, ref RecursionContext recursionContext)
            {
                if (ruleApplication == null)
                {
                    var innerProc = context.Matcher.MatchOrCreateMatchProcessor(_rule.InnerRule, context, ref recursionContext, ref position);
                    if (innerProc.IsMatchProcessor)
                    {
                        return innerProc;
                    }
                    ruleApplication = innerProc.Match;
                }
                if (!ruleApplication.IsPositive)
                {
                    position = ruleApplication.CurrentPosition;
                }
                return new MatchOrMatchProcessor(new SingleRuleApplication(_rule, ruleApplication, ruleApplication.IsPositive ? ruleApplication.Length : default, ruleApplication.ExaminedTo));
            }
        }

        internal override void Write(PrettyPrintWriter writer, ParseContext context, SingleRuleApplication ruleApplication)
        {
            if (ruleApplication.Inner != null && ruleApplication.Inner.IsPositive)
            {
                ruleApplication.Inner.Write(writer, context);
                RuleHelper.ApplyFormattingInstructions(FormattingInstructions, writer);
            }
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            return true;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({InnerRule})?";
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var inner = InnerRule.Synthesize(semanticElement, position, context);
            return new SingleRuleApplication(this, inner, inner.IsPositive ? inner.Length : default, default);
        }


    }
}
