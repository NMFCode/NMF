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

        public ObservableDistinct(INotifyEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");

            this.source = source;
            occurences = new Dictionary<TSource, int>(comparer);

            Attach();
        }

        public override IEnumerator<TSource> GetEnumerator()
        {
            return occurences.Keys.GetEnumerator();
        }

        protected override void AttachCore()
        {
            foreach (var item in source)
            {
                AddItem(item);
            }
            source.CollectionChanged += SourceCollectionChanged;
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move) return;
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    var removed = new List<TSource>();
                    foreach (TSource item in e.OldItems)
                    {
                        if (RemoveItem(item))
                        {
                            removed.Add(item);
                        }
                    }
                    if (removed.Count > 0) OnRemoveItems(removed);
                }
                if (e.NewItems != null)
                {
                    AddItems(e.NewItems);
                }
            }
            else
            {
                occurences.Clear();
                OnCleared();
                AddItems(source);
            }
        }

        private void AddItems(IEnumerable items)
        {
            var added = new List<TSource>();
            foreach (TSource item in items)
            {
                if (AddItem(item))
                {
                    added.Add(item);
                }
            }
            if (added.Count > 0) OnAddItems(added);
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

        protected override void DetachCore()
        {
            occurences.Clear();
            source.CollectionChanged -= SourceCollectionChanged;
        }
    }
}
