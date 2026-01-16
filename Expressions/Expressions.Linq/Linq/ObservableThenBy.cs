using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Collections.ObjectModel;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableThenBy<TItem, TKey> : ObservableEnumerable<TItem>, IOrderableNotifyEnumerable<TItem>
    {
        public override string ToString()
        {
            return "[ThenBy]";
        }

        private readonly IOrderableNotifyEnumerable<TItem> source;
        private readonly Dictionary<IEnumerable<TItem>, SortedDictionary<TKey, Collection<TItem>>> searchTrees = new Dictionary<IEnumerable<TItem>, SortedDictionary<TKey, Collection<TItem>>>();
        private readonly Dictionary<TItem, Stack<TaggedObservableValue<TKey, SequenceInfo>>> lambdaResults = new Dictionary<TItem, Stack<TaggedObservableValue<TKey, SequenceInfo>>>();
        private readonly ObservingFunc<TItem, TKey> keySelector;
        private readonly IComparer<TKey> comparer;

        private struct SequenceInfo
        {
            public TItem Item { get; set; }

            public SortedDictionary<TKey, Collection<TItem>> SearchTree { get; set; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                foreach (var stack in lambdaResults.Values)
                {
                    foreach (var lambdaResult in stack)
                        yield return lambdaResult;
                }
            }
        }

        public ObservableThenBy(IOrderableNotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            this.source = source;
            this.keySelector = keySelector;
            this.comparer = comparer;
        }

        private void AttachSequence(IEnumerable<TItem> sequence)
        {
            var subSearchTree = new SortedDictionary<TKey, Collection<TItem>>(comparer);
            searchTrees.Add(sequence, subSearchTree);

            foreach (var item in sequence)
            {
                AttachItem(subSearchTree, item);
            }
        }

        private void AttachItem(SortedDictionary<TKey, Collection<TItem>> searchTree, TItem item)
        {
            var lambdaResult = keySelector.InvokeTagged(item, new SequenceInfo() { Item = item, SearchTree = searchTree });
            lambdaResult.Successors.Set(this);
            Stack<TaggedObservableValue<TKey, SequenceInfo>> lambdaStack;
            if (!lambdaResults.TryGetValue(item, out lambdaStack))
            {
                lambdaStack = new Stack<TaggedObservableValue<TKey, SequenceInfo>>();
                lambdaResults.Add(item, lambdaStack);
            }
            lambdaStack.Push(lambdaResult);

            Collection<TItem> itemSequence;
            if (!searchTree.TryGetValue(lambdaResult.Value, out itemSequence))
            {
                itemSequence = new Collection<TItem>();
                searchTree.Add(lambdaResult.Value, itemSequence);
            }
            itemSequence.Add(item);
        }

        private void DetachItem(SortedDictionary<TKey, Collection<TItem>> searchTree, TItem item)
        {
            Stack<TaggedObservableValue<TKey, SequenceInfo>> lambdaStack;
            if (lambdaResults.TryGetValue(item, out lambdaStack))
            {
                var lambdaResult = lambdaStack.Pop();
                lambdaResult.Successors.Unset(this);

                if (lambdaStack.Count == 0)
                {
                    lambdaResults.Remove(item);
                }

                Collection<TItem> itemSequence;
                if (searchTree.TryGetValue(lambdaResult.Value, out itemSequence))
                {
                    itemSequence.Remove(item);
                    if (itemSequence.Count == 0)
                    {
                        searchTree.Remove(lambdaResult.Value);
                    }
                }
            }
        }

        public override IEnumerator<TItem> GetEnumerator()
        {
            return SL.SelectMany(Sequences, items => items).GetEnumerator();
        }

        public IEnumerable<IEnumerable<TItem>> Sequences
        {
            get { return SL.SelectMany(source.Sequences, sequence => searchTrees[sequence].Values); }
        }

        public IEnumerable<TItem> GetSequenceForItem(TItem item)
        {
            Stack<TaggedObservableValue<TKey, SequenceInfo>> stack;
            if (lambdaResults.TryGetValue(item, out stack))
            {
                var lambdaResult = stack.Peek();
                Collection<TItem> sequence;
                if (lambdaResult.Tag.SearchTree.TryGetValue(lambdaResult.Value, out sequence))
                {
                    return sequence;
                }
            }
            return null;
        }

        protected override void OnAttach()
        {
            foreach (var sequence in source.Sequences)
            {
                AttachSequence(sequence);
            }
        }

        protected override void OnDetach()
        {
            foreach (var lambdaStack in lambdaResults.Values)
            {
                foreach (var lambda in lambdaStack)
                {
                    lambda.Successors.Unset(this);
                }
            }

            searchTrees.Clear();
            lambdaResults.Clear();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var notification = CollectionChangedNotificationResult<TItem>.Create(this);
            var added = notification.AddedItems;
            var removed = notification.RemovedItems;
            var moved = notification.MovedItems;

            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var sourceChange = (ICollectionChangedNotificationResult<TItem>)change;
                    if (sourceChange.IsReset)
                    {
                        OnDetach();
                        OnAttach();
                        OnCleared();
                        notification.TurnIntoReset();
                        return notification;
                    }
                    else
                    {
                        NotifySource(sourceChange, added, removed, moved);
                    }
                }
                else
                {
                    NotifyKeyChange(moved, change);
                }
            }

            OnRemoveItems(removed);
            OnAddItems(added);
            OnMoveItems(moved);
            return notification;
        }

        private static void NotifyKeyChange(List<TItem> moved, INotificationResult change)
        {
            var lambdaResult = (TaggedObservableValue<TKey, SequenceInfo>)change.Source;
            var searchTree = lambdaResult.Tag.SearchTree;
            var keyChange = (IValueChangedNotificationResult<TKey>)change;

            Collection<TItem> itemSequence;
            if (searchTree.TryGetValue(keyChange.OldValue, out itemSequence))
            {
                itemSequence.Remove(lambdaResult.Tag.Item);
                if (itemSequence.Count == 0)
                {
                    searchTree.Remove(keyChange.OldValue);
                }
            }
            if (!searchTree.TryGetValue(keyChange.NewValue, out itemSequence))
            {
                itemSequence = new Collection<TItem>();
                searchTree.Add(keyChange.NewValue, itemSequence);
            }
            itemSequence.Add(lambdaResult.Tag.Item);

            moved.Add(lambdaResult.Tag.Item);
        }

        private void NotifySource(ICollectionChangedNotificationResult<TItem> sourceChange, List<TItem> added, List<TItem> removed, List<TItem> moved)
        {
            if (sourceChange.RemovedItems != null)
            {
                foreach (var item in sourceChange.RemovedItems)
                {
                    var searchTree = lambdaResults[item].Peek().Tag.SearchTree;
                    DetachItem(searchTree, item);
                }
                removed.AddRange(sourceChange.RemovedItems);
            }

            if (sourceChange.AddedItems != null)
            {
                foreach (var item in sourceChange.AddedItems)
                {
                    var sequence = source.GetSequenceForItem(item);
                    SortedDictionary<TKey, Collection<TItem>> searchTree;
                    if (!searchTrees.TryGetValue(sequence, out searchTree))
                    {
                        searchTree = new SortedDictionary<TKey, Collection<TItem>>(comparer);
                        searchTrees.Add(sequence, searchTree);
                    }
                    AttachItem(searchTree, item);
                }
                added.AddRange(sourceChange.AddedItems);
            }

            if (sourceChange.MovedItems != null)
            {
                foreach (var item in sourceChange.MovedItems)
                {
                    var searchTree = lambdaResults[item].Peek().Tag.SearchTree;
                    DetachItem(searchTree, item);

                    var sequence = source.GetSequenceForItem(item);
                    if (!searchTrees.TryGetValue(sequence, out searchTree))
                    {
                        searchTree = new SortedDictionary<TKey, Collection<TItem>>(comparer);
                        searchTrees.Add(sequence, searchTree);
                    }
                    AttachItem(searchTree, item);
                }

                moved.AddRange(sourceChange.MovedItems);
            }
        }

        public override void RequireOrder(bool isOrderRequired)
        {
        }

        public override bool IsOrdered => true;
    }
}
