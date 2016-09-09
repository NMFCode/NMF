using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableIntersect<TSource> : ObservableEnumerable<TSource>
    {
        private INotifyEnumerable<TSource> source;
        private IEnumerable<TSource> source2;
        private INotifyEnumerable<TSource> observableSource2;
        private Dictionary<TSource, int> sourceItems;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                if (observableSource2 != null)
                    yield return observableSource2;
            }
        }

        public ObservableIntersect(INotifyEnumerable<TSource> source, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
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
            return SL.Where(source, item => sourceItems.ContainsKey(item)).GetEnumerator();
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
            CollectionChangedNotificationResult<TSource> sourceChange = null;
            CollectionChangedNotificationResult<TSource> source2Change = null;

            if (sources[0].Source == source)
                sourceChange = (CollectionChangedNotificationResult<TSource>)sources[0];
            else
                source2Change = (CollectionChangedNotificationResult<TSource>)sources[0];

            if (sources.Count > 1)
            {
                if (sources[1].Source == source)
                    sourceChange = (CollectionChangedNotificationResult<TSource>)sources[1];
                else
                    source2Change = (CollectionChangedNotificationResult<TSource>)sources[1];
            }

            var added = new List<TSource>();
            var removed = new List<TSource>();

            if (source2Change != null)
            {
                if (source2Change.IsReset)
                {
                    sourceItems.Clear();
                    OnCleared();
                    return new CollectionChangedNotificationResult<TSource>(this);
                }
                else
                {
                    NotifySource2(source2Change, added, removed);
                }
            }

            if (sourceChange != null)
            {
                if (sourceChange.IsReset)
                {
                    OnCleared();
                    return new CollectionChangedNotificationResult<TSource>(this);
                }
                else
                {
                    NotifySource(sourceChange, added, removed);
                }
            }

            if (removed.Count == 0 && added.Count == 0)
                return UnchangedNotificationResult.Instance;

            OnRemoveItems(removed);
            OnAddItems(added);
            return new CollectionChangedNotificationResult<TSource>(this, SL.ToList(added), SL.ToList(removed));
        }

        private void NotifySource(CollectionChangedNotificationResult<TSource> change, List<TSource> added, List<TSource> removed)
        {
            foreach (var item in change.AllRemovedItems)
            {
                if (sourceItems.ContainsKey(item))
                {
                    removed.Add(item);
                }
            }
                
            foreach (var item in change.AllAddedItems)
            {
                if (sourceItems.ContainsKey(item))
                {
                    added.Add(item);
                }
            }
        }

        private void NotifySource2(CollectionChangedNotificationResult<TSource> change, List<TSource> added, List<TSource> removed)
        {
            var uniqueRemoved = new HashSet<TSource>(sourceItems.Comparer);
            foreach (var item in change.AllRemovedItems)
            {
                if (RemoveItem(item))
                {
                    uniqueRemoved.Add(item);
                }
            }
            removed.AddRange(SL.Where(source, item => uniqueRemoved.Contains(item)));

            var uniqueAdded = new HashSet<TSource>(sourceItems.Comparer);
            foreach (var item in change.AllAddedItems)
            {
                if (AddItem(item))
                {
                    uniqueAdded.Add(item);
                }
            }
            added.AddRange(SL.Where(source, item => uniqueAdded.Contains(item)));
        }
    }
}
