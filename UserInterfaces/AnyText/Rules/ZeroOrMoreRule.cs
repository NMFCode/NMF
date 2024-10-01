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
            return new MultiRuleApplication(this, applications, position - savedPosition, examined);
        }

    }
}
