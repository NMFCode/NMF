using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.Transformation
{
    /// <summary>
    /// Denotes settings for code generation in Metamodel
    /// </summary>
    public class CodeGeneratorSettings
    {
        private static List<string> _defaultIdentifierNames = new List<string> { "name", "id" };

        /// <summary>
        /// Gets the default collection of identifier names
        /// </summary>
        public static readonly IEnumerable<string> DefaultIdentifierNames = _defaultIdentifierNames.AsReadOnly();

        /// <summary>
        /// Gets or sets the namespace in which the code should be generated
        /// </summary>
        public string Namespace { get; set; } = "Generated";

        /// <summary>
        /// Gets a collection of imported namespaces
        /// </summary>
        public List<string> ImportedNamespaces { get; } = new List<string>
        {
            "NMF.AnyText.Model",
            "NMF.AnyText.Rules",
            "NMF.AnyText.PrettyPrinting",
            "System",
            "System.Collections.Generic",
            "System.Text.RegularExpressions"
        };

        /// <summary>
        /// Gets the names used as identifiers
        /// </summary>
        public IEnumerable<string> IdentifierNames { get; set; } = DefaultIdentifierNames;
    }
}
