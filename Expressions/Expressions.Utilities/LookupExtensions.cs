using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    public static class LookupExtensions
    {
        public static ILookupExpression<TSource, TKey> ToLookup<TSource, TKey>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return new Lookup<TSource, TKey>(source, keySelector);
        }
    }
}
