//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.21
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

namespace NMF.Glsp.Notation
{
    
    
    /// <summary>
    /// The default implementation of the NotationElement class
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/glsp/notation")]
    [XmlNamespacePrefixAttribute("notation")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/glsp/notation#//NotationElement")]
    public abstract partial class NotationElement : ModelElement, INotationElement, IModelElement
    {
        
        /// <summary>
        /// The backing field for the Id property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private string _id;
        
        private static Lazy<ITypedElement> _idAttribute = new Lazy<ITypedElement>(RetrieveIdAttribute);
        
        private static Lazy<ITypedElement> _semanticElementReference = new Lazy<ITypedElement>(RetrieveSemanticElementReference);
        
        /// <summary>
        /// The backing field for the SemanticElement property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IModelElement _semanticElement;
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The id property
        /// </summary>
        [DisplayNameAttribute("id")]
        [CategoryAttribute("NotationElement")]
        [XmlElementNameAttribute("id")]
        [XmlAttributeAttribute(true)]
        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    string old = this._id;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnIdChanging(e);
                    this.OnPropertyChanging("Id", e, _idAttribute);
                    this._id = value;
                    this.OnIdChanged(e);
                    this.OnPropertyChanged("Id", e, _idAttribute);
                }
            }
        }
        
        /// <summary>
        /// The semanticElement property
        /// </summary>
        [DisplayNameAttribute("semanticElement")]
        [CategoryAttribute("NotationElement")]
        [XmlElementNameAttribute("semanticElement")]
        [XmlAttributeAttribute(true)]
        public IModelElement SemanticElement
        {
            get
            {
                return this._semanticElement;
            }
            set
            {
                if ((this._semanticElement != value))
                {
                    IModelElement old = this._semanticElement;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnSemanticElementChanging(e);
                    this.OnPropertyChanging("SemanticElement", e, _semanticElementReference);
                    this._semanticElement = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetSemanticElement;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetSemanticElement;
                    }
                    this.OnSemanticElementChanged(e);
                    this.OnPropertyChanged("SemanticElement", e, _semanticElementReference);
                }
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new NotationElementReferencedElementsCollection(this));
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/glsp/notation#//NotationElement")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets fired before the Id property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> IdChanging;
        
        /// <summary>
        /// Gets fired when the Id property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> IdChanged;
        
        /// <summary>
        /// Gets fired before the SemanticElement property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> SemanticElementChanging;
        
        /// <summary>
        /// Gets fired when the SemanticElement property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> SemanticElementChanged;
        
        private static ITypedElement RetrieveIdAttribute()
        {
            return ((ITypedElement)(((ModelElement)(NMF.Glsp.Notation.NotationElement.ClassInstance)).Resolve("id")));
        }
        
        /// <summary>
        /// Raises the IdChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnIdChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.IdChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the IdChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnIdChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.IdChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        private static ITypedElement RetrieveSemanticElementReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.Glsp.Notation.NotationElement.ClassInstance)).Resolve("semanticElement")));
        }
        
        /// <summary>
        /// Raises the SemanticElementChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSemanticElementChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.SemanticElementChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the SemanticElementChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSemanticElementChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.SemanticElementChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Handles the event that the SemanticElement property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetSemanticElement(object sender, System.EventArgs eventArgs)
        {
            this.SemanticElement = null;
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "SEMANTICELEMENT"))
            {
                return this.SemanticElement;
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
            if ((attribute == "ID"))
            {
                return this.Id;
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
            if ((feature == "SEMANTICELEMENT"))
            {
                this.SemanticElement = ((IModelElement)(value));
                return;
            }
            if ((feature == "ID"))
            {
                this.Id = ((string)(value));
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
            if ((attribute == "ID"))
            {
                return new IdProxy(this);
            }
            return base.GetExpressionForAttribute(attribute);
        }
        
        /// <summary>
        /// Gets the property expression for the given reference
        /// </summary>
        /// <returns>An incremental property expression</returns>
        /// <param name="reference">The requested reference in upper case</param>
        protected override NMF.Expressions.INotifyExpression<NMF.Models.IModelElement> GetExpressionForReference(string reference)
        {
            if ((reference == "SEMANTICELEMENT"))
            {
                return new SemanticElementProxy(this);
            }
            return base.GetExpressionForReference(reference);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/glsp/notation#//NotationElement")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the NotationElement class
        /// </summary>
        public class NotationElementReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private NotationElement _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public NotationElementReferencedElementsCollection(NotationElement parent)
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
                    if ((this._parent.SemanticElement != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.SemanticElementChanged += this.PropagateValueChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.SemanticElementChanged -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.SemanticElement == null))
                {
                    this._parent.SemanticElement = item;
                    return;
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.SemanticElement = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.SemanticElement))
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
                if ((this._parent.SemanticElement != null))
                {
                    array[arrayIndex] = this._parent.SemanticElement;
                    arrayIndex = (arrayIndex + 1);
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                if ((this._parent.SemanticElement == item))
                {
                    this._parent.SemanticElement = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.SemanticElement).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the id property
        /// </summary>
        private sealed class IdProxy : ModelPropertyChange<INotationElement, string>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public IdProxy(INotationElement modelElement) : 
                    base(modelElement, "id")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override string Value
            {
                get
                {
                    return this.ModelElement.Id;
                }
                set
                {
                    this.ModelElement.Id = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the semanticElement property
        /// </summary>
        private sealed class SemanticElementProxy : ModelPropertyChange<INotationElement, IModelElement>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SemanticElementProxy(INotationElement modelElement) : 
                    base(modelElement, "semanticElement")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IModelElement Value
            {
                get
                {
                    return this.ModelElement.SemanticElement;
                }
                set
                {
                    this.ModelElement.SemanticElement = value;
                }
            }
        }
    }
}
