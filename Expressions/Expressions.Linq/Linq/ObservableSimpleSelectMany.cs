using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableSimpleSelectMany<TSource, TResult> : ObservableEnumerable<TResult>
    {
        private INotifyEnumerable<TSource> source;
        private ObservingFunc<TSource, IEnumerable<TResult>> selector;

        private Dictionary<TSource, Stack<INotifyValue<IEnumerable<TResult>>>> results = new Dictionary<TSource, Stack<INotifyValue<IEnumerable<TResult>>>>();

        public ObservableSimpleSelectMany(INotifyEnumerable<TSource> source,
            ObservingFunc<TSource, IEnumerable<TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            this.source = source;
            this.selector = selector;

            Attach();
        }

        public override IEnumerator<TResult> GetEnumerator()
        {
            return results.Values.SelectMany(r => r.SelectMany(s => s.Value)).GetEnumerator();
        }

        public override bool Contains(TResult item)
        {
            foreach (var items in results.Values.SelectMany(s => s))
            {
                if (items.Value.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public override int Count
        {
            get
            {
                return results.Values.SelectMany(s => s).Sum(r => r.Value.Count());
            }
        }

        protected override void AttachCore()
        {
            foreach (var item in source)
            {
                AttachItem(item);
            }

            source.CollectionChanged += SourceCollectionChanged;
        }

        private IEnumerable<TResult> AttachItem(TSource item)
        {
            Stack<INotifyValue<IEnumerable<TResult>>> stack;
            if (!results.TryGetValue(item, out stack))
            {
                stack = new Stack<INotifyValue<IEnumerable<TResult>>>();
                results.Add(item, stack);
            }
            var subSource = selector.Observe(item);
            stack.Push(subSource);
            var notifier = subSource.Value as INotifyCollectionChanged;
            if (notifier != null)
            {
                notifier.CollectionChanged += SubSourceCollectionChanged;
            }
            subSource.ValueChanged += SubSourceChanged;
            return subSource.Value;
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                DetachItems();
                OnCleared();
            }
            else if (e.Action != NotifyCollectionChangedAction.Move)
            {
                if (e.OldItems != null)
                {
                    var removed = new List<TResult>();
                    foreach (TSource item in e.OldItems)
                    {
                        var stack = results[item];
                        var resultItems = stack.Pop();
                        if (stack.Count == 0)
                        {
                            results.Remove(item);
                        }
                        removed.AddRange(resultItems.Value);
                        var resultNotify = resultItems.Value as INotifyCollectionChanged;
                        if (resultNotify != null)
                        {
                            resultNotify.CollectionChanged -= SubSourceCollectionChanged;
                        }
                        resultItems.ValueChanged -= SubSourceChanged;
                        resultItems.Detach();
                    }
                    OnRemoveItems(removed);
                }
                if (e.NewItems != null)
                {
                    var added = new List<TResult>();
                    foreach (TSource item in e.NewItems)
                    {
                        added.AddRange(AttachItem(item));
                    }
                    OnAddItems(added);
                }
            }
        }

        private void SubSourceChanged(object sender, ValueChangedEventArgs e)
        {
            var oldItems = e.OldValue as IEnumerable<TResult>;
            var newItems = e.NewValue as IEnumerable<TResult>;

            var oldNotify = oldItems as INotifyCollectionChanged;
            if (oldNotify != null)
            {
                oldNotify.CollectionChanged -= SubSourceCollectionChanged;
            }
            OnRemoveItems(oldItems);
            var newNotify = newItems as INotifyCollectionChanged;
            if (newNotify != null)
            {
                newNotify.CollectionChanged += SubSourceCollectionChanged;
            }
            OnAddItems(newItems);
        }

        private void SubSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        protected override void DetachCore()
        {
            DetachItems();

            source.CollectionChanged -= SourceCollectionChanged;
        }

        private void DetachItems()
        {
            foreach (var stack in results.Values)
            {
                foreach (var result in stack)
                {
                    result.ValueChanged -= SubSourceChanged;
                    result.Detach();
                }
            }
            results.Clear();
        }
    }

}
