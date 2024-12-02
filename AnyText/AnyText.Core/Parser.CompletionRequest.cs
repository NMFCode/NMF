using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class Parser
    {
        public virtual IEnumerable<string> GetCompletionList(ParsePosition position)
        {
            // Default implementation aggregates suggestions from rule applications
            var completions = new List<string>();

            var ruleApplications = GetPotentialRulesAt(position);

            foreach (var rule in ruleApplications)
            {
                var suggestions = rule.SuggestCompletions();
                completions.AddRange(suggestions);
            }            

            return completions.Distinct().OrderBy(s => s);
        }

        /// <summary>
        /// Retrieves a collection of potential rules that could match at a given parse position.
        /// </summary>
        /// <param name="position">The parse position for which potential rules are identified.</param>
        /// <returns>An enumerable collection of distinct rules that may match at the specified position.</returns>
        /// <remarks>
        /// This method uses the memoization table of the <see cref="Matcher"/> to identify rules that have been applied 
        /// or could potentially apply at the given position. The result includes only distinct rules.
        /// </remarks>
        public IEnumerable<Rule> GetPotentialRulesAt(ParsePosition position)
        {
            var potentialRules = new List<Rule>();

            var ruleApplications = _matcher.GetRuleApplicationsAt(position);

            foreach (var application in ruleApplications)
            {
                potentialRules.Add(application.Rule);
            }

            return potentialRules.Distinct();
        }
    }
}
