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
    /// The default implementation of the Shape class
    /// </summary>
    [XmlNamespaceAttribute("http://www.eclipse.org/glsp/notation")]
    [XmlNamespacePrefixAttribute("notation")]
    [ModelRepresentationClassAttribute("http://www.eclipse.org/glsp/notation#//Shape")]
    public partial class Shape : NotationElement, IShape, IModelElement
    {
        
        private static Lazy<ITypedElement> _positionReference = new Lazy<ITypedElement>(RetrievePositionReference);
        
        /// <summary>
        /// The backing field for the Position property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IGPoint _position;
        
        private static Lazy<ITypedElement> _sizeReference = new Lazy<ITypedElement>(RetrieveSizeReference);
        
        /// <summary>
        /// The backing field for the Size property
        /// </summary>
        [DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)]
        private IGDimension _size;
        
        private static IClass _classInstance;
        
        /// <summary>
        /// The position property
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("position")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IGPoint Position
        {
            get
            {
                return this._position;
            }
            set
            {
                if ((this._position != value))
                {
                    IGPoint old = this._position;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnPositionChanging(e);
                    this.OnPropertyChanging("Position", e, _positionReference);
                    this._position = value;
                    if ((old != null))
                    {
                        old.Parent = null;
                        old.ParentChanged -= this.OnResetPosition;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetPosition;
                    }
                    this.OnPositionChanged(e);
                    this.OnPropertyChanged("Position", e, _positionReference);
                }
            }
        }
        
        /// <summary>
        /// The size property
        /// </summary>
        [BrowsableAttribute(false)]
        [XmlElementNameAttribute("size")]
        [XmlAttributeAttribute(false)]
        [ContainmentAttribute()]
        public IGDimension Size
        {
            get
            {
                return this._size;
            }
            set
            {
                if ((this._size != value))
                {
                    IGDimension old = this._size;
                    ValueChangedEventArgs e = new ValueChangedEventArgs(old, value);
                    this.OnSizeChanging(e);
                    this.OnPropertyChanging("Size", e, _sizeReference);
                    this._size = value;
                    if ((old != null))
                    {
                        old.Parent = null;
                        old.ParentChanged -= this.OnResetSize;
                    }
                    if ((value != null))
                    {
                        value.Parent = this;
                        value.ParentChanged += this.OnResetSize;
                    }
                    this.OnSizeChanged(e);
                    this.OnPropertyChanged("Size", e, _sizeReference);
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
                return base.Children.Concat(new ShapeChildrenCollection(this));
            }
        }
        
        /// <summary>
        /// Gets the referenced model elements of this model element
        /// </summary>
        public override IEnumerableExpression<IModelElement> ReferencedElements
        {
            get
            {
                return base.ReferencedElements.Concat(new ShapeReferencedElementsCollection(this));
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
                    _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/glsp/notation#//Shape")));
                }
                return _classInstance;
            }
        }
        
        /// <summary>
        /// Gets fired before the Position property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> PositionChanging;
        
        /// <summary>
        /// Gets fired when the Position property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> PositionChanged;
        
        /// <summary>
        /// Gets fired before the Size property changes its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> SizeChanging;
        
        /// <summary>
        /// Gets fired when the Size property changed its value
        /// </summary>
        public event System.EventHandler<ValueChangedEventArgs> SizeChanged;
        
        private static ITypedElement RetrievePositionReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.Glsp.Notation.Shape.ClassInstance)).Resolve("position")));
        }
        
        /// <summary>
        /// Raises the PositionChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnPositionChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.PositionChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the PositionChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnPositionChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.PositionChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Handles the event that the Position property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetPosition(object sender, System.EventArgs eventArgs)
        {
            if (sender == this.Position)
            {
                this.Position = null;
            }
        }
        
        private static ITypedElement RetrieveSizeReference()
        {
            return ((ITypedElement)(((ModelElement)(NMF.Glsp.Notation.Shape.ClassInstance)).Resolve("size")));
        }
        
        /// <summary>
        /// Raises the SizeChanging event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSizeChanging(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.SizeChanging;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Raises the SizeChanged event
        /// </summary>
        /// <param name="eventArgs">The event data</param>
        protected virtual void OnSizeChanged(ValueChangedEventArgs eventArgs)
        {
            System.EventHandler<ValueChangedEventArgs> handler = this.SizeChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        /// <summary>
        /// Handles the event that the Size property must reset
        /// </summary>
        /// <param name="sender">The object that sent this reset request</param>
        /// <param name="eventArgs">The event data for the reset event</param>
        private void OnResetSize(object sender, ValueChangedEventArgs eventArgs)
        {
            if (sender == this.Size)
            {
                this.Size = null;
            }
        }
        
        /// <summary>
        /// Gets the relative URI fragment for the given child model element
        /// </summary>
        /// <returns>A fragment of the relative URI</returns>
        /// <param name="element">The element that should be looked for</param>
        protected override string GetRelativePathForNonIdentifiedChild(IModelElement element)
        {
            if ((element == this.Position))
            {
                return ModelHelper.CreatePath("Position");
            }
            if ((element == this.Size))
            {
                return ModelHelper.CreatePath("Size");
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
            if ((reference == "POSITION"))
            {
                return this.Position;
            }
            if ((reference == "SIZE"))
            {
                return this.Size;
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
            if ((feature == "POSITION"))
            {
                this.Position = ((IGPoint)(value));
                return;
            }
            if ((feature == "SIZE"))
            {
                this.Size = ((IGDimension)(value));
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
            if ((reference == "POSITION"))
            {
                return new PositionProxy(this);
            }
            if ((reference == "SIZE"))
            {
                return new SizeProxy(this);
            }
            return base.GetExpressionForReference(reference);
        }
        
        /// <summary>
        /// Gets the Class for this model element
        /// </summary>
        public override IClass GetClass()
        {
            if ((_classInstance == null))
            {
                _classInstance = ((IClass)(MetaRepository.Instance.Resolve("http://www.eclipse.org/glsp/notation#//Shape")));
            }
            return _classInstance;
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Shape class
        /// </summary>
        public class ShapeChildrenCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Shape _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ShapeChildrenCollection(Shape parent)
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
                    if ((this._parent.Position != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.Size != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }

            /// <inheritdoc />
            protected override void AttachCore()
            {
                this._parent.PositionChanged += this.PropagateValueChanges;
                this._parent.SizeChanged += this.PropagateValueChanges;
            }

            /// <inheritdoc />
            protected override void DetachCore()
            {
                this._parent.PositionChanged -= this.PropagateValueChanges;
                this._parent.SizeChanged -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.Position == null))
                {
                    IGPoint positionCasted = item.As<IGPoint>();
                    if ((positionCasted != null))
                    {
                        this._parent.Position = positionCasted;
                        return;
                    }
                }
                if ((this._parent.Size == null))
                {
                    IGDimension sizeCasted = item.As<IGDimension>();
                    if ((sizeCasted != null))
                    {
                        this._parent.Size = sizeCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Position = null;
                this._parent.Size = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Position))
                {
                    return true;
                }
                if ((item == this._parent.Size))
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
                if ((this._parent.Position != null))
                {
                    array[arrayIndex] = this._parent.Position;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.Size != null))
                {
                    array[arrayIndex] = this._parent.Size;
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
                if ((this._parent.Position == item))
                {
                    this._parent.Position = null;
                    return true;
                }
                if ((this._parent.Size == item))
                {
                    this._parent.Size = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Position).Concat(this._parent.Size).GetEnumerator();
            }
        }
        
        /// <summary>
        /// The collection class to to represent the children of the Shape class
        /// </summary>
        public class ShapeReferencedElementsCollection : ReferenceCollection, ICollectionExpression<IModelElement>, ICollection<IModelElement>
        {
            
            private Shape _parent;
            
            /// <summary>
            /// Creates a new instance
            /// </summary>
            public ShapeReferencedElementsCollection(Shape parent)
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
                    if ((this._parent.Position != null))
                    {
                        count = (count + 1);
                    }
                    if ((this._parent.Size != null))
                    {
                        count = (count + 1);
                    }
                    return count;
                }
            }

            /// <inheritdoc />
            protected override void AttachCore()
            {
                this._parent.PositionChanged += this.PropagateValueChanges;
                this._parent.SizeChanged += this.PropagateValueChanges;
            }

            /// <inheritdoc />
            protected override void DetachCore()
            {
                this._parent.PositionChanged -= this.PropagateValueChanges;
                this._parent.SizeChanged -= this.PropagateValueChanges;
            }
            
            /// <summary>
            /// Adds the given element to the collection
            /// </summary>
            /// <param name="item">The item to add</param>
            public override void Add(IModelElement item)
            {
                if ((this._parent.Position == null))
                {
                    IGPoint positionCasted = item.As<IGPoint>();
                    if ((positionCasted != null))
                    {
                        this._parent.Position = positionCasted;
                        return;
                    }
                }
                if ((this._parent.Size == null))
                {
                    IGDimension sizeCasted = item.As<IGDimension>();
                    if ((sizeCasted != null))
                    {
                        this._parent.Size = sizeCasted;
                        return;
                    }
                }
            }
            
            /// <summary>
            /// Clears the collection and resets all references that implement it.
            /// </summary>
            public override void Clear()
            {
                this._parent.Position = null;
                this._parent.Size = null;
            }
            
            /// <summary>
            /// Gets a value indicating whether the given element is contained in the collection
            /// </summary>
            /// <returns>True, if it is contained, otherwise False</returns>
            /// <param name="item">The item that should be looked out for</param>
            public override bool Contains(IModelElement item)
            {
                if ((item == this._parent.Position))
                {
                    return true;
                }
                if ((item == this._parent.Size))
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
                if ((this._parent.Position != null))
                {
                    array[arrayIndex] = this._parent.Position;
                    arrayIndex = (arrayIndex + 1);
                }
                if ((this._parent.Size != null))
                {
                    array[arrayIndex] = this._parent.Size;
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
                if ((this._parent.Position == item))
                {
                    this._parent.Position = null;
                    return true;
                }
                if ((this._parent.Size == item))
                {
                    this._parent.Size = null;
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
                return Enumerable.Empty<IModelElement>().Concat(this._parent.Position).Concat(this._parent.Size).GetEnumerator();
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the position property
        /// </summary>
        private sealed class PositionProxy : ModelPropertyChange<IShape, IGPoint>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public PositionProxy(IShape modelElement) : 
                    base(modelElement, "position")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IGPoint Value
            {
                get
                {
                    return this.ModelElement.Position;
                }
                set
                {
                    this.ModelElement.Position = value;
                }
            }
        }
        
        /// <summary>
        /// Represents a proxy to represent an incremental access to the size property
        /// </summary>
        private sealed class SizeProxy : ModelPropertyChange<IShape, IGDimension>
        {
            
            /// <summary>
            /// Creates a new observable property access proxy
            /// </summary>
            /// <param name="modelElement">The model instance element for which to create the property access proxy</param>
            public SizeProxy(IShape modelElement) : 
                    base(modelElement, "size")
            {
            }
            
            /// <summary>
            /// Gets or sets the value of this expression
            /// </summary>
            public override IGDimension Value
            {
                get
                {
                    return this.ModelElement.Size;
                }
                set
                {
                    this.ModelElement.Size = value;
                }
            }
        }
    }
}

