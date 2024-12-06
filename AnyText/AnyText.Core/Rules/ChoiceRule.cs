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
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var examined = new ParsePositionDelta();
            foreach (var rule in Alternatives.Select(a => a.Rule))
            {
                var match = context.Matcher.MatchCore(rule, context, ref position);
                examined = ParsePositionDelta.Larger(examined, match.ExaminedTo);
                if (match.IsPositive)
                {
                    return CreateRuleApplication(match, examined);
                }
                position = savedPosition;
            }
            return new FailedRuleApplication(this, position, examined, position, "No viable choice");
        }

        /// <summary>
        /// Creates a rule application for a success
        /// </summary>
        /// <param name="match">the matched candidate</param>
        /// <param name="examined">the amount of text examined</param>
        /// <returns>a new rule application</returns>
        protected virtual RuleApplication CreateRuleApplication(RuleApplication match, ParsePositionDelta examined)
        {
            return new SingleRuleApplication(this, match, match.Length, examined);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            return Array.Exists(Alternatives, r => r.Rule.CanSynthesize(semanticElement));
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            foreach (var rule in Alternatives.Select(a => a.Rule))
            {
                if (rule.CanSynthesize(semanticElement))
                {
                    return rule.Synthesize(semanticElement, position, context);
                }
            }
            return new FailedRuleApplication(this, position, default, position, $"Failed to synthesize {semanticElement}");
        }

        internal override void Write(PrettyPrintWriter writer, ParseContext context, SingleRuleApplication ruleApplication)
        {
            var index = Array.FindIndex(Alternatives, a => a.Rule == ruleApplication.Inner.Rule);
            ruleApplication.Inner.Write(writer, context);
            RuleHelper.ApplyFormattingInstructions(Alternatives[index].FormattingInstructions, writer);
        }
    }
}
