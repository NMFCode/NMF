using System;

namespace NMF.Serialization
{
    /// <summary>
    /// Denotes the default implementation for an interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited=false)]
    public class XmlDefaultImplementationTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets the default implementation type
        /// </summary>
        public Type DefaultImplementationType { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="implementationType">the default implementation type</param>
        public XmlDefaultImplementationTypeAttribute(Type implementationType)
        {
            DefaultImplementationType = implementationType;
        }
    }
}
