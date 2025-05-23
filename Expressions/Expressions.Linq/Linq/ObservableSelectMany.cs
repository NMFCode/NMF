﻿using System;
using System.Collections.Generic;
using System.Linq;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableSelectMany<TSource, TIntermediate, TResult> : ObservableEnumerable<TResult>
    {
        public override string ToString()
        {
            return "[SelectMany]";
        }

        private readonly INotifyEnumerable<TSource> source;
        private readonly ObservingFunc<TSource, IEnumerable<TIntermediate>> func;

        private readonly ObservingFunc<TSource, TIntermediate, TResult> selector;

        private readonly Dictionary<TSource, SubSourcePair> sourceItems = new Dictionary<TSource, SubSourcePair>();
        
        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                foreach (var subSourcePair in sourceItems.Values)
                    yield return subSourcePair;
            }
        }

        public ObservableSelectMany(INotifyEnumerable<TSource> source,
            ObservingFunc<TSource, IEnumerable<TIntermediate>> func,
            ObservingFunc<TSource, TIntermediate, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            this.source = source;
            this.func = func;
            this.selector = selector;
        }
        
        public override IEnumerator<TResult> GetEnumerator()
        {
            return SL.SelectMany(sourceItems.Values, sub => sub).GetEnumerator();
        }

        public override int Count
        {
            get
            {
                return SL.Sum(SL.Select(sourceItems.Values, sub => sub.Results.Count));
            }
        }

        protected override void OnAttach()
        {
            foreach (var item in source)
            {
                var subSource = func.Observe(item);
                var wrapper = new SubSourcePair(subSource, item, this);
                wrapper.Successors.Set(this);
                sourceItems.Add(item, wrapper);
            }
        }

        protected override void OnDetach()
        {
            foreach (var sub in sourceItems.Values)
            {
                sub.Successors.Unset(this);
            }
            sourceItems.Clear();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var notification = CollectionChangedNotificationResult<TResult>.Create(this);
            var added = notification.AddedItems;
            var removed = notification.RemovedItems;
            var moved = notification.MovedItems;

            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var sourceChange = (ICollectionChangedNotificationResult<TSource>)change;
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
                    var subSourceChange = (ICollectionChangedNotificationResult<TResult>)change;
                    if (subSourceChange.RemovedItems != null)
                        removed.AddRange(subSourceChange.RemovedItems);
                    if (subSourceChange.AddedItems != null)
                        added.AddRange(subSourceChange.AddedItems);
                    if (subSourceChange.MovedItems != null && ObservableExtensions.KeepOrder)
                        moved.AddRange(subSourceChange.MovedItems);
                }
            }
            RaiseEvents(added, removed, moved);
            return notification;
        }

        private void NotifySource(ICollectionChangedNotificationResult<TSource> sourceChange, List<TResult> added, List<TResult> removed, List<TResult> moved)
        {
            if (sourceChange.RemovedItems != null && sourceChange.RemovedItems.Count > 0)
            {
                foreach (var item in sourceChange.RemovedItems)
                {
                    if (sourceItems.TryGetValue(item, out var wrapper))
                    {
                        removed.AddRange(wrapper);
                        wrapper.Successors.Unset(this);
                        sourceItems.Remove(item);
                    }
                }
            }

            if (sourceChange.AddedItems != null && sourceChange.AddedItems.Count > 0)
            {
                foreach (var item in sourceChange.AddedItems)
                {
                    var subSource = func.Observe(item);
                    var wrapper = new SubSourcePair(subSource, item, this);
                    wrapper.Successors.Set(this);
                    sourceItems.Add(item, wrapper);
                    added.AddRange(wrapper);
                }
            }

            if (ObservableExtensions.KeepOrder && sourceChange.MovedItems != null && sourceChange.MovedItems.Count > 0)
            {
                foreach (var item in sourceChange.MovedItems)
                {
                    if (sourceItems.TryGetValue(item, out var wrapper))
                    {
                        moved.AddRange(wrapper);
                    }
                }
            }
        }

        private class SubSourcePair : ObservableEnumerable<TResult>
        {
            public Dictionary<TIntermediate, TaggedObservableValue<TResult, int>> Results = new Dictionary<TIntermediate, TaggedObservableValue<TResult, int>>();

            public INotifyValue<IEnumerable<TIntermediate>> SubSource { get; set; }

            public INotifyEnumerable<TIntermediate> NotifySubSource { get; set; }

            public TSource Item { get; private set; }

            public ObservableSelectMany<TSource, TIntermediate, TResult> Parent { get; private set; }

            public override IEnumerable<INotifiable> Dependencies
            {
                get
                {
                    yield return SubSource;
                    if (NotifySubSource != null)
                        yield return NotifySubSource;
                    foreach (var tagged in Results.Values)
                        yield return tagged;
                }
            }

            public override string ToString()
            {
                return "[SelectManyChild]";
            }

            public SubSourcePair(INotifyValue<IEnumerable<TIntermediate>> subSource, TSource item, ObservableSelectMany<TSource, TIntermediate, TResult> parent)
            {
                SubSource = subSource;
                Item = item;
                Parent = parent;
            }

            public override IEnumerator<TResult> GetEnumerator()
            {
                return SL.Select(Results.Values, t => t.Value).GetEnumerator();
            }

            private void AttachResult(TIntermediate element)
            {
                TaggedObservableValue<TResult, int> result;
                if (!Results.TryGetValue(element, out result))
                {
                    result = Parent.selector.InvokeTagged(Item, element, 0);
                    result.Successors.Set(this);
                    Results.Add(element, result);
                }
                result.Tag++;
            }

            private void DetachResult(TIntermediate element)
            {
                TaggedObservableValue<TResult, int> result;
                if (Results.TryGetValue(element, out result))
                {
                    result.Tag--;
                    if (result.Tag == 0)
                    {
                        Results.Remove(element);
                        result.Successors.Unset(this);
                    }
                }
            }

            private void AttachSubSourceValue()
            {
                if (SubSource.Value != null)
                {
                    var notifiable = SubSource.Value.WithUpdates(false);
                    NotifySubSource = notifiable;
                    if (notifiable != null)
                        notifiable.Successors.SetDummy();

                    foreach (var element in SubSource.Value)
                    {
                        AttachResult(element);
                    }
                }
            }

            private void DetachSubSourceValue(IEnumerable<TIntermediate> oldSubSource, List<TResult> removed)
            {
                foreach (var result in Results.Values)
                {
                    if (removed != null)
                        removed.Add(result.Value);
                    result.Successors.Unset(this);
                }
                Results.Clear();

                var notifiable = NotifySubSource;
                if (notifiable != null)
                {
                    notifiable.Successors.Unset(this);
                    NotifySubSource = null;
                }
            }

            protected override void OnAttach()
            {
                AttachSubSourceValue();
                if (NotifySubSource != null)
                {
                    NotifySubSource.Successors.Set(this);
                }
            }

            protected override void OnDetach()
            {
                DetachSubSourceValue(SubSource.Value, null);
            }

            public override INotificationResult Notify(IList<INotificationResult> sources)
            {
                var notification = CollectionChangedNotificationResult<TResult>.Create(this);
                var added = notification.AddedItems;
                var removed = notification.RemovedItems;
                var moved = notification.MovedItems;

                foreach (var change in sources)
                {
                    if (change.Source == SubSource)
                    {
                        var subSourceChange = (IValueChangedNotificationResult)change;
                        DetachSubSourceValue((IEnumerable<TIntermediate>)subSourceChange.OldValue, removed);
                        OnAttach();
                        added.AddRange(SL.Select(Results.Values, res => res.Value));
                    }
                    else if (change.Source is TaggedObservableValue<TResult, int>)
                    {
                        var resultChange = (IValueChangedNotificationResult<TResult>)change;
                        removed.Add(resultChange.OldValue);
                        added.Add(resultChange.NewValue);
                    }
                    else
                    {
                        var subSourceValueChange = (ICollectionChangedNotificationResult<TIntermediate>)change;
                        if (subSourceValueChange.IsReset)
                        {
                            DetachSubSourceValue(SubSource.Value, removed);
                            AttachSubSourceValue();
                            added.AddRange(SL.Select(Results.Values, res => res.Value));
                        }
                        else
                        {
                            NotifySubSourceValue(added, removed, moved, subSourceValueChange);
                        }
                    }
                }

                RaiseEvents(added, removed, null);
                return notification;
            }

            private void NotifySubSourceValue(List<TResult> added, List<TResult> removed, List<TResult> moved, ICollectionChangedNotificationResult<TIntermediate> subSourceValueChange)
            {
                if (subSourceValueChange.RemovedItems != null && subSourceValueChange.RemovedItems.Count > 0)
                {
                    foreach (var element in subSourceValueChange.RemovedItems)
                    {
                        removed.Add(Results[element].Value);
                        DetachResult(element);
                    }
                }

                if (subSourceValueChange.AddedItems != null && subSourceValueChange.AddedItems.Count > 0)
                {
                    foreach (var element in subSourceValueChange.AddedItems)
                    {
                        AttachResult(element);
                        added.Add(Results[element].Value);
                    }
                }

                if (subSourceValueChange.MovedItems != null && ObservableExtensions.KeepOrder && subSourceValueChange.MovedItems.Count > 0)
                {
                    moved.AddRange(subSourceValueChange.MovedItems.Select(el => Results[el].Value));
                }
            }
        }
    }
}
