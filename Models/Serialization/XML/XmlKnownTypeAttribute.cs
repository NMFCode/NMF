using System;

namespace NMF.Serialization
{
    /// <summary>
    /// Instructs the serializer to explicitly read the provided type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class XmlKnownTypeAttribute : Attribute
    {
        /// <summary>
        /// The type
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="type">the known type</param>
        public XmlKnownTypeAttribute(Type type) { Type = type; }
    }
}
