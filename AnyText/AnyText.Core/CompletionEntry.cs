using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Represents an entry for code completion.
    /// </summary>
    public readonly record struct CompletionEntry(string Completion, SymbolKind Kind, ParsePosition StartPosition);

}
