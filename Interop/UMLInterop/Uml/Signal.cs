//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.25
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Interop.Ecore;
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

namespace NMF.Interop.Uml
{
    
    
    /// <summary>
    /// A Signal is a specification of a kind of communication between objects in which a reaction is asynchronously triggered in the receiver without a reply.
    ///<p>From package UML::SimpleClassifiers.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Signal")]
    [DebuggerDisplayAttribute("Signal {Name}")]
    public partial class Signal : Classifier, ISignal, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _createOwnedAttributeOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveCreateOwnedAttributeOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _ownedAttributeReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOwnedAttributeReference);
        
        /// <summary>
        /// The backing field for the OwnedAttribute property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IProperty> _ownedAttribute;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        public Signal()
        {
            this._ownedAttribute = new ObservableCompositionOrderedSet<IProperty>(this);
            this._ownedAttribute.CollectionChanging += this.OwnedAttributeCollectionChanging;
            this._ownedAttribute.CollectionChanged += this.OwnedAttributeCollectionChanged;
        }
        
        /// <summary>
        /// The attributes owned by the Signal.
        ///<p>From package UML::SimpleClassifiers.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedAttribute")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IProperty> OwnedAttribute
        {
            get
            {
                return this._ownedAttribute;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new SignalChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new SignalReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Signal")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Creates a property with the specified name, type, lower bound, and upper bound as an owned attribute of this signal.
        /// </summary>
        /// <param name="name">The name for the new attribute, or null.</param>
        /// <param name="type">The type for the new attribute, or null.</param>
        /// <param name="lower">The lower bound for the new attribute.</param>
        /// <param name="upper">The upper bound for the new attribute.</param>
        public IProperty CreateOwnedAttribute(string name, NMF.Interop.Uml.IType type, int lower, object upper)
        {
            System.Func<ISignal, string, NMF.Interop.Uml.IType, int, object, IProperty> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ISignal, string, NMF.Interop.Uml.IType, int, object, IProperty>>(_createOwnedAttributeOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method createOwnedAttribute registered. Use the me" +
                        "thod broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _createOwnedAttributeOperation.Value, name, type, lower, upper);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _createOwnedAttributeOperation.Value, e));
            IProperty result = handler.Invoke(this, name, type, lower, upper);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _createOwnedAttributeOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveCreateOwnedAttributeOperation()
        {
            return ClassInstance.LookupOperation("createOwnedAttribute");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveOwnedAttributeReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Signal.ClassInstance)).Resolve("ownedAttribute")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the OwnedAttribute property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedAttributeCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("OwnedAttribute", e, _ownedAttributeReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the OwnedAttribute property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedAttributeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("OwnedAttribute", e, _ownedAttributeReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int ownedAttributeIndex = ModelHelper.IndexOfReference(this.OwnedAttribute, element);
            if ((ownedAttributeIndex != -1))
            {
                return ModelHelper.CreatePath("ownedAttribute", ownedAttributeIndex);
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
            if ((reference == "OWNEDATTRIBUTE"))
            {
                if ((index < this.OwnedAttribute.Count))
                {
                    return this.OwnedAttribute[index];
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
            if ((feature == "OWNEDATTRIBUTE"))
            {
                return this._ownedAttribute;
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
            if ((container == this._ownedAttribute))
            {
                return "ownedAttribute";
            }
            return base.GetCompositionName(container);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override NMF.Models.Meta.IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Signal")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Signal class
        /// </summary>
        public class SignalChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Signal _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public SignalChildrenCollection(Signal parent)
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
                    count = (count + this._parent.OwnedAttribute.Count);
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IProperty ownedAttributeCasted = item.As<IProperty>();
                if ((ownedAttributeCasted != null))
                {
                    this._parent.OwnedAttribute.Add(ownedAttributeCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OwnedAttribute.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.OwnedAttribute.Contains(item))
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
                IEnumerator<IModelElement> ownedAttributeEnumerator = this._parent.OwnedAttribute.GetEnumerator();
                try
                {
                    for (
                    ; ownedAttributeEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = ownedAttributeEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    ownedAttributeEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IProperty propertyItem = item.As<IProperty>();
                if (((propertyItem != null) 
                            && this._parent.OwnedAttribute.Remove(propertyItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OwnedAttribute).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Signal class
        /// </summary>
        public class SignalReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Signal _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public SignalReferencedElementsCollection(Signal parent)
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
                    count = (count + this._parent.OwnedAttribute.Count);
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.OwnedAttribute.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IProperty ownedAttributeCasted = item.As<IProperty>();
                if ((ownedAttributeCasted != null))
                {
                    this._parent.OwnedAttribute.Add(ownedAttributeCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OwnedAttribute.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.OwnedAttribute.Contains(item))
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
                IEnumerator<IModelElement> ownedAttributeEnumerator = this._parent.OwnedAttribute.GetEnumerator();
                try
                {
                    for (
                    ; ownedAttributeEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = ownedAttributeEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    ownedAttributeEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IProperty propertyItem = item.As<IProperty>();
                if (((propertyItem != null) 
                            && this._parent.OwnedAttribute.Remove(propertyItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OwnedAttribute).GetEnumerator();
            }
        }
    }
}
