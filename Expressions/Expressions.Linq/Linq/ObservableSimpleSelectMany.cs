using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableSimpleSelectMany<TSource, TResult> : ObservableEnumerable<TResult>
    {
        public override string ToString()
        {
            return "[Flatten]";
        }

        private readonly INotifyEnumerable<TSource> source;
        private readonly ObservingFunc<TSource, IEnumerable<TResult>> selector;

        private readonly Dictionary<TSource, Itemdata> results = new Dictionary<TSource, Itemdata>();
        
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
            return results.Values.SelectMany(r => Enumerable.Repeat(0, r.Count).SelectMany(_ => r.Item.Value)).GetEnumerator();
        }

        public override bool Contains(TResult item)
        {
            foreach (var items in results.Values.Select(s => s.Item))
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
                return results.Values.Select(s => s.Count * s.Item.Value.Count()).Sum();
            }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                foreach (var result in results.Values)
                {
                    yield return result.Item;
                    if (result.Notifiable != null)
                    {
                        yield return result.Notifiable;
                    }
                }
            }
        }

        private IEnumerable<TResult> AttachItem(TSource item)
        {
            var subSource = selector.Observe(item);
            subSource.Successors.Set(this);
            var notifiable = subSource.Value as INotifyEnumerable<TResult>;
            if (notifiable == null)
            {
                if (subSource.Value is IEnumerableExpression<TResult> expression)
                {
                    notifiable = expression.AsNotifiable();
                }
            }
            if (notifiable != null)
            {
                notifiable.Successors.Set(this);
            }
            Itemdata data;
            if (!results.TryGetValue(item, out data))
            {
                results.Add(item, new Itemdata(subSource, notifiable, 1));
            }
            else
            {
                results[item] = new Itemdata(data.Item, data.Notifiable, data.Count + 1);
            }
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
            foreach (var result in results.Values)
            {
                result.Item.Successors.Unset(this);
                if (result.Notifiable != null)
                {
                    result.Notifiable.Successors.Unset(this);
                }
            }
            results.Clear();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var notification = CollectionChangedNotificationResult<TResult>.Create(this);
            var added = notification.AddedItems;
            var removed = notification.RemovedItems;

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
                        NotifySource(sourceChange, added, removed);
                    }
                }
                else
                {
                    if (change is ICollectionChangedNotificationResult innerCollectionChange)
                    {
                        if (innerCollectionChange.AddedItems != null)
                        {
                            added.AddRange(innerCollectionChange.AddedItems.Cast<TResult>());
                        }
                        if (innerCollectionChange.RemovedItems != null)
                        {
                            removed.AddRange(innerCollectionChange.RemovedItems.Cast<TResult>());
                        }
                    }
                    else
                    {
                        var subSourceChange = (IValueChangedNotificationResult<IEnumerable<TResult>>)change;
                        if (subSourceChange.OldValue != null)
                        {
                            removed.AddRange(subSourceChange.OldValue);
                        }
                        if (subSourceChange.NewValue != null)
                        {
                            added.AddRange(subSourceChange.NewValue);
                        }
                    }
                }
            }

            OnRemoveItems(removed);
            OnAddItems(added);
            return notification;
        }

        private void NotifySource(ICollectionChangedNotificationResult<TSource> sourceChange, List<TResult> added, List<TResult> removed)
        {
            if (sourceChange.RemovedItems != null)
            {
                foreach (var item in sourceChange.RemovedItems)
                {
                    var data = results[item];
                    var resultItems = data.Item;
                    if (data.Count == 1)
                    {
                        if (data.Notifiable != null)
                        {
                            data.Notifiable.Successors.Unset(this);
                        }
                        resultItems.Successors.Unset(this);
                        results.Remove(item);
                    }
                    else
                    {
                        results[item] = new Itemdata(data.Item, data.Notifiable, data.Count - 1);
                    }
                    removed.AddRange(resultItems.Value);
                }
            }

            if (sourceChange.AddedItems != null)
            {
                foreach (var item in sourceChange.AddedItems)
                {
                    added.AddRange(AttachItem(item));
                }
            }
        }


        private struct Itemdata
        {
            public Itemdata(INotifyValue<IEnumerable<TResult>> item, INotifyEnumerable<TResult> notifiable, int count)
                : this()
            {
                Item = item;
                Notifiable = notifiable;
                Count = count;
            }

            public INotifyValue<IEnumerable<TResult>> Item { get; set; }

            public INotifyEnumerable<TResult> Notifiable { get; set; }

            public int Count { get; set; }
        }
    }

}
