using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public class CompletionEntry
    {
        public string Completion { get; }
        public SymbolKind Kind { get; }

        public CompletionEntry(string completion, SymbolKind kind)
        {
            Completion = completion;
            Kind = kind;
        }
    }
}
