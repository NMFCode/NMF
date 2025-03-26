using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.InlayClasses
{
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
        public object? Tooltip { get; set; } // Can be string or MarkupContent

    }
}
