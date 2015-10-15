using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableConcat<TSource> : ObservableEnumerable<TSource>
    {
        private INotifyEnumerable<TSource> source;
        private IEnumerable<TSource> source2;

        public ObservableConcat(INotifyEnumerable<TSource> source, IEnumerable<TSource> source2)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (source2 == null) throw new ArgumentNullException("source2");

            this.source = source;
            this.source2 = source2;

            Attach();
        }

        public override IEnumerator<TSource> GetEnumerator()
        {
            return SL.Concat(source, source2).GetEnumerator();
        }

        protected override void AttachCore()
        {
            source.CollectionChanged += SourceCollectionChanged;
            var notifier = source2 as INotifyCollectionChanged;
            if (notifier != null)
            {
                notifier.CollectionChanged += Source2CollectionChanged;
            }
        }

        private void Source2CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            int sourceCount = SL.Count(source);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e = new NotifyCollectionChangedEventArgs(e.Action, e.NewItems, sourceCount + e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    e = new NotifyCollectionChangedEventArgs(e.Action, e.NewItems, sourceCount + e.NewStartingIndex, sourceCount + e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    e = new NotifyCollectionChangedEventArgs(e.Action, e.OldItems, sourceCount + e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    e = new NotifyCollectionChangedEventArgs(e.Action, e.NewItems, e.OldItems, sourceCount + e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    OnCleared();
                    return;
                default:
                    throw new InvalidOperationException();
            }
            OnCollectionChanged(e);
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }

        protected override void DetachCore()
        {
            source.CollectionChanged -= SourceCollectionChanged;
            var notifier = source2 as INotifyCollectionChanged;
            if (notifier != null)
            {
                notifier.CollectionChanged -= Source2CollectionChanged;
            }
        }
    }
}
