using System;

using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace NMF.Collections.ObjectModel
{
    /// <summary>
    /// Denotes an observable readonly ordered set view
    /// </summary>
    /// <typeparam name="T">the element type</typeparam>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ObservableReadOnlyOrderedSet<T> : ReadOnlyOrderedSet<T>, INotifyCollectionChanged, INotifyCollectionChanging, IEnumerableExpression<T>
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="parent">the collection for which the view should be created</param>
        public ObservableReadOnlyOrderedSet(ObservableOrderedSet<T> parent)
            : base(parent)
        {
            parent.CollectionChanging += ParentCollectionChanging;
            parent.CollectionChanged += ParentCollectionChanged;
        }

        private void ParentCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanging(e);
        }

        private void ParentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        /// <summary>
        /// Gets called when an attempt is made to change the collection
        /// </summary>
        /// <param name="e">the event args</param>
        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        /// <summary>
        /// Gets called when the collection was changed
        /// </summary>
        /// <param name="e">the event args</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <inheritdoc />
        public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;

        private INotifyEnumerable<T> proxy;

        /// <inheritdoc />
        public INotifyEnumerable<T> AsNotifiable()
        {
            if (proxy == null) proxy = this.WithUpdates();
            return proxy;
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[OrderedSet Count={Count}]";
        }
    }
}
