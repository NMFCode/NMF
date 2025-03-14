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
    /// A ParameterSet designates alternative sets of inputs or outputs that a Behavior may use.
    ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//ParameterSet")]
    [DebuggerDisplayAttribute("ParameterSet {Name}")]
    public partial class ParameterSet : NamedElement, IParameterSet, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _same_parameterized_entityOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveSame_parameterized_entityOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _inputOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveInputOperation);
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _two_parameter_setsOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveTwo_parameter_setsOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _conditionReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveConditionReference);
        
        /// <summary>
        /// The backing field for the Condition property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IConstraint> _condition;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _parameterReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveParameterReference);
        
        /// <summary>
        /// The backing field for the Parameter property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ParameterSetParameterCollection _parameter;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ParameterSet()
        {
            this._condition = new ObservableCompositionOrderedSet<IConstraint>(this);
            this._condition.CollectionChanging += this.ConditionCollectionChanging;
            this._condition.CollectionChanged += this.ConditionCollectionChanged;
            this._parameter = new ParameterSetParameterCollection(this);
            this._parameter.CollectionChanging += this.ParameterCollectionChanging;
            this._parameter.CollectionChanged += this.ParameterCollectionChanged;
        }
        
        /// <summary>
        /// A constraint that should be satisfied for the owner of the Parameters in an input ParameterSet to start execution using the values provided for those Parameters, or the owner of the Parameters in an output ParameterSet to end execution providing the values for those Parameters, if all preconditions and conditions on input ParameterSets were satisfied.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("condition")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IConstraint> Condition
        {
            get
            {
                return this._condition;
            }
        }
        
        /// <summary>
        /// Parameters in the ParameterSet.
        ///&lt;p&gt;From package UML::Classification.&lt;/p&gt;
        /// </summary>
        [LowerBoundAttribute(1)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("parameter")]
        [DescriptionAttribute("Parameters in the ParameterSet.\n<p>From package UML::Classification.</p>")]
        [CategoryAttribute("ParameterSet")]
        [XmlElementNameAttribute("parameter")]
        [XmlAttributeAttribute(true)]
        [XmlOppositeAttribute("parameterSet")]
        [ConstantAttribute()]
        public ISetExpression<NMF.Interop.Uml.IParameter> Parameter
        {
            get
            {
                return this._parameter;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new ParameterSetChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ParameterSetReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ParameterSet")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// The Parameters in a ParameterSet must all be inputs or all be outputs of the same parameterized entity, and the ParameterSet is owned by that entity.
        ///parameter-&gt;forAll(p1, p2 | self.owner = p1.owner and self.owner = p2.owner and p1.direction = p2.direction)
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Same_parameterized_entity(object diagnostics, object context)
        {
            System.Func<IParameterSet, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IParameterSet, object, object, bool>>(_same_parameterized_entityOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method same_parameterized_entity registered. Use t" +
                        "he method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _same_parameterized_entityOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _same_parameterized_entityOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _same_parameterized_entityOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveSame_parameterized_entityOperation()
        {
            return ClassInstance.LookupOperation("same_parameterized_entity");
        }
        
        /// <summary>
        /// If a parameterized entity has input Parameters that are in a ParameterSet, then any inputs that are not in a ParameterSet must be streaming. Same for output Parameters.
        ///((parameter-&gt;exists(direction = ParameterDirectionKind::_&apos;in&apos;)) implies 
        ///    behavioralFeature.ownedParameter-&gt;select(p | p.direction = ParameterDirectionKind::_&apos;in&apos; and p.parameterSet-&gt;isEmpty())-&gt;forAll(isStream))
        ///    and
        ///((parameter-&gt;exists(direction = ParameterDirectionKind::out)) implies 
        ///    behavioralFeature.ownedParameter-&gt;select(p | p.direction = ParameterDirectionKind::out and p.parameterSet-&gt;isEmpty())-&gt;forAll(isStream))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Input(object diagnostics, object context)
        {
            System.Func<IParameterSet, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IParameterSet, object, object, bool>>(_inputOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method input registered. Use the method broker to " +
                        "register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _inputOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _inputOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _inputOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveInputOperation()
        {
            return ClassInstance.LookupOperation("input");
        }
        
        /// <summary>
        /// Two ParameterSets cannot have exactly the same set of Parameters.
        ///parameter-&gt;forAll(parameterSet-&gt;forAll(s1, s2 | s1-&gt;size() = s2-&gt;size() implies s1.parameter-&gt;exists(p | not s2.parameter-&gt;includes(p))))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool Two_parameter_sets(object diagnostics, object context)
        {
            System.Func<IParameterSet, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IParameterSet, object, object, bool>>(_two_parameter_setsOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method two_parameter_sets registered. Use the meth" +
                        "od broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _two_parameter_setsOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _two_parameter_setsOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _two_parameter_setsOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveTwo_parameter_setsOperation()
        {
            return ClassInstance.LookupOperation("two_parameter_sets");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveConditionReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.ParameterSet.ClassInstance)).Resolve("condition")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Condition property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ConditionCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Condition", e, _conditionReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Condition property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ConditionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Condition", e, _conditionReference);
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveParameterReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.ParameterSet.ClassInstance)).Resolve("parameter")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Parameter property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ParameterCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Parameter", e, _parameterReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Parameter property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ParameterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Parameter", e, _parameterReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int conditionIndex = ModelHelper.IndexOfReference(this.Condition, element);
            if ((conditionIndex != -1))
            {
                return ModelHelper.CreatePath("condition", conditionIndex);
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
            if ((reference == "CONDITION"))
            {
                if ((index < this.Condition.Count))
                {
                    return this.Condition[index];
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
            if ((feature == "CONDITION"))
            {
                return this._condition;
            }
            if ((feature == "PARAMETER"))
            {
                return this._parameter;
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
            if ((container == this._condition))
            {
                return "condition";
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//ParameterSet")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ParameterSet class
        /// </summary>
        public class ParameterSetChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ParameterSet _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ParameterSetChildrenCollection(ParameterSet parent)
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
                    count = (count + this._parent.Condition.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.Condition.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.Condition.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IConstraint conditionCasted = item.As<IConstraint>();
                if ((conditionCasted != null))
                {
                    this._parent.Condition.Add(conditionCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Condition.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.Condition.Contains(item))
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
                IEnumerator<IModelElement> conditionEnumerator = this._parent.Condition.GetEnumerator();
                try
                {
                    for (
                    ; conditionEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = conditionEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    conditionEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IConstraint constraintItem = item.As<IConstraint>();
                if (((constraintItem != null) 
                            && this._parent.Condition.Remove(constraintItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Condition).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the ParameterSet class
        /// </summary>
        public class ParameterSetReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private ParameterSet _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ParameterSetReferencedElementsCollection(ParameterSet parent)
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
                    count = (count + this._parent.Condition.Count);
                    count = (count + this._parent.Parameter.Count);
                    return count;
                }
            }
            
            /// <summary>
            /// Registers event hooks to keep the collection up to date
            /// </summary>
            protected override void AttachCore()
            {
                this._parent.Condition.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.Parameter.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Unregisters all event hooks registered by AttachCore
            /// </summary>
            protected override void DetachCore()
            {
                this._parent.Condition.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.Parameter.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IConstraint conditionCasted = item.As<IConstraint>();
                if ((conditionCasted != null))
                {
                    this._parent.Condition.Add(conditionCasted);
                }
                NMF.Interop.Uml.IParameter parameterCasted = item.As<NMF.Interop.Uml.IParameter>();
                if ((parameterCasted != null))
                {
                    this._parent.Parameter.Add(parameterCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Condition.Clear();
                this._parent.Parameter.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.Condition.Contains(item))
                {
                    return true;
                }
                if (this._parent.Parameter.Contains(item))
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
                IEnumerator<IModelElement> conditionEnumerator = this._parent.Condition.GetEnumerator();
                try
                {
                    for (
                    ; conditionEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = conditionEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    conditionEnumerator.Dispose();
                }
                IEnumerator<IModelElement> parameterEnumerator = this._parent.Parameter.GetEnumerator();
                try
                {
                    for (
                    ; parameterEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = parameterEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    parameterEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IConstraint constraintItem = item.As<IConstraint>();
                if (((constraintItem != null) 
                            && this._parent.Condition.Remove(constraintItem)))
                {
                    return true;
                }
                NMF.Interop.Uml.IParameter parameterItem = item.As<NMF.Interop.Uml.IParameter>();
                if (((parameterItem != null) 
                            && this._parent.Parameter.Remove(parameterItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Condition).Concat(this._parent.Parameter).GetEnumerator();
            }
        }
    }
}
