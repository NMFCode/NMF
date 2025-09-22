using NMF.AnyText.PrettyPrinting;
using System.Collections.Generic;

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
        public ZeroOrMoreRule(FormattedRule innerRule)
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
            return true;
        }

        /// <inheritdoc/>
        public override RuleApplication CreateEpsilonRuleApplication(RuleApplication position)
        {
            var star = new StarRuleApplication(this, new List<RuleApplication>(), null, default, default);
            star.CopyPosition(position);
            return star;
        }

        /// <inheritdoc />
        public override bool HasFoldingKind(out string kind)
        {
            if (InnerRule.IsImports())
            {
                kind = "imports";
                return true;
            }
            
            return base.HasFoldingKind(out kind);
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
        public override RuleApplication Match(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            var savedPosition = position;
            var applications = new List<RuleApplication>();
            var examined = new ParsePositionDelta();
            var isRecovered = false;
            var fail = RuleHelper.Star(context, recursionContext, InnerRule, applications, savedPosition, Accept, ref position, ref examined, ref isRecovered);
            return new StarRuleApplication(this, applications, fail, position - savedPosition, examined).SetRecovered(isRecovered);
        }

        /// <summary>
        /// Decides whether the provided rule application shall be accepted
        /// </summary>
        /// <param name="ruleApplication">the rule application that shall be accepted</param>
        /// <param name="ruleApplications">the rule applications accepted so far</param>
        /// <param name="context">the parse context in which the rule applications shall be accepted</param>
        /// <returns>true, if the rule application shall be accepted, otherwise false</returns>
        protected bool Accept(RuleApplication ruleApplication, List<RuleApplication> ruleApplications, ParseContext context)
        {
            return context.AcceptZeroOrMoreAdd(this, ruleApplication, ruleApplications);
        }

        internal override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            return new MatchOrMatchProcessor(new ZeroOrMoreProcessor(this, position));
        }


        private sealed class ZeroOrMoreProcessor : MatchProcessor
        {
            private readonly ZeroOrMoreRule _parent;
            private readonly List<RuleApplication> _applications = new List<RuleApplication>();
            private ParsePositionDelta _examined;
            private readonly ParsePosition _startPosition;
            private ParsePosition _beforeLast;

            public ZeroOrMoreProcessor(ZeroOrMoreRule parent, ParsePosition position)
            {
                _parent = parent;
                _startPosition = position;
                _beforeLast = position;
            }

            public override Rule Rule => _parent;

            public override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RuleApplication ruleApplication, ref ParsePosition position, ref RecursionContext recursionContext)
            {
                if (ruleApplication != null && ProcessRuleApplication(context, ref position, ref ruleApplication))
                {
                    return new MatchOrMatchProcessor(ruleApplication);
                }
                while (true)
                {
                    var next = context.Matcher.MatchOrCreateMatchProcessor(_parent.InnerRule, context, ref recursionContext, ref position);
                    if (next.IsMatchProcessor)
                    {
                        return next;
                    }
                    ruleApplication = next.Match;
                    if (ProcessRuleApplication(context, ref position, ref ruleApplication))
                    {
                        return new MatchOrMatchProcessor(ruleApplication);
                    }
                }
            }

            private bool ProcessRuleApplication(ParseContext context, ref ParsePosition position, ref RuleApplication ruleApplication)
            {
                _examined = ParsePositionDelta.Larger(_examined, (_beforeLast + ruleApplication.ExaminedTo - _startPosition));
                if (ruleApplication.IsPositive && _parent.Accept(ruleApplication, _applications, context))
                {
                    _applications.Add(ruleApplication);
                    _beforeLast = position;
                    return false;
                }
                else
                {
                    position = _beforeLast;
                    ruleApplication = new StarRuleApplication(_parent, _applications, ruleApplication, position - _startPosition, _examined);
                    return true;
                }
            }
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
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            return true;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({InnerRule})*";
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var applications = new List<RuleApplication>();
            var length = RuleHelper.SynthesizeStar(semanticElement, InnerRule, applications, position, context);
            return new MultiRuleApplication(this, applications, length, default);
        }
    }
}
