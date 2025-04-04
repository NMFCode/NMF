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
    /// A Reception is a declaration stating that a Classifier is prepared to react to the receipt of a Signal.
    ///&lt;p&gt;From package UML::SimpleClassifiers.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Reception")]
    [DebuggerDisplayAttribute("Reception {Name}")]
    public partial class Reception : BehavioralFeature, IReception, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _same_name_as_signalOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveSame_name_as_signalOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _same_structure_as_signalOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveSame_structure_as_signalOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _signalReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveSignalReference);
        
        /// <summary>
        /// The backing field for the Signal property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ISignal _signal;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The Signal that this Reception handles.
        ///&lt;p&gt;From package UML::SimpleClassifiers.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("signal")]
        [DescriptionAttribute("The Signal that this Reception handles.\n<p>From package UML::SimpleClassifiers.</" +
            "p>")]
        [CategoryAttribute("Reception")]
        [XmlElementNameAttribute("signal")]
        [XmlAttributeAttribute(true)]
        public ISignal Signal
        {
            get
            {
                return this._signal;
            }
            set
            {
                if ((this._signal != value))
                {
                    ISignal old = this._signal;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Signal", e, _signalReference);
                    this._signal = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetSignal;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetSignal;
                    }
                    this.OnPropertyChanged("Signal", e, _signalReference);
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
                return base.ReferencedElements.Concat(new ReceptionReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Reception")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// A Reception has the same name as its signal
        ///name = signal.name
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Same_name_as_signal(object diagnostics, object context)
        {
            System.Func<IReception, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IReception, object, object, bool>>(_same_name_as_signalOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method same_name_as_signal registered. Use the met" +
                        "hod broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _same_name_as_signalOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _same_name_as_signalOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _same_name_as_signalOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveSame_name_as_signalOperation()
        {
            return ClassInstance.LookupOperation("same_name_as_signal");
        }
        
        /// <summary>
        /// A Reception&apos;s parameters match the ownedAttributes of its signal by name, type, and multiplicity
        ///signal.ownedAttribute-&gt;size() = ownedParameter-&gt;size() and
        ///Sequence{1..signal.ownedAttribute-&gt;size()}-&gt;forAll( i | 
        ///    ownedParameter-&gt;at(i).direction = ParameterDirectionKind::_&apos;in&apos; and 
        ///    ownedParameter-&gt;at(i).name = signal.ownedAttribute-&gt;at(i).name and
        ///    ownedParameter-&gt;at(i).type = signal.ownedAttribute-&gt;at(i).type and
        ///    ownedParameter-&gt;at(i).lowerBound() = signal.ownedAttribute-&gt;at(i).lowerBound() and
        ///    ownedParameter-&gt;at(i).upperBound() = signal.ownedAttribute-&gt;at(i).upperBound()
        ///)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Same_structure_as_signal(object diagnostics, object context)
        {
            System.Func<IReception, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IReception, object, object, bool>>(_same_structure_as_signalOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method same_structure_as_signal registered. Use th" +
                        "e method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _same_structure_as_signalOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _same_structure_as_signalOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _same_structure_as_signalOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveSame_structure_as_signalOperation()
        {
            return ClassInstance.LookupOperation("same_structure_as_signal");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveSignalReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Reception.ClassInstance)).Resolve("signal")));
        }
        
        /// <summary>
        /// Handles the event that the Signal property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetSignal(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.Signal))
            {
                this.Signal = null;
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
            if ((reference == "SIGNAL"))
            {
                return this.Signal;
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
            if ((feature == "SIGNAL"))
            {
                this.Signal = ((ISignal)(value));
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
            if ((reference == "SIGNAL"))
            {
                return new SignalProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Reception")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Reception class
        /// </summary>
        public class ReceptionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Reception _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ReceptionReferencedElementsCollection(Reception parent)
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
                    if ((this._parent.Signal != null))
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
                if ((this._parent.Signal == null))
                {
                    ISignal signalCasted = item.As<ISignal>();
                    if ((signalCasted != null))
                    {
                        this._parent.Signal = signalCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Signal = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Signal))
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
                if ((this._parent.Signal != null))
                {
                    array[arrayIndex] = this._parent.Signal;
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
                if ((this._parent.Signal == item))
                {
                    this._parent.Signal = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Signal).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the signal property
        /// </summary>
        private sealed class SignalProxy : ModelPropertyChange<IReception, ISignal>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SignalProxy(IReception modelElement) : 
                    base(modelElement, "signal")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override ISignal Value
            {
                get
                {
                    return this.ModelElement.Signal;
                }
                set
                {
                    this.ModelElement.Signal = value;
                }
            }
        }
    }
}
