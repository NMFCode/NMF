using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    public class OrderableList<T> : ObservableEnumerable<T>, IOrderableNotifyEnumerable<T>
    {
        public override IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }
        
        public OrderableList()
        {
            Sequences = new NotifyCollection<IEnumerable<T>>();
            Sequences.CollectionChanged += SequencesCollectionChanged;
        }

        void SequencesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                var removed = new List<T>();
                var added = new List<T>();

                if (e.OldItems != null)
                {
                    foreach (IEnumerable<T> sequence in e.OldItems)
                    {
                        removed.AddRange(sequence);
                        INotifyCollectionChanged notifier = sequence as INotifyCollectionChanged;
                        if (notifier != null)
                        {
                            ExecutionContext.Instance.RemoveChangeListener(this, notifier);
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
                            ExecutionContext.Instance.AddChangeListener<T>(this, notifier);
                        }
                    }
                }

                ExecutionMetaData.Sources.Add(new CollectionChangedNotificationResult<T>(this, added, removed));
            }
            ExecutionEngine.Current.ManualInvalidation(this);
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
                return new CollectionChangedNotificationResult<T>(this);
            }
            else
            {
                var change = (CollectionChangedNotificationResult<T>)sources[0];
                if (change.IsReset)
                    OnCleared();
                else
                {
                    OnRemoveItems(change.AllRemovedItems);
                    OnAddItems(change.AllAddedItems);
                }
                return CollectionChangedNotificationResult<T>.Transfer(change, this);
            }
        }
    }
}
