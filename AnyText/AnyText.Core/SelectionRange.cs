using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// A selection range around a cursor position that the user might be interested in selecting.
    /// Analogous to LspTypes SelectionRange.
    /// </summary>
    public class SelectionRange
    {
        /// <summary>
        /// The range of this selection range
        /// </summary>
        public ParseRange Range {  get; set; }

        /// <summary>
        /// The parent selection range containing this selection range
        /// </summary>
        public SelectionRange Parent { get; set; }
    }
}
