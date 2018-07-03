using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Expressions.Linq
{
    public interface ILookup<TSource, TKey>
    {
        IEnumerableExpression<TSource> this[TKey key] { get; }

        IEnumerableExpression<TKey> Keys { get; }
    }

    public interface INotifyLookup<TSource, TKey>
    {
        INotifyEnumerable<TSource> this[TKey key] { get; }

        INotifyEnumerable<TKey> Keys { get; }
    }
}
