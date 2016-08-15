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
                return new CollectionChangedNotificationResult<T>(this);
            }

            List<T> removed = null;
            if (change.RemovedItems != null)
            {
                removed = SL.OfType<T>(change.RemovedItems).ToList();
                OnRemoveItems(removed);
            }

            List<T> added = null;
            if (change.AddedItems != null)
            {
                added = SL.OfType<T>(change.AddedItems).ToList();
                OnAddItems(added);
            }

            if ((removed == null || removed.Count == 0) && (added == null || added.Count == 0))
                return new UnchangedNotificationResult(this);

            return new CollectionChangedNotificationResult<T>(this, added, removed);
        }
    }

    internal class ObservableOfTypeCollection<TSource, T> : ObservableOfType<T>, INotifyCollection<T>
        where T : TSource
    {
        public ObservableOfTypeCollection(INotifyCollection<TSource> source)
            : base(source) { }


        void ICollection<T>.Add(T item)
        {
            var coll = Source as INotifyCollection<TSource>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            coll.Add(item);
        }

        void ICollection<T>.Clear()
        {
            var coll = Source as INotifyCollection<TSource>;
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
                var coll = Source as INotifyCollection<TSource>;
                return coll == null || coll.IsReadOnly;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            var coll = Source as INotifyCollection<TSource>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            return coll.Remove(item);
        }
    }
}
