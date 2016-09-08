using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SL = System.Linq.Enumerable;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableOrderBy<TItem, TKey> : ObservableEnumerable<TItem>, IOrderableNotifyEnumerable<TItem>
    {
        private INotifyEnumerable<TItem> source;
        private ObservingFunc<TItem, TKey> keySelector;
        private Dictionary<TItem, TaggedObservableValue<TKey, Multiplicity<TItem>>> lambdas = new Dictionary<TItem, TaggedObservableValue<TKey, Multiplicity<TItem>>>();
        private SortedDictionary<TKey, Collection<TItem>> searchTree;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                foreach (var tagged in lambdas.Values)
                    yield return tagged;
            }
        }

        public ObservableOrderBy(INotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            this.source = source;
            this.keySelector = keySelector;

            this.searchTree = new SortedDictionary<TKey, Collection<TItem>>(comparer);
        }

        private void AttachItem(TItem item)
        {
            TaggedObservableValue<TKey, Multiplicity<TItem>> lambdaResult;
            if (!lambdas.TryGetValue(item, out lambdaResult))
            {
                lambdaResult = keySelector.InvokeTagged<Multiplicity<TItem>>(item);
                lambdas.Add(item, lambdaResult);
                lambdaResult.Successors.Set(this);
            }
            lambdaResult.Tag = new Multiplicity<TItem>(item, lambdaResult.Tag.Count + 1);
            Collection<TItem> sequence;
            if (!searchTree.TryGetValue(lambdaResult.Value, out sequence))
            {
                sequence = new Collection<TItem>();
                searchTree.Add(lambdaResult.Value, sequence);
            }
            sequence.Add(item);
        }

        public IEnumerable<IEnumerable<TItem>> Sequences
        {
            get { return searchTree.Values; }
        }

        public override IEnumerator<TItem> GetEnumerator()
        {
            return SL.SelectMany(searchTree.Values, o => o).GetEnumerator();
        }

        public IEnumerable<TItem> GetSequenceForItem(TItem item)
        {
            TaggedObservableValue<TKey, Multiplicity<TItem>> lambdaResult;
            if (lambdas.TryGetValue(item, out lambdaResult))
            {
                Collection<TItem> sequence;
                if (searchTree.TryGetValue(lambdaResult.Value, out sequence))
                {
                    return sequence;
                }
            }
            return null;
        }

        protected override void OnAttach()
        {
            foreach (var item in source)
            {
                AttachItem(item);
            }
        }

        protected override void OnDetach()
        {
            foreach (var tagged in lambdas.Values)
            {
                tagged.Successors.Unset(this);
            }
            lambdas.Clear();
            searchTree.Clear();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var added = new List<TItem>();
            var removed = new List<TItem>();
            var moved = new List<TItem>();

            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var sourceChange = (CollectionChangedNotificationResult<TItem>)change;
                    if (sourceChange.IsReset)
                    {
                        OnDetach();
                        OnAttach();
                        OnCleared();
                        return new CollectionChangedNotificationResult<TItem>(this);
                    }
                    else
                    {
                        NotifySource(sourceChange, added, removed);
                    }
                }
                else
                {
                    var tagged = (TaggedObservableValue<TKey, Multiplicity<TItem>>)change.Source;
                    var keyChange = (ValueChangedNotificationResult<TKey>)change;
                    
                    Collection<TItem> sequence;
                    if (searchTree.TryGetValue(keyChange.OldValue, out sequence))
                    {
                        sequence.Remove(tagged.Tag.Item);
                        if (sequence.Count == 0)
                        {
                            searchTree.Remove(keyChange.OldValue);
                        }
                    }
                    if (!searchTree.TryGetValue(keyChange.NewValue, out sequence))
                    {
                        sequence = new Collection<TItem>();
                        searchTree.Add(keyChange.NewValue, sequence);
                    }
                    sequence.Add(tagged.Tag.Item);
                    
                    moved.Add(tagged.Tag.Item);
                }
            }

            if (added.Count == 0 && removed.Count == 0 && moved.Count == 0)
                return UnchangedNotificationResult.Instance;

            OnRemoveItems(removed);
            OnAddItems(added);
            OnMoveItems(moved);
            return new CollectionChangedNotificationResult<TItem>(this, added, removed, moved);
        }

        private void NotifySource(CollectionChangedNotificationResult<TItem> sourceChange, List<TItem> added, List<TItem> removed)
        {
            foreach (var item in sourceChange.AllRemovedItems)
            {
                var lambdaResult = lambdas[item];
                Collection<TItem> sequence;
                if (searchTree.TryGetValue(lambdaResult.Value, out sequence))
                {
                    sequence.Remove(item);
                    if (sequence.Count == 0)
                    {
                        searchTree.Remove(lambdaResult.Value);
                    }
                }
                removed.Add(item);
                        
                if (lambdaResult.Tag.Count == 1)
                {
                    lambdas.Remove(item);
                    lambdaResult.Successors.Unset(this);
                }
                else
                {
                    lambdaResult.Tag = new Multiplicity<TItem>(lambdaResult.Tag.Item, lambdaResult.Tag.Count - 1);
                }
            }
                
            foreach (var item in sourceChange.AllAddedItems)
            {
                AttachItem(item);
                added.Add(item);
            }
        }
    }
}
