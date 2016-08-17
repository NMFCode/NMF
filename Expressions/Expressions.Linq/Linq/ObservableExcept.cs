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

        public ObservableExcept(INotifyEnumerable<TSource> source, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (source2 == null) throw new ArgumentNullException("source2");

            this.source = source;
            this.source2 = source2;
            this.observableSource2 = source2 as INotifyEnumerable<TSource>;
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
            var change = (CollectionChangedNotificationResult<TSource>)sources[0];
            if (sources.Count > 1 || change.IsReset)
            {
                sourceItems.Clear();
                OnCleared();
                foreach (var item in source2)
                    AddItem(item);
                return new CollectionChangedNotificationResult<TSource>(this);
            }

            var added = new List<TSource>();
            var removed = new List<TSource>();

            if (change.Source == source)
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
            else //change.Source == source2
            {
                if (change.RemovedItems != null)
                {
                    var uniqueRemoved = new HashSet<TSource>(sourceItems.Comparer);
                    foreach (var item in change.RemovedItems)
                    {
                        if (RemoveItem(item))
                        {
                            removed.Add(item);
                        }
                    }
                    added = SL.Where(source, item => removed.Contains(item)).ToList();
                }
                if (change.AddedItems != null)
                {
                    var uniqueAdded = new HashSet<TSource>(sourceItems.Comparer);
                    foreach (var item in change.AddedItems)
                    {
                        if (AddItem(item))
                        {
                            added.Add(item);
                        }
                    }
                    removed = SL.Where(source, item => added.Contains(item)).ToList();
                }
            }

            if (removed.Count != 0)
                OnRemoveItems(removed);
            if (added.Count != 0)
                OnAddItems(added);
            return new CollectionChangedNotificationResult<TSource>(this, added, removed);
        }
    }
}
