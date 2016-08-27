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
        private INotifyEnumerable<TSource> observableSource2;

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                if (observableSource2 != null)
                    yield return observableSource2;
            }
        }

        public ObservableConcat(INotifyEnumerable<TSource> source, IEnumerable<TSource> source2)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (source2 == null) throw new ArgumentNullException("source2");

            this.source = source;
            this.source2 = source2;
            this.observableSource2 = source2 as INotifyEnumerable<TSource>;
        }

        public override IEnumerator<TSource> GetEnumerator()
        {
            return SL.Concat(source, source2).GetEnumerator();
        }
        
        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var added = new List<TSource>();
            var removed = new List<TSource>();

            foreach (CollectionChangedNotificationResult<TSource> change in sources)
            {
                if (change.IsReset)
                {
                    OnCleared();
                    return new CollectionChangedNotificationResult<TSource>(this);
                }

                added.AddRange(change.AddedItems);
                removed.AddRange(change.RemovedItems);
            }

            OnRemoveItems(removed);
            OnAddItems(added);
            return new CollectionChangedNotificationResult<TSource>(this, added, removed);
        }
    }
}
