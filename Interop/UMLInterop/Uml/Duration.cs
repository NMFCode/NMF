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
    /// A Duration is a ValueSpecification that specifies the temporal distance between two time instants.
    ///<p>From package UML::Values.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Duration")]
    [DebuggerDisplayAttribute("Duration {Name}")]
    public partial class Duration : ValueSpecification, IDuration, IModelElement
    {
        
        private static System.Lazy<NMF.Models.Meta.IOperation> _no_expr_requires_observationOperation = new System.Lazy<NMF.Models.Meta.IOperation>(RetrieveNo_expr_requires_observationOperation);
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _exprReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveExprReference);
        
        /// <summary>
        /// The backing field for the Expr property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IValueSpecification _expr;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _observationReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveObservationReference);
        
        /// <summary>
        /// The backing field for the Observation property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableAssociationSet<IObservation> _observation;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        public Duration()
        {
            this._observation = new ObservableAssociationSet<IObservation>();
            this._observation.CollectionChanging += this.ObservationCollectionChanging;
            this._observation.CollectionChanged += this.ObservationCollectionChanged;
        }
        
        /// <summary>
        /// A ValueSpecification that evaluates to the value of the Duration.
        ///<p>From package UML::Values.</p>
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("expr")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IValueSpecification Expr
        {
            get
            {
                return this._expr;
            }
            set
            {
                if ((this._expr != value))
                {
                    IValueSpecification old = this._expr;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Expr", e, _exprReference);
                    this._expr = value;
                    if ((old != null))
                    {
                        old.Parent = null;
                        old.ParentChanged -= this.OnResetExpr;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetExpr;
                    }
                    this.OnPropertyChanged("Expr", e, _exprReference);
                }
            }
        }
        
        /// <summary>
        /// Refers to the Observations that are involved in the computation of the Duration value
        ///<p>From package UML::Values.</p>
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [DisplayNameAttribute("observation")]
        [DescriptionAttribute("Refers to the Observations that are involved in the computation of the Duration v" +
            "alue\n<p>From package UML::Values.</p>")]
        [CategoryAttribute("Duration")]
        [XmlElementNameAttribute("observation")]
        [XmlAttributeAttribute(true)]
        [ConstantAttribute()]
        public ISetExpression<IObservation> Observation
        {
            get
            {
                return this._observation;
            }
        }
        
        /// <summary>
        /// Gets the child model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> Children
        {
            get
            {
                return base.Children.Concat(new DurationChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new DurationReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Duration")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// If a Duration has no expr, then it must have a single observation that is a DurationObservation.
        ///expr = null implies (observation->size() = 1 and observation->forAll(oclIsKindOf(DurationObservation)))
        /// </summary>
        /// <param name="diagnostics">The chain of diagnostics to which problems are to be appended.</param>
        /// <param name="context">The cache of context-specific information.</param>
        public bool No_expr_requires_observation(object diagnostics, object context)
        {
            System.Func<IDuration, object, object, bool> handler = OperationBroker.Instance.GetRegisteredDelegate<System.Func<IDuration, object, object, bool>>(_no_expr_requires_observationOperation);
            if ((handler != null))
            {
            }
            else
            {
                throw new System.InvalidOperationException("There is no implementation for method no_expr_requires_observation registered. Us" +
                        "e the method broker to register a method implementation.");
            }
            OperationCallEventArgs e = new OperationCallEventArgs(this, _no_expr_requires_observationOperation.Value, diagnostics, context);
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalling(this, _no_expr_requires_observationOperation.Value, e));
            bool result = handler.Invoke(this, diagnostics, context);
            e.Result = result;
            this.OnBubbledChange(BubbledChangeEventArgs.OperationCalled(this, _no_expr_requires_observationOperation.Value, e));
            return result;
        }
        
        private static NMF.Models.Meta.IOperation RetrieveNo_expr_requires_observationOperation()
        {
            return ClassInstance.LookupOperation("no_expr_requires_observation");
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveExprReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Duration.ClassInstance)).Resolve("expr")));
        }
        
        /// <summary>
        /// Handles the event that the Expr property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetExpr(object sender, System.EventArgs eventArgs)
        {
            this.Expr = null;
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveObservationReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Duration.ClassInstance)).Resolve("observation")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the Observation property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ObservationCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("Observation", e, _observationReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the Observation property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void ObservationCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("Observation", e, _observationReference);
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.Expr))
            {
                return ModelHelper.CreatePath("expr");
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
            if ((reference == "EXPR"))
            {
                return this.Expr;
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
            if ((feature == "OBSERVATION"))
            {
                return this._observation;
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
            if ((feature == "EXPR"))
            {
                this.Expr = ((IValueSpecification)(value));
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
            if ((reference == "EXPR"))
            {
                return new ExprProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Duration")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Duration class
        /// </summary>
        public class DurationChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Duration _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public DurationChildrenCollection(Duration parent)
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
                    if ((this._parent.Expr != null))
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
                if ((this._parent.Expr == null))
                {
                    IValueSpecification exprCasted = item.As<IValueSpecification>();
                    if ((exprCasted != null))
                    {
                        this._parent.Expr = exprCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Expr = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Expr))
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
                if ((this._parent.Expr != null))
                {
                    array[arrayIndex] = this._parent.Expr;
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
                if ((this._parent.Expr == item))
                {
                    this._parent.Expr = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Expr).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Duration class
        /// </summary>
        public class DurationReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Duration _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public DurationReferencedElementsCollection(Duration parent)
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
                    if ((this._parent.Expr != null))
                    {
                        count = (count + 1);
                    }
                    count = (count + this._parent.Observation.Count);
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.BubbledChange += this.PropagateValueChanges;
                this._parent.Observation.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.BubbledChange -= this.PropagateValueChanges;
                this._parent.Observation.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.Expr == null))
                {
                    IValueSpecification exprCasted = item.As<IValueSpecification>();
                    if ((exprCasted != null))
                    {
                        this._parent.Expr = exprCasted;
                        return;
                    }
                }
                IObservation observationCasted = item.As<IObservation>();
                if ((observationCasted != null))
                {
                    this._parent.Observation.Add(observationCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Expr = null;
                this._parent.Observation.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Expr))
                {
                    return true;
                }
                if (this._parent.Observation.Contains(item))
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
                if ((this._parent.Expr != null))
                {
                    array[arrayIndex] = this._parent.Expr;
                    arrayIndex = (arrayIndex + 1);
                }
                IEnumerator<IModelElement> observationEnumerator = this._parent.Observation.GetEnumerator();
                try
                {
                    for (
                    ; observationEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = observationEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    observationEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                if ((this._parent.Expr == item))
                {
                    this._parent.Expr = null;
                    return true;
                }
                IObservation observationItem = item.As<IObservation>();
                if (((observationItem != null) 
                            && this._parent.Observation.Remove(observationItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Expr).Concat(this._parent.Observation).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the expr property
        /// </summary>
        private sealed class ExprProxy : ModelPropertyChange<IDuration, IValueSpecification>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public ExprProxy(IDuration modelElement) : 
                    base(modelElement, "expr")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IValueSpecification Value
            {
                get
                {
                    return this.ModelElement.Expr;
                }
                set
                {
                    this.ModelElement.Expr = value;
                }
            }
        }
    }
}
