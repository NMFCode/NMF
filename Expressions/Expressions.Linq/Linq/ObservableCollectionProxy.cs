using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    [DebuggerDisplay("proxy for {Inner}")]
    internal sealed class ObservableCollectionProxy<T> : ObservableEnumerable<T>, INotifyCollection<T>
    {
        private readonly CollectionChangeListener<T> listener;
        public IEnumerable<T> Inner { get; }

        public override IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public ObservableCollectionProxy(IEnumerable<T> inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            Inner = inner;
            listener = new CollectionChangeListener<T>(this);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return Inner.GetEnumerator();
        }

        protected override void OnAttach()
        {
            if (Inner is INotifyCollectionChanged notifiable)
                listener.Subscribe(notifiable);
        }

        protected override void OnDetach()
        {
            listener.Unsubscribe();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var backParsed = (ICollectionChangedNotificationResult)sources[0];
            if (HasEventSubscriber)
            {
                if (backParsed.IsReset)
                {
                    OnCleared();
                }
                else
                {
                    if (backParsed.RemovedItems != null && backParsed.RemovedItems.Count > 0)
                    {
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, backParsed.RemovedItems, backParsed.OldItemsStartIndex));
                    }
                    if (backParsed.AddedItems != null && backParsed.AddedItems.Count > 0)
                    {
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, backParsed.AddedItems, backParsed.NewItemsStartIndex));
                    }
                    if (backParsed.MovedItems != null && backParsed.MovedItems.Count > 0)
                    {
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, backParsed.MovedItems, backParsed.NewItemsStartIndex, backParsed.OldItemsStartIndex));
                    }
                }
            }
            return CollectionChangedNotificationResult<T>.Transfer(backParsed, this);
        }

        public override bool Contains(T item)
        {
            return Inner.Contains(item);
        }

        public override int Count
        {
            get
            {
                return Inner.Count();
            }
        }

        public override string ToString()
        {
            return Inner.ToString();
        }

        void ICollection<T>.Add(T item)
        {
            if (Inner is not ICollection<T> coll || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            coll.Add(item);
        }

        void ICollection<T>.Clear()
        {
            if (Inner is not ICollection<T> coll || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            var list = new List<T>(this);
            if (list.Count == coll.Count)
            {
                coll.Clear();
            }
            else
            {
                foreach (var item in list)
                {
                    coll.Remove(item);
                }
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return Inner is not ICollection<T> collection || collection.IsReadOnly;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            if (Inner is not ICollection<T> coll || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            return coll.Remove(item);
        }
    }
}
