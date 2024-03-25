using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Specifies that the given different property is an opposite
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class XmlOppositeAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="oppositeProperty">the name of the opposite property</param>
        public XmlOppositeAttribute(string oppositeProperty)
        {
            OppositeType = null;
            OppositeProperty = oppositeProperty;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="oppositeProperty">the name of the opposite property</param>
        /// <param name="oppositeType">the opposite type</param>
        public XmlOppositeAttribute(Type oppositeType, string oppositeProperty)
        {
            OppositeType = oppositeType;
            OppositeProperty = oppositeProperty;
        }

        /// <summary>
        /// Gets the opposite type
        /// </summary>
        public Type OppositeType { get; private set; }

        /// <summary>
        /// Gets the name of the opposite property
        /// </summary>
        public string OppositeProperty { get; private set; }
    }
}
