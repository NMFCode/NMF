using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class OfTypeExpression<T> : IEnumerableExpression<T>
    {
        public IEnumerableExpression Source { get; private set; }

        public OfTypeExpression(IEnumerableExpression source)
        {
            if (source == null) throw new ArgumentNullException("source");

            Source = source;
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            return Source.AsNotifiable().OfType<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return SL.OfType<T>(Source).GetEnumerator();
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
