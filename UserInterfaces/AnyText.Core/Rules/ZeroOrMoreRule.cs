using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Gets or sets the inner rule
        /// </summary>
        public Rule InnerRule { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var applications = new List<RuleApplication>();
            var examined = new ParsePositionDelta();
            RuleHelper.Star(context, InnerRule, applications, ref position, ref examined);
            return new MultiRuleApplication(this, savedPosition, applications, position - savedPosition, examined);
        }

    }
}
