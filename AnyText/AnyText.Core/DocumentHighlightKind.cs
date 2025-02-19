using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Document highlight kinds used for document highlights requests
    /// </summary>
    public enum DocumentHighlightKind
    {
        /// <summary>
        /// A textual occurrence.
        /// </summary>
        Text = 1,

        /// <summary>
        /// Read-access of a symbol, like reading a variable.
        /// </summary>
        Read = 2,

        /// <summary>
        /// Write-access of a symbol, like writing to a variable.
        /// </summary>
        Write = 3,
    }
}
