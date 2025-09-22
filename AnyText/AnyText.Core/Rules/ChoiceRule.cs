using NMF.AnyText.PrettyPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a choice of multiple alternative rules
    /// </summary>
    public class ChoiceRule : Rule
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ChoiceRule() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="alternatives">the alternatives</param>
        public ChoiceRule(params FormattedRule[] alternatives)
        {
            Alternatives = alternatives;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (GetType() == typeof(ChoiceRule))
            {
                return string.Join('|', Alternatives.Select(a => a.Rule.ToString()));
            }
            return base.ToString();
        }

        /// <summary>
        /// Gets or sets the alternatives
        /// </summary>
        public FormattedRule[] Alternatives { get; set; }

        /// <inheritdoc />
        protected internal override bool CanStartWith(Rule rule, List<Rule> trace)
        {
            if (trace.Contains(this))
            {
                return false;
            }
            trace.Add(this);
            return Array.Exists(Alternatives, r => r.Rule == rule || r.Rule.CanStartWith(rule, trace));
        }

        /// <inheritdoc />
        protected internal override void AddLeftRecursionRules(List<Rule> trace, List<RecursiveContinuation> continuations)
        {
            if (!trace.Contains(this) && CanStartWith(this))
            {
                trace.Add(this);
                foreach (var child in Alternatives)
                {
                    child.Rule.AddLeftRecursionRules(trace, continuations);
                }
            }
        }

        /// <inheritdoc />
        protected internal override bool IsEpsilonAllowed(List<Rule> trace)
        {
            if (trace.Contains(this))
            {
                return false;
            }
            trace.Add(this);
            return Array.Exists(Alternatives, r => r.Rule.IsEpsilonAllowed(trace));
        }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            var savedPosition = position;
            var examined = new ParsePositionDelta();
            foreach (var rule in Alternatives.Select(a => a.Rule))
            {
                var match = context.Matcher.MatchCore(rule, recursionContext, context, ref position);
                examined = ParsePositionDelta.Larger(examined, match.ExaminedTo);
                if (match.IsPositive)
                {
                    return CreateRuleApplication(match, examined);
                }
                position = savedPosition;
            }
            return new FailedChoiceRuleApplication(this, context.Matcher.GetErrorsExactlyAt(savedPosition).Where(r => Array.Exists(Alternatives, a => a.Rule == r.Rule)), default, examined);
        }

        internal override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RecursionContext recursionContext, ref ParsePosition position)
        {
            return new MatchOrMatchProcessor(new ChoiceMatchProcessor(this, position));
        }

        private sealed class ChoiceMatchProcessor : MatchProcessor
        {
            private readonly ChoiceRule _parent;
            private int _index;
            private ParsePositionDelta _examined;
            private ParsePosition _savedPosition;

            public ChoiceMatchProcessor(ChoiceRule parent, ParsePosition position)
            {
                _parent = parent;
                _savedPosition = position;
            }

            public override Rule Rule => _parent;

            public override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RuleApplication ruleApplication, ref ParsePosition position, ref RecursionContext recursionContext)
            {
                if (_index > 0 && TryCreateMatch(ref position, ref ruleApplication))
                {
                    return new MatchOrMatchProcessor(ruleApplication);
                }
                while (_index < _parent.Alternatives.Length)
                {
                    var rule = _parent.Alternatives[_index].Rule;
                    _index++;
                    var next = context.Matcher.MatchOrCreateMatchProcessor(rule, context, ref recursionContext, ref position);
                    if (next.IsMatch)
                    {
                        ruleApplication = next.Match;
                        if (TryCreateMatch(ref position, ref ruleApplication))
                        {
                            return new MatchOrMatchProcessor(ruleApplication);
                        }
                    }
                    else
                    {
                        return new MatchOrMatchProcessor(next.MatchProcessor);
                    }
                }
                return new MatchOrMatchProcessor(new FailedChoiceRuleApplication(_parent, context.Matcher.GetErrorsExactlyAt(_savedPosition).Where(r => Array.Exists(_parent.Alternatives, a => a.Rule == r.Rule)), default, _examined));
            }

            private bool TryCreateMatch(ref ParsePosition position, ref RuleApplication ruleApplication)
            {
                _examined = ParsePositionDelta.Larger(_examined, ruleApplication.ExaminedTo);
                if (ruleApplication.IsPositive)
                {
                    ruleApplication = _parent.CreateRuleApplication(ruleApplication, _examined);
                    return true;
                }
                else
                {
                    position = _savedPosition;
                    ruleApplication = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Creates a rule application for a success
        /// </summary>
        /// <param name="match">the matched candidate</param>
        /// <param name="examined">the amount of text examined</param>
        /// <returns>a new rule application</returns>
        protected internal virtual RuleApplication CreateRuleApplication(RuleApplication match, ParsePositionDelta examined)
        {
            return new SingleRuleApplication(this, match, match.Length, examined);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement, ParseContext context, SynthesisPlan synthesisPlan)
        {
            synthesisPlan ??= new SynthesisPlan();
            synthesisPlan.BlockRecursion(this, semanticElement);
            return Array.Exists(Alternatives, r => synthesisPlan.CanSynthesize(r.Rule, semanticElement, context));
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var synthesisPlan = new SynthesisPlan();
            var alternative = Array.Find(Alternatives, a => synthesisPlan.CanSynthesize(a.Rule, semanticElement, context));
            if (alternative.Rule != null)
            {
                return alternative.Rule.Synthesize(semanticElement, position, context);
            }
            return new FailedRuleApplication(this, default, $"Failed to synthesize {semanticElement}");
        }

        internal override void Write(PrettyPrintWriter writer, ParseContext context, SingleRuleApplication ruleApplication)
        {
            var index = Array.FindIndex(Alternatives, a => a.Rule == ruleApplication.Inner.Rule);
            ruleApplication.Inner.Write(writer, context);
            RuleHelper.ApplyFormattingInstructions(Alternatives[index].FormattingInstructions, writer);
        }

    }
}
