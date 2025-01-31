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
            completions.Add(string.Empty);

            var ruleApplications = _matcher.GetRuleApplicationsAtWithLookahead(position, _context, true);
            var keywords = Context.Grammar.Keywords;

            // TODO: ResolveRule mittels separatem Mechanismus (AssignReferenceRule, AddAssignReferenceRule)
            foreach (var ruleApp in ruleApplications)
            {
                //string suggestion = null;
               /*
                foreach (var keyword in keywords)
                {
                    if (rule.Rule.CanStartWith(keyword))
                    {
                        var suggestions = keyword.SuggestCompletions(_context, position);
                        completions.AddRange(suggestions);
                    }
                }*/
                
                
                if (!ruleApp.IsPositive)
                {
                    var suggestions = ruleApp.SuggestCompletions(_context, position);
                    completions.AddRange(suggestions);
                }


                {
                    var result = _context.GetPotentialReferences<object>(ruleApp.ContextElement);
                    result.TryGetNonEnumeratedCount(out var count);
                }


            }            

            return completions.Distinct().OrderBy(s => s);
        }


    }
}
