using LspTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.InlayClasses
{
    /// <summary>
    /// Denotes an
    /// </summary>
    public class InlayHintLabelPart
    {
        /// <summary>
        /// The value of this label part.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// The tooltip text when you hover over this label part.
        /// Depending on the client capability `inlayHint.resolveSupport` clients might resolve
        /// this property late using the resolve request.
        /// </summary>
        public SumType<string, MarkupContent> Tooltip { get; set; }

    }
}
