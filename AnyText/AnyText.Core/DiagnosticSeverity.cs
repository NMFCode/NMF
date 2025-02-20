using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes the severity of a diagnostic item
    /// </summary>
    public enum DiagnosticSeverity
    {
        /// <summary>
        /// Denotes no actual severity
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates an error
        /// </summary>
        Error = 1,
        /// <summary>
        /// Indicates a warning
        /// </summary>
        Warning,
        /// <summary>
        /// Indicates an informational message
        /// </summary>
        Information,
        /// <summary>
        /// Indicates a hint
        /// </summary>
        Hint
    }
}
