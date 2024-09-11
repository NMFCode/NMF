using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Denotes that a class may also occur under a different namespace
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
    public class AlternativeNamespaceAttribute : Attribute
    {
        /// <summary>
        /// Gets the alternative namespace URI
        /// </summary>
        public string AlternativeNamespace { get; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="alternativeNamespace">the alternative namespace URI</param>
        public AlternativeNamespaceAttribute(string alternativeNamespace)
        {
            AlternativeNamespace = alternativeNamespace;
        }
    }
}
