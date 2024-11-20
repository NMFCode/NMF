using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes default parser error sources
    /// </summary>
    public static class ParseErrorSources
    {
        /// <summary>
        /// Denotes that an error occured while parsing
        /// </summary>
        public const string Parser = nameof(Parser);

        /// <summary>
        /// Denotes that an error occured while resolving references
        /// </summary>
        public const string ResolveReferences = nameof(ResolveReferences);

        /// <summary>
        /// Denotes that there is an error in the grammar
        /// </summary>
        public const string Grammar = nameof(Grammar);
    }
}
