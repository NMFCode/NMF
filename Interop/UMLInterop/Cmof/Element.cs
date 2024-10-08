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
    /// An element is a constituent of a model. As such, it has the capability of owning other elements.
    /// </summary>
    [XmlNamespaceAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi")]
    [XmlNamespacePrefixAttribute("cmof")]
    [ModelRepresentationClassAttribute("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//Element")]
    public abstract partial class Element : Object, IElement, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _has_ownerOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveHas_ownerOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _not_own_selfOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveNot_own_selfOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _allOwnedElementsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveAllOwnedElementsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _mustBeOwnedOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveMustBeOwnedOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getMetaClassOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetMetaClassOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _containerOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveContainerOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _isInstanceOfTypeOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveIsInstanceOfTypeOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _deleteOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveDeleteOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _ownedCommentReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveOwnedCommentReference);
        
        /// <summary>
        /// The backing field for the OwnedComment property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IComment> _ownedComment;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Element()
        {
            this._ownedComment = new ObservableCompositionOrderedSet<IComment>(this);
            this._ownedComment.CollectionChanging += this.OwnedCommentCollectionChanging;
            this._ownedComment.CollectionChanged += this.OwnedCommentCollectionChanged;
        }
        
        /// <summary>
        /// The Comments owned by this element.
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("ownedComment")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IComment> OwnedComment
        {
            get
            {
                return this._ownedComment;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new ElementChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ElementReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//Element")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Elements that must be owned must have an owner.
        ///self.mustBeOwned() implies owner-&gt;notEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Has_owner(object diagnostics, object context)
        {
            System.Func<IElement, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IElement, object, object, bool>>(_has_ownerOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method has_owner registered. Use the method broker" +
                        " to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _has_ownerOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _has_ownerOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _has_ownerOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveHas_ownerOperation()
        {
            return ClassInstance.LookupOperation("has_owner");
        }
        
        /// <summary>
        /// An element may not directly or indirectly own itself.
        ///not self.allOwnedElements()-&gt;includes(self)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Not_own_self(object diagnostics, object context)
        {
            System.Func<IElement, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IElement, object, object, bool>>(_not_own_selfOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method not_own_self registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _not_own_selfOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _not_own_selfOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _not_own_selfOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveNot_own_selfOperation()
        {
            return ClassInstance.LookupOperation("not_own_self");
        }
        
        /// <summary>
        /// The query allOwnedElements() gives all of the direct and indirect owned elements of an element.
        ///result = ownedElement-&gt;union(ownedElement-&gt;collect(e | e.allOwnedElements()))
        /// </summary>
        public ISetExpression<IElement> AllOwnedElements()
        {
            System.Func<IElement, ISetExpression<IElement>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IElement, ISetExpression<IElement>>>(_allOwnedElementsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method allOwnedElements registered. Use the method" +
                        " broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _allOwnedElementsOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _allOwnedElementsOperation.Value, e));
            ISetExpression<IElement> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _allOwnedElementsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveAllOwnedElementsOperation()
        {
            return ClassInstance.LookupOperation("allOwnedElements");
        }
        
        /// <summary>
        /// The query mustBeOwned() indicates whether elements of this type must have an owner. Subclasses of Element that do not require an owner must override this operation.
        ///result = true
        /// </summary>
        public bool MustBeOwned()
        {
            System.Func<IElement, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IElement, bool>>(_mustBeOwnedOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method mustBeOwned registered. Use the method brok" +
                        "er to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _mustBeOwnedOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _mustBeOwnedOperation.Value, e));
            bool result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _mustBeOwnedOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveMustBeOwnedOperation()
        {
            return ClassInstance.LookupOperation("mustBeOwned");
        }
        
        /// <summary>
        /// 
        /// </summary>
        public NMF.Interop.Cmof.IClass GetMetaClass()
        {
            System.Func<IElement, NMF.Interop.Cmof.IClass> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IElement, NMF.Interop.Cmof.IClass>>(_getMetaClassOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getMetaClass registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getMetaClassOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getMetaClassOperation.Value, e));
            NMF.Interop.Cmof.IClass result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getMetaClassOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetMetaClassOperation()
        {
            return ClassInstance.LookupOperation("getMetaClass");
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IElement Container()
        {
            System.Func<IElement, IElement> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IElement, IElement>>(_containerOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method container registered. Use the method broker" +
                        " to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _containerOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _containerOperation.Value, e));
            IElement result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _containerOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveContainerOperation()
        {
            return ClassInstance.LookupOperation("container");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includesSubtypes"></param>
        public bool IsInstanceOfType(NMF.Interop.Cmof.IClass type, bool includesSubtypes)
        {
            System.Func<IElement, NMF.Interop.Cmof.IClass, bool, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IElement, NMF.Interop.Cmof.IClass, bool, bool>>(_isInstanceOfTypeOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method isInstanceOfType registered. Use the method" +
                        " broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _isInstanceOfTypeOperation.Value, type, includesSubtypes);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _isInstanceOfTypeOperation.Value, e));
            bool result = handler.Invoke(this, type, includesSubtypes);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _isInstanceOfTypeOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveIsInstanceOfTypeOperation()
        {
            return ClassInstance.LookupOperation("isInstanceOfType");
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Delete()
        {
            System.Action<IElement> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Action<IElement>>(_deleteOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method delete registered. Use the method broker to" +
                        " register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _deleteOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _deleteOperation.Value, e));
            handler.Invoke(this);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _deleteOperation.Value, e));
        }
        
        private static NMF.Models.Meta.IOperation RetrieveDeleteOperation()
        {
            return ClassInstance.LookupOperation("delete");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveOwnedCommentReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Cmof.Element.ClassInstance)).Resolve("ownedComment")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the OwnedComment property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedCommentCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("OwnedComment", e, _ownedCommentReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the OwnedComment property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void OwnedCommentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("OwnedComment", e, _ownedCommentReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int ownedCommentIndex = ModelHelper.IndexOfReference(this.OwnedComment, element);
            if ((ownedCommentIndex != -1))
            {
                return ModelHelper.CreatePath("ownedComment", ownedCommentIndex);
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
            if ((reference == "OWNEDCOMMENT"))
            {
                if ((index < this.OwnedComment.Count))
                {
                    return this.OwnedComment[index];
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
            if ((feature == "OWNEDCOMMENT"))
            {
                return this._ownedComment;
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
            if ((container == this._ownedComment))
            {
                return "ownedComment";
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.omg.org/spec/MOF/20131001/cmof.xmi#//Element")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Element class
        /// </summary>
        public class ElementChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Element _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ElementChildrenCollection(Element parent)
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
                    count = (count + this._parent.OwnedComment.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.OwnedComment.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.OwnedComment.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IComment ownedCommentCasted = item.As<IComment>();
                if ((ownedCommentCasted != null))
                {
                    this._parent.OwnedComment.Add(ownedCommentCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OwnedComment.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.OwnedComment.Contains(item))
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
                IEnumerator<IModelElement> ownedCommentEnumerator = this._parent.OwnedComment.GetEnumerator();
                try
                {
                    for (
                    ; ownedCommentEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = ownedCommentEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    ownedCommentEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IComment commentItem = item.As<IComment>();
                if (((commentItem != null) 
                            && this._parent.OwnedComment.Remove(commentItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OwnedComment).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Element class
        /// </summary>
        public class ElementReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Element _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ElementReferencedElementsCollection(Element parent)
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
                    count = (count + this._parent.OwnedComment.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.OwnedComment.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.OwnedComment.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IComment ownedCommentCasted = item.As<IComment>();
                if ((ownedCommentCasted != null))
                {
                    this._parent.OwnedComment.Add(ownedCommentCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.OwnedComment.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.OwnedComment.Contains(item))
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
                IEnumerator<IModelElement> ownedCommentEnumerator = this._parent.OwnedComment.GetEnumerator();
                try
                {
                    for (
                    ; ownedCommentEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = ownedCommentEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    ownedCommentEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IComment commentItem = item.As<IComment>();
                if (((commentItem != null) 
                            && this._parent.OwnedComment.Remove(commentItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.OwnedComment).GetEnumerator();
            }
        }
    }
}
