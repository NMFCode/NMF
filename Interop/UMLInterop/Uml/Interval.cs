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
    /// An Interval defines the range between two ValueSpecifications.
    ///<p>From package UML::Values.</p>
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/uml2/5.0.0/UML")]
    [XmlNamespacePrefixAttribute("uml")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/uml2/5.0.0/UML#//Interval")]
    [DebuggerDisplayAttribute("Interval {Name}")]
    public partial class Interval : ValueSpecification, IInterval, IModelElement
    {
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _maxReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveMaxReference);
        
        /// <summary>
        /// The backing field for the Max property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IValueSpecification _max;
        
        private static Lazy<NMF.Models.Meta.ITypedElement> _minReference = new Lazy<NMF.Models.Meta.ITypedElement>(RetrieveMinReference);
        
        /// <summary>
        /// The backing field for the Min property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IValueSpecification _min;
        
        private static NMF.Models.Meta.IClass _classInstance;
        
        /// <summary>
        /// Refers to the ValueSpecification denoting the maximum value of the range.
        ///<p>From package UML::Values.</p>
        /// </summary>
        [DisplayNameAttribute("max")]
        [DescriptionAttribute("Refers to the ValueSpecification denoting the maximum value of the range.\n<p>From" +
            " package UML::Values.</p>")]
        [CategoryAttribute("Interval")]
        [XmlElementNameAttribute("max")]
        [XmlAttributeAttribute(true)]
        public IValueSpecification Max
        {
            get
            {
                return this._max;
            }
            set
            {
                if ((this._max != value))
                {
                    IValueSpecification old = this._max;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Max", e, _maxReference);
                    this._max = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetMax;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetMax;
                    }
                    this.OnPropertyChanged("Max", e, _maxReference);
                }
            }
        }
        
        /// <summary>
        /// Refers to the ValueSpecification denoting the minimum value of the range.
        ///<p>From package UML::Values.</p>
        /// </summary>
        [DisplayNameAttribute("min")]
        [DescriptionAttribute("Refers to the ValueSpecification denoting the minimum value of the range.\n<p>From" +
            " package UML::Values.</p>")]
        [CategoryAttribute("Interval")]
        [XmlElementNameAttribute("min")]
        [XmlAttributeAttribute(true)]
        public IValueSpecification Min
        {
            get
            {
                return this._min;
            }
            set
            {
                if ((this._min != value))
                {
                    IValueSpecification old = this._min;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPropertyChanging("Min", e, _minReference);
                    this._min = value;
                    if ((old != null))
                    {
                        old.Deleted -= this.OnResetMin;
                    }
                    if ((value != null))
                    {
                        value.Deleted += this.OnResetMin;
                    }
                    this.OnPropertyChanged("Min", e, _minReference);
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
                return base.ReferencedElements.Concat(new IntervalReferencedElementsCollection(this));
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
                    _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Interval")));
                }
                return _classInstance;
            }
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveMaxReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Interval.ClassInstance)).Resolve("max")));
        }
        
        /// <summary>
        /// Handles the event that the Max property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetMax(object sender, System.EventArgs eventArgs)
        {
            this.Max = null;
        }
        
        private static NMF.Models.Meta.ITypedElement RetrieveMinReference()
        {
            return ((NMF.Models.Meta.ITypedElement)(((ModelElement)(NMF.Interop.Uml.Interval.ClassInstance)).Resolve("min")));
        }
        
        /// <summary>
        /// Handles the event that the Min property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetMin(object sender, System.EventArgs eventArgs)
        {
            this.Min = null;
        }
        
        /// <summary>
        /// Resolves the given URI to a child model element
        /// </summary>
        /// <returns>The model element or null if it could not be found</returns>
        /// <param name="reference">The requested reference name</param>
        /// <param name="index">The index of this reference</param>
        protected override IModelElement GetModelElementForReference(string reference, int index)
        {
            if ((reference == "MAX"))
            {
                return this.Max;
            }
            if ((reference == "MIN"))
            {
                return this.Min;
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
            if ((feature == "MAX"))
            {
                this.Max = ((IValueSpecification)(value));
                return;
            }
            if ((feature == "MIN"))
            {
                this.Min = ((IValueSpecification)(value));
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
            if ((reference == "MAX"))
            {
                return new MaxProxy(this);
            }
            if ((reference == "MIN"))
            {
                return new MinProxy(this);
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
                _classInstance = ((NMF.Models.Meta.IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/uml2/5.0.0/UML#//Interval")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Interval class
        /// </summary>
        public class IntervalReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Interval _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public IntervalReferencedElementsCollection(Interval parent)
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
                    if ((this._parent.Max != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.Min != null))
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
                if ((this._parent.Max == null))
                {
                    IValueSpecification maxCasted = item.As<IValueSpecification>();
                    if ((maxCasted != null))
                    {
                        this._parent.Max = maxCasted;
                        return;
                    }
                }
                if ((this._parent.Min == null))
                {
                    IValueSpecification minCasted = item.As<IValueSpecification>();
                    if ((minCasted != null))
                    {
                        this._parent.Min = minCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Max = null;
                this._parent.Min = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Max))
                {
                    return true;
                }
                if ((item == this._parent.Min))
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
                if ((this._parent.Max != null))
                {
                    array[arrayIndex] = this._parent.Max;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.Min != null))
                {
                    array[arrayIndex] = this._parent.Min;
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
                if ((this._parent.Max == item))
                {
                    this._parent.Max = null;
                    return true;
                }
                if ((this._parent.Min == item))
                {
                    this._parent.Min = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Max).Concat(this._parent.Min).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the max property
        /// </summary>
        private sealed class MaxProxy : ModelPropertyChange<IInterval, IValueSpecification>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public MaxProxy(IInterval modelElement) : 
                    base(modelElement, "max")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IValueSpecification Value
            {
                get
                {
                    return this.ModelElement.Max;
                }
                set
                {
                    this.ModelElement.Max = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the min property
        /// </summary>
        private sealed class MinProxy : ModelPropertyChange<IInterval, IValueSpecification>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public MinProxy(IInterval modelElement) : 
                    base(modelElement, "min")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IValueSpecification Value
            {
                get
                {
                    return this.ModelElement.Min;
                }
                set
                {
                    this.ModelElement.Min = value;
                }
            }
        }
    }
}
