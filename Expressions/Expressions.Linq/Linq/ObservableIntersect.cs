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
        private Dictionary<TSource, int> sourceItems;

        public ObservableIntersect(INotifyEnumerable<TSource> source, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (source2 == null) throw new ArgumentNullException("source2");
           
            this.source = source;
            this.source2 = source2;
            sourceItems = new Dictionary<TSource, int>(comparer);

            Attach();
        }

        public override IEnumerator<TSource> GetEnumerator()
        {
            return SL.Where(source, item => sourceItems.ContainsKey(item)).GetEnumerator();
        }

        protected override void AttachCore()
        {
            source.CollectionChanged += SourceCollectionChanged;
            foreach (var item in source2)
            {
                AddItem(item);
            }
            var notifier = source2 as INotifyCollectionChanged;
            if (notifier != null)
            {
                notifier.CollectionChanged += Source2CollectionChanged;
            }
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

        protected override void DetachCore()
        {
            source.CollectionChanged -= SourceCollectionChanged;
            sourceItems.Clear();
            var notifier = source2 as INotifyCollectionChanged;
            if (notifier != null)
            {
                notifier.CollectionChanged -= Source2CollectionChanged;
            }
        }
    }
}
