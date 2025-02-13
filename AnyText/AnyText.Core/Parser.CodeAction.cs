using System;
using System.Collections.Generic;
using System.Linq;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Retrieves code action information within a specified range of parse positions.
        /// </summary>
        /// <param name="start">The starting position of the range.</param>
        /// <param name="end">The ending position of the range.</param>
        /// <param name="predicate">An optional predicate to filter rule applications.</param>
        /// <returns>A collection of <see cref="ActionInfo"/> objects representing available code actions.</returns>
        public IEnumerable<ActionInfoApplication> GetCodeActionInfo(ParsePosition start, ParsePosition end,
            Predicate<RuleApplication> predicate = null)
        {

            var ruleApp = Context.RootRuleApplication.GetLiteralAt(start);
            if (ruleApp == null) return Enumerable.Empty<ActionInfoApplication>();

            predicate ??= _ => true;
            var codeActionInfos = new List<ActionInfoApplication>();

            while (!(ruleApp.CurrentPosition <= start &&
                     ruleApp.CurrentPosition + ruleApp.Length >= end))
            {
                ruleApp = ruleApp.Parent;
                if (ruleApp == null)
                    return codeActionInfos;
            }

            CollectCodeActionsWithRuleApplication(ruleApp, predicate, codeActionInfos);

            var parent = ruleApp.Parent;
            while (parent != null && parent.Length == ruleApp.Length)
            {
                CollectCodeActionsWithRuleApplication(parent, predicate, codeActionInfos);
                parent = parent.Parent;
                
            }


            return codeActionInfos;
        }

        private static void CollectCodeActionsWithRuleApplication(RuleApplication ruleApp, Predicate<RuleApplication> predicate,
            List<ActionInfoApplication> codeActionInfos)
        {
            if (predicate.Invoke(ruleApp))
                codeActionInfos.AddRange(ruleApp.Rule.SupportedCodeActions.Select(a => new ActionInfoApplication(a, ruleApp)));
        }
    }
}