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
    /// WriteVariableAction is an abstract class for VariableActions that change Variable values.
    ///<p>From package UML::Actions.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//WriteVariableAction")]
    [DebuggerDisplayAttribute("WriteVariableAction {Name}")]
    public abstract partial class WriteVariableAction : VariableAction, IWriteVariableAction, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _value_typeOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveValue_typeOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _multiplicityOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveMultiplicityOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _valueReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveValueReference);
        
        /// <summary>
        /// The backing field for the Value property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IInputPin _value;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The InputPin that gives the value to be added or removed from the Variable.
        ///<p>From package UML::Actions.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("value")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IInputPin Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if ((this._value != value))
                {
                    IInputPin old = this._value;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Value", e, _valueReference);
                    this._value = value;
                    if ((old != null))
                    {
                        old.Parent = null;
                        old.ParentChanged -= this.OnResetValue;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetValue;
                    }
                    this.OnPropertyChanged("Value", e, _valueReference);
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
                return base.Children.Concat(new WriteVariableActionChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new WriteVariableActionReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//WriteVariableAction")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The type of the value InputPin must conform to the type of the variable.
        ///value <> null implies value.type.conformsTo(variable.type)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Value_type(object diagnostics, object context)
        {
            System.Func<IWriteVariableAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IWriteVariableAction, object, object, bool>>(_value_typeOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method value_type registered. Use the method broke" +
                        "r to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _value_typeOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _value_typeOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _value_typeOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveValue_typeOperation()
        {
            return ClassInstance.LookupOperation("value_type");
        }
        
        /// <summary>
        /// The multiplicity of the value InputPin is 1..1.
        ///value<>null implies value.is(1,1)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Multiplicity(object diagnostics, object context)
        {
            System.Func<IWriteVariableAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IWriteVariableAction, object, object, bool>>(_multiplicityOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method multiplicity registered. Use the method bro" +
                        "ker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _multiplicityOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _multiplicityOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _multiplicityOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveMultiplicityOperation()
        {
            return ClassInstance.LookupOperation("multiplicity");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveValueReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.WriteVariableAction.ClassInstance)).Resolve("value")));
        }
        
        /// <summary>
        /// Handles the event that the Value property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetValue(object sender, System.EventArgs eventArgs)
        {
            this.Value = null;
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.Value))
            {
                return ModelHelper.CreatePath("value");
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
            if ((reference == "VALUE"))
            {
                return this.Value;
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
            if ((feature == "VALUE"))
            {
                this.Value = ((IInputPin)(value));
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
            if ((reference == "VALUE"))
            {
                return new ValueProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//WriteVariableAction")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the WriteVariableAction class
        /// </summary>
        public class WriteVariableActionChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private WriteVariableAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public WriteVariableActionChildrenCollection(WriteVariableAction parent)
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
                    if ((this._parent.Value != null))
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
                if ((this._parent.Value == null))
                {
                    IInputPin valueCasted = item.As<IInputPin>();
                    if ((valueCasted != null))
                    {
                        this._parent.Value = valueCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Value = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Value))
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
                if ((this._parent.Value != null))
                {
                    array[arrayIndex] = this._parent.Value;
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
                if ((this._parent.Value == item))
                {
                    this._parent.Value = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Value).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the WriteVariableAction class
        /// </summary>
        public class WriteVariableActionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private WriteVariableAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public WriteVariableActionReferencedElementsCollection(WriteVariableAction parent)
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
                    if ((this._parent.Value != null))
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
                if ((this._parent.Value == null))
                {
                    IInputPin valueCasted = item.As<IInputPin>();
                    if ((valueCasted != null))
                    {
                        this._parent.Value = valueCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Value = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Value))
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
                if ((this._parent.Value != null))
                {
                    array[arrayIndex] = this._parent.Value;
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
                if ((this._parent.Value == item))
                {
                    this._parent.Value = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Value).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the value property
        /// </summary>
        private sealed class ValueProxy : ModelPropertyChange<IWriteVariableAction, IInputPin>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ValueProxy(IWriteVariableAction modelElement) : 
                    base(modelElement, "value")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IInputPin Value
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
}
