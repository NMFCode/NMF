using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Rules
{
    /// <summary>
    /// Denotes a rule that other rules occur in sequence
    /// </summary>
    public class SequenceRule : Rule
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public SequenceRule() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rules">The rules that should occur in sequence</param>
        public SequenceRule(params Rule[] rules)
        {
            Rules = rules;
        }

        /// <summary>
        /// The rules that should occur in sequence
        /// </summary>
        public Rule[] Rules { get; set; }

        /// <inheritdoc />
        public override RuleApplication Match(ParseContext context, ref ParsePosition position)
        {
            var savedPosition = position;
            var applications = new List<RuleApplication>();
            var examined = new ParsePositionDelta();
            foreach (var rule in Rules)
            {
                var app = context.Matcher.MatchCore(rule, context, ref position);
                examined = ParsePositionDelta.Larger(examined, app.ExaminedTo);
                if (app.IsPositive)
                {
                    applications.Add(app);
                }
                else
                {
                    position = savedPosition;
                    return new FailedRuleApplication(this, examined, app.ErrorPosition, app.Message);
                }
            }
            return CreateRuleApplication(applications, position - savedPosition, examined);
        }

        /// <summary>
        /// Creates a rule application for a success
        /// </summary>
        /// <param name="inner">the inner list of rule applications</param>
        /// <param name="length">the length of the match</param>
        /// <param name="examined">the amount of text examined</param>
        /// <returns>a new rule application</returns>
        protected virtual RuleApplication CreateRuleApplication(List<RuleApplication> inner, ParsePositionDelta length, ParsePositionDelta examined)
        {
            return new MultiRuleApplication(this, inner, length, examined);
        }
    }
}
