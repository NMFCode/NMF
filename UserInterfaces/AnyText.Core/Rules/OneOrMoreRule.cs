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


        /// <inheritdoc />
        public override bool CanStartWith(Rule rule)
        {
            return rule == InnerRule || InnerRule.CanStartWith(rule);
        }

        /// <inheritdoc />
        public override bool IsEpsilonAllowed()
        {
            return InnerRule.IsEpsilonAllowed();
        }

        /// <summary>
        /// Gets or sets the inner rule
        /// </summary>
        public Rule InnerRule { get; set; }

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
            RuleHelper.Star(context, InnerRule, applications, ref position, ref examined);
            return new MultiRuleApplication(this, attempt.CurrentPosition, applications, position - savedPosition, examined);
        }
    }
}
