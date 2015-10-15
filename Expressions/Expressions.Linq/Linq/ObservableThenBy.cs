using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableThenBy<TItem, TKey> : ObservableEnumerable<TItem>, IOrderableNotifyEnumerable<TItem>
    {
        private IOrderableNotifyEnumerable<TItem> source;
        private Dictionary<IEnumerable<TItem>, SortedDictionary<TKey, ObservableCollection<TItem>>> searchTrees = new Dictionary<IEnumerable<TItem>, SortedDictionary<TKey, ObservableCollection<TItem>>>();
        private ManualObservableCollectionView<IEnumerable<TItem>> manualNotifier;
        private Dictionary<TItem, Stack<TaggedObservableValue<TKey, SequenceInfo>>> lambdaResults = new Dictionary<TItem, Stack<TaggedObservableValue<TKey, SequenceInfo>>>();
        private ObservingFunc<TItem, TKey> keySelector;
        private IComparer<TKey> comparer;

        private struct SequenceInfo
        {
            public TItem Item { get; set; }

            public SortedDictionary<TKey, ObservableCollection<TItem>> SearchTree { get; set; }
        }

        public ObservableThenBy(IOrderableNotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            this.source = source;
            this.keySelector = keySelector;
            this.comparer = comparer;
            manualNotifier = new ManualObservableCollectionView<IEnumerable<TItem>>(SL.SelectMany(source.Sequences,
                sequence => searchTrees[sequence].Values));

            Attach();
        }

        private void DetachSource()
        {
            foreach (var lambdaStack in lambdaResults.Values)
            {
                foreach (var lambda in lambdaStack)
                {
                    lambda.ValueChanged -= KeySelectorValueChanged;
                    lambda.Detach();
                }
            }
            foreach (var sequence in searchTrees.Keys)
            {
                var notifier = sequence as INotifyCollectionChanged;
                if (notifier != null)
                {
                    notifier.CollectionChanged -= SequenceChanged;
                }
            }
            searchTrees.Clear();
            lambdaResults.Clear();
            source.Sequences.CollectionChanged -= SourceSequencesChanged;
            OnCleared();
        }

        private void AttachSequence(IEnumerable<TItem> sequence)
        {
            var subSearchTree = new SortedDictionary<TKey, ObservableCollection<TItem>>(comparer);
            searchTrees.Add(sequence, subSearchTree);

            foreach (var item in sequence)
            {
                AttachItem(subSearchTree, item);
            }

            var notifier = sequence as INotifyCollectionChanged;
            if (notifier != null)
            {
                notifier.CollectionChanged += SequenceChanged;
            }
        }

        private void DetachSequence(IEnumerable<TItem> sequence)
        {
            SortedDictionary<TKey, ObservableCollection<TItem>> subSearchTree;
            if (searchTrees.TryGetValue(sequence, out subSearchTree))
            {
                searchTrees.Remove(sequence);

                foreach (var item in sequence)
                {
                    DetachItem(subSearchTree, item);
                }

                var notifier = sequence as INotifyCollectionChanged;
                if (notifier != null)
                {
                    notifier.CollectionChanged -= SequenceChanged;
                }
            }
        }

        private void AttachItem(SortedDictionary<TKey, ObservableCollection<TItem>> searchTree, TItem item)
        {
            var lambdaResult = keySelector.InvokeTagged<SequenceInfo>(item, new SequenceInfo() { Item = item, SearchTree = searchTree });
            lambdaResult.ValueChanged += KeySelectorValueChanged;
            Stack<TaggedObservableValue<TKey, SequenceInfo>> lambdaStack;
            if (!lambdaResults.TryGetValue(item, out lambdaStack))
            {
                lambdaStack = new Stack<TaggedObservableValue<TKey, SequenceInfo>>();
                lambdaResults.Add(item, lambdaStack);
            }
            lambdaStack.Push(lambdaResult);

            ObservableCollection<TItem> itemSequence;
            if (!searchTree.TryGetValue(lambdaResult.Value, out itemSequence))
            {
                itemSequence = new ObservableCollection<TItem>();
                searchTree.Add(lambdaResult.Value, itemSequence);
                manualNotifier.NotifyAddItem(itemSequence);
            }
            itemSequence.Add(item);
        }

        private void DetachItem(SortedDictionary<TKey, ObservableCollection<TItem>> searchTree, TItem item)
        {
            Stack<TaggedObservableValue<TKey, SequenceInfo>> lambdaStack;
            if (lambdaResults.TryGetValue(item, out lambdaStack))
            {
                var lambdaResult = lambdaStack.Pop();
                lambdaResult.ValueChanged -= KeySelectorValueChanged;
                lambdaResult.Detach();

                if (lambdaStack.Count == 0)
                {
                    lambdaResults.Remove(item);
                }

                ObservableCollection<TItem> itemSequence;
                if (searchTree.TryGetValue(lambdaResult.Value, out itemSequence))
                {
                    itemSequence.Remove(item);
                    if (itemSequence.Count == 0)
                    {
                        searchTree.Remove(lambdaResult.Value);
                        manualNotifier.NotifyRemoveItem(itemSequence);
                    }
                }
            }
        }

        private void KeySelectorValueChanged(object sender, ValueChangedEventArgs e)
        {
            var lambdaResult = sender as TaggedObservableValue<TKey, SequenceInfo>;
            var searchTree = lambdaResult.Tag.SearchTree;
            ObservableCollection<TItem> itemSequence;
            if (searchTree.TryGetValue((TKey)e.OldValue, out itemSequence))
            {
                itemSequence.Remove(lambdaResult.Tag.Item);
                if (itemSequence.Count == 0)
                {
                    searchTree.Remove((TKey)e.OldValue);
                    manualNotifier.NotifyRemoveItem(itemSequence);
                }
            }
            if (!searchTree.TryGetValue(lambdaResult.Value, out itemSequence))
            {
                itemSequence = new ObservableCollection<TItem>();
                searchTree.Add(lambdaResult.Value, itemSequence);
                manualNotifier.NotifyAddItem(itemSequence);
            }
            itemSequence.Add(lambdaResult.Tag.Item);
            OnMoveItem(lambdaResult.Tag.Item);
        }

        private void SourceSequencesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move) return;
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    var removed = new List<TItem>();
                    foreach (IEnumerable<TItem> sequence in e.OldItems)
                    {
                        DetachSequence(sequence);
                        removed.AddRange(sequence);
                    }
                    OnRemoveItems(removed);
                }
                if (e.NewItems != null)
                {
                    var added = new List<TItem>();
                    foreach (IEnumerable<TItem> sequence in e.NewItems)
                    {
                        AttachSequence(sequence);
                        added.AddRange(sequence);
                    }
                    OnAddItems(added);
                }
            }
            else
            {
                DetachSource();
                OnCleared();
            }
        }

        private void SequenceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SortedDictionary<TKey, ObservableCollection<TItem>> searchTree;
            if (searchTrees.TryGetValue(sender as IEnumerable<TItem>, out searchTree))
            {
                if (e.OldItems != null)
                {
                    foreach (TItem item in e.OldItems)
                    {
                        DetachItem(searchTree, item);
                    }
                    OnRemoveItems(SL.Cast<TItem>(e.OldItems));
                }
                if (e.NewItems != null)
                {
                    foreach (TItem item in e.NewItems)
                    {
                        AttachItem(searchTree, item);
                    }
                    OnAddItems(SL.Cast<TItem>(e.NewItems));
                }
            }
        }

        public override IEnumerator<TItem> GetEnumerator()
        {
            return SL.SelectMany(SL.SelectMany(source.Sequences, sequence => searchTrees[sequence].Values), items => items)
                .GetEnumerator();
        }

        public INotifyEnumerable<IEnumerable<TItem>> Sequences
        {
            get { return manualNotifier; }
        }

        protected override void AttachCore()
        {
            foreach (var sequence in source.Sequences)
            {
                AttachSequence(sequence);
            }

            source.Sequences.CollectionChanged += SourceSequencesChanged;
        }

        protected override void DetachCore()
        {
            DetachSource();
        }
    }
}
