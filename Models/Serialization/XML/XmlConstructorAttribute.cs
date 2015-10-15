using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Defines an attribute to set the serializer to use a different constructor than the default constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class XmlConstructorAttribute : Attribute
    {
        /// <summary>
        /// Creates a new XmlConstructorAttribute to get the serializer to use a different constructor than the default constructor
        /// </summary>
        /// <param name="parameterCount">The amount of parameters to use</param>
        public XmlConstructorAttribute(int parameterCount)
        {
            ParameterCount = parameterCount;
        }

        /// <summary>
        /// Gets the amount of constructor parameters
        /// </summary>
        public int ParameterCount { get; private set; }

    }
}
