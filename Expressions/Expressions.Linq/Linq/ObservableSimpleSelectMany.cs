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

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                foreach (var stack in results.Values)
                {
                    foreach (var item in stack)
                        yield return item;
                }
            }
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
            //TODO do we need to handle INotifyCollectionChanged in subSource.Value or does it do that automatically?
            stack.Push(subSource);
            subSource.Successors.Set(this);
            return subSource.Value;
        }

        protected override void OnAttach()
        {
            foreach (var item in source)
            {
                AttachItem(item);
            }
        }

        protected override void OnDetach()
        {
            foreach (var stack in results.Values)
            {
                foreach (var result in stack)
                {
                    result.Successors.Unset(this);
                }
            }
            results.Clear();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var added = new List<TResult>();
            var removed = new List<TResult>();

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
                    var subSourceChange = (ValueChangedNotificationResult<IEnumerable<TResult>>)change;
                    removed.AddRange(subSourceChange.OldValue);
                    added.AddRange(subSourceChange.NewValue);
                }
            }

            if (added.Count == 0 && removed.Count == 0)
                return UnchangedNotificationResult.Instance;

            OnRemoveItems(removed);
            OnAddItems(added);
            return new CollectionChangedNotificationResult<TResult>(this, added, removed);
        }

        private void NotifySource(CollectionChangedNotificationResult<TSource> sourceChange, List<TResult> added, List<TResult> removed)
        {
            foreach (var item in sourceChange.AllRemovedItems)
            {
                var stack = results[item];
                var resultItems = stack.Pop();
                if (stack.Count == 0)
                {
                    results.Remove(item);
                }
                removed.AddRange(resultItems.Value);
                resultItems.Successors.Unset(this);
            }

            foreach (var item in sourceChange.AllAddedItems)
            {
                added.AddRange(AttachItem(item));
            }
        }
    }

}
