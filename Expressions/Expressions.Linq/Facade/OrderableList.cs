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
        public OrderableList()
        {
            Sequences = new NotifyCollection<IEnumerable<T>>();
            Sequences.CollectionChanged += SequencesCollectionChanged;
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
                            notifier.CollectionChanged -= SequenceCollectionChanged;
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
                            notifier.CollectionChanged += SequenceCollectionChanged;
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

        protected override void AttachCore() { }

        protected override void DetachCore() { }

        INotifyEnumerable<IEnumerable<T>> IOrderableNotifyEnumerable<T>.Sequences
        {
            get { return Sequences; }
        }
    }
}
