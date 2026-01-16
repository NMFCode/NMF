using NMF.AnyText.Rules;
using System.Collections.Generic;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Parses folding ranges starting from the root rule application
        /// </summary>
        /// <returns>An IEnumerable of <see cref="FoldingRange"/> objects, each containing details on a folding range in the document.</returns>
        public IEnumerable<FoldingRange> GetFoldingRangesFromRoot()
        {
            RuleApplication rootApplication = Context.RootRuleApplication;

            if (rootApplication.IsPositive)
            {
                var result = new List<FoldingRange>();
                rootApplication.AddFoldingRanges(result);
                return result;
            }

            return null;
        }
    }
}
