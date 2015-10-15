using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class CastExpression<T> : IEnumerableExpression<T>
    {
        public IEnumerableExpression Source { get; private set; }

        public CastExpression(IEnumerableExpression source)
        {
            if (source == null) throw new ArgumentNullException("source");

            Source = source;
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            return Source.AsNotifiable().Cast<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
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
