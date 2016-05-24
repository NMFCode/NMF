using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NMF.Expressions;
using NMF.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace NMF.Collections.ObjectModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ObservableReadOnlyOrderedSet<T> : ReadOnlyOrderedSet<T>, INotifyCollectionChanged, INotifyCollectionChanging, IEnumerableExpression<T>, INotifyEnumerable<T>
    {
        public ObservableReadOnlyOrderedSet(ObservableOrderedSet<T> parent)
            : base(parent)
        {
            parent.CollectionChanging += ParentCollectionChanging;
            parent.CollectionChanged += ParentCollectionChanged;
        }

        private void ParentCollectionChanging(object sender, NotifyCollectionChangingEventArgs e)
        {
            OnCollectionChanging(e);
        }

        private void ParentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        protected virtual void OnCollectionChanging(NotifyCollectionChangingEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null) CollectionChanged(this, e);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;

        public INotifyEnumerable<T> AsNotifiable()
        {
            return this;
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return this;
        }

        void INotifyEnumerable.Attach() { }

        void INotifyEnumerable.Detach() { }

        bool INotifyEnumerable.IsAttached
        {
            get { return true; }
        }
    }
}
