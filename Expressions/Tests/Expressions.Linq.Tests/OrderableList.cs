﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    public class OrderableList<T> : ObservableEnumerable<T>, IOrderableNotifyEnumerable<T>
    {
        private readonly Dictionary<IEnumerable<T>, CollectionChangeListener<T>> changeListener;

        public override IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }
        
        public OrderableList()
        {
            Sequences = new NotifyCollection<IEnumerable<T>>();
            Sequences.CollectionChanged += SequencesCollectionChanged;
            changeListener = new Dictionary<IEnumerable<T>, CollectionChangeListener<T>>();
        }

        void SequencesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                var notification = CollectionChangedNotificationResult<T>.Create(this);
                var removed = notification.RemovedItems;
                var added = notification.AddedItems;

                if (e.OldItems != null)
                {
                    foreach (IEnumerable<T> sequence in e.OldItems)
                    {
                        removed.AddRange(sequence);
                        INotifyCollectionChanged notifier = sequence as INotifyCollectionChanged;
                        if (notifier != null)
                        {
                            var listener = changeListener[sequence];
                            listener.Unsubscribe();
                            changeListener.Remove(sequence);
                        }
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (IEnumerable<T> sequence in e.NewItems)
                    {
                        added.AddRange(sequence);
                        INotifyCollectionChanged notifier = sequence as INotifyCollectionChanged;
                        if (notifier != null)
                        {
                            var listener = new CollectionChangeListener<T>(this);
                            listener.Subscribe(notifier);
                            changeListener.Add(sequence, listener);
                        }
                    }
                }

                ExecutionMetaData.Results.Add(notification);
            }
            ExecutionEngine.Current.InvalidateNode(this);
        }

        public NotifyCollection<IEnumerable<T>> Sequences { get; private set; }

        public override IEnumerator<T> GetEnumerator()
        {
            return Enumerable.SelectMany(Sequences, coll => coll).GetEnumerator();
        }

        public IEnumerable<T> GetSequenceForItem(T item)
        {
            return Sequences.FirstOrDefault(s => s.Contains(item));
        }

        IEnumerable<IEnumerable<T>> IOrderableNotifyEnumerable<T>.Sequences
        {
            get { return Sequences; }
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count == 0)
            {
                OnCleared();
                return CollectionChangedNotificationResult<T>.Create(this, true);
            }
            else
            {
                var change = (ICollectionChangedNotificationResult)sources[0];
                if (change.IsReset)
                    OnCleared();
                else
                {
                    if (change.RemovedItems != null)
                        OnRemoveItems(change.RemovedItems.Cast<T>());
                    if (change.AddedItems != null)
                        OnAddItems(change.AddedItems.Cast<T>());
                }
                return CollectionChangedNotificationResult<T>.Transfer(change, this);
            }
        }
    }
}
