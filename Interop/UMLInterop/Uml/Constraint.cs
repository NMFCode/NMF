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
    /// A Constraint is a condition or restriction expressed in natural language text or in a machine readable language for the purpose of declaring some of the semantics of an Element or set of Elements.
    ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Constraint")]
    [DebuggerDisplayAttribute("Constraint {Name}")]
    public partial class Constraint : PackageableElement, IConstraint, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _boolean_valueOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveBoolean_valueOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _no_side_effectsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveNo_side_effectsOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _not_apply_to_selfOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveNot_apply_to_selfOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _constrainedElementReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveConstrainedElementReference);
        
        /// <summary>
        /// The backing field for the ConstrainedElement property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationOrderedSet<IElement> _constrainedElement;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _contextReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveContextReference);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _specificationReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveSpecificationReference);
        
        /// <summary>
        /// The backing field for the Specification property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IValueSpecification _specification;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Constraint()
        {
            this._constrainedElement = new ObservableAssociationOrderedSet<IElement>();
            this._constrainedElement.CollectionChanging += this.ConstrainedElementCollectionChanging;
            this._constrainedElement.CollectionChanged += this.ConstrainedElementCollectionChanged;
        }
        
        /// <summary>
        /// The ordered set of Elements referenced by this Constraint.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("constrainedElement")]
        [DescriptionAttribute("The ordered set of Elements referenced by this Constraint.\n<p>From package UML::C" +
            "ommonStructure.</p>")]
        [CategoryAttribute("Constraint")]
        [XmlElementNameAttribute("constrainedElement")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public IOrderedSetExpression<IElement> ConstrainedElement
        {
            get
            {
                return this._constrainedElement;
            }
        }
        
        /// <summary>
        /// Specifies the Namespace that owns the Constraint.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("context")]
        [XmlAttributeAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        [XmlOppositeAttribute("ownedRule")]
        public NMF.Interop.Uml.INamespace Context
        {
            get
            {
                return ModelHelper.CastAs<NMF.Interop.Uml.INamespace>(this.Parent);
            }
            set
            {
                this.Parent = value;
            }
        }
        
        /// <summary>
        /// A condition that must be true when evaluated in order for the Constraint to be satisfied.
        ///&lt;p&gt;From package UML::CommonStructure.&lt;/p&gt;
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("specification")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IValueSpecification Specification
        {
            get
            {
                return this._specification;
            }
            set
            {
                if ((this._specification != value))
                {
                    IValueSpecification old = this._specification;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Specification", e, _specificationReference);
                    this._specification = value;
                    if ((old != null))
                    {
                        if ((old.Parent == this))
                        {
                            old.Parent = null;
                        }
                        old.ParentChanged -= this.OnResetSpecification;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetSpecification;
                    }
                    this.OnPropertyChanged("Specification", e, _specificationReference);
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
                return base.Children.Concat(new ConstraintChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ConstraintReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Constraint")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The ValueSpecification for a Constraint must evaluate to a Boolean value.
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Boolean_value(object diagnostics, object context)
        {
            System.Func<IConstraint, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IConstraint, object, object, bool>>(_boolean_valueOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method boolean_value registered. Use the method br" +
                        "oker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _boolean_valueOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _boolean_valueOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _boolean_valueOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveBoolean_valueOperation()
        {
            return ClassInstance.LookupOperation("boolean_value");
        }
        
        /// <summary>
        /// Evaluating the ValueSpecification for a Constraint must not have side effects.
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool No_side_effects(object diagnostics, object context)
        {
            System.Func<IConstraint, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IConstraint, object, object, bool>>(_no_side_effectsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method no_side_effects registered. Use the method " +
                        "broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _no_side_effectsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _no_side_effectsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _no_side_effectsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveNo_side_effectsOperation()
        {
            return ClassInstance.LookupOperation("no_side_effects");
        }
        
        /// <summary>
        /// A Constraint cannot be applied to itself.
        ///not constrainedElement-&gt;includes(self)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Not_apply_to_self(object diagnostics, object context)
        {
            System.Func<IConstraint, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IConstraint, object, object, bool>>(_not_apply_to_selfOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method not_apply_to_self registered. Use the metho" +
                        "d broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _not_apply_to_selfOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _not_apply_to_selfOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _not_apply_to_selfOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveNot_apply_to_selfOperation()
        {
            return ClassInstance.LookupOperation("not_apply_to_self");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveConstrainedElementReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Constraint.ClassInstance)).Resolve("constrainedElement")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the ConstrainedElement property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ConstrainedElementCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("ConstrainedElement", e, _constrainedElementReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the ConstrainedElement property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ConstrainedElementCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("ConstrainedElement", e, _constrainedElementReference);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveContextReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Constraint.ClassInstance)).Resolve("context")));
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element is about to change
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanging(IModelElement newParent, IModelElement oldParent)
        {
            NMF.Interop.Uml.INamespace oldContext = ModelHelper.CastAs<NMF.Interop.Uml.INamespace>(oldParent);
            NMF.Interop.Uml.INamespace newContext = ModelHelper.CastAs<NMF.Interop.Uml.INamespace>(newParent);
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldContext, newContext);
            this.OnPropertyChanging("Context", e, _contextReference);
        }
        
        /// <summary>
        /// Gets called when the parent model element of the current model element changes
        /// </summary>
        /// <param name="oldParent">The old parent model element</param>
        /// <param name="newParent">The new parent model element</param>
        protected override void OnParentChanged(IModelElement newParent, IModelElement oldParent)
        {
            NMF.Interop.Uml.INamespace oldContext = ModelHelper.CastAs<NMF.Interop.Uml.INamespace>(oldParent);
            NMF.Interop.Uml.INamespace newContext = ModelHelper.CastAs<NMF.Interop.Uml.INamespace>(newParent);
            if ((oldContext != null))
            {
                oldContext.OwnedRule.Remove(this);
            }
            if ((newContext != null))
            {
                newContext.OwnedRule.Add(this);
            }
            ValueChangedEventArgs e = new ValueChangedEventArgs(oldContext, newContext);
            this.OnPropertyChanged("Context", e, _contextReference);
            base.OnParentChanged(newParent, oldParent);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveSpecificationReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Constraint.ClassInstance)).Resolve("specification")));
        }
        
        /// <summary>
        /// Handles the event that the Specification property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetSpecification(object sender, System.EventArgs eventArgs)
        {
            if ((sender == this.Specification))
            {
                this.Specification = null;
            }
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.Specification))
            {
                return ModelHelper.CreatePath("specification");
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
            if ((reference == "CONSTRAINEDELEMENT"))
            {
                if ((index < this.ConstrainedElement.Count))
                {
                    return this.ConstrainedElement[index];
                }
                else
                {
                    return null;
                }
            }
            if ((reference == "CONTEXT"))
            {
                return this.Context;
            }
            if ((reference == "SPECIFICATION"))
            {
                return this.Specification;
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
            if ((feature == "CONSTRAINEDELEMENT"))
            {
                return this._constrainedElement;
            }
            return base.GetCollectionForFeature(feature);
        }
        
        /// <summary>
        /// Sets a value to the given feature
        /// </summary>
        /// <param name="feature">The requested feature</param>
        /// <param name="value">The value that should be set to that feature</param>
        protected override void SetFeature(string feature, object value)
        {
            if ((feature == "CONTEXT"))
            {
                this.Context = ((NMF.Interop.Uml.INamespace)(value));
                return;
            }
            if ((feature == "SPECIFICATION"))
            {
                this.Specification = ((IValueSpecification)(value));
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
            if ((reference == "CONTEXT"))
            {
                return new ContextProxy(this);
            }
            if ((reference == "SPECIFICATION"))
            {
                return new SpecificationProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Constraint")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Constraint class
        /// </summary>
        public class ConstraintChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Constraint _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ConstraintChildrenCollection(Constraint parent)
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
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Constraint class
        /// </summary>
        public class ConstraintReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Constraint _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ConstraintReferencedElementsCollection(Constraint parent)
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
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Copies the contents of the collection to the given array starting from the given array index
            /// </summary>
            /// <param name="array">The array in which the elements should be copied</param>
            /// <param name="arrayIndex">The starting index</param>
            public override void CopyTo(IModelElement[] array, int arrayIndex)
            {
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                return false;
            }
            
            /// <summary>
            /// Gets an enumerator that enumerates the collection
            /// </summary>
            /// <returns>A generic enumerator</returns>
            public override IEnumerator<IModelElement> GetEnumerator()
            {
                return Enumerable.Empty<IModelElement>().GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the context property
        /// </summary>
        private sealed class ContextProxy : ModelPropertyChange<IConstraint, NMF.Interop.Uml.INamespace>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ContextProxy(IConstraint modelElement) : 
                    base(modelElement, "context")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override NMF.Interop.Uml.INamespace Value
            {
                get
                {
                    return this.ModelElement.Context;
                }
                set
                {
                    this.ModelElement.Context = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the specification property
        /// </summary>
        private sealed class SpecificationProxy : ModelPropertyChange<IConstraint, IValueSpecification>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SpecificationProxy(IConstraint modelElement) : 
                    base(modelElement, "specification")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IValueSpecification Value
            {
                get
                {
                    return this.ModelElement.Specification;
                }
                set
                {
                    this.ModelElement.Specification = value;
                }
            }
        }
    }
}
