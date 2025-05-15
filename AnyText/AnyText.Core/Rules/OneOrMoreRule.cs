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
        public override RuleApplication Match(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            var savedPosition = position;
            var attempt = context.Matcher.MatchCore(InnerRule, recursionContext, context, ref position);
            if (!attempt.IsPositive)
            {
                position = savedPosition;
                return new InheritedFailRuleApplication(this, attempt, attempt.ExaminedTo);
            }
            var applications = new List<RuleApplication> { attempt };
            var examined = attempt.ExaminedTo;
            var fail = RuleHelper.Star(context, recursionContext, InnerRule, applications, savedPosition, Accept, ref position, ref examined);
            return new StarRuleApplication(this, applications, fail, position - savedPosition, examined);
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
            return context.AcceptOneOrMoreAdd(this, ruleApplication, ruleApplications);
        }

        internal override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            return new MatchOrMatchProcessor(new OneOrMoreProcessor(this, position));
        }

        private sealed class OneOrMoreProcessor : MatchProcessor
        {
            private readonly OneOrMoreRule _parent;
            private readonly List<RuleApplication> _applications = new List<RuleApplication>();
            private ParsePositionDelta _examined;
            private readonly ParsePosition _startPosition;
            private ParsePosition _beforeLast;

            public OneOrMoreProcessor(OneOrMoreRule parent, ParsePosition position)
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
                    if (_applications.Count == 0)
                    {
                        position = _startPosition;
                        ruleApplication = new InheritedFailRuleApplication(_parent, ruleApplication, _examined);
                    }
                    else
                    {
                        position = _beforeLast;
                        ruleApplication = new StarRuleApplication(_parent, _applications, ruleApplication, position - _startPosition, _examined);
                    }
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
        public override IEnumerable<SynthesisRequirement> CreateSynthesisRequirements()
        {
            return InnerRule.CreateSynthesisRequirements();
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            return InnerRule.CanSynthesize(semanticElement, context, synthesisPlan);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({InnerRule})+";
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var attempt = InnerRule.Synthesize(semanticElement, position, context);
            if (!attempt.IsPositive)
            {
                return new InheritedFailRuleApplication(this, attempt, default);
            }
            var applications = new List<RuleApplication>() { attempt };
            var length = RuleHelper.SynthesizeStar(semanticElement, InnerRule, applications, position + attempt.Length, context);
            return new MultiRuleApplication(this, applications, length, default);
        }
    }
}
