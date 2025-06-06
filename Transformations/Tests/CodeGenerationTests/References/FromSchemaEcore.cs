//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TemporaryGeneratedCode.Simulink
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using NMF.Expressions;
    using NMF.Expressions.Linq;
    using NMF.Models;
    using NMF.Models.Meta;
    using NMF.Models.Collections;
    using NMF.Models.Expressions;
    using NMF.Collections.Generic;
    using NMF.Collections.ObjectModel;
    using NMF.Serialization;
    using NMF.Utilities;
    using System.Collections.Specialized;
    
    
    /// <summary>
    /// The default implementation of the AnnotationDefaultsType class
    /// </summary>
    [XmlNamespaceAttribute("")]
    [XmlElementNameAttribute("AnnotationDefaults")]
    public partial class AnnotationDefaultsType : ModelElement, IAnnotationDefaultsType, IModelElement
    {
        
        private static Lazy<ITypedElement> _pReference = new Lazy<ITypedElement>(RetrievePReference);
        
        /// <summary>
        /// The backing field for the P property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IPType> _p;
        
        private static IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public AnnotationDefaultsType()
        {
            this._p = new ObservableCompositionOrderedSet<IPType>(this);
            this._p.CollectionChanging += this.PCollectionChanging;
            this._p.CollectionChanged += this.PCollectionChanged;
        }
        
        /// <summary>
        /// The p property
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IPType> P
        {
            get
            {
                return this._p;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new AnnotationDefaultsTypeChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new AnnotationDefaultsTypeReferencedElementsCollection(this));
            }
        }
        
        private static ITypedElement RetrievePReference()
        {
            return ((ITypedElement)(((ModelElement)(TemporaryGeneratedCode.Simulink.AnnotationDefaultsType.ClassInstance)).Resolve("p")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the P property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void PCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("P", e, _pReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the P property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void PCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("P", e, _pReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int pIndex = ModelHelper.IndexOfReference(this.P, element);
            if ((pIndex != -1))
            {
                return ModelHelper.CreatePath("p", pIndex);
            }
            return base.GetRelativePathForNonIdentifiedChild(element);
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "P"))
            {
                if ((index < this.P.Count))
                {
                    return this.P[index];
                }
                else
                {
                    return null;
                }
            }
            return base.GetModelElementForReference(reference, index);
        }
        
        /// <summary>
        /// Gets the Model element collection for the given feature
        /// </summary>
        /// <returns>A non-generic list of elements</returns>
        /// <param name="feature">The requested feature</param>
        protected override System.Collections.IList GetCollectionForFeature(string feature)
        {
            if ((feature == "P"))
            {
                return this._p;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Gets the property name for the given container
        /// </summary>
        /// <returns>The name of the respective container reference</returns>
        /// <param name="container">The container object</param>
        protected override string GetCompositionName(object container)
        {
            if ((container == this._p))
            {
                return "p";
            }
            return base.GetCompositionName(container);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            throw new NotSupportedException(("AnnotationDefaultsType does not have an absolute URI and therefore cannot be reso" +
                    "lved."));
        }
        
        /// <summary>
        /// The collection class to to represent the children of the AnnotationDefaultsType class
        /// </summary>
        public class AnnotationDefaultsTypeChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private AnnotationDefaultsType _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public AnnotationDefaultsTypeChildrenCollection(AnnotationDefaultsType parent)
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
                    count = (count + this._parent.P.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.P.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.P.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IPType pCasted = item.As<IPType>();
                if ((pCasted != null))
                {
                    this._parent.P.Add(pCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.P.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.P.Contains(item))
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
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
                IEnumerator<IModelElement> pEnumerator = this._parent.P.GetEnumerator();
                try
                {
                    for (
                    ; pEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = pEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    pEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IPType pTypeItem = item.As<IPType>();
                if (((pTypeItem != null) 
                            && this._parent.P.Remove(pTypeItem)))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().Concat(this._parent.P).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the AnnotationDefaultsType class
        /// </summary>
        public class AnnotationDefaultsTypeReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private AnnotationDefaultsType _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public AnnotationDefaultsTypeReferencedElementsCollection(AnnotationDefaultsType parent)
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
                    count = (count + this._parent.P.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.P.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.P.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IPType pCasted = item.As<IPType>();
                if ((pCasted != null))
                {
                    this._parent.P.Add(pCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.P.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.P.Contains(item))
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
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
                IEnumerator<IModelElement> pEnumerator = this._parent.P.GetEnumerator();
                try
                {
                    for (
                    ; pEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = pEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    pEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IPType pTypeItem = item.As<IPType>();
                if (((pTypeItem != null) 
                            && this._parent.P.Remove(pTypeItem)))
                {
                    return true;
                }
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().Concat(this._parent.P).GetEnumerator();
            }
        }
    }
    
    /// <summary>
    /// The default implementation of the PType class
    /// </summary>
    [XmlNamespaceAttribute("")]
    [XmlElementNameAttribute("P")]
    public partial class PType : ModelElement, IPType, IModelElement
    {
        
        /// <summary>
        /// The backing field for the Value property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private string _value;
        
        private static Lazy<ITypedElement> _valueAttribute = new Lazy<ITypedElement>(RetrieveValueAttribute);
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The value property
        /// </summary>
        [DisplayNameAttribute("value")]
        [CategoryAttribute("PType")]
        [XmlDefaultPropertyAttribute(true)]
        [XmlAttributeAttribute(true)]
        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if ((this._value != value))
                {
                    string old = this._value;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Value", e, _valueAttribute);
                    this._value = value;
                    this.OnPropertyChanged("Value", e, _valueAttribute);
                }
            }
        }
        
        private static ITypedElement RetrieveValueAttribute()
        {
            return ((ITypedElement)(((ModelElement)(TemporaryGeneratedCode.Simulink.PType.ClassInstance)).Resolve("value")));
        }
        
        /// <summary>
        /// Resolves the given attribute name
        /// </summary>
        /// <returns>The attribute value or null if it could not be found</returns>
        /// <param name="attribute">The requested attribute name</param>
        /// <param name="index">The index of this attribute</param>
        protected override object GetAttributeValue(string attribute, int index)
        {
            if ((attribute == "VALUE"))
            {
                return this.Value;
            }
            return base.GetAttributeValue(attribute, index);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "VALUE"))
            {
                this.Value = ((string)(value));
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
            if ((attribute == "VALUE"))
            {
                return new ValueProxy(this);
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            throw new NotSupportedException("PType does not have an absolute URI and therefore cannot be resolved.");
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the value property
        /// </summary>
        private sealed class ValueProxy : ModelPropertyChange<IPType, string>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ValueProxy(IPType modelElement) : 
                    base(modelElement, "value")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override string Value
            {
                get
                {
                    return this.ModelElement.Value;
                }
                set
                {
                    this.ModelElement.Value = value;
                }
            }
        }
    }
    
    /// <summary>
    /// The public interface for PType
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(PType))]
    [XmlDefaultImplementationTypeAttribute(typeof(PType))]
    public partial interface IPType : IModelElement
    {
        
        /// <summary>
        /// The value property
        /// </summary>
        [DisplayNameAttribute("value")]
        [CategoryAttribute("PType")]
        [XmlDefaultPropertyAttribute(true)]
        [XmlAttributeAttribute(true)]
        string Value
        {
            get;
            set;
        }
    }
    
    /// <summary>
    /// The public interface for AnnotationDefaultsType
    /// </summary>
    [DefaultImplementationTypeAttribute(typeof(AnnotationDefaultsType))]
    [XmlDefaultImplementationTypeAttribute(typeof(AnnotationDefaultsType))]
    public partial interface IAnnotationDefaultsType : IModelElement
    {
        
        /// <summary>
        /// The p property
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        IOrderedSetExpression<IPType> P
        {
            get;
        }
    }
}
