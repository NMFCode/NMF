using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NMF.AnyText.Rules;
using System.Diagnostics;
using NMF.AnyText.Model;
using System.Text.RegularExpressions;

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

            if (rootApplication.IsPositive)
            {
                var result = new List<DocumentSymbol>();
                rootApplication.AddDocumentSymbols(Context, result);
                return result;
            }

            return null;
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
