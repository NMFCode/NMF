using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// A document highlight is a range inside a text document which deserves
    /// special attention. Usually a document highlight is visualized by changing
    /// the background color of its range.
    /// Analogous to the LspTypes DocumentHighlight interface.
    /// </summary>
    public class DocumentHighlight
    {
        /// <summary>
        /// The range this highlight applies to.
        /// </summary>
        public ParseRange Range { get; set; }

        /// <summary>
        /// The highlight kind, default is DocumentHighlightKind.Text.
        /// </summary>
        public DocumentHighlightKind Kind { get; set; }
    }
}
