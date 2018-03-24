using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class DistinctExpression<T> : IEnumerableExpression<T>, ISQO
    {
        public IEnumerableExpression<T> Source { get; private set; }
        public IEqualityComparer<T> Comparer { get; set; }
        private INotifyEnumerable<T> notifyEnumerable;

        public IEnumerableExpression OptSource => Source;

        public DistinctExpression(IEnumerableExpression<T> source, IEqualityComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");

            Source = source;
            Comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                notifyEnumerable = Source.AsNotifiable().Distinct(Comparer);
            }
            return notifyEnumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.Distinct(Source, Comparer).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }
    }
}
