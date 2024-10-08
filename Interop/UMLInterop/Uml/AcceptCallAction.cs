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
    /// An AcceptCallAction is an AcceptEventAction that handles the receipt of a synchronous call request. In addition to the values from the Operation input parameters, the Action produces an output that is needed later to supply the information to the ReplyAction necessary to return control to the caller. An AcceptCallAction is for synchronous calls. If it is used to handle an asynchronous call, execution of the subsequent ReplyAction will complete immediately with no effect.
    ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//AcceptCallAction")]
    [DebuggerDisplayAttribute("AcceptCallAction {Name}")]
    public partial class AcceptCallAction : AcceptEventAction, IAcceptCallAction, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _result_pinsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveResult_pinsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _trigger_call_eventOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveTrigger_call_eventOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _unmarshallOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveUnmarshallOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _returnInformationReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveReturnInformationReference);
        
        /// <summary>
        /// The backing field for the ReturnInformation property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IOutputPin _returnInformation;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// An OutputPin where a value is placed containing sufficient information to perform a subsequent ReplyAction and return control to the caller. The contents of this value are opaque. It can be passed and copied but it cannot be manipulated by the model.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("returnInformation")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IOutputPin ReturnInformation
        {
            get
            {
                return this._returnInformation;
            }
            set
            {
                if ((this._returnInformation != value))
                {
                    IOutputPin old = this._returnInformation;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("ReturnInformation", e, _returnInformationReference);
                    this._returnInformation = value;
                    if ((old != null))
                    {
                        if ((old.Parent == this))
                        {
                            old.Parent = null;
                        }
                        old.ParentChanged -= this.OnResetReturnInformation;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetReturnInformation;
                    }
                    this.OnPropertyChanged("ReturnInformation", e, _returnInformationReference);
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
                return base.Children.Concat(new AcceptCallActionChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new AcceptCallActionReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//AcceptCallAction")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The number of result OutputPins must be the same as the number of input (in and inout) ownedParameters of the Operation specified by the trigger Event. The type, ordering and multiplicity of each result OutputPin must be consistent with the corresponding input Parameter.
        ///let parameter: OrderedSet(Parameter) = trigger.event-&gt;asSequence()-&gt;first().oclAsType(CallEvent).operation.inputParameters() in
        ///result-&gt;size() = parameter-&gt;size() and
        ///Sequence{1..result-&gt;size()}-&gt;forAll(i | 
        ///	parameter-&gt;at(i).type.conformsTo(result-&gt;at(i).type) and 
        ///	parameter-&gt;at(i).isOrdered = result-&gt;at(i).isOrdered and
        ///	parameter-&gt;at(i).compatibleWith(result-&gt;at(i)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Result_pins(object diagnostics, object context)
        {
            System.Func<IAcceptCallAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAcceptCallAction, object, object, bool>>(_result_pinsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method result_pins registered. Use the method brok" +
                        "er to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _result_pinsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _result_pinsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _result_pinsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveResult_pinsOperation()
        {
            return ClassInstance.LookupOperation("result_pins");
        }
        
        /// <summary>
        /// The action must have exactly one trigger, which must be for a CallEvent.
        ///trigger-&gt;size()=1 and
        ///trigger-&gt;asSequence()-&gt;first().event.oclIsKindOf(CallEvent)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Trigger_call_event(object diagnostics, object context)
        {
            System.Func<IAcceptCallAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAcceptCallAction, object, object, bool>>(_trigger_call_eventOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method trigger_call_event registered. Use the meth" +
                        "od broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _trigger_call_eventOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _trigger_call_eventOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _trigger_call_eventOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveTrigger_call_eventOperation()
        {
            return ClassInstance.LookupOperation("trigger_call_event");
        }
        
        /// <summary>
        /// isUnmrashall must be true for an AcceptCallAction.
        ///isUnmarshall = true
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Unmarshall(object diagnostics, object context)
        {
            System.Func<IAcceptCallAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IAcceptCallAction, object, object, bool>>(_unmarshallOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method unmarshall registered. Use the method broke" +
                        "r to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _unmarshallOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _unmarshallOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _unmarshallOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveUnmarshallOperation()
        {
            return ClassInstance.LookupOperation("unmarshall");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveReturnInformationReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.AcceptCallAction.ClassInstance)).Resolve("returnInformation")));
        }
        
        /// <summary>
        /// Handles the event that the ReturnInformation property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetReturnInformation(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.ReturnInformation))
            {
                this.ReturnInformation = null;
            }
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.ReturnInformation))
            {
                return ModelHelper.CreatePath("returnInformation");
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
            if ((reference == "RETURNINFORMATION"))
            {
                return this.ReturnInformation;
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
            if ((feature == "RETURNINFORMATION"))
            {
                this.ReturnInformation = ((IOutputPin)(value));
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
            if ((reference == "RETURNINFORMATION"))
            {
                return new ReturnInformationProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//AcceptCallAction")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the AcceptCallAction class
        /// </summary>
        public class AcceptCallActionChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private AcceptCallAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public AcceptCallActionChildrenCollection(AcceptCallAction parent)
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
                    if ((this._parent.ReturnInformation != null))
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
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.BubbledChange -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.ReturnInformation == null))
                {
                    IOutputPin returnInformationCasted = item.As<IOutputPin>();
                    if ((returnInformationCasted != null))
                    {
                        this._parent.ReturnInformation = returnInformationCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.ReturnInformation = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.ReturnInformation))
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
                if ((this._parent.ReturnInformation != null))
                {
                    array[arrayIndex] = this._parent.ReturnInformation;
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
                if ((this._parent.ReturnInformation == item))
                {
                    this._parent.ReturnInformation = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.ReturnInformation).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the AcceptCallAction class
        /// </summary>
        public class AcceptCallActionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private AcceptCallAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public AcceptCallActionReferencedElementsCollection(AcceptCallAction parent)
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
                    if ((this._parent.ReturnInformation != null))
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
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.BubbledChange -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.ReturnInformation == null))
                {
                    IOutputPin returnInformationCasted = item.As<IOutputPin>();
                    if ((returnInformationCasted != null))
                    {
                        this._parent.ReturnInformation = returnInformationCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.ReturnInformation = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.ReturnInformation))
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
                if ((this._parent.ReturnInformation != null))
                {
                    array[arrayIndex] = this._parent.ReturnInformation;
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
                if ((this._parent.ReturnInformation == item))
                {
                    this._parent.ReturnInformation = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.ReturnInformation).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the returnInformation property
        /// </summary>
        private sealed class ReturnInformationProxy : ModelPropertyChange<IAcceptCallAction, IOutputPin>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ReturnInformationProxy(IAcceptCallAction modelElement) : 
                    base(modelElement, "returnInformation")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IOutputPin Value
            {
                get
                {
                    return this.ModelElement.ReturnInformation;
                }
                set
                {
                    this.ModelElement.ReturnInformation = value;
                }
            }
        }
    }
}
