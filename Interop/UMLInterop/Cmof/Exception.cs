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
using System.Globalization;
using System.Linq;


namespace NMF.Interop.Cmof
{
    
    
    /// <summary>
    /// The default implementation of the Exception class
    /// </summary>
    [XmlNamespaceAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi")]
    [XmlNamespacePrefixAttribute("cmof")]
    [ModelRepresentationClassAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//Exception")]
    public partial class Exception : ModelElement, IException, IModelElement
    {
        
        /// <summary>
        /// The backing field for the Description property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private string _description;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _descriptionAttribute = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveDescriptionAttribute);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _objectInErrorReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveObjectInErrorReference);
        
        /// <summary>
        /// The backing field for the ObjectInError property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IElement _objectInError;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _elementInErrorReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveElementInErrorReference);
        
        /// <summary>
        /// The backing field for the ElementInError property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IElement _elementInError;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The description property
        /// </summary>
        [DisplayNameAttribute("description")]
        [CategoryAttribute("Exception")]
        [XmlElementNameAttribute("description")]
        [XmlAttributeAttribute(true)]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                if ((this._description != value))
                {
                    string old = this._description;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Description", e, _descriptionAttribute);
                    this._description = value;
                    this.OnPropertyChanged("Description", e, _descriptionAttribute);
                }
            }
        }
        
        /// <summary>
        /// The objectInError property
        /// </summary>
        [DisplayNameAttribute("objectInError")]
        [CategoryAttribute("Exception")]
        [XmlElementNameAttribute("objectInError")]
        [XmlAttributeAttribute(true)]
        public IElement ObjectInError
        {
            get
            {
                return this._objectInError;
            }
            set
            {
                if ((this._objectInError != value))
                {
                    IElement old = this._objectInError;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("ObjectInError", e, _objectInErrorReference);
                    this._objectInError = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetObjectInError;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetObjectInError;
                    }
                    this.OnPropertyChanged("ObjectInError", e, _objectInErrorReference);
                }
            }
        }
        
        /// <summary>
        /// The elementInError property
        /// </summary>
        [DisplayNameAttribute("elementInError")]
        [CategoryAttribute("Exception")]
        [XmlElementNameAttribute("elementInError")]
        [XmlAttributeAttribute(true)]
        public IElement ElementInError
        {
            get
            {
                return this._elementInError;
            }
            set
            {
                if ((this._elementInError != value))
                {
                    IElement old = this._elementInError;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("ElementInError", e, _elementInErrorReference);
                    this._elementInError = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetElementInError;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetElementInError;
                    }
                    this.OnPropertyChanged("ElementInError", e, _elementInErrorReference);
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
                return base.ReferencedElements.Concat(new ExceptionReferencedElementsCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static NMF.Models.Meta.IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//Exception")));
                }
                return _classInstance;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveDescriptionAttribute()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Cmof.Exception.ClassInstance)).Resolve("description")));
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveObjectInErrorReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Cmof.Exception.ClassInstance)).Resolve("objectInError")));
        }
        
        /// <summary>
        /// Handles the event that the ObjectInError property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetObjectInError(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.ObjectInError))
            {
                this.ObjectInError = null;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveElementInErrorReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Cmof.Exception.ClassInstance)).Resolve("elementInError")));
        }
        
        /// <summary>
        /// Handles the event that the ElementInError property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetElementInError(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.ElementInError))
            {
                this.ElementInError = null;
            }
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "OBJECTINERROR"))
            {
                return this.ObjectInError;
            }
            if ((reference == "ELEMENTINERROR"))
            {
                return this.ElementInError;
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
            if ((attribute == "DESCRIPTION"))
            {
                return this.Description;
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
            if ((feature == "OBJECTINERROR"))
            {
                this.ObjectInError = ((IElement)(value));
                return;
            }
            if ((feature == "ELEMENTINERROR"))
            {
                this.ElementInError = ((IElement)(value));
                return;
            }
            if ((feature == "DESCRIPTION"))
            {
                this.Description = ((string)(value));
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
            if ((attribute == "DESCRIPTION"))
            {
                return new DescriptionProxy(this);
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
            if ((reference == "OBJECTINERROR"))
            {
                return new ObjectInErrorProxy(this);
            }
            if ((reference == "ELEMENTINERROR"))
            {
                return new ElementInErrorProxy(this);
            }
            return base.GetExpressionForReference(reference);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//Exception")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Exception class
        /// </summary>
        public class ExceptionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Exception _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ExceptionReferencedElementsCollection(Exception parent)
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
                    if ((this._parent.ObjectInError != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.ElementInError != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.BubbledChange += this.PropagateValueChanges;
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.BubbledChange -= this.PropagateValueChanges;
                this._parent.BubbledChange -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.ObjectInError == null))
                {
                    IElement objectInErrorCasted = item.As<IElement>();
                    if ((objectInErrorCasted != null))
                    {
                        this._parent.ObjectInError = objectInErrorCasted;
                        return;
                    }
                }
                if ((this._parent.ElementInError == null))
                {
                    IElement elementInErrorCasted = item.As<IElement>();
                    if ((elementInErrorCasted != null))
                    {
                        this._parent.ElementInError = elementInErrorCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.ObjectInError = null;
                this._parent.ElementInError = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.ObjectInError))
                {
                    return true;
                }
                if ((item == this._parent.ElementInError))
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
                if ((this._parent.ObjectInError != null))
                {
                    array[arrayIndex] = this._parent.ObjectInError;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.ElementInError != null))
                {
                    array[arrayIndex] = this._parent.ElementInError;
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
                if ((this._parent.ObjectInError == item))
                {
                    this._parent.ObjectInError = null;
                    return true;
                }
                if ((this._parent.ElementInError == item))
                {
                    this._parent.ElementInError = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.ObjectInError).Concat(this._parent.ElementInError).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the description property
        /// </summary>
        private sealed class DescriptionProxy : ModelPropertyChange<IException, string>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public DescriptionProxy(IException modelElement) : 
                    base(modelElement, "description")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override string Value
            {
                get
                {
                    return this.ModelElement.Description;
                }
                set
                {
                    this.ModelElement.Description = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the objectInError property
        /// </summary>
        private sealed class ObjectInErrorProxy : ModelPropertyChange<IException, IElement>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ObjectInErrorProxy(IException modelElement) : 
                    base(modelElement, "objectInError")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IElement Value
            {
                get
                {
                    return this.ModelElement.ObjectInError;
                }
                set
                {
                    this.ModelElement.ObjectInError = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the elementInError property
        /// </summary>
        private sealed class ElementInErrorProxy : ModelPropertyChange<IException, IElement>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ElementInErrorProxy(IException modelElement) : 
                    base(modelElement, "elementInError")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IElement Value
            {
                get
                {
                    return this.ModelElement.ElementInError;
                }
                set
                {
                    this.ModelElement.ElementInError = value;
                }
            }
        }
    }
}