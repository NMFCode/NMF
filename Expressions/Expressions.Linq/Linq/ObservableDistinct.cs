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
        public override string ToString()
        {
            return "[Distinct]";
        }

        private readonly INotifyEnumerable<TSource> source;
        private readonly Dictionary<TSource, int> occurences;
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
            var change = (ICollectionChangedNotificationResult<TSource>)sources[0];

            if (change.IsReset)
            {
                OnDetach();
                OnAttach();
                OnCleared();
                return CollectionChangedNotificationResult<TSource>.Create(this, true);
            }

            var notification = CollectionChangedNotificationResult<TSource>.Create(this);
            var removed = notification.RemovedItems;
            var added = notification.AddedItems;

            if (change.RemovedItems != null)
            {
                foreach (var item in change.RemovedItems)
                {
                    if (RemoveItem(item))
                    {
                        removed.Add(item);
                    }
                }
            }

            if (change.AddedItems != null)
            {
                foreach (var item in change.AddedItems)
                {
                    if (AddItem(item))
                    {
                        added.Add(item);
                    }
                }
            }

            OnRemoveItems(removed);
            OnAddItems(added);
            return notification;
        }
    }
}
