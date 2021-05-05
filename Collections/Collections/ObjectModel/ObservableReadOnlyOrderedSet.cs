using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace NMF.Collections.ObjectModel
{
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ObservableReadOnlyOrderedSet<T> : ReadOnlyOrderedSet<T>, INotifyCollectionChanged, INotifyCollectionChanging, IEnumerableExpression<T>
    {
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

        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;

        private INotifyEnumerable<T> proxy;

        public INotifyEnumerable<T> AsNotifiable()
        {
            if (proxy == null) proxy = this.WithUpdates();
            return proxy;
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        public override string ToString()
        {
            return $"[OrderedSet Count={Count}]";
        }
    }
}
