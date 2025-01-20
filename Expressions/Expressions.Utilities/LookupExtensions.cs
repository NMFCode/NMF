using System;
using System.Linq.Expressions;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes extensions to create a lookup
    /// </summary>
    public static class LookupExtensions
    {
        /// <summary>
        /// Creates a lookup of the given collection
        /// </summary>
        /// <typeparam name="TSource">The source type of elements</typeparam>
        /// <typeparam name="TKey">The key type</typeparam>
        /// <param name="source">The underlying source collection</param>
        /// <param name="keySelector">A predicate to select the key of an element</param>
        /// <returns>A lookup expression</returns>
        public static ILookupExpression<TSource, TKey> ToLookup<TSource, TKey>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return new Lookup<TSource, TKey>(source, keySelector);
        }

        /// <summary>
        /// Creates a lookup of the given collection
        /// </summary>
        /// <typeparam name="TSource">The source type of elements</typeparam>
        /// <typeparam name="TKey">The key type</typeparam>
        /// <param name="source">The underlying source collection</param>
        /// <param name="keySelector">A predicate to select the key of an element</param>
        /// <returns>A lookup expression</returns>
        public static INotifyLookup<TSource, TKey> ToLookup<TSource, TKey>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            var lookup = new IncrementalLookup<TSource, TKey>(source, keySelector);
            lookup.Successors.SetDummy();
            return lookup;
        }
    }
}
