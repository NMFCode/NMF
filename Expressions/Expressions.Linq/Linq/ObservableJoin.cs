using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableJoin<TOuter, TInner, TKey, TResult> : ObservableEnumerable<TResult>
    {
        private INotifyEnumerable<TOuter> outerSource;
        private IEnumerable<TInner> innerSource;
        private ObservingFunc<TOuter, TKey> outerKeySelector;
        private ObservingFunc<TInner, TKey> innerKeySelector;
        private ObservingFunc<TOuter, TInner, TResult> resultSelector;

        private Dictionary<TKey, KeyGroup> groups;
        private Dictionary<TInner, Stack<TaggedObservableValue<TKey, TInner>>> innerValues = new Dictionary<TInner, Stack<TaggedObservableValue<TKey, TInner>>>();
        private Dictionary<TOuter, Stack<TaggedObservableValue<TKey, TOuter>>> outerValues = new Dictionary<TOuter, Stack<TaggedObservableValue<TKey, TOuter>>>();

        public ObservableJoin(INotifyEnumerable<TOuter> outerSource, IEnumerable<TInner> innerSource, ObservingFunc<TOuter, TKey> outerKeySelector, ObservingFunc<TInner, TKey> innerKeySelector, ObservingFunc<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
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
            public List<TaggedObservableValue<TKey, TOuter>> OuterKeys = new List<TaggedObservableValue<TKey, TOuter>>();
            public List<TaggedObservableValue<TKey, TInner>> InnerKeys = new List<TaggedObservableValue<TKey, TInner>>();
            public Dictionary<Match, Stack<INotifyValue<TResult>>> Results = new Dictionary<Match, Stack<INotifyValue<TResult>>>();
        }

        private struct Match : IEquatable<Match>
        {
            public TInner Inner;
            public TOuter Outer;

            public Match(TOuter outer, TInner inner)
            {
                Outer = outer;
                Inner = inner;
            }

            public override int GetHashCode()
            {
                int hash = 0;
                if (Outer != null)
                {
                    hash ^= Outer.GetHashCode();
                }
                if (Inner != null)
                {
                    hash ^= Inner.GetHashCode();
                }
                return hash;
            }

            public override bool Equals(object obj)
            {
                if (obj != null && obj is Match)
                {
                    return Equals((Match)obj);
                }
                else
                {
                    return false;
                }
            }

            public bool Equals(Match other)
            {
                return EqualityComparer<TOuter>.Default.Equals(Outer, other.Outer)
                    && EqualityComparer<TInner>.Default.Equals(Inner, other.Inner);
            }
        }

        public override IEnumerator<TResult> GetEnumerator()
        {
            return groups.Values.SelectMany(group => group.Results.Values.SelectMany(val => val).Select(val => val.Value)).GetEnumerator();
        }

        public override int Count
        {
            get
            {
                return groups.Values.Sum(group => group.Results.Count);
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
                group.OuterKeys.Add(keyValue);
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
            if (added == null)
            {
                foreach (var outer in group.OuterKeys)
                {
                    AttachResult(group, outer.Tag, item);
                }
            }
            else
            {
                foreach (var outer in group.OuterKeys)
                {
                    added.Add(AttachResult(group, outer.Tag, item));
                }
            }
        }

        private TResult DetachResult(KeyGroup group, TOuter outer, TInner inner)
        {
            var match = new Match(outer, inner);
            var resultStack = group.Results[match];
            var result = resultStack.Pop();
            result.ValueChanged -= ResultValueChanged;
            result.Detach();
            if (resultStack.Count == 0)
            {
                group.Results.Remove(match);
            }
            return result.Value;
        }

        private TResult AttachResult(KeyGroup group, TOuter outer, TInner inner)
        {
            var match = new Match(outer, inner);
            Stack<INotifyValue<TResult>> resultStack;
            if (!group.Results.TryGetValue(match, out resultStack))
            {
                resultStack = new Stack<INotifyValue<TResult>>();
                group.Results.Add(match, resultStack);
            }
            var result = resultSelector.Observe(outer, inner);
            result.ValueChanged += ResultValueChanged;
            resultStack.Push(result);
            return result.Value;
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
            group.OuterKeys.Remove(value);
            var removed = new List<TResult>();
            foreach (var inner in group.InnerKeys)
            {
                removed.Add(DetachResult(group, value.Tag, inner.Tag));
            }
            if (!groups.TryGetValue(value.Value, out group))
            {
                group = new KeyGroup();
                groups.Add(value.Value, group);
            }
            group.OuterKeys.Add(value);
            var added = new List<TResult>();
            foreach (var inner in group.InnerKeys)
            {
                added.Add(AttachResult(group, value.Tag, inner.Tag));
            }
            OnReplaceItems(removed, added);
        }

        private void InnerKeyChanged(object sender, ValueChangedEventArgs e)
        {
            var value = sender as TaggedObservableValue<TKey, TInner>;
            var oldKey = (TKey)e.OldValue;
            var group = groups[oldKey];
            group.InnerKeys.Remove(value);
            var removed = new List<TResult>();
            foreach (var outer in group.OuterKeys)
            {
                removed.Add(DetachResult(group, outer.Tag, value.Tag));
            }
            if (!groups.TryGetValue(value.Value, out group))
            {
                group = new KeyGroup();
                groups.Add(value.Value, group);
            }
            group.InnerKeys.Add(value);
            var added = new List<TResult>();
            foreach (var outer in group.OuterKeys)
            {
                added.Add(AttachResult(group, outer.Tag, value.Tag));
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
                    foreach (var stack in group.Results.Values)
                    {
                        foreach (var val in stack)
                        {
                            val.Detach();
                            val.ValueChanged -= ResultValueChanged;
                        }
                    }
                    group.Results.Clear();
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
                        if (group.InnerKeys.Count == 0 && group.OuterKeys.Count == 0)
                        {
                            groups.Remove(value.Value);
                        }
                        foreach (var outer in group.OuterKeys)
                        {
                            removed.Add(DetachResult(group, outer.Tag, inner));
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
                    foreach (var val in group.OuterKeys)
                    {
                        val.Detach();
                        val.ValueChanged -= OuterKeyChanged;
                    }
                    group.OuterKeys.Clear();
                    foreach (var stack in group.Results.Values)
                    {
                        foreach (var val in stack)
                        {
                            val.Detach();
                            val.ValueChanged -= ResultValueChanged;
                        }
                    }
                    group.Results.Clear();
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
                        group.OuterKeys.Remove(value);
                        if (group.OuterKeys.Count == 0 && group.InnerKeys.Count == 0)
                        {
                            groups.Remove(value.Value);
                        }
                        foreach (var inner in group.InnerKeys)
                        {
                            removed.Add(DetachResult(group, outer, inner.Tag));
                        }
                        value.ValueChanged -= OuterKeyChanged;
                        value.Detach();
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
                        group.OuterKeys.Add(value);
                        foreach (var inner in group.InnerKeys)
                        {
                            added.Add(AttachResult(group, outer, inner.Tag));
                        }
                        value.ValueChanged += InnerKeyChanged;
                    }
                    OnAddItems(added);
                }
            }
        }

        protected override void DetachCore()
        {
            foreach (var group in groups.Values)
            {
                foreach (var val in group.OuterKeys)
                {
                    val.Detach();
                    val.ValueChanged -= OuterKeyChanged;
                }
                foreach (var val in group.InnerKeys)
                {
                    val.Detach();
                    val.ValueChanged -= InnerKeyChanged;
                }
                foreach (var stack in group.Results.Values)
                {
                    foreach (var val in stack)
                    {
                        val.Detach();
                        val.ValueChanged -= ResultValueChanged;
                    }
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
