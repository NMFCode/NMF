using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    [global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class XmlIdentifierAttribute : Attribute
    {
        readonly string identifier;

        /// <summary>
        /// Creates a new XmlIdentifierAttribute with the given identifier
        /// </summary>
        /// <param name="identifier">The property to identify instances for this class</param>
        public XmlIdentifierAttribute(string identifier)
        {
            this.identifier = identifier;
        }

        /// <summary>
        /// Gets the identifier to identify the instances of this class
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }
    }
}
