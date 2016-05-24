using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using NMF.Collections.ObjectModel;

namespace NMF.Models
{
    /// <summary>
    /// Describes that an elementary change in the model elements containment hierarchy has happened
    /// </summary>
    public class BubbledChangeEventArgs : EventArgs
    {
        private BubbledChangeEventArgs() { }

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
        /// Gets the type of change that occured. This defines the type of OriginalEventArgs
        /// and whether PropertyName is used.
        /// </summary>
        public ChangeType ChangeType { get; private set; }

        public override string ToString()
        {
            return "BubbledChange: " + ChangeType + " in " + Element;
        }

        /// <summary>
        /// Gets a value indicating whether the underlying change has been a elementary collection change
        /// </summary>
        [Obsolete("Use ChangeType instead.")]
        public bool IsCollectionChangeEvent
        {
            get { return ChangeType == ChangeType.CollectionChanged; }
        }

        /// <summary>
        /// Gets a value indicating whether the underlying change was a changed property value
        /// </summary>
        [Obsolete("Use ChangeType instead.")]
        public bool IsPropertyChangedEvent
        {
            get { return ChangeType == ChangeType.PropertyChanged; }
        }

        /// <summary>
        /// Gets a value indicating whether the change was that a new element was created
        /// </summary>
        [Obsolete("Use ChangeType instead.")]
        public bool IsElementCreated
        {
            get { return ChangeType == ChangeType.ModelElementCreated; }
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing the creation of the given model element.
        /// </summary>
        /// <param name="createdElement">The new model element.</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs ElementCreated(IModelElement createdElement)
        {
            if (createdElement == null)
                throw new ArgumentNullException(nameof(createdElement));

            return new BubbledChangeEventArgs
            {
                ChangeType = ChangeType.ModelElementCreated,
                Element = createdElement
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing the upcoming deletion of the given model element.
        /// </summary>
        /// <param name="deletedElement">The deleted model element.</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs ElementDeleting(IModelElement deletingElement)
        {
            if (deletingElement == null)
                throw new ArgumentNullException(nameof(deletingElement));

            return new BubbledChangeEventArgs
            {
                ChangeType = ChangeType.ModelElementDeleting,
                Element = deletingElement
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing the deletion of the given model element.
        /// </summary>
        /// <param name="deletedElement">The deleted model element.</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs ElementDeleted(IModelElement deletedElement)
        {
            if (deletedElement == null)
                throw new ArgumentNullException(nameof(deletedElement));

            return new BubbledChangeEventArgs
            {
                ChangeType = ChangeType.ModelElementDeleted,
                Element = deletedElement
            };
        }

        /// <summary>
        /// Create an instance of BubbledChangeEventArgs describing an upcoming change of a property value.
        /// </summary>
        /// <param name="source">The model element containing the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs PropertyChanging(IModelElement source, string propertyName)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            return new BubbledChangeEventArgs
            {
                ChangeType = ChangeType.PropertyChanging,
                Element = source,
                PropertyName = propertyName
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing a change of a property value.
        /// </summary>
        /// <param name="source">The model element containing the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="args">The original ValueChangedEventArgs.</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs PropertyChanged(IModelElement source, string propertyName, ValueChangedEventArgs args)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            return new BubbledChangeEventArgs
            {
                ChangeType = ChangeType.PropertyChanged,
                Element = source,
                OriginalEventArgs = args,
                PropertyName = propertyName
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing an upcoming change in a collection.
        /// </summary>
        /// <param name="source">The model element containing the collection.</param>
        /// <param name="propertyName">The name of the collection property.</param>
        /// <param name="args">The original NotifyCollectionChangingEventArgs.</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs CollectionChanging(IModelElement source, string propertyName, NotifyCollectionChangingEventArgs args)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            return new BubbledChangeEventArgs
            {
                ChangeType = ChangeType.CollectionChanging,
                Element = source,
                OriginalEventArgs = args,
                PropertyName = propertyName
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing a change in a collection.
        /// </summary>
        /// <param name="source">The model element containing the collection.</param>
        /// <param name="propertyName">The name of the collection property.</param>
        /// <param name="args">The original NotifyCollectionChangedEventArgs.</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs CollectionChanged(IModelElement source, string propertyName, NotifyCollectionChangedEventArgs args)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            return new BubbledChangeEventArgs
            {
                ChangeType = ChangeType.CollectionChanged,
                Element = source,
                OriginalEventArgs = args,
                PropertyName = propertyName
            };
        }
    }

    /// <summary>
    /// Describes what kind of change a BubbledChangeEvent wraps.
    /// </summary>
    public enum ChangeType
    {
        ModelElementCreated,
        ModelElementDeleting,
        ModelElementDeleted,
        PropertyChanging,
        PropertyChanged,
        CollectionChanging,
        CollectionChanged
    }
}
