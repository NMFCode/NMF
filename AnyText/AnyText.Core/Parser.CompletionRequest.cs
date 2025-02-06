using NMF.AnyText.Model;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <inheritdoc/>
        public virtual IEnumerable<string> GetCompletionList(ParsePosition position)
        {
            var completions = new List<string>();

            var ruleApplications = _matcher.GetRuleApplicationsAtWithLookahead(position, _context, true);
            var keywords = Context.Grammar.Keywords;

            completions.AddRange(from keyword in keywords
                                 where ruleApplications.Any(a => a.CurrentPosition > position && a.Rule.CanStartWith(keyword))
                                 select keyword.Literal);

            foreach (var ruleApp in ruleApplications)
            {
                var suggestions = ruleApp.Rule.SuggestCompletions(_context, ruleApp, position);
                if (suggestions != null)
                {
                    completions.AddRange(suggestions);
                }
            }

            return completions.Distinct().OrderBy(s => s);
        }


    }
}
