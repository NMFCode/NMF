//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NMF.Interop.Legacy.Cmof
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using NMF.Expressions;
    using NMF.Expressions.Linq;
    using NMF.Models;
    using NMF.Models.Meta;
    using NMF.Models.Collections;
    using NMF.Models.Expressions;
    using NMF.Collections.Generic;
    using NMF.Collections.ObjectModel;
    using NMF.Serialization;
    using NMF.Utilities;
    using System.Collections.Specialized;
    using NMF.Models.Repository;
    using System.Globalization;
    
    
    /// <summary>
    /// The collection class to implement the refined ownedRule reference for the Operation class
    /// </summary>
    public class OperationOwnedRuleCollection : ICollectionExpression<IConstraint>, ICollection<IConstraint>
    {
        
        private NMF.Interop.Legacy.Cmof.IOperation _parent;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public OperationOwnedRuleCollection(NMF.Interop.Legacy.Cmof.IOperation parent)
        {
            this._parent = parent;
            parent.Precondition.AsNotifiable().CollectionChanged += this.HandleCollectionChange;
            parent.Postcondition.AsNotifiable().CollectionChanged += this.HandleCollectionChange;
            parent.BodyCondition.AsNotifiable().CollectionChanged += this.HandleCollectionChange;
        }
        
        /// <summary>
        /// Gets the amount of elements contained in this collection
        /// </summary>
        public virtual int Count
        {
            get
            {
                int count = 0;
                count = (count + this._parent.Precondition.Count);
                count = (count + this._parent.Postcondition.Count);
                count = (count + this._parent.BodyCondition.Count);
                return count;
            }
        }
        
        /// <summary>
        /// Gets a value indicating that the collection is not read-only
        /// </summary>
        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        
        /// <summary>
        /// Returns that this composed collection is always attached.
        /// </summary>
        public bool IsAttached
        {
            get
            {
                return true;
            }
        }
        
        /// <summary>
        /// Gets fired when the contents of this collection changes
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        
        /// <summary>
        /// Fires the CollectionChanged event
        /// </summary>
        protected virtual void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs eventArgs)
        {
            System.Collections.Specialized.NotifyCollectionChangedEventHandler handler = this.CollectionChanged;
            if ((handler != null))
            {
                handler.Invoke(this, eventArgs);
            }
        }
        
        private void HandleCollectionChange(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs eventArgs)
        {
            this.OnCollectionChanged(eventArgs);
        }
        
        /// <summary>
        /// Adds the given element to the collection
        /// </summary>
        /// <param name="item">The item to add</param>
        public virtual void Add(IConstraint item)
        {
            this._parent.Precondition.Add(item);
        }
        
        /// <summary>
        /// Clears the collection and resets all references that implement it.
        /// </summary>
        public virtual void Clear()
        {
            this._parent.Precondition.Clear();
            this._parent.Postcondition.Clear();
            this._parent.BodyCondition.Clear();
        }
        
        /// <summary>
        /// Gets a value indicating whether the given element is contained in the collection
        /// </summary>
        /// <returns>True, if it is contained, otherwise False</returns>
        /// <param name="item">The item that should be looked out for</param>
        public virtual bool Contains(IConstraint item)
        {
            if (this._parent.Precondition.Contains(item))
            {
                return true;
            }
            if (this._parent.Postcondition.Contains(item))
            {
                return true;
            }
            if (this._parent.BodyCondition.Contains(item))
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
        public virtual void CopyTo(IConstraint[] array, int arrayIndex)
        {
            this._parent.Precondition.CopyTo(array, arrayIndex);
            arrayIndex = (arrayIndex + this._parent.Precondition.Count);
            this._parent.Postcondition.CopyTo(array, arrayIndex);
            arrayIndex = (arrayIndex + this._parent.Postcondition.Count);
            this._parent.BodyCondition.CopyTo(array, arrayIndex);
            arrayIndex = (arrayIndex + this._parent.BodyCondition.Count);
        }
        
        /// <summary>
        /// Removes the given item from the collection
        /// </summary>
        /// <returns>True, if the item was removed, otherwise False</returns>
        /// <param name="item">The item that should be removed</param>
        public virtual bool Remove(IConstraint item)
        {
            if (this._parent.Precondition.Remove(item))
            {
                return true;
            }
            if (this._parent.Postcondition.Remove(item))
            {
                return true;
            }
            if (this._parent.BodyCondition.Remove(item))
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Gets an enumerator that enumerates the collection
        /// </summary>
        /// <returns>A generic enumerator</returns>
        public virtual IEnumerator<IConstraint> GetEnumerator()
        {
            return Enumerable.Empty<IConstraint>().Concat(this._parent.Precondition).Concat(this._parent.Postcondition).Concat(this._parent.BodyCondition).GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        /// <summary>
        /// Gets an observable version of this collection
        /// </summary>
        public virtual INotifyCollection<IConstraint> AsNotifiable()
        {
            return this.WithUpdates();
        }
        
        /// <summary>
        /// Gets an observable version of this collection
        /// </summary>
        INotifyEnumerable<IConstraint> IEnumerableExpression<IConstraint>.AsNotifiable()
        {
            return this.WithUpdates();
        }
        
        /// <summary>
        /// Gets an observable version of this collection
        /// </summary>
        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return this.WithUpdates();
        }
        
        /// <summary>
        /// Attaches this collection class
        /// </summary>
        public void Attach()
        {
        }
        
        /// <summary>
        /// Detaches this collection class
        /// </summary>
        public void Detach()
        {
        }
    }
}