using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class DummyExpression<T> : List<T>, IEnumerableExpression<T>
    {
        public INotifyEnumerable<T> AsNotifiable()
        {
            throw new NotSupportedException( "This collection is not meant for incremental evaluation because a parent collection was executed in batch mode." );
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }
    }
}
