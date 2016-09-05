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
                OnDetach();
                OnAttach();
                OnCleared();
                return new CollectionChangedNotificationResult<TSource>(this);
            }

            var removed = new List<TSource>();
            var added = new List<TSource>();
            
            foreach (var item in change.AllRemovedItems)
            {
                if (RemoveItem(item))
                {
                    removed.Add(item);
                }
            }

            foreach (var item in change.AllAddedItems)
            {
                if (AddItem(item))
                {
                    added.Add(item);
                }
            }

            if (removed.Count == 0 && added.Count == 0)
                return UnchangedNotificationResult.Instance;

            OnRemoveItems(removed);
            OnAddItems(added);
            return new CollectionChangedNotificationResult<TSource>(this, added, removed);
        }
    }
}
