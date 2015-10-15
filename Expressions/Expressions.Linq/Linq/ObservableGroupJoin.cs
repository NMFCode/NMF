using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableGroupJoin<TOuter, TInner, TKey, TResult> : ObservableEnumerable<TResult>
    {
        private INotifyEnumerable<TOuter> outerSource;
        private IEnumerable<TInner> innerSource;
        private ObservingFunc<TOuter, TKey> outerKeySelector;
        private ObservingFunc<TInner, TKey> innerKeySelector;
        private ObservingFunc<TOuter, IEnumerable<TInner>, TResult> resultSelector;

        private Dictionary<TKey, KeyGroup> groups;
        private Dictionary<TInner, Stack<TaggedObservableValue<TKey, TInner>>> innerValues = new Dictionary<TInner, Stack<TaggedObservableValue<TKey, TInner>>>();
        private Dictionary<TOuter, Stack<TaggedObservableValue<TKey, TOuter>>> outerValues = new Dictionary<TOuter, Stack<TaggedObservableValue<TKey, TOuter>>>();

        public ObservableGroupJoin(INotifyEnumerable<TOuter> outerSource, IEnumerable<TInner> innerSource, ObservingFunc<TOuter, TKey> outerKeySelector, ObservingFunc<TInner, TKey> innerKeySelector, ObservingFunc<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outerSource == null) throw new ArgumentNullException("outerSource");
            if (innerSource == null) throw new ArgumentNullException("innerSource");
            if (outerKeySelector == null) throw new ArgumentNullException("outerKeySelector");
            if (innerKeySelector == null) throw new ArgumentNullException("innerKeySelector");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            this.outerSource = outerSource;
            this.innerSource = innerSource;
            this.outerKeySelector = outerKeySelector;
            this.innerKeySelector = innerKeySelector;
            this.resultSelector = resultSelector;

            groups = new Dictionary<TKey, KeyGroup>(comparer);

            Attach();
        }

        private class KeyGroup
        {
            public Dictionary<TaggedObservableValue<TKey, TOuter>, TaggedObservableValue<TResult, TOuter>> OuterElements = new Dictionary<TaggedObservableValue<TKey, TOuter>, TaggedObservableValue<TResult, TOuter>>();
            public List<TaggedObservableValue<TKey, TInner>> InnerKeys = new List<TaggedObservableValue<TKey, TInner>>();

            public ManualObservableCollectionView<TInner> collectionView;

            public KeyGroup()
            {
                collectionView = new ManualObservableCollectionView<TInner>(InnerKeys.Select(key => key.Tag));
            }
        }

        public override IEnumerator<TResult> GetEnumerator()
        {
            return groups.Values.SelectMany(group => group.OuterElements.Values.Select(result => result.Value)).GetEnumerator();
        }

        public override int Count
        {
            get
            {
                return groups.Values.Sum(group => group.OuterElements.Count);
            }
        }

        protected override void AttachCore()
        {
            outerSource.Attach();
            outerSource.CollectionChanged += OuterSourceCollectionChanged;
            var innerSourceNotifiable = innerSource as INotifyEnumerable<TInner>;
            if (innerSourceNotifiable != null)
            {
                innerSourceNotifiable.Attach();
                innerSourceNotifiable.CollectionChanged += InnerSourceCollectionChanged;
            }
            else
            {
                var notifier = innerSource as INotifyCollectionChanged;
                if (notifier != null)
                {
                    notifier.CollectionChanged += InnerSourceCollectionChanged;
                }
            }
            foreach (var item in outerSource)
            {
                var keyValue = outerKeySelector.InvokeTagged(item, item);
                Stack<TaggedObservableValue<TKey, TOuter>> valueStack;
                if (!outerValues.TryGetValue(item, out valueStack))
                {
                    valueStack = new Stack<TaggedObservableValue<TKey, TOuter>>();
                    outerValues.Add(item, valueStack);
                }
                valueStack.Push(keyValue);
                KeyGroup group;
                if (!groups.TryGetValue(keyValue.Value, out group))
                {
                    group = new KeyGroup();
                    groups.Add(keyValue.Value, group);
                }
                keyValue.ValueChanged += OuterKeyChanged;
                var resultValue = resultSelector.InvokeTagged(item, group.InnerKeys.Select(t => t.Tag), item);
                group.OuterElements.Add(keyValue, resultValue);
            }
            foreach (var item in innerSource)
            {
                AttachInner(item, null);
            }
        }

        private void AttachInner(TInner item, ICollection<TResult> added)
        {
            var keyValue = innerKeySelector.InvokeTagged(item, item);
            Stack<TaggedObservableValue<TKey, TInner>> valueStack;
            if (!innerValues.TryGetValue(item, out valueStack))
            {
                valueStack = new Stack<TaggedObservableValue<TKey, TInner>>();
                innerValues.Add(item, valueStack);
            }
            valueStack.Push(keyValue);
            KeyGroup group;
            if (!groups.TryGetValue(keyValue.Value, out group))
            {
                group = new KeyGroup();
                groups.Add(keyValue.Value, group);
            }
            keyValue.ValueChanged += InnerKeyChanged;
            group.InnerKeys.Add(keyValue);
            group.collectionView.NotifyAddItem(item);
            if (group.InnerKeys.Count == 1 && added != null)
            {
                foreach (var result in group.OuterElements.Values)
                {
                    added.Add(result.Value);
                }
            }
        }

        private void ResultValueChanged(object sender, ValueChangedEventArgs e)
        {
            OnUpdateItem((TResult)e.NewValue, (TResult)e.OldValue);
        }

        private void OuterKeyChanged(object sender, ValueChangedEventArgs e)
        {
            var value = sender as TaggedObservableValue<TKey, TOuter>;
            var oldKey = (TKey)e.OldValue;
            var group = groups[oldKey];
            group.OuterElements.Remove(value);
            bool isRemoving = false;
            TResult removed = default(TResult);
            if (group.InnerKeys.Count != 0)
            {
                isRemoving = true;
                var result = group.OuterElements[value];
                removed = result.Value;
                result.ValueChanged -= ResultValueChanged;
                result.Detach();
            }
            if (!groups.TryGetValue(value.Value, out group))
            {
                group = new KeyGroup();
                groups.Add(value.Value, group);
            }
            var newResult = resultSelector.InvokeTagged(value.Tag, group.collectionView, value.Tag);
            newResult.ValueChanged += ResultValueChanged;
            group.OuterElements.Add(value, newResult);
            if (isRemoving)
            {
                OnRemoveItem(removed);
            }
            if (group.InnerKeys.Count != 0)
            {
                OnAddItem(newResult.Value);
            }
        }

        private void InnerKeyChanged(object sender, ValueChangedEventArgs e)
        {
            var value = sender as TaggedObservableValue<TKey, TInner>;
            var oldKey = (TKey)e.OldValue;
            var group = groups[oldKey];
            group.InnerKeys.Remove(value);
            group.collectionView.NotifyRemoveItem(value.Tag);
            var removed = new List<TResult>();
            if (group.InnerKeys.Count == 0)
            {
                removed.AddRange(group.OuterElements.Values.Select(r => r.Value));
            }
            if (!groups.TryGetValue(value.Value, out group))
            {
                group = new KeyGroup();
                groups.Add(value.Value, group);
            }
            group.InnerKeys.Add(value);
            group.collectionView.NotifyAddItem(value.Tag);
            var added = new List<TResult>();
            if (group.InnerKeys.Count == 1)
            {
                added.AddRange(group.OuterElements.Values.Select(r => r.Value));
            }
            OnReplaceItems(removed, added);
        }

        private void InnerSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var group in groups.Values)
                {
                    foreach (var val in group.InnerKeys)
                    {
                        val.Detach();
                        val.ValueChanged -= InnerKeyChanged;
                    }
                    group.InnerKeys.Clear();
                }
                innerValues.Clear();
                OnCleared();
            }
            else if (e.Action != NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    var removed = new List<TResult>();
                    foreach (TInner inner in e.OldItems)
                    {
                        var valueStack = innerValues[inner];
                        var value = valueStack.Pop();
                        if (valueStack.Count == 0)
                        {
                            innerValues.Remove(inner);
                        }
                        var group = groups[value.Value];
                        group.InnerKeys.Remove(value);
                        group.collectionView.NotifyRemoveItem(inner);
                        if (group.InnerKeys.Count == 0)
                        {
                            if (group.OuterElements.Count == 0)
                            {
                                groups.Remove(value.Value);
                            }
                            else
                            {
                                removed.AddRange(group.OuterElements.Values.Select(r => r.Value));
                            }
                        }
                        value.ValueChanged -= InnerKeyChanged;
                        value.Detach();
                    }
                    OnRemoveItems(removed);
                }
                if (e.NewItems != null)
                {
                    var added = new List<TResult>();
                    foreach (TInner inner in e.NewItems)
                    {
                        AttachInner(inner, added);
                    }
                    OnAddItems(added);
                }
            }
        }

        private void OuterSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var group in groups.Values)
                {
                    foreach (var val in group.OuterElements)
                    {
                        val.Key.Detach();
                        val.Key.ValueChanged -= OuterKeyChanged;
                        val.Value.Detach();
                        val.Value.ValueChanged -= ResultValueChanged;
                    }
                    group.OuterElements.Clear();
                }
                outerValues.Clear();
                OnCleared();
            }
            else if (e.Action != NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    var removed = new List<TResult>();
                    foreach (TOuter outer in e.OldItems)
                    {
                        var valueStack = outerValues[outer];
                        var value = valueStack.Pop();
                        if (valueStack.Count == 0)
                        {
                            outerValues.Remove(outer);
                        }
                        var group = groups[value.Value];
                        var result = group.OuterElements[value];
                        if (group.InnerKeys.Count == 0)
                        {
                            if (group.OuterElements.Count == 0)
                            {
                                groups.Remove(value.Value);
                            }
                        }
                        else
                        {
                            removed.Add(result.Value);
                        }
                        group.OuterElements.Remove(value);
                        value.ValueChanged -= OuterKeyChanged;
                        value.Detach();
                        result.ValueChanged -= ResultValueChanged;
                        result.Detach();
                    }
                    OnRemoveItems(removed);
                }
                if (e.NewItems != null)
                {
                    var added = new List<TResult>();
                    foreach (TOuter outer in e.NewItems)
                    {
                        Stack<TaggedObservableValue<TKey, TOuter>> valueStack;
                        if (!outerValues.TryGetValue(outer, out valueStack))
                        {
                            valueStack = new Stack<TaggedObservableValue<TKey, TOuter>>();
                            outerValues.Add(outer, valueStack);
                        }
                        var value = outerKeySelector.InvokeTagged(outer, outer);
                        valueStack.Push(value);
                        KeyGroup group;
                        if (!groups.TryGetValue(value.Value, out group))
                        {
                            group = new KeyGroup();
                            groups.Add(value.Value, group);
                        }
                        var result = resultSelector.InvokeTagged(outer, group.collectionView, outer);
                        group.OuterElements.Add(value, result);
                        added.Add(result.Value);
                        value.ValueChanged += InnerKeyChanged;
                        result.ValueChanged += ResultValueChanged;
                    }
                    OnAddItems(added);
                }
            }
        }

        protected override void DetachCore()
        {
            foreach (var group in groups.Values)
            {
                foreach (var val in group.OuterElements)
                {
                    val.Key.Detach();
                    val.Key.ValueChanged -= OuterKeyChanged;
                    val.Value.Detach();
                    val.Value.ValueChanged -= ResultValueChanged;
                }
                foreach (var val in group.InnerKeys)
                {
                    val.Detach();
                    val.ValueChanged -= InnerKeyChanged;
                }
            }
            groups.Clear();
            outerSource.Detach();
            outerValues.Clear();
            innerValues.Clear();
            outerSource.CollectionChanged -= OuterSourceCollectionChanged;
            var innerSourceNotifiable = innerSource as INotifyEnumerable<TInner>;
            if (innerSourceNotifiable != null)
            {
                innerSourceNotifiable.Detach();
                innerSourceNotifiable.CollectionChanged -= InnerSourceCollectionChanged;
            }
            else
            {
                var notifier = innerSource as INotifyCollectionChanged;
                if (notifier != null)
                {
                    notifier.CollectionChanged -= InnerSourceCollectionChanged;
                }
            }
        }
    }
}
