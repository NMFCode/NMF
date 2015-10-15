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
        public BubbledChangeEventArgs(IModelElement source, string propertyName)
        {
            SourceElement = source;
            PropertyName = propertyName;
        }

        /// <summary>
        /// Creates a bubbled change event for the given elementary collection change event
        /// </summary>
        /// <param name="source">The source model element</param>
        /// <param name="propertyName">The name of the property that has changed</param>
        /// <param name="collectionChanged">The original collection change event data</param>
        public BubbledChangeEventArgs(IModelElement source, string propertyName, NotifyCollectionChangedEventArgs collectionChanged)
        {
            SourceElement = source;
            PropertyName = propertyName;
            OriginalEventArgs = collectionChanged;
        }

        public IModelElement SourceElement { get; private set; }

        public string PropertyName { get; private set; }

        public NotifyCollectionChangedEventArgs OriginalEventArgs { get; private set; }

        public bool IsCollectionChangeEvent
        {
            get { return OriginalEventArgs != null; }
        }

        public bool IsValueChangedEvent
        {
            get { return OriginalEventArgs == null; }
        }
    }
}
