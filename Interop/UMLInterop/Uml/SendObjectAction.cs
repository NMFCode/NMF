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
    /// A SendObjectAction is an InvocationAction that transmits an input object to the target object, which is handled as a request message by the target object. The requestor continues execution immediately after the object is sent out and cannot receive reply values.
    ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//SendObjectAction")]
    [DebuggerDisplayAttribute("SendObjectAction {Name}")]
    public partial class SendObjectAction : InvocationAction, ISendObjectAction, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _type_target_pinOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveType_target_pinOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _requestReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveRequestReference);
        
        /// <summary>
        /// The backing field for the Request property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IInputPin _request;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _targetReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveTargetReference);
        
        /// <summary>
        /// The backing field for the Target property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IInputPin _target;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The request object, which is transmitted to the target object. The object may be copied in transmission, so identity might not be preserved.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("request")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IInputPin Request
        {
            get
            {
                return this._request;
            }
            set
            {
                if ((this._request != value))
                {
                    IInputPin old = this._request;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Request", e, _requestReference);
                    this._request = value;
                    if ((old != null))
                    {
                        if ((old.Parent == this))
                        {
                            old.Parent = null;
                        }
                        old.ParentChanged -= this.OnResetRequest;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetRequest;
                    }
                    this.OnPropertyChanged("Request", e, _requestReference);
                }
            }
        }
        
        /// <summary>
        /// The target object to which the object is sent.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("target")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IInputPin Target
        {
            get
            {
                return this._target;
            }
            set
            {
                if ((this._target != value))
                {
                    IInputPin old = this._target;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Target", e, _targetReference);
                    this._target = value;
                    if ((old != null))
                    {
                        if ((old.Parent == this))
                        {
                            old.Parent = null;
                        }
                        old.ParentChanged -= this.OnResetTarget;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetTarget;
                    }
                    this.OnPropertyChanged("Target", e, _targetReference);
                }
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new SendObjectActionChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new SendObjectActionReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//SendObjectAction")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// If onPort is not empty, the Port given by onPort must be an owned or inherited feature of the type of the target InputPin.
        ///onPort&lt;&gt;null implies target.type.oclAsType(Classifier).allFeatures()-&gt;includes(onPort)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Type_target_pin(object diagnostics, object context)
        {
            System.Func<ISendObjectAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<ISendObjectAction, object, object, bool>>(_type_target_pinOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method type_target_pin registered. Use the method " +
                        "broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _type_target_pinOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _type_target_pinOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _type_target_pinOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveType_target_pinOperation()
        {
            return ClassInstance.LookupOperation("type_target_pin");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveRequestReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.SendObjectAction.ClassInstance)).Resolve("request")));
        }
        
        /// <summary>
        /// Handles the event that the Request property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetRequest(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.Request))
            {
                this.Request = null;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveTargetReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.SendObjectAction.ClassInstance)).Resolve("target")));
        }
        
        /// <summary>
        /// Handles the event that the Target property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetTarget(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.Target))
            {
                this.Target = null;
            }
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.Request))
            {
                return ModelHelper.CreatePath("request");
            }
            if ((element == this.Target))
            {
                return ModelHelper.CreatePath("target");
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
            if ((reference == "REQUEST"))
            {
                return this.Request;
            }
            if ((reference == "TARGET"))
            {
                return this.Target;
            }
            return base.GetModelElementForReference(reference, index);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "REQUEST"))
            {
                this.Request = ((IInputPin)(value));
                return;
            }
            if ((feature == "TARGET"))
            {
                this.Target = ((IInputPin)(value));
                return;
            }
            base.SetFeature(feature, value);
        }
        
        /// <summary>
        /// Gets the property expression for the given reference
        /// </summary>
        /// <returns>An incremental property expression</returns>
        /// <param name="reference">The requested reference in upper case</param>
        protected override NMF.Expressions.INotifyExpression<NMF.Models.IModelElement> GetExpressionForReference(string reference)
        {
            if ((reference == "REQUEST"))
            {
                return new RequestProxy(this);
            }
            if ((reference == "TARGET"))
            {
                return new TargetProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//SendObjectAction")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the SendObjectAction class
        /// </summary>
        public class SendObjectActionChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private SendObjectAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public SendObjectActionChildrenCollection(SendObjectAction parent)
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
                    if ((this._parent.Request != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.Target != null))
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
                if ((this._parent.Request == null))
                {
                    IInputPin requestCasted = item.As<IInputPin>();
                    if ((requestCasted != null))
                    {
                        this._parent.Request = requestCasted;
                        return;
                    }
                }
                if ((this._parent.Target == null))
                {
                    IInputPin targetCasted = item.As<IInputPin>();
                    if ((targetCasted != null))
                    {
                        this._parent.Target = targetCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Request = null;
                this._parent.Target = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Request))
                {
                    return true;
                }
                if ((item == this._parent.Target))
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
                if ((this._parent.Request != null))
                {
                    array[arrayIndex] = this._parent.Request;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.Target != null))
                {
                    array[arrayIndex] = this._parent.Target;
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
                if ((this._parent.Request == item))
                {
                    this._parent.Request = null;
                    return true;
                }
                if ((this._parent.Target == item))
                {
                    this._parent.Target = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Request).Concat(this._parent.Target).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the SendObjectAction class
        /// </summary>
        public class SendObjectActionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private SendObjectAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public SendObjectActionReferencedElementsCollection(SendObjectAction parent)
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
                    if ((this._parent.Request != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.Target != null))
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
                if ((this._parent.Request == null))
                {
                    IInputPin requestCasted = item.As<IInputPin>();
                    if ((requestCasted != null))
                    {
                        this._parent.Request = requestCasted;
                        return;
                    }
                }
                if ((this._parent.Target == null))
                {
                    IInputPin targetCasted = item.As<IInputPin>();
                    if ((targetCasted != null))
                    {
                        this._parent.Target = targetCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Request = null;
                this._parent.Target = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Request))
                {
                    return true;
                }
                if ((item == this._parent.Target))
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
                if ((this._parent.Request != null))
                {
                    array[arrayIndex] = this._parent.Request;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.Target != null))
                {
                    array[arrayIndex] = this._parent.Target;
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
                if ((this._parent.Request == item))
                {
                    this._parent.Request = null;
                    return true;
                }
                if ((this._parent.Target == item))
                {
                    this._parent.Target = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Request).Concat(this._parent.Target).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the request property
        /// </summary>
        private sealed class RequestProxy : ModelPropertyChange<ISendObjectAction, IInputPin>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public RequestProxy(ISendObjectAction modelElement) : 
                    base(modelElement, "request")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IInputPin Value
            {
                get
                {
                    return this.ModelElement.Request;
                }
                set
                {
                    this.ModelElement.Request = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the target property
        /// </summary>
        private sealed class TargetProxy : ModelPropertyChange<ISendObjectAction, IInputPin>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public TargetProxy(ISendObjectAction modelElement) : 
                    base(modelElement, "target")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IInputPin Value
            {
                get
                {
                    return this.ModelElement.Target;
                }
                set
                {
                    this.ModelElement.Target = value;
                }
            }
        }
    }
}
