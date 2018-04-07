﻿using System;
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
        private INotifyEnumerable<TInner> observableInnerSource;
        private ObservingFunc<TOuter, TKey> outerKeySelector;
        private ObservingFunc<TInner, TKey> innerKeySelector;
        private ObservingFunc<TOuter, IEnumerable<TInner>, TResult> resultSelector;

        private Dictionary<TKey, KeyGroup> groups;
        private Dictionary<TInner, Stack<TaggedObservableValue<TKey, TInner>>> innerValues = new Dictionary<TInner, Stack<TaggedObservableValue<TKey, TInner>>>();
        private Dictionary<TOuter, Stack<TaggedObservableValue<TKey, TOuter>>> outerValues = new Dictionary<TOuter, Stack<TaggedObservableValue<TKey, TOuter>>>();

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
                    foreach (var result in group.OuterElements.Values)
                        yield return result;
                }
            }
        }

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

            this.observableInnerSource = innerSource as INotifyEnumerable<TInner>;
            if (observableInnerSource == null)
                observableInnerSource = (innerSource as IEnumerableExpression<TInner>)?.AsNotifiable();
            groups = new Dictionary<TKey, KeyGroup>(comparer);
        }

        private class KeyGroup
        {
            public Dictionary<TaggedObservableValue<TKey, TOuter>, TaggedObservableValue<TResult, TOuter>> OuterElements = new Dictionary<TaggedObservableValue<TKey, TOuter>, TaggedObservableValue<TResult, TOuter>>();
            public List<TaggedObservableValue<TKey, TInner>> InnerKeys = new List<TaggedObservableValue<TKey, TInner>>();
            public IEnumerable<TInner> InnerElements { get { return InnerKeys.Select(key => key.Tag); } }
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
        
        private TResult AttachOuter(TOuter item)
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
            var resultValue = resultSelector.InvokeTagged(item, group.InnerElements, item);
            resultValue.Successors.Set(this);
            group.OuterElements.Add(keyValue, resultValue);
            return resultValue.Value;
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
            if (group.InnerKeys.Count == 1 && added != null)
            {
                foreach (var result in group.OuterElements.Values)
                {
                    added.Add(result.Value);
                }
            }
        }

        protected override void OnAttach()
        {
            foreach (var item in outerSource)
            {
                AttachOuter(item);
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
                foreach (var val in group.OuterElements)
                {
                    val.Key.Successors.Unset(this);
                    val.Value.Successors.Unset(this);
                }
                foreach (var val in group.InnerKeys)
                {
                    val.Successors.Unset(this);
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
                            foreach (var val in group.OuterElements)
                            {
                                val.Key.Successors.Unset(this);
                                val.Value.Successors.Unset(this);
                            }
                            group.OuterElements.Clear();
                        }
                        outerValues.Clear();

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
                    value.Successors.Unset(this);
                    result.Successors.Unset(this);
                }
            }

            if (outerChange.AddedItems != null)
            {
                foreach (var outer in outerChange.AddedItems)
                {
                    added.Add(AttachOuter(outer));
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

        private void NotifyOuterKey(IValueChangedNotificationResult<TKey> keyChange, List<TResult> added, List<TResult> removed)
        {
            var value = (TaggedObservableValue<TKey, TOuter>)keyChange.Source;
            var group = groups[keyChange.OldValue];
            group.OuterElements.Remove(value);
            
            if (group.InnerKeys.Count != 0)
            {
                var result = group.OuterElements[value];
                removed.Add(result.Value);
                result.Successors.Unset(this);
            }

            if (!groups.TryGetValue(value.Value, out group))
            {
                group = new KeyGroup();
                groups.Add(value.Value, group);
            }

            var newResult = resultSelector.InvokeTagged(value.Tag, group.InnerElements, value.Tag);
            newResult.Successors.Set(this);
            group.OuterElements.Add(value, newResult);
            if (group.InnerKeys.Count != 0)
            {
                added.Add(newResult.Value);
            }
        }

        private void NotifyInnerKey(IValueChangedNotificationResult<TKey> keyChange, List<TResult> added, List<TResult> removed)
        {
            var value = (TaggedObservableValue<TKey, TInner>)keyChange.Source;
            var group = groups[keyChange.OldValue];
            group.InnerKeys.Remove(value);
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
            if (group.InnerKeys.Count == 1)
            {
                added.AddRange(group.OuterElements.Values.Select(r => r.Value));
            }
        }
    }
}
