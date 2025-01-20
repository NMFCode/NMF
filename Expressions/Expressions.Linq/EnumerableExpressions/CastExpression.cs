using System;
using System.Collections.Generic;
using System.Linq;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class CastExpression<T> : IEnumerableExpression<T>, ISQO
    {
        public IEnumerableExpression Source { get; private set; }
        private INotifyEnumerable<T> notifyEnumerable;

        public IEnumerableExpression OptSource => Source;

        public CastExpression(IEnumerableExpression source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Source = source;
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                notifyEnumerable = Source.AsNotifiable().Cast<T>();
            }
            return notifyEnumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.Cast<T>(Source).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Source.GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return Source.AsNotifiable();
        }
    }
}
