using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Rule Inner { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var app = context.Matcher.MatchCore(Inner, context, ref position);
            if (app.IsPositive)
            {
                return CreateRuleApplication(app, context);
            }
            if (app is RecursiveRuleApplication recurse)
            {
                return new RecursiveRuleApplication(this, recurse);
            }
            return new FailedRuleApplication(this, app.CurrentPosition, app.ExaminedTo, app.ErrorPosition, app.Message);
        }

        /// <inheritdoc />
        public override IEnumerable<SynthesisRequirement> CreateSynthesisRequirements()
        {
            return Inner.CreateSynthesisRequirements();
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
            return rule == Inner || Inner.CanStartWith(rule, trace);
        }

        /// <inheritdoc />
        protected internal override bool IsEpsilonAllowed(List<Rule> trace)
        {
            return Inner.IsEpsilonAllowed(trace);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            return Inner.CanSynthesize(semanticElement);
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var inner = Inner.Synthesize(semanticElement, position, context);
            if (inner.IsPositive)
            {
                return CreateRuleApplication(inner, context);
            }
            return new FailedRuleApplication(this, inner.CurrentPosition, inner.ExaminedTo, inner.ErrorPosition, inner.Message);
        }
    }
}
