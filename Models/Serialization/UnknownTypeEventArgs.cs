using System;

namespace NMF.Serialization
{
    /// <summary>
    /// Event args for the event that the serializer encounters an unknown type
    /// </summary>
    public class UnknownTypeEventArgs : EventArgs
    {
        /// <summary>
        /// The property for which the type is needed
        /// </summary>
        public IPropertySerializationInfo Property { get; }

        /// <summary>
        /// The namespace of the element
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// The local name of the element
        /// </summary>
        public string LocalName { get; }

        /// <summary>
        /// Gets or sets the resolved type
        /// </summary>
        public ITypeSerializationInfo Type { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="property">The property for which the type is needed</param>
        /// <param name="ns">The namespace of the element</param>
        /// <param name="localName">The local name of the element</param>
        /// <exception cref="ArgumentNullException">Thrown if local name is null</exception>
        public UnknownTypeEventArgs(IPropertySerializationInfo property, string ns, string localName)
        {
            if (localName == null) throw new ArgumentNullException(nameof(localName));

            Namespace = ns;
            LocalName = localName;
            Property = property;
        }
    }
}
