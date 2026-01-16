using LspTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.InlayClasses
{
    /// <summary>
    /// Denotes the server options for inlay support
    /// </summary>
    public class InlayHintOptions : WorkDoneProgressOptions
    {
        /// <summary>
        /// The server provides support to resolve additional information for an inlay hint item.
        /// </summary>
        public bool? ResolveProvider { get; set; }
    }
}
