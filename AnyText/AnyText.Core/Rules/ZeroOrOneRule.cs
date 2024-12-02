using NMF.AnyText.PrettyPrinting;
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
        /// <param name="formattingInstructions">formatting instructions</param>
        public ZeroOrOneRule(Rule innerRule, params FormattingInstruction[] formattingInstructions)
        {
            InnerRule = innerRule;
            FormattingInstructions = formattingInstructions;
        }

        /// <inheritdoc />
        public override bool CanStartWith(Rule rule)
        {
            return rule == InnerRule || InnerRule.CanStartWith(rule);
        }

        /// <inheritdoc />
        public override bool IsEpsilonAllowed()
        {
            return true;
        }

        /// <summary>
        /// The inner rule
        /// </summary>
        public Rule InnerRule { get; }
        
        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var attempt = context.Matcher.MatchCore(InnerRule, context, ref position);
            if (!attempt.IsPositive)
            {
                position = savedPosition;
            }
            return new SingleRuleApplication(this, attempt, attempt.IsPositive ? attempt.Length : default, attempt.ExaminedTo);
        }

        /// <inheritdoc />
        public override bool CanSynthesize(object semanticElement)
        {
            return true;
        }

        /// <inheritdoc />
        public override RuleApplication Synthesize(object semanticElement, ParsePosition position, ParseContext context)
        {
            var inner = InnerRule.Synthesize(semanticElement, position, context);
            return new SingleRuleApplication(this, inner, inner.IsPositive ? inner.Length : default, default);
        }

        /// <inheritdoc />
        public override IEnumerable<string> SuggestCompletions()
        {
            // Prüfen, ob die innere Regel definiert ist
            if (InnerRule == null)
            {
                return Enumerable.Empty<string>();
            }

            // Vorschläge der inneren Regel übernehmen
            return InnerRule.SuggestCompletions();
        }
    }
}
