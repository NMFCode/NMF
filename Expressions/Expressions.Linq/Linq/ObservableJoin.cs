using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableJoin<TOuter, TInner, TKey, TResult> : ObservableEnumerable<TResult>
    {
        public override string ToString()
        {
            return "[Join]";
        }

        private readonly INotifyEnumerable<TOuter> outerSource;
        private readonly IEnumerable<TInner> innerSource;
        private readonly INotifyEnumerable<TInner> observableInnerSource;
        private readonly ObservingFunc<TOuter, TKey> outerKeySelector;
        private readonly ObservingFunc<TInner, TKey> innerKeySelector;
        private readonly ObservingFunc<TOuter, TInner, TResult> resultSelector;

        private readonly Dictionary<TKey, KeyGroup> groups;
        private readonly Dictionary<TInner, Stack<TaggedObservableValue<TKey, TInner>>> innerValues = new Dictionary<TInner, Stack<TaggedObservableValue<TKey, TInner>>>();
        private readonly Dictionary<TOuter, Stack<TaggedObservableValue<TKey, TOuter>>> outerValues = new Dictionary<TOuter, Stack<TaggedObservableValue<TKey, TOuter>>>();

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return outerSource;
                if (observableInnerSource != null)
                    yield return observableInnerSource;
                foreach (var stack in outerValues.Values)
                {
                    foreach (var tagged in stack)
                        yield return tagged;
                }
                foreach (var stack in innerValues.Values)
                {
                    foreach (var tagged in stack)
                        yield return tagged;
                }
                foreach (var group in groups.Values)
                {
                    foreach (var stack in group.Results.Values)
                    {
                        foreach (var result in stack)
                            yield return result;
                    }
                }
            }
        }

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

            this.observableInnerSource = innerSource as INotifyEnumerable<TInner>;
            if (observableInnerSource == null)
                observableInnerSource = (innerSource as IEnumerableExpression<TInner>)?.AsNotifiable();
            groups = new Dictionary<TKey, KeyGroup>(comparer);
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
        
        private void AttachOuter(TOuter item, ICollection<TResult> added)
        {
            var keyValue = outerKeySelector.InvokeTagged(item, item);
            keyValue.Successors.Set(this);
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
            group.OuterKeys.Add(keyValue);

            if (added != null)
            {
                foreach (var inner in group.InnerKeys)
                {
                    added.Add(AttachResult(group, item, inner.Tag));
                }
            }
            else
            {
                foreach (var inner in group.InnerKeys)
                {
                    AttachResult(group, item, inner.Tag);
                }
            }
        }

        private void AttachInner(TInner item, ICollection<TResult> added)
        {
            var keyValue = innerKeySelector.InvokeTagged(item, item);
            keyValue.Successors.Set(this);
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
            result.Successors.Set(this);
            resultStack.Push(result);
            return result.Value;
        }

        private TResult DetachResult(KeyGroup group, TOuter outer, TInner inner)
        {
            var match = new Match(outer, inner);
            var resultStack = group.Results[match];
            var result = resultStack.Pop();
            var value = result.Value;
            result.Successors.Unset(this);
            if (resultStack.Count == 0)
            {
                group.Results.Remove(match);
            }
            return value;
        }

        protected override void OnAttach()
        {
            foreach (var item in outerSource)
            {
                AttachOuter(item, null);
            }
            foreach (var item in innerSource)
            {
                AttachInner(item, null);
            }
        }

        protected override void OnDetach()
        {
            foreach (var group in groups.Values)
            {
                foreach (var val in group.OuterKeys)
                {
                    val.Successors.Unset(this);
                }
                foreach (var val in group.InnerKeys)
                {
                    val.Successors.Unset(this);
                }
                foreach (var stack in group.Results.Values)
                {
                    foreach (var val in stack)
                    {
                        val.Successors.Unset(this);
                    }
                }
            }

            groups.Clear();
            outerValues.Clear();
            innerValues.Clear();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var notification = CollectionChangedNotificationResult<TResult>.Create(this);
            var added = notification.AddedItems;
            var removed = notification.RemovedItems;
            bool reset = false;

            foreach (var change in sources)
            {
                if (change.Source == outerSource)
                {
                    var outerChange = (ICollectionChangedNotificationResult<TOuter>)change;
                    if (outerChange.IsReset)
                    {
                        foreach (var group in groups.Values)
                        {
                            foreach (var val in group.OuterKeys)
                            {
                                val.Successors.Unset(this);
                            }
                            group.OuterKeys.Clear();
                            foreach (var stack in group.Results.Values)
                            {
                                removed.AddRange(stack.Select(s => s.Value));
                                foreach (var val in stack)
                                {
                                    val.Successors.Unset(this);
                                }
                            }
                            group.Results.Clear();
                        }
                        outerValues.Clear();

                        if (reset || observableInnerSource == null) //both source collections may be reset, only return after handling both
                        {
                            OnCleared();
                            notification.TurnIntoReset();
                            return notification;
                        }
                        reset = true;
                    }
                    else
                    {
                        NotifyOuter(outerChange, added, removed);
                    }
                }
                else if (change.Source == observableInnerSource)
                {
                    var innerChange = (ICollectionChangedNotificationResult<TInner>)change;
                    if (innerChange.IsReset)
                    {
                        foreach (var group in groups.Values)
                        {
                            foreach (var val in group.InnerKeys)
                            {
                                val.Successors.Unset(this);
                            }
                            group.InnerKeys.Clear();
                            foreach (var stack in group.Results.Values)
                            {
                                removed.AddRange(stack.Select(s => s.Value));
                                foreach (var val in stack)
                                {
                                    val.Successors.Unset(this);
                                }
                            }
                            group.Results.Clear();
                        }
                        innerValues.Clear();

                        if (reset) //both source collections may be reset, only return after handling both
                        {
                            OnCleared();
                            notification.TurnIntoReset();
                            return notification;
                        }
                        reset = true;
                    }
                    else
                    {
                        NotifyInner(innerChange, added, removed);
                    }
                }
                else if (change is IValueChangedNotificationResult<TKey> keyChange)
                {
                    if (keyChange.Source is TaggedObservableValue<TKey, TOuter>)
                        NotifyOuterKey(keyChange, added, removed);
                    else
                        NotifyInnerKey(keyChange, added, removed);
                }
                else
                {
                    var resultChange = (IValueChangedNotificationResult<TResult>)change;
                    removed.Add(resultChange.OldValue);
                    added.Add(resultChange.NewValue);
                }
            }

            if (reset) //only one source was reset
            {
                OnCleared();
                notification.TurnIntoReset();
                return notification;
            }

            RaiseEvents(added, removed, null);
            return notification;
        }

        private void NotifyOuter(ICollectionChangedNotificationResult<TOuter> outerChange, List<TResult> added, List<TResult> removed)
        {
            if (outerChange.RemovedItems != null)
            {
                foreach (var outer in outerChange.RemovedItems)
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
                    value.Successors.Unset(this);
                }
            }

            if (outerChange.AddedItems != null)
            {
                foreach (var outer in outerChange.AddedItems)
                {
                    AttachOuter(outer, added);
                }
            }
        }

        private void NotifyInner(ICollectionChangedNotificationResult<TInner> innerChange, List<TResult> added, List<TResult> removed)
        {
            if (innerChange.RemovedItems != null)
            {
                foreach (var inner in innerChange.RemovedItems)
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
                    value.Successors.Unset(this);
                }
            }

            if (innerChange.AddedItems != null)
            {
                foreach (var inner in innerChange.AddedItems)
                {
                    AttachInner(inner, added);
                }
            }
        }

        private void NotifyOuterKey(IValueChangedNotificationResult<TKey> keyChange, List<TResult> replaceAdded, List<TResult> replaceRemoved)
        {
            var value = (TaggedObservableValue<TKey, TOuter>)keyChange.Source;
            var group = groups[keyChange.OldValue];
            group.OuterKeys.Remove(value);
            foreach (var inner in group.InnerKeys)
            {
                replaceRemoved.Add(DetachResult(group, value.Tag, inner.Tag));
            }

            if (!groups.TryGetValue(value.Value, out group))
            {
                group = new KeyGroup();
                groups.Add(value.Value, group);
            }
            group.OuterKeys.Add(value);

            foreach (var inner in group.InnerKeys)
            {
                replaceAdded.Add(AttachResult(group, value.Tag, inner.Tag));
            }
        }

        private void NotifyInnerKey(IValueChangedNotificationResult<TKey> keyChange, List<TResult> replaceAdded, List<TResult> replaceRemoved)
        {
            var value = (TaggedObservableValue<TKey, TInner>)keyChange.Source;
            var group = groups[keyChange.OldValue];
            group.InnerKeys.Remove(value);
            foreach (var outer in group.OuterKeys)
            {
                replaceRemoved.Add(DetachResult(group, outer.Tag, value.Tag));
            }

            if (!groups.TryGetValue(value.Value, out group))
            {
                group = new KeyGroup();
                groups.Add(value.Value, group);
            }
            group.InnerKeys.Add(value);
            
            foreach (var outer in group.OuterKeys)
            {
                replaceAdded.Add(AttachResult(group, outer.Tag, value.Tag));
            }
        }
    }
}
