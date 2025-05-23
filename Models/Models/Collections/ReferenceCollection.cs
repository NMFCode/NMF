﻿using NMF.Expressions;
using NMF.Expressions.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NMF.Models.Collections
{
    /// <summary>
    /// Denotes the base class for a collection of referenced elements
    /// </summary>
    public abstract class ReferenceCollection : ICollectionExpression<IModelElement>, INotifyCollectionChanged, IList, IDisposable
    {
        /// <inheritdoc />
        public abstract IEnumerator<IModelElement> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Attaches the collection
        /// </summary>
        protected abstract void AttachCore();

        /// <summary>
        /// Detaches the collection
        /// </summary>
        protected abstract void DetachCore();

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <inheritdoc />
        public abstract void Add(IModelElement item);

        /// <inheritdoc />
        public abstract void Clear();

        /// <inheritdoc />
        public abstract bool Contains(IModelElement item);

        /// <inheritdoc />
        public abstract void CopyTo(IModelElement[] array, int arrayIndex);

        /// <inheritdoc />
        public abstract int Count
        {
            get;
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        bool IList.IsFixedSize => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => null;

        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <inheritdoc />
        public abstract bool Remove(IModelElement item);

        /// <inheritdoc />
        public INotifyCollection<IModelElement> AsNotifiable()
        {
            AttachCore();
            return this.WithUpdates();
        }

        INotifyEnumerable<IModelElement> IEnumerableExpression<IModelElement>.AsNotifiable()
        {
            return AsNotifiable();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        /// <summary>
        /// Propagate a collection changed event
        /// </summary>
        /// <param name="sender">The original sender</param>
        /// <param name="e">The event args</param>
        protected void PropagateCollectionChanges(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        /// <summary>
        /// Propagate a value changed event
        /// </summary>
        /// <param name="sender">The original sender</param>
        /// <param name="e">The original event args</param>
        protected void PropagateValueChanges(object sender, ValueChangedEventArgs e)
        {
            if (e.OldValue == null)
            {
                if (e.NewValue != null)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.NewValue));
                }
            }
            else
            {
                if (e.NewValue == null)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.OldValue));
                }
                else
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewValue, e.OldValue));
                }
            }
        }
        /// <summary>
        /// Propagates a bubbled change
        /// </summary>
        /// <param name="sender">The original sender</param>
        /// <param name="e">The original event args</param>
        protected void PropagateValueChanges(object sender, BubbledChangeEventArgs e)
        {
            if (e.ChangeType == ChangeType.PropertyChanged)
            {
                PropagateValueChanges(sender, (ValueChangedEventArgs)e.OriginalEventArgs);
            }
        }

        /// <inheritdoc />
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            DetachCore();
        }

        int IList.Add(object value)
        {
            if (value is IModelElement element)
            {
                Add(element);
                return Count;
            }
            return -1;
        }

        bool IList.Contains(object value)
        {
            if (value is IModelElement element)
            {
                return Contains(element);
            }
            return false;
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            if (value is IModelElement element)
            {
                Remove(element);
            }
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((IModelElement[])array, index);
        }
    }
}
