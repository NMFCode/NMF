using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableCollectionProxy<T> : ObservableEnumerable<T>, INotifyCollection<T>
    {
        private readonly IExecutionContext context;
        private IEnumerable<T> inner;

        public override IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public ObservableCollectionProxy(IEnumerable<T> inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            this.inner = inner;
            this.context = ExecutionEngine.Current.Context;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        protected override void OnAttach()
        {
            var notifiable = inner as INotifyCollectionChanged;
            if (notifiable != null)
                context.AddChangeListener(this, notifiable);
        }

        protected override void OnDetach()
        {
            var notifiable = inner as INotifyCollectionChanged;
            if (notifiable != null)
                context.RemoveChangeListener(this, notifiable);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            //TODO Need information from the NotifyCollectionChangedEventArgs here.
            return new CollectionChangedNotificationResult<T>(this);
        }

        public override bool Contains(T item)
        {
            return inner.Contains(item);
        }

        public override int Count
        {
            get
            {
                return inner.Count();
            }
        }

        void ICollection<T>.Add(T item)
        {
            var coll = inner as ICollection<T>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            coll.Add(item);
        }

        void ICollection<T>.Clear()
        {
            var coll = inner as ICollection<T>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            var list = new List<T>(this);
            if (list.Count == coll.Count)
            {
                coll.Clear();
            }
            else
            {
                foreach (var item in list)
                {
                    coll.Remove(item);
                }
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                var collection = inner as ICollection<T>;
                return collection == null || collection.IsReadOnly;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            var coll = inner as ICollection<T>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            return coll.Remove(item);
        }
    }
}
