using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableSelectMany<TSource, TIntermediate, TResult> : ObservableEnumerable<TResult>
    {
        private INotifyEnumerable<TSource> source;
        private ObservingFunc<TSource, IEnumerable<TIntermediate>> func;

        private ObservingFunc<TSource, TIntermediate, TResult> selector;

        private Dictionary<TSource, SubSourcePair> sourceItems = new Dictionary<TSource, SubSourcePair>();
        
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
            if (source == null) throw new ArgumentNullException("source");
            if (func == null) throw new ArgumentNullException("func");
            if (selector == null) throw new ArgumentNullException("selector");

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
            var added = new List<TResult>();
            var removed = new List<TResult>();
            var replaceAdded = new List<TResult>();
            var replaceRemoved = new List<TResult>();

            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var sourceChange = (CollectionChangedNotificationResult<TSource>)change;
                    if (sourceChange.IsReset)
                    {
                        OnDetach();
                        OnAttach();
                        OnCleared();
                        return new CollectionChangedNotificationResult<TResult>(this);
                    }
                    else
                    {
                        NotifySource(sourceChange, added, removed);
                    }
                }
                else
                {
                    var subSourceChange = (CollectionChangedNotificationResult<TResult>)change;
                    if (subSourceChange.RemovedItems != null)
                        removed.AddRange(subSourceChange.RemovedItems);
                    if (subSourceChange.AddedItems != null)
                        added.AddRange(subSourceChange.AddedItems);
                    if (subSourceChange.ReplaceAddedItems != null)
                        replaceAdded.AddRange(subSourceChange.ReplaceAddedItems);
                    if (subSourceChange.ReplaceRemovedItems != null)
                        replaceRemoved.AddRange(subSourceChange.ReplaceRemovedItems);
                }
            }

            if (added.Count == 0 && removed.Count == 0 && replaceAdded.Count == 0)
                return UnchangedNotificationResult.Instance;

            OnRemoveItems(removed);
            OnAddItems(added);
            OnReplaceItems(replaceRemoved, replaceAdded);
            return new CollectionChangedNotificationResult<TResult>(this, added, removed, replaceAdded, replaceRemoved);
        }

        private void NotifySource(CollectionChangedNotificationResult<TSource> sourceChange, List<TResult> added, List<TResult> removed)
        {
            if (sourceChange.RemovedItems != null)
            {
                foreach (var item in sourceChange.RemovedItems)
                {
                    SubSourcePair wrapper;
                    if (sourceItems.TryGetValue(item, out wrapper))
                    {
                        removed.AddRange(wrapper);
                        wrapper.Successors.Unset(this);
                        sourceItems.Remove(item);
                    }
                }
            }

            if (sourceChange.AddedItems != null)
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

            if (sourceChange.ReplaceAddedItems != null)
            {
                for (int i = 0; i < sourceChange.ReplaceAddedItems.Count; i++)
                {
                    var oldItem = sourceChange.ReplaceRemovedItems[i];
                    var newItem = sourceChange.ReplaceAddedItems[i];

                    var newSubSource = func.Observe(newItem);

                    SubSourcePair wrapper;
                    if (sourceItems.TryGetValue(oldItem, out wrapper))
                    {
                        removed.AddRange(SL.Select(wrapper.Results.Values, res => res.Value));
                        wrapper.Successors.Unset(this);
                        sourceItems.Remove(oldItem);
                    }

                    wrapper = new SubSourcePair(newSubSource, newItem, this);
                    sourceItems.Add(newItem, wrapper);
                    wrapper.Successors.Set(this);
                    added.AddRange(SL.Select(wrapper.Results.Values, res => res.Value));
                }
            }
        }

        private class SubSourcePair : ObservableEnumerable<TResult>
        {
            public Dictionary<TIntermediate, TaggedObservableValue<TResult, int>> Results = new Dictionary<TIntermediate, TaggedObservableValue<TResult, int>>();

            public INotifyValue<IEnumerable<TIntermediate>> SubSource { get; set; }

            public TSource Item { get; private set; }

            public ObservableSelectMany<TSource, TIntermediate, TResult> Parent { get; private set; }

            public override IEnumerable<INotifiable> Dependencies
            {
                get
                {
                    yield return SubSource;
                    var notifiable = SubSource.Value as INotifyEnumerable<TIntermediate>;
                    if (notifiable != null)
                        yield return notifiable;
                    foreach (var tagged in Results.Values)
                        yield return tagged;
                }
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
                    var notifiable = SubSource.Value as INotifyEnumerable<TIntermediate>;
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

                var notifiable = oldSubSource as INotifyEnumerable<TIntermediate>;
                if (notifiable != null)
                    notifiable.Successors.Unset(this);
            }

            protected override void OnAttach()
            {
                AttachSubSourceValue();
            }

            protected override void OnDetach()
            {
                DetachSubSourceValue(SubSource.Value, null);
            }

            public override INotificationResult Notify(IList<INotificationResult> sources)
            {
                var added = new List<TResult>();
                var removed = new List<TResult>();
                var replaceAdded = new List<TResult>();
                var replaceRemoved = new List<TResult>();

                foreach (var change in sources)
                {
                    if (change.Source == SubSource)
                    {
                        var subSourceChange = (IValueChangedNotificationResult)change;
                        DetachSubSourceValue((IEnumerable<TIntermediate>)subSourceChange.OldValue, removed);

                        var notifiable = SubSource.Value as INotifyEnumerable<TIntermediate>;
                        if (notifiable != null)
                            notifiable.Successors.Set(this);
                        AttachSubSourceValue();
                        added.AddRange(SL.Select(Results.Values, res => res.Value));
                    }
                    else if (change.Source is TaggedObservableValue<TResult, int>)
                    {
                        var resultChange = (ValueChangedNotificationResult<TResult>)change;
                        replaceRemoved.Add(resultChange.OldValue);
                        replaceAdded.Add(resultChange.NewValue);
                    }
                    else
                    {
                        var subSourceValueChange = (CollectionChangedNotificationResult<TIntermediate>)change;
                        if (subSourceValueChange.IsReset)
                        {
                            DetachSubSourceValue(SubSource.Value, removed);
                            AttachSubSourceValue();
                            added.AddRange(SL.Select(Results.Values, res => res.Value));
                        }
                        else
                        {
                            NotifySubSourceValue(added, removed, subSourceValueChange);
                        }
                    }
                }

                if (added.Count == 0 && removed.Count == 0 && replaceAdded.Count == 0)
                    return UnchangedNotificationResult.Instance;

                OnRemoveItems(removed);
                OnAddItems(added);
                OnReplaceItems(replaceRemoved, replaceAdded);
                return new CollectionChangedNotificationResult<TResult>(this, added, removed, replaceAdded, replaceRemoved);
            }

            private void NotifySubSourceValue(List<TResult> added, List<TResult> removed, CollectionChangedNotificationResult<TIntermediate> subSourceValueChange)
            {
                foreach (var element in subSourceValueChange.AllRemovedItems)
                {
                    removed.Add(Results[element].Value);
                    DetachResult(element);
                }
                    
                foreach (var element in subSourceValueChange.AllAddedItems)
                {
                    AttachResult(element);
                    added.Add(Results[element].Value);
                }
            }
        }
    }
}
