using NMF.Collections.Generic;
using NMF.Expressions;
using NMF.Expressions.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    /// <summary>
    /// Denotes a set implementation that raises events when the collection contents are changed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ObservableSet<T> : DecoratedSet<T>, ISet<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, INotifyCollectionChanged, INotifyCollectionChanging, INotifyPropertyChanged, ISetExpression<T>
    {
        /// <inheritdoc />
        public override bool Add(T item)
        {
            if (!RequireEvents())
            {
                if (base.Add(item))
                {
                    return true;
                }
                return false;
            }
            if (item != null && !base.Contains(item))
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item);
                OnCollectionChanging(e);
                SilentAdd(item);
                OnCollectionChanged(e);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether it is necessary to raise events
        /// </summary>
        /// <returns>True, if there is any subscriber to either CollectionChanged, CollectionChanging or PropertyChanged</returns>
        protected bool RequireEvents()
        {
            return CollectionChanged != null || PropertyChanged != null || CollectionChanging != null;
        }

        /// <summary>
        /// Adds an element without notifications
        /// </summary>
        /// <param name="item">the item to add</param>
        /// <returns>true, if successful, otherwise false</returns>
        protected bool SilentAdd(T item)
        {
            return base.Add(item);
        }


        /// <inheritdoc />
        public override void Clear()
        {
            if (!RequireEvents())
            {
                base.Clear();
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                SilentClear();
                OnCollectionChanged(e);
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
        public override bool Remove(T item)
        {
            if (CollectionChanged == null && PropertyChanged == null && CollectionChanging == null)
            {
                return base.Remove(item);
            }
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item);
            OnCollectionChanging(e);
            if (SilentRemove(item))
            {
                OnCollectionChanged(e);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the given item without notifications
        /// </summary>
        /// <param name="item">the item to remove</param>
        /// <returns>true, if successful,  otherwise false</returns>
        protected bool SilentRemove(T item)
        {
            return base.Remove(item);
        }

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <inheritdoc />
        public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;

        /// <summary>
        /// Raises PropertyChanged
        /// </summary>
        /// <param name="property">the name of the property</param>
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Raises CollectionChanged
        /// </summary>
        /// <param name="e">the event args</param>
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Count");
            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises CollectionChanging
        /// </summary>
        /// <param name="e">the event args</param>
        protected void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

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

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Set Count={Count}]";
        }
    }
}
