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
        private INotifyEnumerable source;

        public ObservableCast(INotifyEnumerable source)
        {
            if (source == null) throw new ArgumentNullException("source");

            this.source = source;
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
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
            var change = (CollectionChangedNotificationResult)sources[0];
            return change.Forward(this);
        }
    }
}
