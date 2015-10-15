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
        private SortedDictionary<TKey, ObservableCollection<TItem>> searchTree;
        private ManualObservableCollectionView<IEnumerable<TItem>> manualRaiseSequences;


        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move) return;
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    var removed = new List<TItem>();
                    foreach (TItem item in e.OldItems)
                    {
                        TaggedObservableValue<TKey, Multiplicity<TItem>> lambdaResult;
                        if (lambdas.TryGetValue(item, out lambdaResult))
                        {
                            ObservableCollection<TItem> sequence;
                            if (searchTree.TryGetValue(lambdaResult.Value, out sequence))
                            {
                                sequence.Remove(item);
                                if (sequence.Count == 0)
                                {
                                    searchTree.Remove(lambdaResult.Value);
                                    manualRaiseSequences.NotifyRemoveItem(sequence);
                                }
                            }
                            removed.Add(lambdaResult.Tag.Item);
                            lambdaResult.Tag = new Multiplicity<TItem>(lambdaResult.Tag.Item, lambdaResult.Tag.Count - 1);
                            if (lambdaResult.Tag.Count == 0)
                            {
                                lambdaResult.ValueChanged -= LambdaChanged;
                                lambdaResult.Detach();
                                lambdas.Remove(item);
                            }
                        }
                        else
                        {
                            //throw new InvalidOperationException();
                        }
                    }
                    OnRemoveItems(removed);
                }
                if (e.NewItems != null)
                {
                    var added = new List<TItem>();
                    foreach (TItem item in e.NewItems)
                    {
                        AttachItem(item);
                        added.Add(item);
                    }
                    OnAddItems(added);
                }
            }
            else
            {
                DetachSource();
                foreach (var item in source)
                {
                    AttachItem(item);
                }
                OnCleared();
            }
        }

        private void DetachSource()
        {
            foreach (var pair in lambdas)
            {
                pair.Value.Detach();
            }
            lambdas.Clear();
            searchTree.Clear();
        }

        private void AttachItem(TItem item)
        {
            TaggedObservableValue<TKey, Multiplicity<TItem>> lambdaResult;
            if (!lambdas.TryGetValue(item, out lambdaResult))
            {
                lambdaResult = keySelector.InvokeTagged<Multiplicity<TItem>>(item);
                lambdaResult.ValueChanged += LambdaChanged;
                lambdas.Add(item, lambdaResult);
            }
            lambdaResult.Tag = new Multiplicity<TItem>(item, lambdaResult.Tag.Count + 1);
            ObservableCollection<TItem> sequence;
            if (!searchTree.TryGetValue(lambdaResult.Value, out sequence))
            {
                sequence = new ObservableCollection<TItem>();
                searchTree.Add(lambdaResult.Value, sequence);
                manualRaiseSequences.NotifyAddItem(sequence);
            }
            sequence.Add(item);
        }

        private void LambdaChanged(object sender, ValueChangedEventArgs e)
        {
            var value = sender as TaggedObservableValue<TKey, Multiplicity<TItem>>;

            TKey oldKey = (TKey)e.OldValue;
            TKey newKey = (TKey)e.NewValue;

            ObservableCollection<TItem> sequence;
            if (searchTree.TryGetValue(oldKey, out sequence))
            {
                sequence.Remove(value.Tag.Item);
                if (sequence.Count == 0)
                {
                    searchTree.Remove(oldKey);
                    manualRaiseSequences.NotifyRemoveItem(sequence);
                }
            }
            if (!searchTree.TryGetValue(newKey, out sequence))
            {
                sequence = new ObservableCollection<TItem>();
                searchTree.Add(newKey, sequence);
                manualRaiseSequences.NotifyAddItem(sequence);
            }
            sequence.Add(value.Tag.Item);
            OnMoveItem(value.Tag.Item);
        }

        protected override void AttachCore()
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    AttachItem(item);
                }
                source.CollectionChanged += SourceCollectionChanged;
            }
        }

        protected override void DetachCore()
        {
            DetachSource();
            source.CollectionChanged -= SourceCollectionChanged;
        }


        public ObservableOrderBy(INotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            this.source = source;
            this.keySelector = keySelector;

            this.searchTree = new SortedDictionary<TKey, ObservableCollection<TItem>>(comparer);
            this.manualRaiseSequences = new ManualObservableCollectionView<IEnumerable<TItem>>(searchTree.Values);

            Attach();
        }

        public INotifyEnumerable<IEnumerable<TItem>> Sequences
        {
            get { return manualRaiseSequences; }
        }

        public override IEnumerator<TItem> GetEnumerator()
        {
            return SL.SelectMany(searchTree.Values, o => o).GetEnumerator();
        }
    }
}
