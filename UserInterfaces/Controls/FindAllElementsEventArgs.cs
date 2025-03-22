using NMF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NMF.Controls
{
    /// <summary>
    /// Denotes the event data when all elements should be obtained
    /// </summary>
    public class FindAllElementsEventArgs : EventArgs
    {
        /// <summary>
        /// The type of elements
        /// </summary>
        public Type ElementType { get; private set; }

        /// <summary>
        /// The instance for which elements are required
        /// </summary>
        public IModelElement Instance { get; private set; }

        /// <summary>
        /// The property for which the elements should be added
        /// </summary>
        public PropertyDescriptor Property { get; private set; }

        /// <summary>
        /// Gets or sets the allowable elements
        /// </summary>
        public IEnumerable<IModelElement> AllowableElements { get; set; }

        /// <summary>
        /// Creates a new event data object
        /// </summary>
        /// <param name="instance">the instance</param>
        /// <param name="property">the property</param>
        /// <param name="elementType">the element type</param>
        /// <param name="elements">the elements</param>
        public FindAllElementsEventArgs(IModelElement instance, PropertyDescriptor property, Type elementType, IEnumerable<IModelElement> elements)
        {
            ElementType = elementType;
            Instance = instance;
            Property = property;
            AllowableElements = elements;
        }
    }
}
