//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Models.Collections;
using NMF.Models.Expressions;
using NMF.Models.Meta;
using NMF.Models.Repository;
using NMF.Serialization;
using NMF.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;


namespace NMF.Models.Meta
{
    
    
    /// <summary>
    /// The default implementation of the AnnotationEntry class
    /// </summary>
    [XmlNamespaceAttribute("http://nmf.codeplex.com/nmeta/")]
    [XmlNamespacePrefixAttribute("nmeta")]
    [ModelRepresentationClassAttribute("http://nmf.codeplex.com/nmeta/#//AnnotationEntry")]
    public partial class AnnotationEntry : ModelElement, NMF.Models.Meta.IAnnotationEntry, NMF.Models.IModelElement
    {
        
        /// <summary>
        /// The backing field for the Source property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private string _source;
        
        private static Lazy<ITypedElement> _sourceAttribute = new Lazy<ITypedElement>(RetrieveSourceAttribute);
        
        /// <summary>
        /// The backing field for the Details property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableList<string> _details;
        
        private static Lazy<ITypedElement> _detailsAttribute = new Lazy<ITypedElement>(RetrieveDetailsAttribute);
        
        private static Lazy<ITypedElement> _annotationsReference = new Lazy<ITypedElement>(RetrieveAnnotationsReference);
        
        /// <summary>
        /// The backing field for the Annotations property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<NMF.Models.Meta.IAnnotationEntry> _annotations;
        
        private static IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public AnnotationEntry()
        {
            this._details = new ObservableList<string>();
            this._details.CollectionChanging += this.DetailsCollectionChanging;
            this._details.CollectionChanged += this.DetailsCollectionChanged;
            this._annotations = new ObservableCompositionOrderedSet<NMF.Models.Meta.IAnnotationEntry>(this);
            this._annotations.CollectionChanging += this.AnnotationsCollectionChanging;
            this._annotations.CollectionChanged += this.AnnotationsCollectionChanged;
        }
        
        /// <summary>
        /// The Source property
        /// </summary>
        [CategoryAttribute("AnnotationEntry")]
        [XmlAttributeAttribute(true)]
        public string Source
        {
            get
            {
                return this._source;
            }
            set
            {
                if ((this._source != value))
                {
                    string old = this._source;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnSourceChanging(e);
                    this.OnPropertyChanging("Source", e, _sourceAttribute);
                    this._source = value;
                    this.OnSourceChanged(e);
                    this.OnPropertyChanged("Source", e, _sourceAttribute);
                }
            }
        }
        
        /// <summary>
        /// The Details property
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [CategoryAttribute("AnnotationEntry")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public IListExpression<string> Details
        {
            get
            {
                return this._details;
            }
        }
        
        /// <summary>
        /// The Annotations property
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<NMF.Models.Meta.IAnnotationEntry> Annotations
        {
            get
            {
                return this._annotations;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<NMF.Models.IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new AnnotationEntryChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<NMF.Models.IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new AnnotationEntryReferencedElementsCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//AnnotationEntry")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets fired when the Source property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> SourceChanged;
        
        /// <summary>
        /// Gets fired before the Source property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> SourceChanging;
        
        private static ITypedElement RetrieveSourceAttribute()
        {
            return ((ITypedElement)(((NMF.Models.ModelElement)(NMF.Models.Meta.AnnotationEntry.ClassInstance)).Resolve("Source")));
        }
        
        /// <summary>
        /// Raises the SourceChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSourceChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.SourceChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the SourceChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSourceChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.SourceChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        private static ITypedElement RetrieveDetailsAttribute()
        {
            return ((ITypedElement)(((NMF.Models.ModelElement)(NMF.Models.Meta.AnnotationEntry.ClassInstance)).Resolve("Details")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Details property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void DetailsCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Details", e, _detailsAttribute);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Details property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void DetailsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Details", e, _detailsAttribute);
        }
        
        private static ITypedElement RetrieveAnnotationsReference()
        {
            return ((ITypedElement)(((NMF.Models.ModelElement)(NMF.Models.Meta.AnnotationEntry.ClassInstance)).Resolve("Annotations")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Annotations property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void AnnotationsCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Annotations", e, _annotationsReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Annotations property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void AnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Annotations", e, _annotationsReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(NMF.Models.IModelElement element)
        {
            int annotationsIndex = ModelHelper.IndexOfReference(this.Annotations, element);
            if ((annotationsIndex != -1))
            {
                return ModelHelper.CreatePath("Annotations", annotationsIndex);
            }
            return base.GetRelativePathForNonIdentifiedChild(element);
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override NMF.Models.IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "ANNOTATIONS"))
            {
                if ((index < this.Annotations.Count))
                {
                    return this.Annotations[index];
                }
                else
                {
                    return null;
                }
            }
            return base.GetModelElementForReference(reference, index);
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "SOURCE"))
            {
                return this.Source;
            }
            if ((attribute == "DETAILS"))
            {
                if ((index < this.Details.Count))
                {
                    return this.Details[index];
                }
                else
                {
                    return null;
                }
            }
            return base.GetAttributeValue(attribute, index);
        }
        
        /// <summary>
        /// Gets the Model element collection for the given feature
        /// </summary>
        /// <returns>A non-generic list of elements</returns>
        /// <param name="feature">The requested feature</param>
        protected override System.Collections.IList GetCollectionForFeature(string feature)
        {
            if ((feature == "ANNOTATIONS"))
            {
                return this._annotations;
            }
            if ((feature == "DETAILS"))
            {
                return this._details;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "SOURCE"))
            {
                this.Source = ((string)(value));
                return;
            }
            base.SetFeature(feature, value);
        }
        
        /// <summary>
        /// Gets the property expression for the given attribute
        /// </summary>
        /// <returns>An incremental property expression</returns>
        /// <param name="attribute">The requested attribute in upper case</param>
        protected override NMF.Expressions.INotifyExpression<object> GetExpressionForAttribute(string attribute)
        {
            if ((attribute == "SOURCE"))
            {
                return new SourceProxy(this);
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the property name for the given container
        /// </summary>
        /// <returns>The name of the respective container reference</returns>
        /// <param name="container">The container object</param>
        protected internal override string GetCompositionName(object container)
        {
            if ((container == this._annotations))
            {
                return "Annotations";
            }
            return base.GetCompositionName(container);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://nmf.codeplex.com/nmeta/#//AnnotationEntry")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the AnnotationEntry class
        /// </summary>
        public class AnnotationEntryChildrenCollection : ReferenceCollection, ICollectionExpression<NMF.Models.IModelElement>, ICollection<NMF.Models.IModelElement>
        {
            
            private AnnotationEntry _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public AnnotationEntryChildrenCollection(AnnotationEntry parent)
            {
                this._parent = parent;
            }
            
            /// <summary>
            /// Gets the amount of elements contained in this collection
            /// </summary>
            public override int Count
            {
                get
                {
                    int count = 0;
                    count = (count + this._parent.Annotations.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.Annotations.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.Annotations.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(NMF.Models.IModelElement item)
            {
                NMF.Models.Meta.IAnnotationEntry annotationsCasted = item.As<NMF.Models.Meta.IAnnotationEntry>();
                if ((annotationsCasted != null))
                {
                    this._parent.Annotations.Add(annotationsCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Annotations.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(NMF.Models.IModelElement item)
            {
                if (this._parent.Annotations.Contains(item))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(NMF.Models.IModelElement[] array, int arrayIndex)
            {
                IEnumerator<NMF.Models.IModelElement> annotationsEnumerator = this._parent.Annotations.GetEnumerator();
                try
                {
                    for (
                    ; annotationsEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = annotationsEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    annotationsEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(NMF.Models.IModelElement item)
            {
                NMF.Models.Meta.IAnnotationEntry annotationEntryItem = item.As<NMF.Models.Meta.IAnnotationEntry>();
                if (((annotationEntryItem != null) 
                            && this._parent.Annotations.Remove(annotationEntryItem)))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<NMF.Models.IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<NMF.Models.IModelElement>().Concat(this._parent.Annotations).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the AnnotationEntry class
        /// </summary>
        public class AnnotationEntryReferencedElementsCollection : ReferenceCollection, ICollectionExpression<NMF.Models.IModelElement>, ICollection<NMF.Models.IModelElement>
        {
            
            private AnnotationEntry _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public AnnotationEntryReferencedElementsCollection(AnnotationEntry parent)
            {
                this._parent = parent;
            }
            
            /// <summary>
            /// Gets the amount of elements contained in this collection
            /// </summary>
            public override int Count
            {
                get
                {
                    int count = 0;
                    count = (count + this._parent.Annotations.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.Annotations.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.Annotations.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(NMF.Models.IModelElement item)
            {
                NMF.Models.Meta.IAnnotationEntry annotationsCasted = item.As<NMF.Models.Meta.IAnnotationEntry>();
                if ((annotationsCasted != null))
                {
                    this._parent.Annotations.Add(annotationsCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Annotations.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(NMF.Models.IModelElement item)
            {
                if (this._parent.Annotations.Contains(item))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(NMF.Models.IModelElement[] array, int arrayIndex)
            {
                IEnumerator<NMF.Models.IModelElement> annotationsEnumerator = this._parent.Annotations.GetEnumerator();
                try
                {
                    for (
                    ; annotationsEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = annotationsEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    annotationsEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(NMF.Models.IModelElement item)
            {
                NMF.Models.Meta.IAnnotationEntry annotationEntryItem = item.As<NMF.Models.Meta.IAnnotationEntry>();
                if (((annotationEntryItem != null) 
                            && this._parent.Annotations.Remove(annotationEntryItem)))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<NMF.Models.IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<NMF.Models.IModelElement>().Concat(this._parent.Annotations).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the Source property
        /// </summary>
        private sealed class SourceProxy : ModelPropertyChange<NMF.Models.Meta.IAnnotationEntry, string>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SourceProxy(NMF.Models.Meta.IAnnotationEntry modelElement) : 
                    base(modelElement, "Source")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override string Value
            {
                get
                {
                    return this.ModelElement.Source;
                }
                set
                {
                    this.ModelElement.Source = value;
                }
            }
        }
    }
}
