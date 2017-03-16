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
        /// Absolute URI of the model element at the time of the event
        /// </summary>
        public virtual Uri AbsoluteUri
        {
            get
            {
                throw new NotSupportedException("You have to request Uris on change notifications first");
            }
        }

        /// <summary>
        /// If the change introduces new or changed model elements, this list
        /// contains their URIs in the same order as their parent collection.
        /// </summary>
        public virtual List<Uri> ChildrenUris
        {
            get
            {
                throw new NotSupportedException("You have to request Uris on change notifications first");
            }
        }

        public virtual bool HasUris
        {
            get
            {
                return false;
            }
        }

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
        /// <param name="requireUris">Determines whether the event data should include absolute Uris</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs ElementCreated(IModelElement createdElement, bool requireUris)
        {
            if (createdElement == null)
                throw new ArgumentNullException(nameof(createdElement));

            if (requireUris)
            {
                return new UriEnabledBubbledChangeEventArgs(createdElement)
                {
                    ChangeType = ChangeType.ModelElementCreated
                };
            }
            else
            {
                return new BubbledChangeEventArgs(createdElement)
                {
                    ChangeType = ChangeType.ModelElementCreated
                };
            }
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing the upcoming deletion of the given model element.
        /// </summary>
        /// <param name="deletedElement">The deleted model element.</param>
        /// <param name="requireUris">Determines whether the event data should include absolute Uris</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs ElementDeleting(IModelElement deletingElement, Uri originalAbsoluteUri)
        {
            if (deletingElement == null)
                throw new ArgumentNullException(nameof(deletingElement));
            
            return new UriEnabledBubbledChangeEventArgs(deletingElement, originalAbsoluteUri)
            {
                ChangeType = ChangeType.ModelElementDeleting
            };
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing the deletion of the given model element.
        /// </summary>
        /// <param name="deletedElement">The deleted model element.</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs ElementDeleted(IModelElement deletedElement, Uri originalAbsoluteUri)
        {
            if (deletedElement == null)
                throw new ArgumentNullException(nameof(deletedElement));

            return new UriEnabledBubbledChangeEventArgs(deletedElement, originalAbsoluteUri)
            {
                ChangeType = ChangeType.ModelElementDeleted
            };
        }

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

            if (requireUris)
            {
                return new UriEnabledBubbledChangeEventArgs(source, feature?.Value)
                {
                    ChangeType = ChangeType.PropertyChanging,
                    OriginalEventArgs = eventArgs,
                    PropertyName = propertyName
                };
            }
            else
            {
                return new BubbledChangeEventArgs(source)
                {
                    ChangeType = ChangeType.PropertyChanging,
                    OriginalEventArgs = eventArgs,
                    PropertyName = propertyName
                };
            }
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

            if (requireUris)
            {
                var newValueUri = (args.NewValue as IModelElement)?.AbsoluteUri;

                return new UriEnabledBubbledChangeEventArgs(source, newValueUri != null ? new List<Uri>() {  newValueUri } : null, feature?.Value)
                {
                    ChangeType = ChangeType.PropertyChanged,
                    OriginalEventArgs = args,
                    PropertyName = propertyName
                };
            }
            else
            {
                return new BubbledChangeEventArgs(source)
                {
                    ChangeType = ChangeType.PropertyChanged,
                    OriginalEventArgs = args,
                    PropertyName = propertyName
                };
            }
        }

        /// <summary>
        /// Creates an instance of BubbledChangeEventArgs describing an upcoming change in a collection.
        /// </summary>
        /// <param name="source">The model element containing the collection.</param>
        /// <param name="propertyName">The name of the collection property.</param>
        /// <param name="args">The original NotifyCollectionChangingEventArgs.</param>
        /// <param name="requireUris">Determines whether the event data should include absolute Uris</param>
        /// <returns></returns>
        public static BubbledChangeEventArgs CollectionChanging(IModelElement source, string propertyName, NotifyCollectionChangingEventArgs args, bool requireUris, Lazy<ITypedElement> feature = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (requireUris)
            {
                return new UriEnabledBubbledChangeEventArgs(source, feature?.Value)
                {
                    ChangeType = ChangeType.CollectionChanging,
                    OriginalEventArgs = args,
                    PropertyName = propertyName
                };
            }
            else
            {
                return new BubbledChangeEventArgs(source)
                {
                    ChangeType = ChangeType.CollectionChanging,
                    OriginalEventArgs = args,
                    PropertyName = propertyName
                };
            }
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

            if (requireUris)
            {
                List<Uri> childrenUris = null;
                if (args.Action == NotifyCollectionChangedAction.Add)
                    childrenUris = args.NewItems.OfType<IModelElement>().Select(e => e.AbsoluteUri).ToList();

                return new UriEnabledBubbledChangeEventArgs(source, childrenUris, feature?.Value)
                {
                    ChangeType = ChangeType.CollectionChanged,
                    OriginalEventArgs = args,
                    PropertyName = propertyName
                };
            }
            else
            {
                return new BubbledChangeEventArgs(source)
                {
                    ChangeType = ChangeType.CollectionChanged,
                    OriginalEventArgs = args,
                    PropertyName = propertyName
                };
            }
        }
    }

    [DebuggerDisplay("BubbledChange: {ChangeType} in {Element} ({AbsoluteUri})")]
    public class UriEnabledBubbledChangeEventArgs : BubbledChangeEventArgs
    {
        private Uri absoluteUri;
        private List<Uri> childrenUris;

        public UriEnabledBubbledChangeEventArgs(IModelElement element, ITypedElement feature = null) : this(element, element.AbsoluteUri) { }

        public UriEnabledBubbledChangeEventArgs(IModelElement element, List<Uri> childrenUris, ITypedElement feature = null) : this(element)
        {
            this.childrenUris = childrenUris;
        }

        public UriEnabledBubbledChangeEventArgs(IModelElement element, Uri absoluteUri, ITypedElement feature = null) : base(element, feature)
        {
            this.absoluteUri = absoluteUri;
        }

        public override bool HasUris
        {
            get
            {
                return true;
            }
        }

        public override Uri AbsoluteUri
        {
            get
            {
                return absoluteUri;
            }
        }

        public override List<Uri> ChildrenUris
        {
            get
            {
                return childrenUris;
            }
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
