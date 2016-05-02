using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// Describes that an elementary change in the model elements containment hierarchy has happened
    /// </summary>
    public class BubbledChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a bubbled change event for the given elementary model change event
        /// </summary>
        /// <param name="source">The source model element</param>
        /// <param name="propertyName">The property that has been changed</param>
        public BubbledChangeEventArgs(IModelElement source, string propertyName, ValueChangedEventArgs valueChangeEvent)
        {
            Element = source;
            PropertyName = propertyName;
        }

        /// <summary>
        /// Creates a bubbled change event for the given elementary collection change event
        /// </summary>
        /// <param name="source">The source model element</param>
        /// <param name="propertyName">The name of the property that has changed</param>
        /// <param name="collectionChangedEvent">The original collection change event data</param>
        public BubbledChangeEventArgs(IModelElement source, string propertyName, NotifyCollectionChangedEventArgs collectionChangedEvent)
        {
            Element = source;
            PropertyName = propertyName;
            OriginalEventArgs = collectionChangedEvent;
        }

        /// <summary>
        /// Creates a bubbled change event for the given elementary item creation event
        /// </summary>
        /// <param name="newElement">The model element that has been created</param>
        public BubbledChangeEventArgs(IModelElement newElement)
        {
            Element = newElement;
        }

        /// <summary>
        /// The original model element directly affected by this change
        /// </summary>
        public IModelElement Element { get; private set; }

        /// <summary>
        /// The name of the affected property or null, if no specific property was affected
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The original event arguments
        /// </summary>
        public EventArgs OriginalEventArgs { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the underlying change has been a elementary collection change
        /// </summary>
        public bool IsCollectionChangeEvent
        {
            get { return OriginalEventArgs is NotifyCollectionChangedEventArgs; }
        }

        /// <summary>
        /// Gets a value indicating whether the underlying change was a changed property value
        /// </summary>
        public bool IsPropertyChangedEvent
        {
            get { return OriginalEventArgs is ValueChangedEventArgs; }
        }
    }
}
