using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableDistinct<TSource> : ObservableEnumerable<TSource>
    {
        private INotifyEnumerable<TSource> source;
        private Dictionary<TSource, int> occurences;
        private int nullOccurences = 0;

        public override IEnumerable<INotifiable> Dependencies { get { yield return source; } }

        public ObservableDistinct(INotifyEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");

            this.source = source;
            occurences = new Dictionary<TSource, int>(comparer);
        }

        public override IEnumerator<TSource> GetEnumerator()
        {
            return occurences.Keys.GetEnumerator();
        }

        private List<TSource> AddItems(IEnumerable items)
        {
            var added = new List<TSource>();
            foreach (TSource item in items)
            {
                if (AddItem(item))
                {
                    added.Add(item);
                }
            }
            if (added.Count > 0)
                OnAddItems(added);
            return added;
        }

        private bool AddItem(TSource item)
        {
            if (item != null)
            {
                int current;
                if (occurences.TryGetValue(item, out current))
                {
                    occurences[item] = current + 1;
                    return false;
                }
                else
                {
                    occurences.Add(item, 1);
                    return true;
                }
            }
            else
            {
                nullOccurences += 1;
                return nullOccurences == 1;
            }
        }

        private bool RemoveItem(TSource item)
        {
            if (item != null)
            {
                int current;
                if (occurences.TryGetValue(item, out current))
                {
                    current--;
                    if (current == 0)
                    {
                        occurences.Remove(item);
                        return true;
                    }
                    else
                    {
                        occurences[item] = current;
                    }
                }
                return false;
            }
            else
            {
                nullOccurences--;
                return nullOccurences == 0;
            }
        }

        protected override void OnAttach()
        {
            foreach (var item in source)
            {
                AddItem(item);
            }
        }

        protected override void OnDetach()
        {
            occurences.Clear();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var change = (CollectionChangedNotificationResult<TSource>)sources[0];

            if (change.IsReset)
            {
                occurences.Clear();
                OnCleared();
                AddItems(source);
                return CollectionChangedNotificationResult<TSource>.Reset(this);
            }

            List<TSource> removed = null;
            if (change.RemovedItems != null)
            {
                removed = new List<TSource>();
                foreach (var item in change.RemovedItems)
                {
                    if (RemoveItem(item))
                    {
                        removed.Add(item);
                    }
                }
                if (removed.Count > 0)
                    OnRemoveItems(removed);
            }

            List<TSource> added = null;
            if (change.AddedItems != null)
            {
                added = AddItems(change.AddedItems);
            }

            if ((removed == null || removed.Count == 0) && (added == null || added.Count == 0))
                return new UnchangedNotificationResult(this);

            return CollectionChangedNotificationResult<TSource>.AddRemove(this, added, removed);
        }
    }
}
