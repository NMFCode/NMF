//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.21
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Expressions.Linq;
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
using System.Linq;

namespace NMF.Glsp.Notation
{
    
    
    /// <summary>
    /// The default implementation of the Edge class
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/glsp/notation")]
    [XmlNamespacePrefixAttribute("notation")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/glsp/notation#//Edge")]
    public partial class Edge : NotationElement, IEdge, IModelElement
    {
        
        private static Lazy<ITypedElement> _bendPointsReference = new Lazy<ITypedElement>(RetrieveBendPointsReference);
        
        /// <summary>
        /// The backing field for the BendPoints property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private ObservableCompositionOrderedSet<IGPoint> _bendPoints;
        
        private static Lazy<ITypedElement> _sourceReference = new Lazy<ITypedElement>(RetrieveSourceReference);
        
        /// <summary>
        /// The backing field for the Source property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private INotationElement _source;
        
        private static Lazy<ITypedElement> _targetReference = new Lazy<ITypedElement>(RetrieveTargetReference);
        
        /// <summary>
        /// The backing field for the Target property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private INotationElement _target;
        
        private static IClass _classInstance;
        
        public Edge()
        {
            this._bendPoints = new ObservableCompositionOrderedSet<IGPoint>(this);
            this._bendPoints.CollectionChanging += this.BendPointsCollectionChanging;
            this._bendPoints.CollectionChanged += this.BendPointsCollectionChanged;
        }
        
        /// <summary>
        /// The bendPoints property
        /// </summary>
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("bendPoints")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        [ConstantAttribute()]
        public IOrderedSetExpression<IGPoint> BendPoints
        {
            get
            {
                return this._bendPoints;
            }
        }
        
        /// <summary>
        /// The source property
        /// </summary>
        [DisplayNameAttribute("source")]
        [CategoryAttribute("Edge")]
        [XmlElementNameAttribute("source")]
        [XmlAttributeAttribute(true)]
        public INotationElement Source
        {
            get
            {
                return this._source;
            }
            set
            {
                if ((this._source != value))
                {
                    INotationElement old = this._source;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnSourceChanging(e);
                    this.OnPropertyChanging("Source", e, _sourceReference);
                    this._source = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetSource;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetSource;
                    }
                    this.OnSourceChanged(e);
                    this.OnPropertyChanged("Source", e, _sourceReference);
                }
            }
        }
        
        /// <summary>
        /// The target property
        /// </summary>
        [DisplayNameAttribute("target")]
        [CategoryAttribute("Edge")]
        [XmlElementNameAttribute("target")]
        [XmlAttributeAttribute(true)]
        public INotationElement Target
        {
            get
            {
                return this._target;
            }
            set
            {
                if ((this._target != value))
                {
                    INotationElement old = this._target;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnTargetChanging(e);
                    this.OnPropertyChanging("Target", e, _targetReference);
                    this._target = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetTarget;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetTarget;
                    }
                    this.OnTargetChanged(e);
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
                return base.Children.Concat(new EdgeChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new EdgeReferencedElementsCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the Class model for this type
        /// </summary>
        public new static IClass ClassInstance
        {
            get
            {
                if ((_classInstance == null))
                {
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/glsp/notation#//Edge")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets fired before the Source property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> SourceChanging;
        
        /// <summary>
        /// Gets fired when the Source property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> SourceChanged;
        
        /// <summary>
        /// Gets fired before the Target property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> TargetChanging;
        
        /// <summary>
        /// Gets fired when the Target property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> TargetChanged;
        
        private static ITypedElement RetrieveBendPointsReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.Glsp.Notation.Edge.ClassInstance)).Resolve("bendPoints")));
        }
        
        /// <summary>
        /// Forwards CollectionChanging notifications for the BendPoints property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void BendPointsCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanging("BendPoints", e, _bendPointsReference);
        }
        
        /// <summary>
        /// Forwards CollectionChanged notifications for the BendPoints property to the parent model element
        /// </summary>
        /// <param name="sender">The collection that raised the change</param>
        /// <param name="e">The original event data</param>
        private void BendPointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged("BendPoints", e, _bendPointsReference);
        }
        
        private static ITypedElement RetrieveSourceReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.Glsp.Notation.Edge.ClassInstance)).Resolve("source")));
        }
        
        /// <summary>
        /// Raises the SourceChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSourceChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.SourceChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the SourceChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSourceChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.SourceChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Handles the event that the Source property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetSource(object sender, System.EventArgs eventArgs)
        {
            this.Source = null;
        }
        
        private static ITypedElement RetrieveTargetReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.Glsp.Notation.Edge.ClassInstance)).Resolve("target")));
        }
        
        /// <summary>
        /// Raises the TargetChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnTargetChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.TargetChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the TargetChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnTargetChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.TargetChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Handles the event that the Target property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetTarget(object sender, System.EventArgs eventArgs)
        {
            this.Target = null;
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            int bendPointsIndex = ModelHelper.IndexOfReference(this.BendPoints, element);
            if ((bendPointsIndex != -1))
            {
                return ModelHelper.CreatePath("bendPoints", bendPointsIndex);
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
            if ((reference == "BENDPOINTS"))
            {
                if ((index < this.BendPoints.Count))
                {
                    return this.BendPoints[index];
                }
                else
                {
                    return null;
                }
            }
            if ((reference == "SOURCE"))
            {
                return this.Source;
            }
            if ((reference == "TARGET"))
            {
                return this.Target;
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
            if ((feature == "BENDPOINTS"))
            {
                return this._bendPoints;
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
            if ((feature == "SOURCE"))
            {
                this.Source = ((INotationElement)(value));
                return;
            }
            if ((feature == "TARGET"))
            {
                this.Target = ((INotationElement)(value));
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
            if ((reference == "SOURCE"))
            {
                return new SourceProxy(this);
            }
            if ((reference == "TARGET"))
            {
                return new TargetProxy(this);
            }
            return base.GetExpressionForReference(reference);
        }
        
        /// <summary>
        /// Gets the property name for the given container
        /// </summary>
        /// <returns>The name of the respective container reference</returns>
        /// <param name="container">The container object</param>
        protected override string GetCompositionName(object container)
        {
            if ((container == this._bendPoints))
            {
                return "bendPoints";
            }
            return base.GetCompositionName(container);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/glsp/notation#//Edge")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Edge class
        /// </summary>
        public class EdgeChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Edge _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public EdgeChildrenCollection(Edge parent)
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
                    count = (count + this._parent.BendPoints.Count);
                    return count;
                }
            }
            
            protected override void AttachCore()
            {
                this._parent.BendPoints.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.BendPoints.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IGPoint bendPointsCasted = item.As<IGPoint>();
                if ((bendPointsCasted != null))
                {
                    this._parent.BendPoints.Add(bendPointsCasted);
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.BendPoints.Clear();
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.BendPoints.Contains(item))
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
                IEnumerator<IModelElement> bendPointsEnumerator = this._parent.BendPoints.GetEnumerator();
                try
                {
                    for (
                    ; bendPointsEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = bendPointsEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    bendPointsEnumerator.Dispose();
                }
            }
            
            /// <summary>
            /// Removes the given item from the collection
            /// </summary>
            /// <returns>True, if the item was removed, otherwise False</returns>
            /// <param name="item">The item that should be removed</param>
            public override bool Remove(IModelElement item)
            {
                IGPoint gPointItem = item.As<IGPoint>();
                if (((gPointItem != null) 
                            && this._parent.BendPoints.Remove(gPointItem)))
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.BendPoints).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Edge class
        /// </summary>
        public class EdgeReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Edge _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public EdgeReferencedElementsCollection(Edge parent)
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
                    count = (count + this._parent.BendPoints.Count);
                    if ((this._parent.Source != null))
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
            
            protected override void AttachCore()
            {
                this._parent.BendPoints.AsNotifiable().CollectionChanged += this.PropagateCollectionChanges;
                this._parent.SourceChanged += this.PropagateValueChanges;
                this._parent.TargetChanged += this.PropagateValueChanges;
            }
            
            protected override void DetachCore()
            {
                this._parent.BendPoints.AsNotifiable().CollectionChanged -= this.PropagateCollectionChanges;
                this._parent.SourceChanged -= this.PropagateValueChanges;
                this._parent.TargetChanged -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                IGPoint bendPointsCasted = item.As<IGPoint>();
                if ((bendPointsCasted != null))
                {
                    this._parent.BendPoints.Add(bendPointsCasted);
                }
                if ((this._parent.Source == null))
                {
                    INotationElement sourceCasted = item.As<INotationElement>();
                    if ((sourceCasted != null))
                    {
                        this._parent.Source = sourceCasted;
                        return;
                    }
                }
                if ((this._parent.Target == null))
                {
                    INotationElement targetCasted = item.As<INotationElement>();
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
                this._parent.BendPoints.Clear();
                this._parent.Source = null;
                this._parent.Target = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if (this._parent.BendPoints.Contains(item))
                {
                    return true;
                }
                if ((item == this._parent.Source))
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
                IEnumerator<IModelElement> bendPointsEnumerator = this._parent.BendPoints.GetEnumerator();
                try
                {
                    for (
                    ; bendPointsEnumerator.MoveNext(); 
                    )
                    {
                        array[arrayIndex] = bendPointsEnumerator.Current;
                        arrayIndex = (arrayIndex + 1);
                    }
                }
                finally
                {
                    bendPointsEnumerator.Dispose();
                }
                if ((this._parent.Source != null))
                {
                    array[arrayIndex] = this._parent.Source;
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
                IGPoint gPointItem = item.As<IGPoint>();
                if (((gPointItem != null) 
                            && this._parent.BendPoints.Remove(gPointItem)))
                {
                    return true;
                }
                if ((this._parent.Source == item))
                {
                    this._parent.Source = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.BendPoints).Concat(this._parent.Source).Concat(this._parent.Target).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the source property
        /// </summary>
        private sealed class SourceProxy : ModelPropertyChange<IEdge, INotationElement>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SourceProxy(IEdge modelElement) : 
                    base(modelElement, "source")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override INotationElement Value
            {
                get
                {
                    return this.ModelElement.Source;
                }
                set
                {
                    this.ModelElement.Source = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the target property
        /// </summary>
        private sealed class TargetProxy : ModelPropertyChange<IEdge, INotationElement>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public TargetProxy(IEdge modelElement) : 
                    base(modelElement, "target")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override INotationElement Value
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
