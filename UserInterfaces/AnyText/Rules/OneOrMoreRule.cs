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
                return new FailedRuleApplication(this, attempt.ExaminedTo, attempt.ErrorPosition, attempt.Message);
            }
            var applications = new List<RuleApplication>();
            var examined = attempt.ExaminedTo;
            RuleHelper.Star(context, InnerRule, applications, ref position, ref examined);
            return new MultiRuleApplication(this, applications, position - savedPosition, examined);
        }
    }
}
