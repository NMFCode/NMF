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
    /// VariableAction is an abstract class for Actions that operate on a specified Variable.
    ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//VariableAction")]
    [DebuggerDisplayAttribute("VariableAction {Name}")]
    public abstract partial class VariableAction : Action, IVariableAction, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _scope_of_variableOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveScope_of_variableOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _variableReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveVariableReference);
        
        /// <summary>
        /// The backing field for the Variable property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IVariable _variable;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// The Variable to be read or written.
        ///&lt;p&gt;From package UML::Actions.&lt;/p&gt;
        /// </summary>
        [DisplayNameAttribute("variable")]
        [DescriptionAttribute("The Variable to be read or written.\n<p>From package UML::Actions.</p>")]
        [CategoryAttribute("VariableAction")]
        [XmlElementNameAttribute("variable")]
        [XmlAttributeAttribute(true)]
        public IVariable Variable
        {
            get
            {
                return this._variable;
            }
            set
            {
                if ((this._variable != value))
                {
                    IVariable old = this._variable;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Variable", e, _variableReference);
                    this._variable = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetVariable;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetVariable;
                    }
                    this.OnPropertyChanged("Variable", e, _variableReference);
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
                return base.ReferencedElements.Concat(new VariableActionReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//VariableAction")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The VariableAction must be in the scope of the variable.
        ///variable.isAccessibleBy(self)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Scope_of_variable(object diagnostics, object context)
        {
            System.Func<IVariableAction, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IVariableAction, object, object, bool>>(_scope_of_variableOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method scope_of_variable registered. Use the metho" +
                        "d broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _scope_of_variableOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _scope_of_variableOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _scope_of_variableOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveScope_of_variableOperation()
        {
            return ClassInstance.LookupOperation("scope_of_variable");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveVariableReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.VariableAction.ClassInstance)).Resolve("variable")));
        }
        
        /// <summary>
        /// Handles the event that the Variable property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetVariable(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.Variable))
            {
                this.Variable = null;
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
            if ((reference == "VARIABLE"))
            {
                return this.Variable;
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
            if ((feature == "VARIABLE"))
            {
                this.Variable = ((IVariable)(value));
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
            if ((reference == "VARIABLE"))
            {
                return new VariableProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//VariableAction")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the VariableAction class
        /// </summary>
        public class VariableActionReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private VariableAction _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public VariableActionReferencedElementsCollection(VariableAction parent)
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
                    if ((this._parent.Variable != null))
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
                if ((this._parent.Variable == null))
                {
                    IVariable variableCasted = item.As<IVariable>();
                    if ((variableCasted != null))
                    {
                        this._parent.Variable = variableCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Variable = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Variable))
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
                if ((this._parent.Variable != null))
                {
                    array[arrayIndex] = this._parent.Variable;
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
                if ((this._parent.Variable == item))
                {
                    this._parent.Variable = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Variable).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the variable property
        /// </summary>
        private sealed class VariableProxy : ModelPropertyChange<IVariableAction, IVariable>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public VariableProxy(IVariableAction modelElement) : 
                    base(modelElement, "variable")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IVariable Value
            {
                get
                {
                    return this.ModelElement.Variable;
                }
                set
                {
                    this.ModelElement.Variable = value;
                }
            }
        }
    }
}
