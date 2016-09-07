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
        private readonly IExecutionContext context = ExecutionEngine.Current.Context;

        public override IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }
        
        public OrderableList()
        {
            Sequences = new NotifyCollection<IEnumerable<T>>();
            Sequences.CollectionChanged += SequencesCollectionChanged;
            context.AddChangeListener<IEnumerable<T>>(this, Sequences);
        }

        void SequencesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    foreach (IEnumerable<T> sequence in e.OldItems)
                    {
                        OnRemoveItems(sequence);
                        INotifyCollectionChanged notifier = sequence as INotifyCollectionChanged;
                        if (notifier != null)
                        {
                            context.RemoveChangeListener(this, notifier);
                        }
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (IEnumerable<T> sequence in e.NewItems)
                    {
                        OnAddItems(sequence);
                        INotifyCollectionChanged notifier = sequence as INotifyCollectionChanged;
                        if (notifier != null)
                        {
                            context.AddChangeListener<IEnumerable<T>>(this, notifier);
                        }
                    }
                }
            }
            else
            {
                OnCleared();
            }
        }

        private void SequenceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                OnCollectionChanged(e);
            }
            else
            {
                OnCleared();
                OnAddItems(this);
            }
        }

        public NotifyCollection<IEnumerable<T>> Sequences { get; private set; }

        public override IEnumerator<T> GetEnumerator()
        {
            return Enumerable.SelectMany(Sequences, coll => coll).GetEnumerator();
        }

        public IEnumerable<T> GetSequenceForItem(T item)
        {
            throw new InvalidOperationException();
        }

        IEnumerable<IEnumerable<T>> IOrderableNotifyEnumerable<T>.Sequences
        {
            get { return Sequences; }
        }

        protected override void Dispose(bool disposing)
        {
            context.RemoveChangeListener(this, Sequences);
            base.Dispose(disposing);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            OnCleared();
            return CollectionChangedNotificationResult<IEnumerable<T>>.Transfer(sources[0] as ICollectionChangedNotificationResult, this);
        }
    }
}
