using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Attribute to provide the Xml-namespace prefix to use in Xml-serialization for the specified element
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public sealed class XmlNamespacePrefixAttribute : Attribute
    {
        private readonly string nameSpacePrefix;

        /// <summary>
        /// Creates a XmlNamespacePrefixAttribute
        /// </summary>
        /// <param name="nameSpacePrefix">Xml-namespace prefix to use in serialization</param>
        public XmlNamespacePrefixAttribute(string nameSpacePrefix)
        {
            this.nameSpacePrefix = nameSpacePrefix;
        }

        /// <summary>
        /// The Xml-namespace prefix to use in serialization in Xml
        /// </summary>
        public string NamespacePrefix
        {
            get { return nameSpacePrefix; }
        }
    }
}
