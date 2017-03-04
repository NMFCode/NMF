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

            Attach();
        }

        private static void AttachSubSource(SubSourcePair wrapper)
        {
            if (wrapper.SubSource.Value != null)
            {
                foreach (var element in wrapper.SubSource.Value)
                {
                    wrapper.AttachResult(element);
                }
                var notifier = wrapper.SubSource.Value as INotifyEnumerable;
                if (notifier == null)
                {
                    var expression = wrapper.SubSource.Value as IEnumerableExpression;
                    if (expression != null)
                    {
                        notifier = expression.AsNotifiable();
                    }
                }
                wrapper.NotifySubSource = notifier;
                if (notifier != null)
                {
                    notifier.CollectionChanged += wrapper.OnCollectionChanged;
                }
            }
        }

        private void DetachSubSource(TSource item)
        {
            SubSourcePair wrapper;
            if (sourceItems.TryGetValue(item, out wrapper))
            {
                sourceItems.Remove(item);
                if (wrapper.SubSource.Value != null)
                {
                    foreach (var element in wrapper.SubSource.Value)
                    {
                        wrapper.DetachResult(element);
                    }
                    var notifier = wrapper.NotifySubSource;
                    if (notifier != null)
                    {
                        notifier.CollectionChanged -= wrapper.OnCollectionChanged;
                    }
                }
                wrapper.SubSource.ValueChanged -= wrapper.OnSourceChanged;
            }
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move) return;
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
                {
                    var removed = new List<TResult>();
                    foreach (TSource item in e.OldItems)
                    {
                        SubSourcePair wrapper;
                        if (sourceItems.TryGetValue(item, out wrapper))
                        {
                            removed.AddRange(SL.Select(wrapper.Values.Values, res => res.Value));
                            DetachSubSource(item);
                        }
                    }
                    OnRemoveItems(removed);
                }
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
                {
                    var added = new List<TResult>();
                    foreach (TSource item in e.NewItems)
                    {
                        var subSource = func.Observe(item);
                        var wrapper = new SubSourcePair(subSource, item, this);
                        sourceItems.Add(item, wrapper);
                        AttachSubSource(wrapper);
                        subSource.ValueChanged += wrapper.OnSourceChanged;
                        added.AddRange(SL.Select(wrapper.Values.Values, res => res.Value));
                    }
                    OnAddItems(added);
                }
                if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    var removed = new List<TResult>();
                    var added = new List<TResult>();
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var oldItem = (TSource)e.OldItems[i];
                        var newItem = (TSource)e.NewItems[i];

                        var newSubSource = func.Observe(newItem);

                        SubSourcePair wrapper;
                        if (sourceItems.TryGetValue(oldItem, out wrapper))
                        {
                            if (wrapper.SubSource.Value != newSubSource.Value)
                            {
                                removed.AddRange(SL.Select(wrapper.Values.Values, res => res.Value));
                                DetachSubSource(oldItem);

                                wrapper = new SubSourcePair(newSubSource, newItem, this);
                                sourceItems.Add(newItem, wrapper);
                                AttachSubSource(wrapper);
                                newSubSource.ValueChanged += wrapper.OnSourceChanged;
                                added.AddRange(SL.Select(wrapper.Values.Values, res => res.Value));
                            }
                            else
                            {
                                wrapper.SubSource.ValueChanged -= wrapper.OnSourceChanged;
                                wrapper.SubSource = newSubSource;
                                wrapper.SubSource.ValueChanged += wrapper.OnSourceChanged;
                                sourceItems.Remove(oldItem);
                                sourceItems.Add(newItem, wrapper);
                                wrapper.ReplaceItem(newItem);
                            }
                        }
                        else
                        {
                            wrapper = new SubSourcePair(newSubSource, newItem, this);
                            sourceItems.Add(newItem, wrapper);
                            AttachSubSource(wrapper);
                            newSubSource.ValueChanged += wrapper.OnSourceChanged;
                            added.AddRange(SL.Select(wrapper.Values.Values, res => res.Value));
                        }
                    }
                    OnAddItems(added);
                    OnRemoveItems(removed);
                }
            }
            else
            {
                DetachSource();
                OnCleared();
            }
        }

        private void DetachSource()
        {
            foreach (var sub in sourceItems.Values)
            {
                sub.UnregisterAllResultEvents();
            }
            sourceItems.Clear();
            source.CollectionChanged -= SourceCollectionChanged;
        }

        private void ResultChanged(object sender, ValueChangedEventArgs e)
        {
            var result = sender as INotifyValue<TResult>;
            OnUpdateItem(result.Value, (TResult)e.OldValue);
        }

        private class SubSourcePair : INotifyValue<TSource>
        {
            public Dictionary<TIntermediate, TaggedObservableValue<TResult, int>> Values = new Dictionary<TIntermediate, TaggedObservableValue<TResult, int>>();

            public INotifyValue<IEnumerable<TIntermediate>> SubSource { get; set; }
            public INotifyEnumerable NotifySubSource { get; set; }

            public TSource Item { get; private set; }

            public ObservableSelectMany<TSource, TIntermediate, TResult> Parent { get; private set; }

            public SubSourcePair(INotifyValue<IEnumerable<TIntermediate>> subSource, TSource item, ObservableSelectMany<TSource, TIntermediate, TResult> parent)
            {
                SubSource = subSource;
                Item = item;
                Parent = parent;
            }

            public void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Move) return;
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    DetachSubsource();
                    AttachSubSource(this);
                    return;
                }
                if (e.OldItems != null)
                {
                    var removed = new List<TResult>();
                    foreach (TIntermediate element in e.OldItems)
                    {
                        removed.Add(Values[element].Value);
                        DetachResult(element);
                    }
                    Parent.OnRemoveItems(removed);
                }
                if (e.NewItems != null)
                {
                    var added = new List<TResult>();
                    foreach (TIntermediate element in e.NewItems)
                    {
                        AttachResult(element);
                        added.Add(Values[element].Value);
                    }
                    Parent.OnAddItems(added);
                }
            }

            public void OnSourceChanged(object sender, ValueChangedEventArgs e)
            {
                DetachSubsource();
                var notifier = e.OldValue as INotifyCollectionChanged;
                if (notifier != null) notifier.CollectionChanged -= OnCollectionChanged;

                AttachSubSource(this);
                Parent.OnAddItems(SL.Select(Values.Values, res => res.Value));
            }

            private void DetachSubsource()
            {
                var removed = new List<TResult>();
                foreach (var element in Values)
                {
                    element.Value.ValueChanged -= Parent.ResultChanged;
                    removed.Add(element.Value.Value);
                }
                if (NotifySubSource != null)
                {
                    NotifySubSource.Detach();
                    NotifySubSource = null;
                }
                Values.Clear();
                Parent.OnRemoveItems(removed);
            }

            public void AttachResult(TIntermediate element)
            {
                TaggedObservableValue<TResult, int> result;
                if (!Values.TryGetValue(element, out result))
                {
                    result = Parent.selector.InvokeTagged(this, element, 0);
                    result.ValueChanged += Parent.ResultChanged;
                    Values.Add(element, result);
                }
                result.Tag++;
            }

            public void DetachResult(TIntermediate element)
            {
                TaggedObservableValue<TResult, int> result;
                if (Values.TryGetValue(element, out result))
                {
                    result.Tag--;
                    if (result.Tag == 0)
                    {
                        Values.Remove(element);
                        result.ValueChanged -= Parent.ResultChanged;
                        result.Detach();
                    }
                }
            }

            internal void UnregisterAllResultEvents()
            {
                foreach (var result in Values.Values)
                {
                    result.ValueChanged -= Parent.ResultChanged;
                    result.Detach();
                }
                var notifier = SubSource.Value as INotifyCollectionChanged;
                if (notifier != null)
                {
                    notifier.CollectionChanged -= OnCollectionChanged;
                }
                SubSource.Detach();
            }

            public TSource Value
            {
                get { return Item; }
            }

            public void ReplaceItem(TSource newItem)
            {
                var oldItem = Item;
                if (!EqualityComparer<TSource>.Default.Equals(oldItem, newItem))
                {
                    Item = newItem;
                    var handler = ValueChanged;
                    if (handler != null)
                    {
                        handler.Invoke(this, new ValueChangedEventArgs(oldItem, newItem));
                    }
                }
        }

            public event EventHandler<ValueChangedEventArgs> ValueChanged;

            public void Detach() { }

            public void Attach() { }

            public bool IsAttached
            {
                get { return true; }
            }
        }


        public override IEnumerator<TResult> GetEnumerator()
        {
            return SL.Select(
                SL.SelectMany(sourceItems.Values, sub => sub.Values.Values), res => res.Value).GetEnumerator();
        }

        public override int Count
        {
            get
            {
                return SL.Sum(SL.Select(sourceItems.Values, sub => sub.Values.Count));
            }
        }

        protected override void AttachCore()
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    var subSource = func.Observe(item);
                    var wrapper = new SubSourcePair(subSource, item, this);
                    sourceItems.Add(item, wrapper);
                    AttachSubSource(wrapper);
                    subSource.ValueChanged += wrapper.OnSourceChanged;
                }
                source.CollectionChanged += SourceCollectionChanged;
            }
        }

        protected override void DetachCore()
        {
            DetachSource();
        }
    }
}
