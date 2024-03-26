using NMF.Collections.Generic;
using NMF.Expressions;
using NMF.Expressions.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace NMF.Collections.ObjectModel
{
    /// <summary>
    /// Denotes an observable ordered set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ObservableOrderedSet<T> : OrderedSet<T>, INotifyCollectionChanged, INotifyCollectionChanging, INotifyPropertyChanged, IOrderedSetExpression<T>, ICollectionExpression
    {
        /// <inheritdoc />
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <inheritdoc />
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            CollectionChanged?.Invoke(this, e);
        }

        /// <inheritdoc />
        protected void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        /// <inheritdoc />
        public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;


        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        private INotifyCollection<T> proxy;

        /// <inheritdoc />
        public INotifyCollection<T> AsNotifiable()
        {
            if (proxy == null) proxy = this.WithUpdates();
            return proxy;
        }

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return AsNotifiable();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        /// <summary>
        /// Determines whether events are subscribed
        /// </summary>
        /// <returns>True, if there is any subscriber to CollectionChanged, PropertyChanged or CollectionChanging</returns>
        protected bool RequireEvents()
        {
            return CollectionChanged != null || PropertyChanged != null || CollectionChanging != null;
        }


        /// <inheritdoc />
        public override bool Add(T item)
        {
            if (!RequireEvents())
            {
                return SilentAdd(item);
            }
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count);
            OnCollectionChanging(e);
            if (SilentAdd(item))
            {
                OnCollectionChanged(e);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the element without notifications
        /// </summary>
        /// <param name="item">the element to be added</param>
        /// <returns>true, if the element was added, otherwise false</returns>
        protected bool SilentAdd(T item)
        {
            return base.Add(item);
        }

        /// <inheritdoc />
        public override void Insert(int index, T item)
        {
            if (!RequireEvents())
            {
                base.Insert(index, item);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                OnCollectionChanging(e);
                SilentInsert(index, item);
                OnCollectionChanged(e);
            }
        }

        /// <summary>
        /// Inserts the element at the given index without notifications
        /// </summary>
        /// <param name="index">the index at which the element should be inserted</param>
        /// <param name="item">the item to insert</param>
        protected void SilentInsert(int index, T item)
        {
            base.Insert(index, item);
        }

        /// <inheritdoc />
        public override void Clear()
        {
            if (RequireEvents())
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                SilentClear();
                OnCollectionChanged(e);
            }
            else
            {
                SilentClear();
            }
        }

        /// <summary>
        /// Clears the collection contents without notifications
        /// </summary>
        protected void SilentClear()
        {
            base.Clear();
        }

        /// <inheritdoc />
        protected override bool Remove(T item, int index)
        {
            if (!RequireEvents())
            {
                return SilentRemove(item, index);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
                OnCollectionChanging(e);
                if (SilentRemove(item, index))
                {
                    OnCollectionChanged(e);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Removes the element without notifications
        /// </summary>
        /// <param name="item">the element to remove</param>
        /// <param name="index">the index of the removed element</param>
        /// <returns>true, if the removal was successful, otherwise false</returns>
        protected bool SilentRemove(T item, int index)
        {
            return base.Remove(item, index);
        }

        /// <inheritdoc />
        protected override void Replace(int index, T oldValue, T newValue)
        {
            if (!RequireEvents())
            {
                SilentReplace(index, oldValue, newValue);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newValue, oldValue, index);
                OnCollectionChanging(e);
                SilentReplace(index, oldValue, newValue);
                OnCollectionChanged(e);
            }
        }

        /// <summary>
        /// Replaces the element without notifications
        /// </summary>
        /// <param name="index">the index of the element</param>
        /// <param name="oldValue">the old value</param>
        /// <param name="newValue">the new value</param>
        protected void SilentReplace(int index, T oldValue, T newValue)
        {
            base.Replace(index, oldValue, newValue);
        }


        /// <inheritdoc />
        public override string ToString()
        {
            return $"[OrderedSet Count={Count}]";
        }
    }
}
