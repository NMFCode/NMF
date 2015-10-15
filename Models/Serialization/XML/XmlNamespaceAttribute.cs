using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Attribute to provide the Xml-namespace to use in Xml-serialization for the specified element
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public sealed class XmlNamespaceAttribute : Attribute
    {
        private readonly string nameSpace;

        /// <summary>
        /// Creates a XmlNamespaceAttribute
        /// </summary>
        /// <param name="nameSpace">Xml-namespace to use in serialization</param>
        public XmlNamespaceAttribute(string nameSpace)
        {
            this.nameSpace = nameSpace;
        }

        /// <summary>
        /// The Xml-namespace to use in serialization in Xml
        /// </summary>
        public string Namespace
        {
            get { return nameSpace; }
        }
    }
}
