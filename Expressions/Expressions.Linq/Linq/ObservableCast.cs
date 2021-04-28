using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableCast<TTarget> : ObservableEnumerable<TTarget>
    {
        public override string ToString()
        {
            return "[Cast]";
        }

        private readonly INotifyEnumerable source;

        public ObservableCast(INotifyEnumerable source)
        {
            if (source == null) throw new ArgumentNullException("source");

            this.source = source;
        }
        
        public override IEnumerator<TTarget> GetEnumerator()
        {
            return SL.Cast<TTarget>(source).GetEnumerator();
        }

        public override bool Contains(TTarget item)
        {
            return SL.Contains(SL.Cast<TTarget>(source), item);
        }

        public override int Count
        {
            get
            {
                return SL.Count(SL.Cast<TTarget>(source));
            }
        }

        public override IEnumerable<INotifiable> Dependencies { get { yield return source; } }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            return CollectionChangedNotificationResult<TTarget>.Transfer((ICollectionChangedNotificationResult)sources[0], this);
        }
    }
}
