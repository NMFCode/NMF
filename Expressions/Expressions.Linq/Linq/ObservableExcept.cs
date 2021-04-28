using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Linq;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableExcept<TSource> : ObservableEnumerable<TSource>
    {
        public override string ToString()
        {
            return "[Except]";
        }

        private readonly INotifyEnumerable<TSource> source;
        private readonly IEnumerable<TSource> source2;
        private readonly INotifyEnumerable<TSource> observableSource2;
        private readonly Dictionary<TSource, int> sourceItems;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                if (observableSource2 != null)
                    yield return observableSource2;
            }
        }

        public ObservableExcept(INotifyEnumerable<TSource> source, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (source2 == null) throw new ArgumentNullException("source2");

            this.source = source;
            this.source2 = source2;
            this.observableSource2 = source2 as INotifyEnumerable<TSource>;
            if (observableSource2 == null)
                observableSource2 = (source2 as IEnumerableExpression<TSource>)?.AsNotifiable();
            sourceItems = new Dictionary<TSource, int>(comparer);
        }

        public override IEnumerator<TSource> GetEnumerator()
        {
            return SL.Where(source, item => !sourceItems.ContainsKey(item)).GetEnumerator();
        }

        private bool AddItem(TSource item)
        {
            int count;
            if (sourceItems.TryGetValue(item, out count))
            {
                sourceItems[item] = count + 1;
                return false;
            }
            else
            {
                sourceItems.Add(item, 1);
                return true;
            }
        }

        private bool RemoveItem(TSource item)
        {
            int count;
            if (sourceItems.TryGetValue(item, out count))
            {
                count--;
                if (count == 0)
                {
                    sourceItems.Remove(item);
                    return true;
                }
                else
                {
                    sourceItems[item] = count;
                }
            }
            return false;
        }

        protected override void OnAttach()
        {
            foreach (var item in source2)
            {
                AddItem(item);
            }
        }

        protected override void OnDetach()
        {
            sourceItems.Clear();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var notification = CollectionChangedNotificationResult<TSource>.Create(this);
            var added = notification.AddedItems;
            var removed = notification.RemovedItems;

            foreach (ICollectionChangedNotificationResult<TSource> change in sources)
            {
                if (change.IsReset)
                {
                    OnDetach();
                    OnAttach();
                    OnCleared();
                    notification.TurnIntoReset();
                    return notification;
                }

                if (change.Source == source)
                {
                    NotifySource(change, added, removed);
                }
                else
                {
                    NotifySource2(change, added, removed);
                }
            }

            OnRemoveItems(removed);
            OnAddItems(added);
            return notification;
        }

        private void NotifySource(ICollectionChangedNotificationResult<TSource> change, List<TSource> added, List<TSource> removed)
        {
            if (change.RemovedItems != null)
            {
                foreach (var item in change.RemovedItems)
                {
                    if (!sourceItems.ContainsKey(item))
                    {
                        removed.Add(item);
                    }
                }
            }

            if (change.AddedItems != null)
            {
                foreach (var item in change.AddedItems)
                {
                    if (!sourceItems.ContainsKey(item))
                    {
                        added.Add(item);
                    }
                }
            }
        }

        private void NotifySource2(ICollectionChangedNotificationResult<TSource> change, List<TSource> added, List<TSource> removed)
        {
            if (change.RemovedItems != null)
            {
                var uniqueRemoved = new HashSet<TSource>(sourceItems.Comparer);
                foreach (var item in change.RemovedItems)
                {
                    if (RemoveItem(item))
                    {
                        uniqueRemoved.Add(item);
                    }
                }
                added.AddRange(SL.Where(source, item => uniqueRemoved.Contains(item)));
            }
            if (change.AddedItems != null)
            {
                var uniqueAdded = new HashSet<TSource>(sourceItems.Comparer);
                foreach (var item in change.AddedItems)
                {
                    if (AddItem(item))
                    {
                        uniqueAdded.Add(item);
                    }
                }
                removed.AddRange(SL.Where(source, item => uniqueAdded.Contains(item)));
            }
        }
    }
}
