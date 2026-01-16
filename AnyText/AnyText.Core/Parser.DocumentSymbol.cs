using System.Collections.Generic;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    public partial class Parser
    {
        /// <summary>
        /// Parses the document symbols starting from the root rule application
        /// </summary>
        /// <returns>An IEnumerable of <see cref="DocumentSymbol"/> objects, each containing details on a document symbol in the document.</returns>
        public IEnumerable<DocumentSymbol> GetDocumentSymbolsFromRoot()
        {
            RuleApplication rootApplication = Context.RootRuleApplication;

            var result = new List<DocumentSymbol>();
            rootApplication.AddDocumentSymbols(Context, result);
            return result;
        }

        /// <summary>
        /// Retrieves document symbols for non-valid documents.
        /// </summary>
        /// <returns>
        /// An IEnumerable of <see cref="DocumentSymbol"/> representing the symbols for non-valid documents.
        /// </returns>
        public IEnumerable<DocumentSymbol> GetDocumentSymbolsForNonValidDocuments()
        {
            RuleApplication rootApplication = Context.RootRuleApplication;

            var result = new List<DocumentSymbol>();
            rootApplication.AddDocumentSymbols(Context, result);
            return result;
            
        }
    }
}
