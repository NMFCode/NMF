using NMF.AnyText.Rules;
using System.Collections.Generic;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Parses the document symbols starting from the root rule application
        /// </summary>
        /// <returns>An IEnumerable of <see cref="DocumentSymbol"/> objects, each containing details on a document symbol in the document.</returns>
        public IEnumerable<InlayEntry> GetInlayEntriesInRange(ParseRange range)
        {

            var inlayEntries = new List<InlayEntry>();

            RuleApplication rootApplication = Context.RootRuleApplication;
            rootApplication.AddInlayEntries(range, inlayEntries, Context);

            return inlayEntries;

        }

    }
}
