using NMF.Expressions;
using NMF.Expressions.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Models.Collections
{
    /// <summary>
    /// Denotes the base class for a collection of referenced elements
    /// </summary>
    public abstract class ReferenceCollection : ICollectionExpression<IModelElement>, INotifyCollectionChanged, IDisposable
    {
        /// <inheritdoc />
        public abstract IEnumerator<IModelElement> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        protected abstract void AttachCore();

        /// <inheritdoc />
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
    }
}
