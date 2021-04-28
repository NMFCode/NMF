using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using NMF.Collections.ObjectModel;
using System.Diagnostics;
using NMF.Models.Meta;

namespace NMF.Models
{
    /// <summary>
    /// Describes that an elementary change in the model elements containment hierarchy has happened
    /// </summary>
    [DebuggerDisplay("BubbledChange: {ChangeType} in {Element}")]
    public class BubbledChangeEventArgs : EventArgs
    {
        internal BubbledChangeEventArgs(IModelElement element, ITypedElement feature = null)
        {
            Element = element;
            Feature = feature;
        }

        /// <summary>
        /// The original model element directly affected by this change
        /// </summary>
        public IModelElement Element { get; private set; }

        /// <summary>
        /// Gets the feature that was affected from the change or null, if not applicable or could not be loaded
        /// </summary>
        public ITypedElement Feature { get; private set; }

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
        

        /// <summary>
        /// Create an instance of BubbledChangeEventArgs describing an upcoming change of a property value.
        /// </summary>
        /// <param name="source">The model element containing the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="requireUris">Determines whether the event data should include absolute Uris</param>
        /// <param name="eventArgs">The event data of the original event</param>
        /// <param name="feature">The affected feature</param>
        /// <returns>The complete event data</returns>
        public static BubbledChangeEventArgs PropertyChanging(IModelElement source, string propertyName, ValueChangedEventArgs eventArgs, bool requireUris, Lazy<ITypedElement> feature = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var featureRef = requireUris ? feature?.Value : null;
            return new BubbledChangeEventArgs(source, featureRef)
            {
                ChangeType = ChangeType.PropertyChanging,
                OriginalEventArgs = eventArgs,
                PropertyName = propertyName
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing a change of a property value.
        /// </summary>
        /// <param name="source">The model element containing the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="requireUris">Determines whether the event data should include absolute Uris</param>
        /// <param name="args">The event data of the original event</param>
        /// <param name="feature">The affected feature</param>
        /// <returns>The complete event data</returns>
        public static BubbledChangeEventArgs PropertyChanged(IModelElement source, string propertyName, ValueChangedEventArgs args, bool requireUris, Lazy<ITypedElement> feature = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (args == null)
                throw new ArgumentNullException(nameof(args));


            var featureRef = requireUris ? feature?.Value : null;
            return new BubbledChangeEventArgs(source, featureRef)
            {
                ChangeType = ChangeType.PropertyChanged,
                OriginalEventArgs = args,
                PropertyName = propertyName
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing an upcoming change in a collection.
        /// </summary>
        /// <param name="source">The model element containing the collection.</param>
        /// <param name="propertyName">The name of the collection property.</param>
        /// <param name="requireUris">Determines whether the event data should include absolute Uris</param>
        /// <param name="args">The event data of the original event</param>
        /// <param name="feature">The affected feature</param>
        /// <returns>The complete event data</returns>
        public static BubbledChangeEventArgs CollectionChanging(IModelElement source, string propertyName, NotifyCollectionChangedEventArgs args, bool requireUris, Lazy<ITypedElement> feature = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var featureRef = requireUris ? feature?.Value : null;
            return new BubbledChangeEventArgs(source, featureRef)
            {
                ChangeType = ChangeType.CollectionChanging,
                OriginalEventArgs = args,
                PropertyName = propertyName
            };
        }

        internal static BubbledChangeEventArgs Unlock(ModelElement modelElement, UnlockEventArgs unlockEventArgs)
        {
            return new BubbledChangeEventArgs(modelElement)
            {
                ChangeType = ChangeType.UnlockRequest,
                OriginalEventArgs = unlockEventArgs
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing a change in a collection.
        /// </summary>
        /// <param name="source">The model element containing the collection.</param>
        /// <param name="propertyName">The name of the collection property.</param>
        /// <param name="requireUris">Determies whether the event data should obtain the absolute Uris</param>
        /// <param name="args">The event data of the original event</param>
        /// <param name="feature">The affected feature</param>
        /// <returns>The complete event data</returns>
        public static BubbledChangeEventArgs CollectionChanged(IModelElement source, string propertyName, NotifyCollectionChangedEventArgs args, bool requireUris, Lazy<ITypedElement> feature = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var featureRef = requireUris ? feature?.Value : null;
            return new BubbledChangeEventArgs(source, featureRef)
            {
                ChangeType = ChangeType.CollectionChanged,
                OriginalEventArgs = args,
                PropertyName = propertyName
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing that an operation is being called
        /// </summary>
        /// <param name="source">The model element that is the target for the call</param>
        /// <param name="operation">The operation that is called</param>
        /// <param name="args">The event arguments, including parameters of the call</param>
        public static BubbledChangeEventArgs OperationCalling(IModelElement source, IOperation operation, OperationCallEventArgs args)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new BubbledChangeEventArgs(source, operation)
            {
                ChangeType = ChangeType.OperationCalling,
                OriginalEventArgs = args
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing that an operation is being called
        /// </summary>
        /// <param name="source">The model element that is the target for the call</param>
        /// <param name="operation">The operation that is called</param>
        /// <param name="args">The event arguments, including parameters of the call</param>
        public static BubbledChangeEventArgs OperationCalled(IModelElement source, IOperation operation, OperationCallEventArgs args)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new BubbledChangeEventArgs(source, operation)
            {
                ChangeType = ChangeType.OperationCalled,
                OriginalEventArgs = args
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing that an element in the tree has been deleted
        /// </summary>
        /// <param name="source">The element that has been deleted</param>
        /// <param name="e">The original event data</param>
        /// <returns>A BubbledChange event data container</returns>
        public static BubbledChangeEventArgs ElementDeleted(ModelElement source, UriChangedEventArgs e)
        {
            return new BubbledChangeEventArgs(source)
            {
                ChangeType = ChangeType.ElementDeleted,
                OriginalEventArgs = e
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing that an element in the tree has been created
        /// </summary>
        /// <param name="child">The child that has been created</param>
        /// <param name="e">The original event data</param>
        /// <returns>A BubbledChange event data container</returns>
        public static BubbledChangeEventArgs ElementCreated(IModelElement child, UriChangedEventArgs e)
        {
            return new BubbledChangeEventArgs(child)
            {
                ChangeType = ChangeType.ElementCreated,
                OriginalEventArgs = e
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing that the Uri of an element has changed
        /// </summary>
        /// <param name="modelElement">The model element whose uri has changed</param>
        /// <param name="e">The original event data</param>
        /// <returns>A BubbledChange event data container</returns>
        public static BubbledChangeEventArgs UriChanged(ModelElement modelElement, UriChangedEventArgs e)
        {
            return new BubbledChangeEventArgs(modelElement)
            {
                ChangeType = ChangeType.UriChanged,
                OriginalEventArgs = e
            };
        }
    }

    /// <summary>
    /// Denotes event data that the uri of a model element has changed
    /// </summary>
    public class UriChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="oldUri">The old uri</param>
        public UriChangedEventArgs(Uri oldUri)
        {
            OldUri = oldUri;
        }

        /// <summary>
        /// Gets the old uri of the model element
        /// </summary>
        public Uri OldUri { get; private set; }
    }

    /// <summary>
    /// Describes what kind of change a BubbledChangeEvent wraps.
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// Denotes that the value of a property is about to change
        /// </summary>
        PropertyChanging,
        /// <summary>
        /// Denotes that the value of a property has been changed
        /// </summary>
        PropertyChanged,
        /// <summary>
        /// Denotes that a collection is about to change
        /// </summary>
        CollectionChanging,
        /// <summary>
        /// Denotes that a collection has changed
        /// </summary>
        CollectionChanged,
        /// <summary>
        /// Denotes that an element has been deleted
        /// </summary>
        ElementDeleted,
        /// <summary>
        /// Denotes that an element has been created
        /// </summary>
        ElementCreated,
        /// <summary>
        /// Denotes that the uri of a model element has changed
        /// </summary>
        UriChanged,
        /// <summary>
        /// Denotes that an operation is about to be called
        /// </summary>
        OperationCalling,
        /// <summary>
        /// Denotes that an operation has been called
        /// </summary>
        OperationCalled,
        /// <summary>
        /// Denotes a request to unlock the model element
        /// </summary>
        UnlockRequest
    }
}
