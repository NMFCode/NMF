using NMF.AnyText.Rules;
using System.Collections.Generic;
using System.Linq;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Parses the selection range for given positions
        /// </summary>
        /// <param name="positions">The positions in the document</param>
        /// <returns>An IEnumerable of <see cref="SelectionRange"/> objects, each containing details on a selection range in the document.</returns>
        public IEnumerable<SelectionRange> GetSelectionRanges(IEnumerable<ParsePosition> positions)
        {
            return positions.Select(position => GetSelectionRange(position));
        }

        private SelectionRange GetSelectionRange(ParsePosition position)
        {
            var ruleApplication = Context.RootRuleApplication.GetLiteralAt(position);
            
            return GetSelectionRange(ruleApplication);
        }

        private SelectionRange GetSelectionRange(RuleApplication ruleApplication)
        {
            if (ruleApplication == null) return null;

            while (ruleApplication.Parent != null && ruleApplication.CurrentPosition == ruleApplication.Parent.CurrentPosition)
            {
                ruleApplication = ruleApplication.Parent;
            }

            return new SelectionRange()
            {
                Range = new ParseRange(ruleApplication.CurrentPosition, ruleApplication.CurrentPosition + ruleApplication.Length),
                Parent = GetSelectionRange(ruleApplication.Parent)
            };
        }
    }
}
