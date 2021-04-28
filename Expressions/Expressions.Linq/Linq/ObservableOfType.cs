using System;
using System.Collections.Generic;
using System.Linq;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace NMF.Expressions.Linq
{
    internal class ObservableOfType<T> : ObservableEnumerable<T>
    {
        public override string ToString()
        {
            return $"[OfType {typeof(T).Name}]";
        }

        public INotifyEnumerable Source { get; private set; }

        public override IEnumerable<INotifiable> Dependencies { get { yield return Source; } }

        public ObservableOfType(INotifyEnumerable source)
        {
            if (source == null) throw new ArgumentNullException("source");

            Source = source;
        }
        
        public override IEnumerator<T> GetEnumerator()
        {
            return SL.OfType<T>(Source).GetEnumerator();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var change = (ICollectionChangedNotificationResult)sources[0];
            if (change.IsReset)
            {
                OnCleared();
                return CollectionChangedNotificationResult<T>.Create(this, true);
            }

            var notification = CollectionChangedNotificationResult<T>.Create(this);
            var removed = notification.RemovedItems;
            var added = notification.AddedItems;
            var moved = notification.MovedItems;

            removed.AddRange(change.RemovedItems.OfType<T>());
            added.AddRange(change.AddedItems.OfType<T>());
            moved.AddRange(change.MovedItems.OfType<T>());

            RaiseEvents(added, removed, moved);
            return notification;
        }
    }

    internal class ObservableOfTypeCollection<TSource, T> : ObservableOfType<T>, INotifyCollection<T>
        where T : TSource
    {
        public ObservableOfTypeCollection(INotifyCollection<TSource> source)
            : base(source) { }


        void ICollection<T>.Add(T item)
        {
            if (Source is not INotifyCollection<TSource> coll || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            coll.Add(item);
        }

        void ICollection<T>.Clear()
        {
            if (Source is not INotifyCollection<TSource> coll || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
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
                return Source is not INotifyCollection<TSource> coll || coll.IsReadOnly;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            if (Source is not INotifyCollection<TSource> coll || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            return coll.Remove(item);
        }
    }
}
