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
    /// An ActionInputPin is a kind of InputPin that executes an Action to determine the values to input to another Action.
    ///<p>From package UML::Actions.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ActionInputPin")]
    [DebuggerDisplayAttribute("ActionInputPin {Name}")]
    public partial class ActionInputPin : InputPin, IActionInputPin, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _input_pinOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveInput_pinOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _one_output_pinOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveOne_output_pinOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _no_control_or_object_flowOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveNo_control_or_object_flowOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _fromActionReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveFromActionReference);
        
        /// <summary>
        /// The backing field for the FromAction property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IAction _fromAction;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The Action used to provide the values of the ActionInputPin.
        ///<p>From package UML::Actions.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("fromAction")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IAction FromAction
        {
            get
            {
                return this._fromAction;
            }
            set
            {
                if ((this._fromAction != value))
                {
                    IAction old = this._fromAction;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("FromAction", e, _fromActionReference);
                    this._fromAction = value;
                    if ((old != null))
                    {
                        old.Parent = null;
                        old.ParentChanged -= this.OnResetFromAction;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetFromAction;
                    }
                    this.OnPropertyChanged("FromAction", e, _fromActionReference);
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
                return base.Children.Concat(new ActionInputPinChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ActionInputPinReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ActionInputPin")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The fromAction of an ActionInputPin must only have ActionInputPins as InputPins.
        ///fromAction.input->forAll(oclIsKindOf(ActionInputPin))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Input_pin(object diagnostics, object context)
        {
            System.Func<IActionInputPin, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IActionInputPin, object, object, bool>>(_input_pinOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method input_pin registered. Use the method broker" +
                        " to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _input_pinOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _input_pinOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _input_pinOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveInput_pinOperation()
        {
            return ClassInstance.LookupOperation("input_pin");
        }
        
        /// <summary>
        /// The fromAction of an ActionInputPin must have exactly one OutputPin.
        ///fromAction.output->size() = 1
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool One_output_pin(object diagnostics, object context)
        {
            System.Func<IActionInputPin, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IActionInputPin, object, object, bool>>(_one_output_pinOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method one_output_pin registered. Use the method b" +
                        "roker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _one_output_pinOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _one_output_pinOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _one_output_pinOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveOne_output_pinOperation()
        {
            return ClassInstance.LookupOperation("one_output_pin");
        }
        
        /// <summary>
        /// The fromAction of an ActionInputPin cannot have ActivityEdges coming into or out of it or its Pins.
        ///fromAction.incoming->union(outgoing)->isEmpty() and
        ///fromAction.input.incoming->isEmpty() and
        ///fromAction.output.outgoing->isEmpty()
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool No_control_or_object_flow(object diagnostics, object context)
        {
            System.Func<IActionInputPin, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IActionInputPin, object, object, bool>>(_no_control_or_object_flowOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method no_control_or_object_flow registered. Use t" +
                        "he method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _no_control_or_object_flowOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _no_control_or_object_flowOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _no_control_or_object_flowOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveNo_control_or_object_flowOperation()
        {
            return ClassInstance.LookupOperation("no_control_or_object_flow");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveFromActionReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.ActionInputPin.ClassInstance)).Resolve("fromAction")));
        }
        
        /// <summary>
        /// Handles the event that the FromAction property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetFromAction(object sender, System.EventArgs eventArgs)
        {
            this.FromAction = null;
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.FromAction))
            {
                return ModelHelper.CreatePath("fromAction");
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
            if ((reference == "FROMACTION"))
            {
                return this.FromAction;
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
            if ((feature == "FROMACTION"))
            {
                this.FromAction = ((IAction)(value));
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
            if ((reference == "FROMACTION"))
            {
                return new FromActionProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ActionInputPin")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ActionInputPin class
        /// </summary>
        public class ActionInputPinChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ActionInputPin _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ActionInputPinChildrenCollection(ActionInputPin parent)
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
                    if ((this._parent.FromAction != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
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
                if ((this._parent.FromAction == null))
                {
                    IAction fromActionCasted = item.As<IAction>();
                    if ((fromActionCasted != null))
                    {
                        this._parent.FromAction = fromActionCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.FromAction = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.FromAction))
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
                if ((this._parent.FromAction != null))
                {
                    array[arrayIndex] = this._parent.FromAction;
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
                if ((this._parent.FromAction == item))
                {
                    this._parent.FromAction = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.FromAction).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ActionInputPin class
        /// </summary>
        public class ActionInputPinReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ActionInputPin _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ActionInputPinReferencedElementsCollection(ActionInputPin parent)
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
                    if ((this._parent.FromAction != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.BubbledChange += this.PropagateValueChanges;
            }
            
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
                if ((this._parent.FromAction == null))
                {
                    IAction fromActionCasted = item.As<IAction>();
                    if ((fromActionCasted != null))
                    {
                        this._parent.FromAction = fromActionCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.FromAction = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.FromAction))
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
                if ((this._parent.FromAction != null))
                {
                    array[arrayIndex] = this._parent.FromAction;
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
                if ((this._parent.FromAction == item))
                {
                    this._parent.FromAction = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.FromAction).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the fromAction property
        /// </summary>
        private sealed class FromActionProxy : ModelPropertyChange<IActionInputPin, IAction>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public FromActionProxy(IActionInputPin modelElement) : 
                    base(modelElement, "fromAction")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IAction Value
            {
                get
                {
                    return this.ModelElement.FromAction;
                }
                set
                {
                    this.ModelElement.FromAction = value;
                }
            }
        }
    }
}
