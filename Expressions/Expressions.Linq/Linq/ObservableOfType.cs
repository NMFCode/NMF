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
            var added = new List<T>();
            var removed = new List<T>();
            var moved = new List<T>();
            var change = (ICollectionChangedNotificationResult)sources[0];

            if (change.IsReset)
            {
                OnCleared();
                return new CollectionChangedNotificationResult<T>(this);
            }
            
            if (change.RemovedItems != null)
                removed.AddRange(SL.OfType<T>(change.RemovedItems));

            if (change.AddedItems != null)
                added.AddRange(SL.OfType<T>(change.AddedItems));

            if (change.MovedItems != null)
                moved.AddRange(SL.OfType<T>(change.MovedItems));

            if (removed.Count == 0 && added.Count == 0 && moved.Count == 0)
                return new UnchangedNotificationResult(this);

            OnRemoveItems(removed);
            OnAddItems(added);
            OnMoveItems(moved);
            return new CollectionChangedNotificationResult<T>(this, added, removed, moved);
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
