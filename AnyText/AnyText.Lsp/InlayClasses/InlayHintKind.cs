using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.InlayClasses
{
    /// <summary>
    /// Inlay hint kinds.
    /// </summary>
    public enum InlayHintKind
    {
        /// <summary>
        /// An inlay hint for a type annotation.
        /// </summary>
        Type = 1,

        /// <summary>
        /// An inlay hint for a parameter.
        /// </summary>
        Parameter = 2
    }
}
