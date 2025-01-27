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
        public IEnumerable<DocumentSymbol> GetDocumentSymbolsFromRoot()
        {
            RuleApplication rootApplication = Context.RootRuleApplication;

            if (rootApplication.IsPositive)
            {
                var result = new List<DocumentSymbol>();
                rootApplication.AddDocumentSymbols(Context, result);
                return result;
            }

            return Enumerable.Empty<DocumentSymbol>();
        }
    }
}
