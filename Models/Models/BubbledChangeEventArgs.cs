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
        /// <returns></returns>
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
        /// <param name="args">The original ValueChangedEventArgs.</param>
        /// <param name="requireUris">Determines whether the event data should include absolute Uris</param>
        /// <returns></returns>
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
        /// <param name="args">The original NotifyCollectionChangedEventArgs.</param>
        /// <param name="requireUris">Determines whether the event data should include absolute Uris</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing a change in a collection.
        /// </summary>
        /// <param name="source">The model element containing the collection.</param>
        /// <param name="propertyName">The name of the collection property.</param>
        /// <param name="args">The original NotifyCollectionChangedEventArgs.</param>
        /// <param name="requireUris">Determies whether the event data should obtain the absolute Uris</param>
        /// <returns></returns>
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

        public static BubbledChangeEventArgs ElementDeleted(ModelElement source, UriChangedEventArgs e)
        {
            return new BubbledChangeEventArgs(source)
            {
                ChangeType = ChangeType.ElementDeleted,
                OriginalEventArgs = e
            };
        }

        public static BubbledChangeEventArgs ElementCreated(IModelElement child, UriChangedEventArgs e)
        {
            return new BubbledChangeEventArgs(child)
            {
                ChangeType = ChangeType.ElementCreated,
                OriginalEventArgs = e
            };
        }

        public static BubbledChangeEventArgs UriChanged(ModelElement modelElement, UriChangedEventArgs e)
        {
            return new BubbledChangeEventArgs(modelElement)
            {
                ChangeType = ChangeType.UriChanged,
                OriginalEventArgs = e
            };
        }
    }

    public class UriChangedEventArgs : EventArgs
    {
        public UriChangedEventArgs(Uri oldUri)
        {
            OldUri = oldUri;
        }

        public Uri OldUri { get; private set; }
    }

    /// <summary>
    /// Describes what kind of change a BubbledChangeEvent wraps.
    /// </summary>
    public enum ChangeType
    {
        PropertyChanging,
        PropertyChanged,
        CollectionChanging,
        CollectionChanged,
        ElementDeleted,
        ElementCreated,
        UriChanged
    }
}
