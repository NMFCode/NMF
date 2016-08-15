using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    public sealed class ObservableCollectionProxy<T> : ObservableEnumerable<T>, INotifyCollection<T>
    {
        private IEnumerable<T> Inner { get; set; }

        public ObservableCollectionProxy(IEnumerable<T> inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            Inner = inner;

            Attach();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return Inner.GetEnumerator();
        }

        protected override void AttachCore()
        {
            var notifier = Inner as INotifyCollectionChanged;
            if (notifier != null)
            {
                notifier.CollectionChanged += InnerCollectionChanged;
            }
        }

        private void InnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        protected override void DetachCore()
        {
            var notifier = Inner as INotifyCollectionChanged;
            if (notifier != null)
            {
                notifier.CollectionChanged -= InnerCollectionChanged;
            }
        }

        public override bool Contains(T item)
        {
            return Inner.Contains(item);
        }

        public override int Count
        {
            get
            {
                return Inner.Count();
            }
        }

        void ICollection<T>.Add(T item)
        {
            var coll = Inner as ICollection<T>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            coll.Add(item);
        }

        void ICollection<T>.Clear()
        {
            var coll = Inner as ICollection<T>;
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
                var collection = Inner as ICollection<T>;
                return collection == null || collection.IsReadOnly;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            var coll = Inner as ICollection<T>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            return coll.Remove(item);
        }
    }
}
