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
    /// A ProtocolTransition specifies a legal Transition for an Operation. Transitions of ProtocolStateMachines have the following information: a pre-condition (guard), a Trigger, and a post-condition. Every ProtocolTransition is associated with at most one BehavioralFeature belonging to the context Classifier of the ProtocolStateMachine.
    ///<p>From package UML::StateMachines.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ProtocolTransition")]
    [DebuggerDisplayAttribute("ProtocolTransition {Name}")]
    public partial class ProtocolTransition : Transition, IProtocolTransition, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _refers_to_operationOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveRefers_to_operationOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _associated_actionsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveAssociated_actionsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _belongs_to_psmOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveBelongs_to_psmOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _getReferredsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveGetReferredsOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _postConditionReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrievePostConditionReference);
        
        /// <summary>
        /// The backing field for the PostCondition property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IConstraint _postCondition;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _preConditionReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrievePreConditionReference);
        
        /// <summary>
        /// The backing field for the PreCondition property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IConstraint _preCondition;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Specifies the post condition of the Transition which is the Condition that should be obtained once the Transition is triggered. This post condition is part of the post condition of the Operation connected to the Transition.
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        [DisplayNameAttribute("postCondition")]
        [DescriptionAttribute(@"Specifies the post condition of the Transition which is the Condition that should be obtained once the Transition is triggered. This post condition is part of the post condition of the Operation connected to the Transition.
<p>From package UML::StateMachines.</p>")]
        [CategoryAttribute("ProtocolTransition")]
        [XmlElementNameAttribute("postCondition")]
        [XmlAttributeAttribute(true)]
        public IConstraint PostCondition
        {
            get
            {
                return this._postCondition;
            }
            set
            {
                if ((this._postCondition != value))
                {
                    IConstraint old = this._postCondition;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("PostCondition", e, _postConditionReference);
                    this._postCondition = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetPostCondition;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetPostCondition;
                    }
                    this.OnPropertyChanged("PostCondition", e, _postConditionReference);
                }
            }
        }
        
        /// <summary>
        /// Specifies the precondition of the Transition. It specifies the Condition that should be verified before triggering the Transition. This guard condition added to the source State will be evaluated as part of the precondition of the Operation referred by the Transition if any.
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        [DisplayNameAttribute("preCondition")]
        [DescriptionAttribute(@"Specifies the precondition of the Transition. It specifies the Condition that should be verified before triggering the Transition. This guard condition added to the source State will be evaluated as part of the precondition of the Operation referred by the Transition if any.
<p>From package UML::StateMachines.</p>")]
        [CategoryAttribute("ProtocolTransition")]
        [XmlElementNameAttribute("preCondition")]
        [XmlAttributeAttribute(true)]
        public IConstraint PreCondition
        {
            get
            {
                return this._preCondition;
            }
            set
            {
                if ((this._preCondition != value))
                {
                    IConstraint old = this._preCondition;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("PreCondition", e, _preConditionReference);
                    this._preCondition = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetPreCondition;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetPreCondition;
                    }
                    this.OnPropertyChanged("PreCondition", e, _preConditionReference);
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
                return base.ReferencedElements.Concat(new ProtocolTransitionReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ProtocolTransition")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// If a ProtocolTransition refers to an Operation (i.e., has a CallEvent trigger corresponding to an Operation), then that Operation should apply to the context Classifier of the StateMachine of the ProtocolTransition.
        ///if (referred()->notEmpty() and containingStateMachine()._'context'->notEmpty()) then 
        ///    containingStateMachine()._'context'.oclAsType(BehavioredClassifier).allFeatures()->includesAll(referred())
        ///else true endif
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Refers_to_operation(object diagnostics, object context)
        {
            System.Func<IProtocolTransition, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IProtocolTransition, object, object, bool>>(_refers_to_operationOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method refers_to_operation registered. Use the met" +
                        "hod broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _refers_to_operationOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _refers_to_operationOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _refers_to_operationOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveRefers_to_operationOperation()
        {
            return ClassInstance.LookupOperation("refers_to_operation");
        }
        
        /// <summary>
        /// A ProtocolTransition never has associated Behaviors.
        ///effect = null
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Associated_actions(object diagnostics, object context)
        {
            System.Func<IProtocolTransition, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IProtocolTransition, object, object, bool>>(_associated_actionsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method associated_actions registered. Use the meth" +
                        "od broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _associated_actionsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _associated_actionsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _associated_actionsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveAssociated_actionsOperation()
        {
            return ClassInstance.LookupOperation("associated_actions");
        }
        
        /// <summary>
        /// A ProtocolTransition always belongs to a ProtocolStateMachine.
        ///container.belongsToPSM()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Belongs_to_psm(object diagnostics, object context)
        {
            System.Func<IProtocolTransition, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IProtocolTransition, object, object, bool>>(_belongs_to_psmOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method belongs_to_psm registered. Use the method b" +
                        "roker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _belongs_to_psmOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _belongs_to_psmOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _belongs_to_psmOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveBelongs_to_psmOperation()
        {
            return ClassInstance.LookupOperation("belongs_to_psm");
        }
        
        /// <summary>
        /// Derivation for ProtocolTransition::/referred
        ///result = (trigger->collect(event)->select(oclIsKindOf(CallEvent))->collect(oclAsType(CallEvent).operation)->asSet())
        ///<p>From package UML::StateMachines.</p>
        /// </summary>
        public ISetExpression<NMF.Interop.Uml.IOperation> GetReferreds()
        {
            System.Func<IProtocolTransition, ISetExpression<NMF.Interop.Uml.IOperation>> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IProtocolTransition, ISetExpression<NMF.Interop.Uml.IOperation>>>(_getReferredsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method getReferreds registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _getReferredsOperation.Value);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _getReferredsOperation.Value, e));
            ISetExpression<NMF.Interop.Uml.IOperation> result = handler.Invoke(this);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _getReferredsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveGetReferredsOperation()
        {
            return ClassInstance.LookupOperation("getReferreds");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrievePostConditionReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.ProtocolTransition.ClassInstance)).Resolve("postCondition")));
        }
        
        /// <summary>
        /// Handles the event that the PostCondition property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetPostCondition(object sender, System.EventArgs eventArgs)
        {
            this.PostCondition = null;
        }
        
        private static NMF.Models.Meta.ITypedElement RetrievePreConditionReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.ProtocolTransition.ClassInstance)).Resolve("preCondition")));
        }
        
        /// <summary>
        /// Handles the event that the PreCondition property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetPreCondition(object sender, System.EventArgs eventArgs)
        {
            this.PreCondition = null;
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "POSTCONDITION"))
            {
                return this.PostCondition;
            }
            if ((reference == "PRECONDITION"))
            {
                return this.PreCondition;
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
            if ((feature == "POSTCONDITION"))
            {
                this.PostCondition = ((IConstraint)(value));
                return;
            }
            if ((feature == "PRECONDITION"))
            {
                this.PreCondition = ((IConstraint)(value));
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
            if ((reference == "POSTCONDITION"))
            {
                return new PostConditionProxy(this);
            }
            if ((reference == "PRECONDITION"))
            {
                return new PreConditionProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ProtocolTransition")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ProtocolTransition class
        /// </summary>
        public class ProtocolTransitionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ProtocolTransition _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ProtocolTransitionReferencedElementsCollection(ProtocolTransition parent)
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
                    if ((this._parent.PostCondition != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.PreCondition != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.BubbledChange += this.PropagateValueChanges;
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
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
                if ((this._parent.PostCondition == null))
                {
                    IConstraint postConditionCasted = item.As<IConstraint>();
                    if ((postConditionCasted != null))
                    {
                        this._parent.PostCondition = postConditionCasted;
                        return;
                    }
                }
                if ((this._parent.PreCondition == null))
                {
                    IConstraint preConditionCasted = item.As<IConstraint>();
                    if ((preConditionCasted != null))
                    {
                        this._parent.PreCondition = preConditionCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.PostCondition = null;
                this._parent.PreCondition = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.PostCondition))
                {
                    return true;
                }
                if ((item == this._parent.PreCondition))
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
                if ((this._parent.PostCondition != null))
                {
                    array[arrayIndex] = this._parent.PostCondition;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.PreCondition != null))
                {
                    array[arrayIndex] = this._parent.PreCondition;
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
                if ((this._parent.PostCondition == item))
                {
                    this._parent.PostCondition = null;
                    return true;
                }
                if ((this._parent.PreCondition == item))
                {
                    this._parent.PreCondition = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.PostCondition).Concat(this._parent.PreCondition).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the postCondition property
        /// </summary>
        private sealed class PostConditionProxy : ModelPropertyChange<IProtocolTransition, IConstraint>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public PostConditionProxy(IProtocolTransition modelElement) : 
                    base(modelElement, "postCondition")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IConstraint Value
            {
                get
                {
                    return this.ModelElement.PostCondition;
                }
                set
                {
                    this.ModelElement.PostCondition = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the preCondition property
        /// </summary>
        private sealed class PreConditionProxy : ModelPropertyChange<IProtocolTransition, IConstraint>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public PreConditionProxy(IProtocolTransition modelElement) : 
                    base(modelElement, "preCondition")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IConstraint Value
            {
                get
                {
                    return this.ModelElement.PreCondition;
                }
                set
                {
                    this.ModelElement.PreCondition = value;
                }
            }
        }
    }
}
