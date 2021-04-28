using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal static class ObservableExtensionProxies
    {

        /// <summary>
        /// Groups the given collection by the given predicate
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <typeparam name="TKey">The type of keys used for grouping</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">The predicate expression selecting the keys for grouping</param>
        /// <returns>A collection of groups</returns>
        public static INotifyEnumerable<INotifyGrouping<TKey, TSource>> GroupBy<TSource, TKey>(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TKey> keySelector)
        {
            return GroupByWithComparer<TSource, TKey>(source, keySelector, null);
        }

        /// <summary>
        /// Groups the given collection by the given predicate
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <typeparam name="TKey">The type of keys used for grouping</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">The predicate expression selecting the keys for grouping</param>
        /// <param name="comparer">A comparer that decides whether items are identical</param>
        /// <returns>A collection of groups</returns>
        public static INotifyEnumerable<INotifyGrouping<TKey, TSource>> GroupByWithComparer<TSource, TKey>(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new ObservableGroupBy<TKey, TSource>(source, keySelector, comparer);
        }

        /// <summary>
        /// Groups the given collection by the given predicate into the given result
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <typeparam name="TKey">The type of keys used for grouping</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">The predicate expression selecting the keys for grouping</param>
        /// <param name="resultSelector">A function to get the result element for a group</param>
        /// <returns>A collection of groups</returns>
        public static INotifyEnumerable<TResult> GroupByWithSelector<TSource, TKey, TResult>(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TKey> keySelector, ObservingFunc<TKey, IEnumerable<TSource>, TResult> resultSelector)
        {
            return GroupByWithSelectorAndComparer<TSource, TKey, TResult>(source, keySelector, resultSelector, null);
        }

        /// <summary>
        /// Groups the given collection by the given predicate into the given result
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <typeparam name="TKey">The type of keys used for grouping</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">The predicate expression selecting the keys for grouping</param>
        /// <param name="comparer">A comparer that decides whether items are identical</param>
        /// <param name="resultSelector">A function to get the result element for a group</param>
        /// <returns>A collection of groups</returns>
        public static INotifyEnumerable<TResult> GroupByWithSelectorAndComparer<TSource, TKey, TResult>(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TKey> keySelector, ObservingFunc<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            var group = Expression.Parameter(typeof(INotifyGrouping<TKey, TSource>), "group");
            var key = Expression.MakeMemberAccess(group, ReflectionHelper.GetProperty(group.Type, "Key"));
            var lambda = Expression.Lambda<Func<INotifyGrouping<TKey, TSource>, TResult>>(Expression.Call(Expression.Constant(resultSelector), ReflectionHelper.GetMethod(typeof(ObservingFunc<TKey, IEnumerable<TSource>, TResult>), "Invoke", new Type[] { typeof(TKey), typeof(IEnumerable<TSource>) }), key, group));
            return Select(GroupByWithComparer<TSource, TKey>(source, keySelector, comparer), Observable.Func(lambda));
        }

        /// <summary>
        /// Joins the given collections based on keys into groups
        /// </summary>
        /// <typeparam name="TOuter">The element type of the outer collection</typeparam>
        /// <typeparam name="TInner">The element type of the inner collection</typeparam>
        /// <typeparam name="TKey">The key type to be matched</typeparam>
        /// <typeparam name="TResult">The resulting type</typeparam>
        /// <param name="outer">The outer collection</param>
        /// <param name="inner">The inner collection</param>
        /// <param name="outerKeySelector">A predicate that returns the key for each outer item</param>
        /// <param name="innerKeySelector">A predicate that returns the key for each inner item</param>
        /// <param name="resultSelector">A function that creates a result for each group of an outer item and a group of inner items</param>
        /// <returns>A collection of grouped results</returns>
        public static INotifyEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(INotifyEnumerable<TOuter> outer, IEnumerable<TInner> inner, ObservingFunc<TOuter, TKey> outerKeySelector, ObservingFunc<TInner, TKey> innerKeySelector, ObservingFunc<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        {
            return new ObservableGroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, null);
        }

        /// <summary>
        /// Joins the given collections based on keys into groups
        /// </summary>
        /// <typeparam name="TOuter">The element type of the outer collection</typeparam>
        /// <typeparam name="TInner">The element type of the inner collection</typeparam>
        /// <typeparam name="TKey">The key type to be matched</typeparam>
        /// <typeparam name="TResult">The resulting type</typeparam>
        /// <param name="outer">The outer collection</param>
        /// <param name="inner">The inner collection</param>
        /// <param name="outerKeySelector">A predicate that returns the key for each outer item</param>
        /// <param name="innerKeySelector">A predicate that returns the key for each inner item</param>
        /// <param name="comparer">A comparer to decide when two items are equal</param>
        /// <param name="resultSelector">A function that creates a result for each group of an outer item and a group of inner items</param>
        /// <returns>A collection of grouped results</returns>
        public static INotifyEnumerable<TResult> GroupJoinWithComparer<TOuter, TInner, TKey, TResult>(INotifyEnumerable<TOuter> outer, IEnumerable<TInner> inner, ObservingFunc<TOuter, TKey> outerKeySelector, ObservingFunc<TInner, TKey> innerKeySelector, ObservingFunc<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return new ObservableGroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static INotifyEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(INotifyEnumerable<TOuter> outerSource, IEnumerable<TInner> innerSource, ObservingFunc<TOuter, TKey> outerKeySelector, ObservingFunc<TInner, TKey> innerKeySelector, ObservingFunc<TOuter, TInner, TResult> resultSelector)
        {
            return JoinWithComparer(outerSource, innerSource, outerKeySelector, innerKeySelector, resultSelector, null);
        }

        public static INotifyEnumerable<TResult> JoinWithComparer<TOuter, TInner, TKey, TResult>(INotifyEnumerable<TOuter> outerSource, IEnumerable<TInner> innerSource, ObservingFunc<TOuter, TKey> outerKeySelector, ObservingFunc<TInner, TKey> innerKeySelector, ObservingFunc<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return new ObservableJoin<TOuter, TInner, TKey, TResult>(outerSource, innerSource, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static IOrderableNotifyEnumerable<TItem> OrderBy<TItem, TKey>(INotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector)
        {
            return OrderByWithComparer(source, keySelector, null);
        }

        public static IOrderableNotifyEnumerable<TItem> OrderByWithComparer<TItem, TKey>(INotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            return new ObservableOrderBy<TItem, TKey>(source, keySelector, comparer);
        }

        public static IOrderableNotifyEnumerable<TItem> OrderByDescending<TItem, TKey>(INotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector)
        {
            return OrderByDescendingWithComparer(source, keySelector, null);
        }

        public static IOrderableNotifyEnumerable<TItem> OrderByDescendingWithComparer<TItem, TKey>(INotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            return new ObservableOrderBy<TItem, TKey>(source, keySelector, new ReverseComparer<TKey>(comparer));
        }

        public static INotifyEnumerable<TResult> Select<TSource, TResult>(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TResult> selector)
        {
            return new ObservableSelect<TSource, TResult>(source, selector);
        }

        public static INotifyEnumerable<TResult> SelectMany<TSource, TIntermediate, TResult>(INotifyEnumerable<TSource> source, ObservingFunc<TSource, IEnumerable<TIntermediate>> func, ObservingFunc<TSource, TIntermediate, TResult> selector)
        {
            return new ObservableSelectMany<TSource, TIntermediate, TResult>(source, func, selector);
        }

        public static INotifyEnumerable<TResult> SimpleSelectMany<TSource, TResult>(INotifyEnumerable<TSource> source, ObservingFunc<TSource, IEnumerable<TResult>> selector)
        {
            return new ObservableSimpleSelectMany<TSource, TResult>(source, selector);
        }

        public static IOrderableNotifyEnumerable<TItem> ThenBy<TItem, TKey>(IOrderableNotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector)
        {
            return ThenByWithComparer(source, keySelector, null);
        }

        public static IOrderableNotifyEnumerable<TItem> ThenByWithComparer<TItem, TKey>(IOrderableNotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            return new ObservableThenBy<TItem, TKey>(source, keySelector, comparer);
        }

        public static IOrderableNotifyEnumerable<TItem> ThenByDescending<TItem, TKey>(IOrderableNotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector)
        {
            return ThenByDescendingWithComparer(source, keySelector, null);
        }

        public static IOrderableNotifyEnumerable<TItem> ThenByDescendingWithComparer<TItem, TKey>(IOrderableNotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IComparer<TKey> comparer)
        {
            return new ObservableThenBy<TItem, TKey>(source, keySelector, new ReverseComparer<TKey>(comparer));
        }

        public static INotifyEnumerable<T> Where<T>(INotifyEnumerable<T> source, ObservingFunc<T, bool> filter)
        {
            return new ObservableWhere<T>(source, filter);
        }

        public static INotifyCollection<T> WhereCollection<T>(INotifyCollection<T> source, ObservingFunc<T, bool> filter)
        {
            return new ObservableWhere<T>(source, filter);
        }
    }
}
