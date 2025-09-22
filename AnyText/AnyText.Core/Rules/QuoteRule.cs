using NMF.AnyText.PrettyPrinting;
using System.Collections.Generic;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule that delegates to an inner rule without changes
    /// </summary>
    public class QuoteRule : Rule
    {
        /// <summary>
        /// Gets or sets the inner rule
        /// </summary>
        public FormattedRule Inner
        {
            get => new(InnerRule, FormattingInstructions);
            set => (InnerRule, FormattingInstructions) = value;
        }

        /// <summary>
        /// Gets or sets the inner rule
        /// </summary>
        public Rule InnerRule { get; set; }

        /// <summary>
        /// Gets or sets formatting instructions
        /// </summary>
        public FormattingInstruction[] FormattingInstructions { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            var app = context.Matcher.MatchCore(InnerRule, recursionContext, context, ref position);
            if (app.IsPositive)
            {
                return CreateRuleApplication(app, context);
            }
            return new InheritedFailRuleApplication(this, app, app.ExaminedTo);
        }

        /// <inheritdoc />
        protected internal override RuleApplication Recover(RuleApplication ruleApplication, RuleApplication failedRuleApplication, RuleApplication currentRoot, ParseContext context, out ParsePosition position)
        {
            var recovery = failedRuleApplication.Recover(currentRoot, context, out position);
            if (recovery.IsPositive)
            {
                return CreateRuleApplication(recovery, context);
            }
            return ruleApplication;
        }

        internal override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            return new MatchOrMatchProcessor(new QuoteMatchProcessor(this));
        }

        private sealed class QuoteMatchProcessor : MatchProcessor
        {
            private readonly QuoteRule _parent;

            public QuoteMatchProcessor(QuoteRule parent)
            {
                _parent = parent;
            }

            public override Rule Rule => _parent;

            public override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RuleApplication ruleApplication, ref ParsePosition position, ref RecursionContext recursionContext)
            {
                if (ruleApplication == null)
                {
                    var inner = context.Matcher.MatchOrCreateMatchProcessor(_parent.InnerRule, context, ref recursionContext, ref position);
                    if (inner.IsMatchProcessor)
                    {
                        return inner;
                    }
                    ruleApplication = inner.Match;
                }
                if (ruleApplication.IsPositive)
                {
                    return new MatchOrMatchProcessor(_parent.CreateRuleApplication(ruleApplication, context));
                }
                else
                {
                    return new MatchOrMatchProcessor(new InheritedFailRuleApplication(_parent, ruleApplication, ruleApplication.ExaminedTo));
                }
            }
        }

        /// <inheritdoc />
        public override IEnumerable<SynthesisRequirement> CreateSynthesisRequirements()
        {
            return InnerRule.CreateSynthesisRequirements();
        }

        /// <summary>
        /// Creates the rule application for this rule
        /// </summary>
        /// <param name="app">the inner rule application</param>
        /// <param name="context">the parse context</param>
        /// <returns>the new rule application</returns>
        protected virtual RuleApplication CreateRuleApplication(RuleApplication app, ParseContext context)
        {
            return new SingleRuleApplication(this, app, app.Length, app.ExaminedTo);
        }

        /// <inheritdoc />
        protected internal override bool CanStartWith(Rule rule, List<Rule> trace)
        {
            return rule == InnerRule || InnerRule.CanStartWith(rule, trace);
        }

        /// <inheritdoc />
        protected internal override void AddLeftRecursionRules(List<Rule> trace, List<RecursiveContinuation> continuations)
        {
            if (!trace.Contains(this))
            {
                trace.Add(this);
                InnerRule.AddLeftRecursionRules(trace, continuations);
            }
        }

        /// <inheritdoc />
        protected internal override bool IsEpsilonAllowed(List<Rule> trace)
        {
            return InnerRule.IsEpsilonAllowed(trace);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            return InnerRule.CanSynthesize(semanticElement, context, synthesisPlan);
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var inner = InnerRule.Synthesize(semanticElement, position, context);
            if (inner.IsPositive)
            {
                return CreateRuleApplication(inner, context);
            }
            return new InheritedFailRuleApplication(this, inner, default);
        }

        internal override void Write(PrettyPrintWriter writer, ParseContext context, SingleRuleApplication ruleApplication)
        {
            ruleApplication.Inner.Write(writer, context);
            RuleHelper.ApplyFormattingInstructions(FormattingInstructions, writer);
        }
    }
}
