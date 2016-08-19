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
            sourceItems = new Dictionary<TSource, int>(comparer);
        }

        public override IEnumerator<TSource> GetEnumerator()
        {
            return SL.Where(source, item => sourceItems.ContainsKey(item)).GetEnumerator();
        }

        private void SourceCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move) return;
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                OnCleared();
                return;
            }
            if (e.OldItems != null)
            {
                var removed = new List<TSource>();
                foreach (TSource item in e.OldItems)
                {
                    if (sourceItems.ContainsKey(item))
                    {
                        removed.Add(item);
                    }
                }
                if (removed.Count != 0) OnRemoveItems(removed);
            }
            if (e.NewItems != null)
            {
                var added = new List<TSource>();
                foreach (TSource item in e.NewItems)
                {
                    if (sourceItems.ContainsKey(item))
                    {
                        added.Add(item);
                    }
                }
                if (added.Count != 0) OnAddItems(added);
            }
        }

        private void Source2CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move) return;
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                sourceItems.Clear();
                OnCleared();
            }
            if (e.OldItems != null)
            {
                var removed = new HashSet<TSource>(sourceItems.Comparer);
                foreach (TSource item in e.OldItems)
                {
                    if (RemoveItem(item))
                    {
                        removed.Add(item);
                    }
                }
                var changed = SL.Where(source, item => removed.Contains(item));
                OnRemoveItems(changed);
            }
            if (e.NewItems != null)
            {
                var added = new HashSet<TSource>(sourceItems.Comparer);
                foreach (TSource item in e.NewItems)
                {
                    if (AddItem(item))
                    {
                        added.Add(item);
                    }
                }
                var changed = SL.Where(source, item => added.Contains(item));
                OnAddItems(changed);
            }
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

            var added = new HashSet<TSource>(sourceItems.Comparer);
            var removed = new HashSet<TSource>(sourceItems.Comparer);

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
                return new UnchangedNotificationResult(this);

            OnRemoveItems(removed);
            OnAddItems(added);
            return new CollectionChangedNotificationResult<TSource>(this, SL.ToList(added), SL.ToList(removed));
        }

        private void NotifySource(CollectionChangedNotificationResult<TSource> change, HashSet<TSource> added, HashSet<TSource> removed)
        {
            if (change.RemovedItems != null)
            {
                foreach (var item in change.RemovedItems)
                {
                    if (sourceItems.ContainsKey(item))
                    {
                        removed.Add(item);
                    }
                }
            }

            if (change.AddedItems != null)
            {
                foreach (var item in change.AddedItems)
                {
                    if (sourceItems.ContainsKey(item))
                    {
                        added.Add(item);
                    }
                }
            }
        }

        private void NotifySource2(CollectionChangedNotificationResult<TSource> change, HashSet<TSource> added, HashSet<TSource> removed)
        {
            if (change.RemovedItems != null)
            {
                foreach (var item in change.RemovedItems)
                {
                    if (RemoveItem(item) && source.Contains(item))
                    {
                        removed.Add(item);
                    }
                }
            }

            if (change.AddedItems != null)
            {
                foreach (var item in change.AddedItems)
                {
                    if (AddItem(item) && source.Contains(item))
                    {
                        added.Add(item);
                    }
                }
            }
        }
    }
}
