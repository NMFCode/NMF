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
    }
}
