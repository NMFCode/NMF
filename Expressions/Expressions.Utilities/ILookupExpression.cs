using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Linq
{
    public interface ILookupExpression<TSource, TKey>
    {
        IEnumerableExpression<TSource> this[TKey key] { get; }

        IEnumerableExpression<TKey> Keys { get; }

        INotifyLookup<TSource, TKey> AsNotifiable();
    }

    public interface INotifyLookup<TSource, TKey>
    {
        INotifyEnumerable<TSource> this[TKey key] { get; }

        INotifyEnumerable<TKey> Keys { get; }
    }
}
