using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// Declares that the given type represents a type from the metamodel
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum)]
    public class ModelRepresentationClassAttribute : Attribute
    {
        /// <summary>
        /// Declares that the given type represents a type from the metamodel
        /// </summary>
        /// <param name="uriString">The URI of the type represented</param>
        public ModelRepresentationClassAttribute(string uriString)
        {
            UriString = uriString;
        }

        /// <summary>
        /// Gets the URI of the type represented by this class
        /// </summary>
        public string UriString { get; private set; }
    }
}
