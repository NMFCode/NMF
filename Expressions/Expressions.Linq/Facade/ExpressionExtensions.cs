using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Defines a set of extension methods on the <see cref="INotifyValue{T}">INotifyValue</see> monad
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets a value indicating whether all items in the given collection match the given predicate
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">A custom predicate that is applied to all items in the collection</param>
        /// <returns>True, if all items in the collection match the given predicate</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAll<>), "CreateExpression")]
        public static bool All<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.All(source, predicate.Compile());
        }

        /// <summary>
        /// Gets a value indicating whether there is any item in the source collection
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <returns>True, if the collection has an item, otherwise false</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableAny<>), "CreateExpression")]
        public static bool Any<TSource>(this IEnumerableExpression<TSource> source)
        {
            return SL.Any(source);
        }

        /// <summary>
        /// Gets a value indicating whether there is any item in the source collection that matches the given predicate
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">A custom predicate that is checked for every item</param>
        /// <returns>True, if there is an item that matches the givn criteria, otherwise false</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableLambdaAny<>), "CreateExpression")]
        public static bool Any<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Any(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableIntAverage), "CreateExpression")]
        public static double Average(this IEnumerableExpression<int> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableLongAverage), "CreateExpression")]
        public static double Average(this IEnumerableExpression<long> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableFloatAverage), "CreateExpression")]
        public static float Average(this IEnumerableExpression<float> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableDoubleAverage), "CreateExpression")]
        public static double Average(this IEnumerableExpression<double> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableDecimalAverage), "CreateExpression")]
        public static decimal Average(this IEnumerableExpression<decimal> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableIntAverage), "CreateExpression")]
        public static double? Average(this IEnumerableExpression<int?> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableLongAverage), "CreateExpression")]
        public static double? Average(this IEnumerableExpression<long?> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableFloatAverage), "CreateExpression")]
        public static float? Average(this IEnumerableExpression<float?> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableDoubleAverage), "CreateExpression")]
        public static double? Average(this IEnumerableExpression<double?> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableDecimalAverage), "CreateExpression")]
        public static decimal? Average(this IEnumerableExpression<decimal?> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateIntExpression")]
        public static double Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, int>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateLongExpression")]
        public static double Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, long>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateFloatExpression")]
        public static float Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, float>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateDoubleExpression")]
        public static double Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, double>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateDecimalExpression")]
        public static decimal Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableIntExpression")]
        public static double? Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, int?>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableLongExpression")]
        public static double? Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, long?>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableFloatExpression")]
        public static float? Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, float?>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableDoubleExpression")]
        public static double? Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, double?>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the average of the given feature based on items of the given collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The feature of the source items that should be averaged</param>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableDecimalExpression")]
        public static decimal? Average<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal?>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Average(source, predicate.Compile());
        }

        /// <summary>
        /// Casts the given notifying enumerable to the given type
        /// </summary>
        /// <typeparam name="TResult">The true type of the items in the collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <returns>A notifying collection casted to the given type</returns>
        /// <remarks>If any item in the source collection is not of type <typeparamref name="TResult"/>, an exception is thrown. Consider using <see cref="OfType{TResult}(IEnumerableExpression)"/> in this scenario.</remarks>
        public static IEnumerableExpression<TResult> Cast<TResult>(this IEnumerableExpression source)
        {
            return new CastExpression<TResult>(source);
        }

        /// <summary>
        /// Concats the given notifying enumerables
        /// </summary>
        /// <typeparam name="TSource">The type of the items</typeparam>
        /// <param name="source">The first source</param>
        /// <param name="source2">The second source</param>
        /// <returns>The concatenation of both sources</returns>
        /// <remarks>The second collection does not have to be a notifying collection, but if it is not, it must not change its contents.</remarks>
        public static IEnumerableExpression<TSource> Concat<TSource>(this IEnumerableExpression<TSource> source, IEnumerable<TSource> source2)
        {
            return new ConcatExpression<TSource>(source, source2);
        }

        /// <summary>
        /// Searches the given collection for the given item
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="item">The item that needs to be checked</param>
        /// <returns>True, if the given source collection contains the provided item, otherwise false</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableContains<>), "CreateExpression")]
        public static bool Contains<TSource>(this IEnumerableExpression<TSource> source, TSource item)
        {
            return Contains(source, item, null);
        }

        /// <summary>
        /// Searches the given collection for the given item
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="item">The item that needs to be checked</param>
        /// <param name="comparer">The equality comparer to decide whether items are equal. Can be omitted</param>
        /// <returns>True, if the given source collection contains the provided item, otherwise false</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableContains<>), "CreateExpressionWithComparer")]
        public static bool Contains<TSource>(this IEnumerableExpression<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
        {
            return SL.Contains(source, item, comparer);
        }

        /// <summary>
        /// Returns how many items are in the source collection
        /// </summary>
        /// <typeparam name="TSource">The type of elements within the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <returns>The amount of elements in the source collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableCount<>), "CreateExpression")]
        public static int Count<TSource>(this IEnumerableExpression<TSource> source)
        {
            return SL.Count(source);
        }

        /// <summary>
        /// Returns how many items are in the source collection that match the given predicate
        /// </summary>
        /// <typeparam name="TSource">The type of elements within the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The predicate that is to be checked for each item</param>
        /// <returns>The amount of elements in the source collection that match the given predicate</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableCount<>), "CreateExpressionWithComparer")]
        public static int Count<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.Count(source, predicate.Compile());
        }

        /// <summary>
        /// Eliminates duplicates from the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <returns>A notifying collection with no duplicates</returns>
        /// <remarks>This method destroys the original order of the items</remarks>
        public static IEnumerableExpression<TSource> Distinct<TSource>(this IEnumerableExpression<TSource> source)
        {
            return Distinct(source, null);
        }

        /// <summary>
        /// Eliminates duplicates from the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="comparer">The comparer to decide whether items match</param>
        /// <returns>A notifying collection with no duplicates</returns>
        /// <remarks>This method destroys the original order of the items</remarks>
        public static IEnumerableExpression<TSource> Distinct<TSource>(this IEnumerableExpression<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return new DistinctExpression<TSource>(source, comparer);
        }

        /// <summary>
        /// Returns the given source collection without the elements from the second collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="exceptions">The exceptions. Can be a static collection, but in that case must not change</param>
        /// <returns>The source collection without the exceptions</returns>
        /// <remarks>If the exceptions collection will ever change, it must implement <see cref="ICollectionExpression"/>, otherwise the implementation will get corrupted.</remarks>
        public static IEnumerableExpression<TSource> Except<TSource>(this IEnumerableExpression<TSource> source, IEnumerable<TSource> exceptions)
        {
            return Except(source, exceptions, null);
        }

        /// <summary>
        /// Returns the given source collection without the elements from the second collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="comparer">A comparer to decide whether two items match</param>
        /// <param name="exceptions">The exceptions. Can be a static collection, but in that case must not change</param>
        /// <returns>The source collection without the exceptions</returns>
        /// <remarks>If the exceptions collection will ever change, it must implement <see cref="ICollectionExpression"/>, otherwise the implementation will get corrupted.</remarks>
        public static IEnumerableExpression<TSource> Except<TSource>(this IEnumerableExpression<TSource> source, IEnumerable<TSource> exceptions, IEqualityComparer<TSource> comparer)
        {
            return new ExceptExpression<TSource>(source, exceptions, comparer);
        }

        /// <summary>
        /// Gets the first item of the given source collection or the item type default value, if the collection is empty
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <returns>The first item of the collection or the type default value, if the collection is empty</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableFirstOrDefault<>), "CreateExpression")]
        [SetExpressionRewriter(typeof(ObservableFirstOrDefault<>), "CreateSetExpression")]
        public static TSource FirstOrDefault<TSource>(this IEnumerableExpression<TSource> source)
        {
            return SL.FirstOrDefault(source);
        }

        /// <summary>
        /// Gets the first item of the given source collection that matches the given predicate or the item type default value, if the collection is empty or no item matches the given predicate
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The filter predicate</param>
        /// <returns>The first item of the collection that matches the predicate or the type default value</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableFirstOrDefault<>), "CreateExpressionForPredicate")]
        [SetExpressionRewriter(typeof(ObservableFirstOrDefault<>), "CreateSetExpressionWithPredicate")]
        public static TSource FirstOrDefault<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.FirstOrDefault(source, predicate.Compile());
        }

        /// <summary>
        /// Groups the given collection by the given predicate
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <typeparam name="TKey">The type of keys used for grouping</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">The predicate expression selecting the keys for grouping</param>
        /// <returns>A collection of groups</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IEnumerableExpression<IGroupingExpression<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return GroupBy<TSource, TKey>(source, keySelector, null);
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
        [ParameterDataflow(1, 0, 0)]
        public static IEnumerableExpression<IGroupingExpression<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new GroupByExpression<TSource, TKey>(source, keySelector, null, comparer);
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
        [ParameterDataflow(1, 0, 0)]
        [ParameterDataflow(2, 0, 1)]
        [ParameterDataflow(2, 1, 0)]
        public static IEnumerableExpression<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector)
        {
            return GroupBy<TSource, TKey, TResult>(source, keySelector, resultSelector, null);
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
        /// <param name="comparer">A comparer that decides whether items are identical</param>
        /// <returns>A collection of groups</returns>
        [ParameterDataflow(1, 0, 0)]
        [ParameterDataflow(2, 0, 1)]
        [ParameterDataflow(2, 1, 0)]
        public static IEnumerableExpression<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            var newP = Expression.Parameter(typeof(INotifyGrouping<TKey, TSource>));
            var dict = new Dictionary<ParameterExpression, Expression>();
            dict.Add(resultSelector.Parameters[0], newP);
            dict.Add(resultSelector.Parameters[1], Expression.Property(newP, "Key"));
            var visitor = new ReplaceParametersVisitor(dict);
            var lambda = Expression.Lambda<Func<IGrouping<TKey, TSource>, TResult>>(visitor.Visit(resultSelector.Body), newP);
            return GroupBy<TSource, TKey>(source, keySelector, comparer).Select(lambda);
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
        [ParameterDataflow(2, 0, 0)]
        [ParameterDataflow(3, 0, 1)]
        [ParameterDataflow(4, 0, 0)]
        [ParameterDataflow(4, 1, 1)]
        public static IEnumerableExpression<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerableExpression<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
        {
            return GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, null);
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
        [ParameterDataflow(2, 0, 0)]
        [ParameterDataflow(3, 0, 1)]
        [ParameterDataflow(4, 0, 0)]
        [ParameterDataflow(4, 1, 1)]
        public static IEnumerableExpression<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerableExpression<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return new GroupJoinExpression<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, null, innerKeySelector, null, resultSelector, null, comparer);
        }

        /// <summary>
        /// Intersects two collections
        /// </summary>
        /// <typeparam name="TSource">The element type of the collections</typeparam>
        /// <param name="source">The first collection</param>
        /// <param name="source2">The second collection</param>
        /// <returns>The intersection of both collections</returns>
        /// <remarks>No deduplication is done</remarks>
        public static IEnumerableExpression<TSource> Intersect<TSource>(this IEnumerableExpression<TSource> source, IEnumerable<TSource> source2)
        {
            return Intersect(source, source2, null);
        }

        /// <summary>
        /// Intersects two collections
        /// </summary>
        /// <typeparam name="TSource">The element type of the collections</typeparam>
        /// <param name="source">The first collection</param>
        /// <param name="source2">The second collection</param>
        /// <param name="comparer">A comparer to decide when two items are equal</param>
        /// <returns>The intersection of both collections</returns>
        /// <remarks>No deduplication is done</remarks>
        public static IEnumerableExpression<TSource> Intersect<TSource>(this IEnumerableExpression<TSource> source, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
        {
            return new IntersectExpression<TSource>(source, source2, comparer);
        }

        /// <summary>
        /// Gets a value indicating whether the given collection is a proper subset of the current collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="other">The collection that is compared to</param>
        /// <returns>True, if all elements of the current collection are contained in the given collection but not inverse, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableProperSubsetOf<>), "CreateExpression")]
        public static bool IsProperSubsetOf<T>(this IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return IsProperSubsetOf<T>(source, other, null);
        }

        /// <summary>
        /// Gets a value indicating whether the given collection is a proper subset of the current collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="other">The collection that is compared to</param>
        /// <param name="comparer">The comparer to define equality</param>
        /// <returns>True, if all elements of the current collection are contained in the given collection but not inverse, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableProperSubsetOf<>), "CreateExpressionWithComparer")]
        public static bool IsProperSubsetOf<T>(this IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            var set = new HashSet<T>(source, comparer);
            return set.IsProperSubsetOf(other);
        }

        /// <summary>
        /// Gets a value indicating whether the given collection is a proper superset of the current collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="other">The collection that is compared to</param>
        /// <returns>True, if all elements of the given collection are contained in the current collection but not inverse, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableProperSupersetOf<>), "CreateExpression")]
        public static bool IsProperSupersetOf<T>(this IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return IsProperSupersetOf<T>(source, other, null);
        }

        /// <summary>
        /// Gets a value indicating whether the given collection is a proper superset of the current collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="other">The collection that is compared to</param>
        /// <param name="comparer">The comparer to define equality</param>
        /// <returns>True, if all elements of the given collection are contained in the current collection but not inverse, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableProperSupersetOf<>), "CreateExpressionWithComparer")]
        public static bool IsProperSupersetOf<T>(this IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            var set = new HashSet<T>(source, comparer);
            return set.IsProperSupersetOf(other);
        }

        /// <summary>
        /// Gets a value indicating whether the given collection is a proper subset of the current collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="other">The collection that is compared to</param>
        /// <returns>True, if all elements of the current collection are contained in the given collection, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSubsetOf<>), "CreateExpression")]
        public static bool IsSubsetOf<T>(this IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return IsSubsetOf<T>(source, other, null);
        }

        /// <summary>
        /// Gets a value indicating whether the given collection is a proper subset of the current collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="other">The collection that is compared to</param>
        /// <param name="comparer">The comparer to define equality</param>
        /// <returns>True, if all elements of the current collection are contained in the given collection, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSubsetOf<>), "CreateExpressionWithComparer")]
        public static bool IsSubsetOf<T>(this IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            var set = new HashSet<T>(source, comparer);
            return set.IsSubsetOf(other);
        }

        /// <summary>
        /// Gets a value indicating whether the given collection is a proper superset of the current collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="other">The collection that is compared to</param>
        /// <returns>True, if all elements of the given collection are contained in the current collection, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSupersetOf<>), "CreateExpression")]
        public static bool IsSupersetOf<T>(this IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return IsSupersetOf<T>(source, other, null);
        }

        /// <summary>
        /// Gets a value indicating whether the given collection is a proper superset of the current collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="other">The collection that is compared to</param>
        /// <param name="comparer">The comparer to define equality</param>
        /// <returns>True, if all elements of the given collection are contained in the current collection, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSupersetOf<>), "CreateExpressionWithComparer")]
        public static bool IsSupersetOf<T>(this IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            var set = new HashSet<T>(source, comparer);
            return set.IsSupersetOf(other);
        }

        /// <summary>
        /// Joins the current collection with the given other collection
        /// </summary>
        /// <typeparam name="TOuter">The element type of the current collection</typeparam>
        /// <typeparam name="TInner">The element type of the other collection</typeparam>
        /// <typeparam name="TKey">The type of the keys for which the collections should be joined</typeparam>
        /// <typeparam name="TResult">The join result type</typeparam>
        /// <param name="outerSource">The current collection</param>
        /// <param name="innerSource">The other collection to join with</param>
        /// <param name="outerKeySelector">A lambda expression to select the key for the current collections items</param>
        /// <param name="innerKeySelector">A lambda expression to select the key for the given other collections items</param>
        /// <param name="resultSelector">A lambda expression to select the result for a given pair of elements</param>
        /// <returns>A joined collection</returns>
        [ParameterDataflow(2, 0, 0)]
        [ParameterDataflow(3, 0, 1)]
        [ParameterDataflow(4, 0, 0)]
        [ParameterDataflow(4, 1, 1)]
        public static IEnumerableExpression<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerableExpression<TOuter> outerSource, IEnumerable<TInner> innerSource, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return Join(outerSource, innerSource, outerKeySelector, innerKeySelector, resultSelector, null);
        }

        /// <summary>
        /// Joins the current collection with the given other collection
        /// </summary>
        /// <typeparam name="TOuter">The element type of the current collection</typeparam>
        /// <typeparam name="TInner">The element type of the other collection</typeparam>
        /// <typeparam name="TKey">The type of the keys for which the collections should be joined</typeparam>
        /// <typeparam name="TResult">The join result type</typeparam>
        /// <param name="outerSource">The current collection</param>
        /// <param name="innerSource">The other collection to join with</param>
        /// <param name="outerKeySelector">A lambda expression to select the key for the current collections items</param>
        /// <param name="innerKeySelector">A lambda expression to select the key for the given other collections items</param>
        /// <param name="resultSelector">A lambda expression to select the result for a given pair of elements</param>
        /// <param name="comparer">An equality comparer to define when two keys are equivalent</param>
        /// <returns>A joined collection</returns>
        [ParameterDataflow(2, 0, 0)]
        [ParameterDataflow(3, 0, 1)]
        [ParameterDataflow(4, 0, 0)]
        [ParameterDataflow(4, 1, 1)]
        public static IEnumerableExpression<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerableExpression<TOuter> outerSource, IEnumerable<TInner> innerSource, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return new JoinExpression<TOuter, TInner, TKey, TResult>(outerSource, innerSource, outerKeySelector, null, innerKeySelector, null, resultSelector, null, comparer);
        }

        /// <summary>
        /// Gets the maximum element of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <param name="source">The collection</param>
        /// <returns>An element which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "MaxExpression")]
        public static TSource Max<TSource>(this IEnumerableExpression<TSource> source)
            where TSource : IComparable<TSource>
        {
            return SL.Max(source);
        }

        /// <summary>
        /// Gets the maximum element of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An element which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "MaxExpressionWithComparer")]
        public static TSource Max<TSource>(this IEnumerableExpression<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");

            comparer = comparer ?? Comparer<TSource>.Default;
            using (var en = source.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    var max = en.Current;
                    while (en.MoveNext())
                    {
                        if (comparer.Compare(en.Current, max) > 0)
                        {
                            max = en.Current;
                        }
                    }
                    return max;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets the maximum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <returns>An elements feature which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableComparisons), "LambdaMaxExpression")]
        [ExpressionCompileRewriter(typeof(Rewrites), "LambdaMaxRewrite")]
        public static TResult Max<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector)
            where TResult : IComparable<TResult>
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return SL.Max(source, selector.Compile());
        }

        /// <summary>
        /// Gets the maximum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="selectorCompiled">A compiled version of the selector</param>
        /// <param name="source">The collection</param>
        /// <returns>An elements feature which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableComparisons), "LambdaMaxExpressionCompiled")]
        public static TResult Max<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, Func<TSource, TResult> selectorCompiled)
            where TResult : IComparable<TResult>
        {
            if (selectorCompiled == null) throw new ArgumentNullException("selector");

            return SL.Max(source, selectorCompiled);
        }

        /// <summary>
        /// Gets the maximum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An elements feature which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableComparisons), "LambdaMaxExpressionWithComparer")]
        public static TResult Max<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, IComparer<TResult> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            comparer = comparer ?? Comparer<TResult>.Default;
            var member = selector.Compile();
            using (var en = source.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    var max = member(en.Current);
                    while (en.MoveNext())
                    {
                        var current = member(en.Current);
                        if (comparer.Compare(current, max) > 0)
                        {
                            max = current;
                        }
                    }
                    return max;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets the maximum element of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <param name="source">The collection</param>
        /// <returns>An element which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "NullableMaxExpression")]
        public static TSource? Max<TSource>(this IEnumerableExpression<TSource?> source)
            where TSource : struct, IComparable<TSource>
        {
            return SL.Max(source);
        }

        /// <summary>
        /// Gets the maximum element of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An element which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "NullableMaxExpressionWithComparer")]
        public static TSource? Max<TSource>(this IEnumerableExpression<TSource?> source, IComparer<TSource> comparer)
            where TSource : struct
        {
            if (source == null) throw new ArgumentNullException("source");

            comparer = comparer ?? Comparer<TSource>.Default;
            using (var en = source.GetEnumerator())
            {
                do
                {
                    if (!en.MoveNext()) return null;
                } while (!en.Current.HasValue);
                var max = en.Current.Value;
                while (en.MoveNext())
                {
                    if (en.Current.HasValue)
                    {
                        if (comparer.Compare(en.Current.Value, max) > 0)
                        {
                            max = en.Current.Value;
                        }
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// Gets the maximum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <returns>An elements feature which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableComparisons), "LambdaNullableMaxExpression")]
        public static TResult? Max<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult?>> selector)
            where TResult : struct, IComparable<TResult>
        {
            return Max(source, selector, Comparer<TResult>.Default);
        }

        /// <summary>
        /// Gets the maximum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An elements feature which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableComparisons), "LambdaNullableMaxExpressionWithComparer")]
        public static TResult? Max<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult?>> selector, IComparer<TResult> comparer)
            where TResult : struct
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            comparer = comparer ?? Comparer<TResult>.Default;
            var member = selector.Compile();
            using (var en = source.GetEnumerator())
            {
                TResult? last;
                do
                {
                    if (!en.MoveNext()) return null;
                    last = member(en.Current);
                } while (!last.HasValue);
                var max = last.Value;
                while (en.MoveNext())
                {
                    last = member(en.Current);
                    if (last.HasValue)
                    {
                        if (comparer.Compare(last.Value, max) > 0)
                        {
                            max = last.Value;
                        }
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// Gets the minimum element of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <param name="source">The collection</param>
        /// <returns>An element which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "MinExpression")]
        public static TSource Min<TSource>(this IEnumerableExpression<TSource> source)
            where TSource : IComparable<TSource>
        {
            return SL.Min(source);
        }

        /// <summary>
        /// Gets the minimum element of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An element which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "MinExpressionWithComparer")]
        public static TSource Min<TSource>(this IEnumerableExpression<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");

            comparer = comparer ?? Comparer<TSource>.Default;
            using (var en = source.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    var min = en.Current;
                    while (en.MoveNext())
                    {
                        if (comparer.Compare(en.Current, min) < 0)
                        {
                            min = en.Current;
                        }
                    }
                    return min;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets the minimum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <returns>An elements feature which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableComparisons), "LambdaMinExpression")]
        public static TResult Min<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector)
            where TResult : IComparable<TResult>
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Min(source, selector.Compile());
        }

        /// <summary>
        /// Gets the minimum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An elements feature which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableComparisons), "LambdaMinExpressionWithComparer")]
        public static TResult Min<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, IComparer<TResult> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            comparer = comparer ?? Comparer<TResult>.Default;
            var member = selector.Compile();
            using (var en = source.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    var min = member(en.Current);
                    while (en.MoveNext())
                    {
                        var current = member(en.Current);
                        if (comparer.Compare(current, min) < 0)
                        {
                            min = current;
                        }
                    }
                    return min;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets the minimum element of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <param name="source">The collection</param>
        /// <returns>An element which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "NullableMinExpression")]
        public static TSource? Min<TSource>(this IEnumerableExpression<TSource?> source)
            where TSource : struct, IComparable<TSource>
        {
            return SL.Min(source);
        }

        /// <summary>
        /// Gets the minimum element of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An element which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "NullableMinExpressionWithComparer")]
        public static TSource? Min<TSource>(this IEnumerableExpression<TSource?> source, IComparer<TSource> comparer)
            where TSource : struct
        {
            if (source == null) throw new ArgumentNullException("source");

            comparer = comparer ?? Comparer<TSource>.Default;
            using (var en = source.GetEnumerator())
            {
                do
                {
                    if (!en.MoveNext()) return null;
                } while (!en.Current.HasValue);
                var min = en.Current.Value;
                while (en.MoveNext())
                {
                    if (en.Current.HasValue)
                    {
                        if (comparer.Compare(en.Current.Value, min) < 0)
                        {
                            min = en.Current.Value;
                        }
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// Gets the minimum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <returns>An elements feature which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableComparisons), "LambdaNullableMinExpression")]
        public static TResult? Min<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult?>> selector)
            where TResult : struct, IComparable<TResult>
        {
            return Min(source, selector, Comparer<TResult>.Default);
        }

        /// <summary>
        /// Gets the minimum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An elements feature which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableComparisons), "LambdaNullableMinExpressionWithComparer")]
        public static TResult? Min<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult?>> selector, IComparer<TResult> comparer)
            where TResult : struct
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            comparer = comparer ?? Comparer<TResult>.Default;
            var member = selector.Compile();
            using (var en = source.GetEnumerator())
            {
                TResult? last;
                do
                {
                    if (!en.MoveNext()) return null;
                    last = member(en.Current);
                } while (!last.HasValue);
                var min = last.Value;
                while (en.MoveNext())
                {
                    last = member(en.Current);
                    if (last.HasValue)
                    {
                        if (comparer.Compare(last.Value, min) < 0)
                        {
                            min = last.Value;
                        }
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// Filters the given collection for elements of the given type
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="source">The current collection</param>
        /// <returns>A collection containing the elements of the given type</returns>
        public static IEnumerableExpression<TResult> OfType<TResult>(this IEnumerableExpression source)
        {
            return new OfTypeExpression<TResult>(source);
        }


        /// <summary>
        /// Filters the given collection for elements of the given type
        /// </summary>
        /// <typeparam name="TSource">The type of the original collection</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="source">The current collection</param>
        /// <returns>A collection containing the elements of the given type</returns>
        public static ICollectionExpression<TResult> OfType<TSource, TResult>(this ICollectionExpression<TSource> source)
            where TResult : TSource
        {
            return new OfTypeCollectionExpression<TSource, TResult>(source);
        }

        /// <summary>
        /// Filters the given collection for elements of the given type as a collection using the non-generic interface
        /// </summary>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="source">The current collection</param>
        /// <returns>A collection containing the elements of the given type</returns>
        /// <remarks>Only use this method if a generic collection interface is for some reason unavailable.</remarks>
        public static ICollectionExpression<TResult> OfType<TResult>(this ICollectionExpression source)
        {
            return new OfTypeCollectionExpression<TResult>(source);
        }

        /// <summary>
        /// Orders the given collection ascending by the given predicate
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The type of the keys used for ordering</typeparam>
        /// <param name="source">The collection that should be sorted</param>
        /// <param name="keySelector">A lambda expression selecting the sorting keys for the given collection</param>
        /// <returns>A collection with the elements contained in the current collection sorted by the given predicate</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IOrderableEnumerableExpression<TItem> OrderBy<TItem, TKey>(this IEnumerableExpression<TItem> source, Expression<Func<TItem, TKey>> keySelector)
        {
            return OrderBy(source, keySelector, null);
        }

        /// <summary>
        /// Orders the given collection ascending by the given predicate
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The type of the keys used for ordering</typeparam>
        /// <param name="source">The collection that should be sorted</param>
        /// <param name="keySelector">A lambda expression selecting the sorting keys for the given collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>A collection with the elements contained in the current collection sorted by the given predicate</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IOrderableEnumerableExpression<TItem> OrderBy<TItem, TKey>(this IEnumerableExpression<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return new OrderByExpression<TItem, TKey>(source, keySelector, null, comparer);
        }

        /// <summary>
        /// Orders the given collection descending by the given predicate
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The type of the keys used for ordering</typeparam>
        /// <param name="source">The collection that should be sorted</param>
        /// <param name="keySelector">A lambda expression selecting the sorting keys for the given collection</param>
        /// <returns>A collection with the elements contained in the current collection sorted by the given predicate</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IOrderableEnumerableExpression<TItem> OrderByDescending<TItem, TKey>(this IEnumerableExpression<TItem> source, Expression<Func<TItem, TKey>> keySelector)
        {
            return OrderByDescending(source, keySelector, null);
        }

        /// <summary>
        /// Orders the given collection descending by the given predicate
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The type of the keys used for ordering</typeparam>
        /// <param name="source">The collection that should be sorted</param>
        /// <param name="keySelector">A lambda expression selecting the sorting keys for the given collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>A collection with the elements contained in the current collection sorted by the given predicate</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IOrderableEnumerableExpression<TItem> OrderByDescending<TItem, TKey>(this IEnumerableExpression<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return new OrderByExpression<TItem, TKey>(source, keySelector, null, new ReverseComparer<TKey>(comparer));
        }

        /// <summary>
        /// Maps the current collection to the given lambda expression
        /// </summary>
        /// <typeparam name="TSource">The elements type</typeparam>
        /// <typeparam name="TResult">The result element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="selector">A lambda expression representing the mapping result for a given item</param>
        /// <returns>A collection with the mapping results</returns>
        [ParameterDataflow(1, 0, 0)]
        [ExpressionCompileRewriter(typeof(Rewrites), "RewriteSelect")]
        public static IEnumerableExpression<TResult> Select<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return new SelectExpression<TSource, TResult>(source, selector, null);
        }

        /// <summary>
        /// Maps the current collection to the given lambda expression
        /// </summary>
        /// <typeparam name="TSource">The elements type</typeparam>
        /// <typeparam name="TResult">The result element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="selector">A lambda expression representing the mapping result for a given item</param>
        /// <param name="selectorCompiled">A compiled version of the selector</param>
        /// <returns>A collection with the mapping results</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IEnumerableExpression<TResult> Select<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, Func<TSource, TResult> selectorCompiled)
        {
            return new SelectExpression<TSource, TResult>(source, selector, selectorCompiled);
        }

        /// <summary>
        /// Flattens the given collection of collections where the subsequent collections are selected by a predicate
        /// </summary>
        /// <typeparam name="TSource">The source element type</typeparam>
        /// <typeparam name="TIntermediate">The element type of the subsequent collection</typeparam>
        /// <typeparam name="TResult">The result element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="func">A lambda expression to select subsequent collections</param>
        /// <param name="selector">A lambda expression that determines the result element given the element of the source collection and the element of the subsequent collection</param>
        /// <returns>A collection with the results</returns>
        [ParameterDataflow(1, 0, 0)]
        [ParameterDataflow(2, 0, 0)]
        [ParameterDataflow(2, 1, 1)]
        public static IEnumerableExpression<TResult> SelectMany<TSource, TIntermediate, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, IEnumerable<TIntermediate>>> func, Expression<Func<TSource, TIntermediate, TResult>> selector)
        {
            return new SelectManyExpression<TSource, TIntermediate, TResult>(source, func, null, selector, null);
        }

        /// <summary>
        /// Flattens the given collection of collections where the subsequent collections are selected by a predicate
        /// </summary>
        /// <typeparam name="TSource">The source element type</typeparam>
        /// <typeparam name="TResult">The result element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="selector">A lambda expression to select subsequent collections</param>
        /// <returns>A collection with the results</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IEnumerableExpression<TResult> SelectMany<TSource, TResult>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
        {
            return new SimpleSelectManyExpression<TSource, TResult>(source, selector, null);
        }

        /// <summary>
        /// Gets a value indicating whether the current collection and the given collection contain the same set of elements, regardless of their order
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="other">The given other collection</param>
        /// <returns>True, if both collections contain the same set of elements, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSetEquals<>), "CreateExpression")]
        public static bool SetEquals<T>(this IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return SetEquals(source, other, null);
        }

        /// <summary>
        /// Gets a value indicating whether the current collection and the given collection contain the same set of elements, regardless of their order
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="other">The given other collection</param>
        /// <param name="comparer">An equality comparer used to determine equality in the sets</param>
        /// <returns>True, if both collections contain the same set of elements, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSetEquals<>), "CreateExpressionWithComparer")]
        public static bool SetEquals<T>(this IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (other == null) throw new ArgumentNullException("other");

            var set = new HashSet<T>(source, comparer);
            return set.SetEquals(other);
        }



        /// <summary>
        /// Gets the single item of the given source collection or the item type default value, if the collection is empty
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <returns>The single item of the collection or the type default value, if the collection is empty</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSingleOrDefault<>), "CreateExpression")]
        [SetExpressionRewriter(typeof(ObservableSingleOrDefault<>), "CreateSetExpression")]
        public static TSource SingleOrDefault<TSource>(this IEnumerableExpression<TSource> source)
        {
            return SL.SingleOrDefault(source);
        }

        /// <summary>
        /// Gets the first item of the given source collection that matches the given predicate or the item type default value, if the collection is empty or no item matches the given predicate
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The filter predicate</param>
        /// <returns>The first item of the collection that matches the predicate or the type default value</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSingleOrDefault<>), "CreateExpressionForPredicate")]
        [SetExpressionRewriter(typeof(ObservableSingleOrDefault<>), "CreateSetExpressionWithPredicate")]
        [ExpressionCompileRewriter(typeof(Rewrites), "RewriteSingleOrDefault")]
        public static TSource SingleOrDefault<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.SingleOrDefault(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the first item of the given source collection that matches the given predicate or the item type default value, if the collection is empty or no item matches the given predicate
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">The filter predicate</param>
        /// <param name="predicateCompiled">The filter predicate precompiled</param>
        /// <returns>The first item of the collection that matches the predicate or the type default value</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSingleOrDefault<>), "CreateExpressionForPredicate2")]
        [SetExpressionRewriter(typeof(ObservableSingleOrDefault<>), "CreateSetExpressionWithPredicate2")]
        public static TSource SingleOrDefault<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> predicate, Func<TSource, bool> predicateCompiled)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            if (predicateCompiled == null) predicateCompiled = predicate.Compile();
            return SL.SingleOrDefault(source, predicateCompiled);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumIntExpression")]
        public static int Sum(this IEnumerableExpression<int> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumLongExpression")]
        public static long Sum(this IEnumerableExpression<long> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumFloatExpression")]
        public static float Sum(this IEnumerableExpression<float> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumDoubleExpression")]
        public static double Sum(this IEnumerableExpression<double> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumDecimalExpression")]
        public static decimal Sum(this IEnumerableExpression<decimal> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableIntExpression")]
        public static int? Sum(this IEnumerableExpression<int?> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableLongExpression")]
        public static long? Sum(this IEnumerableExpression<long?> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableFloatExpression")]
        public static float? Sum(this IEnumerableExpression<float?> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableDoubleExpression")]
        public static double? Sum(this IEnumerableExpression<double?> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableDecimalExpression")]
        public static decimal? Sum(this IEnumerableExpression<decimal?> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaIntExpression")]
        public static int Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, int>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaLongExpression")]
        public static long Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, long>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaFloatExpression")]
        public static float Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, float>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaDoubleExpression")]
        public static double Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, double>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaDecimalExpression")]
        public static decimal Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableIntExpression")]
        public static int? Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableLongExpression")]
        public static long? Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableFloatExpression")]
        public static float? Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, float?>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableDoubleExpression")]
        public static double? Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collections features
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <param name="selector">A lambda expression to represent the feature to be summed up</param>
        /// <returns>The sum of the numbers contained in this collection elements features</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ParameterDataflow(1, 0, 0)]
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableDecimalExpression")]
        public static decimal? Sum<TSource>(this IEnumerableExpression<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Sum(source, selector.Compile());
        }

        /// <summary>
        /// Orders the given orderable collection by the given predicate ascending
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The ordering key type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="keySelector">A lambda expression to select the features used for ordering</param>
        /// <returns>A collection with the elements of the current collection but ordered in lower priority for the given predicate</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IOrderableEnumerableExpression<TItem> ThenBy<TItem, TKey>(this IOrderableEnumerableExpression<TItem> source, Expression<Func<TItem, TKey>> keySelector)
        {
            return ThenBy(source, keySelector, null);
        }

        /// <summary>
        /// Orders the given orderable collection by the given predicate ascending
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The ordering key type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="keySelector">A lambda expression to select the features used for ordering</param>
        /// <param name="comparer">A comparer to determine comparison</param>
        /// <returns>A collection with the elements of the current collection but ordered in lower priority for the given predicate</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IOrderableEnumerableExpression<TItem> ThenBy<TItem, TKey>(this IOrderableEnumerableExpression<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return new ThenByExpression<TItem, TKey>(source, keySelector, null, comparer);
        }

        /// <summary>
        /// Orders the given orderable collection by the given predicate descending
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The ordering key type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="keySelector">A lambda expression to select the features used for ordering</param>
        /// <returns>A collection with the elements of the current collection but ordered in lower priority for the given predicate</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IOrderableEnumerableExpression<TItem> ThenByDescending<TItem, TKey>(this IOrderableEnumerableExpression<TItem> source, Expression<Func<TItem, TKey>> keySelector)
        {
            return ThenByDescending(source, keySelector, null);
        }

        /// <summary>
        /// Orders the given orderable collection by the given predicate descending
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The ordering key type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="keySelector">A lambda expression to select the features used for ordering</param>
        /// <param name="comparer">A comparer to determine comparison</param>
        /// <returns>A collection with the elements of the current collection but ordered in lower priority for the given predicate</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IOrderableEnumerableExpression<TItem> ThenByDescending<TItem, TKey>(this IOrderableEnumerableExpression<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return new ThenByExpression<TItem, TKey>(source, keySelector, null, new ReverseComparer<TKey>(comparer));
        }

        /// <summary>
        /// Gets the top x elements of the given collection, ordered by the given feature
        /// </summary>
        /// <typeparam name="TItem">The item type</typeparam>
        /// <typeparam name="TKey">The key type</typeparam>
        /// <param name="source">The element source</param>
        /// <param name="x">A number indicating how many items should be selected</param>
        /// <param name="keySelector">An expression to denote the selection of key features</param>
        /// <returns>An array with the largest entries of the underlying collection</returns>
        [ObservableProxy(typeof(ObservableTopX<,>), "CreateExpressionSelector")]
        public static KeyValuePair<TItem, TKey>[] TopX<TItem, TKey>(this IEnumerableExpression<TItem> source, int x, Expression<Func<TItem, TKey>> keySelector)
        {
            return TopX(source, x, keySelector, null);
        }

        /// <summary>
        /// Gets the top x elements of the given collection, ordered by the given feature
        /// </summary>
        /// <typeparam name="TItem">The item type</typeparam>
        /// <typeparam name="TKey">The key type</typeparam>
        /// <param name="source">The element source</param>
        /// <param name="x">A number indicating how many items should be selected</param>
        /// <param name="keySelector">An expression to denote the selection of key features</param>
        /// <param name="comparer">A custom comparer</param>
        /// <returns>An array with the largest entries of the underlying collection</returns>
        [ObservableProxy(typeof(ObservableTopX<,>), "CreateExpressionSelectorComparer")]
        public static KeyValuePair<TItem, TKey>[] TopX<TItem, TKey>(this IEnumerableExpression<TItem> source, int x, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (x <= 0) throw new ArgumentOutOfRangeException(nameof(x));

            comparer = comparer ?? Comparer<TKey>.Default;
            var selector = keySelector.Compile();
            var result = new List<KeyValuePair<TItem, TKey>>(x + 1);
            foreach (var item in source)
            {
                var key = selector(item);
                if (result.Count < x || comparer.Compare(key, result[result.Count - 1].Value) > 0)
                {
                    var inserted = false;
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (comparer.Compare(key, result[i].Value) > 0)
                        {
                            inserted = true;
                            result.Insert(i, new KeyValuePair<TItem, TKey>(item, key));
                            if (result.Count == x + 1) result.RemoveAt(x);
                            break;
                        }
                    }
                    if (!inserted) result.Add(new KeyValuePair<TItem, TKey>(item, key));
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Unions the current collection with the given other collection 
        /// </summary>
        /// <typeparam name="TSource">The elements type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="source2">The given other collection</param>
        /// <returns>A collection containing the union of both collections</returns>
        public static IEnumerableExpression<TSource> Union<TSource>(this IEnumerableExpression<TSource> source, IEnumerable<TSource> source2)
        {
            return Distinct(Concat(source, source2));
        }

        /// <summary>
        /// Unions the current collection with the given other collection 
        /// </summary>
        /// <typeparam name="TSource">The elements type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="source2">The given other collection</param>
        /// <param name="comparer">A comparer to determine equality</param>
        /// <returns>A collection containing the union of both collections</returns>
        public static IEnumerableExpression<TSource> Union<TSource>(this IEnumerableExpression<TSource> source, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
        {
            return Distinct(Concat(source, source2), comparer);
        }

        /// <summary>
        /// Filters the given collection with the given predicate
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="filter">The predicate used for filtering</param>
        /// <returns>A collection containing the elements that passed the filter</returns>
        [ParameterDataflow(1, 0, 0)]
        [ExpressionCompileRewriter(typeof(Rewrites), "RewriteWhereEnumerable")]
        public static IEnumerableExpression<T> Where<T>(this IEnumerableExpression<T> source, Expression<Func<T, bool>> filter)
        {
            return new WhereExpression<T>(source, filter, null);
        }

        /// <summary>
        /// Filters the given collection with the given predicate
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="filter">The predicate used for filtering</param>
        /// <param name="filterCompiled">The filter predicate precompiled</param>
        /// <returns>A collection containing the elements that passed the filter</returns>
        [ParameterDataflow(1, 0, 0)]
        public static IEnumerableExpression<T> Where<T>(this IEnumerableExpression<T> source, Expression<Func<T, bool>> filter, Func<T, bool> filterCompiled)
        {
            return new WhereExpression<T>(source, filter, filterCompiled);
        }

        /// <summary>
        /// Filters the given collection with the given predicate
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="filter">The predicate used for filtering</param>
        /// <returns>A collection containing the elements that passed the filter</returns>
        [ParameterDataflow(1, 0, 0)]
        [ExpressionCompileRewriter(typeof(Rewrites), "RewriteWhereCollection")]
        public static ICollectionExpression<T> Where<T>(this ICollectionExpression<T> source, Expression<Func<T, bool>> filter)
        {
            return new WhereCollectionExpression<T>(source, filter, null, null);
        }

        /// <summary>
        /// Filters the given collection with the given predicate
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="filter">The predicate used for filtering</param>
        /// <param name="filterGetter">A precompiled filter getter</param>
        /// <param name="filterSetter">A precompiled filter setter</param>
        /// <returns>A collection containing the elements that passed the filter</returns>
        [ParameterDataflow(1, 0, 0)]
        public static ICollectionExpression<T> Where<T>(this ICollectionExpression<T> source, Expression<Func<T, bool>> filter, Func<T, bool> filterGetter, Action<T, bool> filterSetter)
        {
            return new WhereCollectionExpression<T>(source, filter, filterGetter, filterSetter);
        }

        private class Rewrites
        {
            public static Expression RewriteSingleOrDefault<T>(MethodCallExpression node)
            {
                var singleOrDefault = ReflectionHelper.GetFunc<IEnumerableExpression<T>, Expression<Func<T, bool>>, Func<T, bool>, T>((source, predicate, predicateCompiled) => SingleOrDefault(source, predicate, predicateCompiled));
                return RewriteSinglePredicate(node, singleOrDefault);
            }

            private static Expression RewriteSinglePredicate(MethodCallExpression node, System.Reflection.MethodInfo alternativeMethod)
            {
                var arg1 = node.Arguments[1];
                if (arg1.NodeType == ExpressionType.Quote)
                {
                    var argUnary = arg1 as UnaryExpression;
                    var getter = argUnary.Operand;
                    return Expression.Call(null, alternativeMethod, node.Arguments[0], arg1, getter);
                }
                return node;
            }

            private static Expression RewriteSinglePredicateWithSetter(MethodCallExpression node, System.Reflection.MethodInfo alternativeMethod)
            {
                var arg1 = node.Arguments[1];
                if (arg1.NodeType == ExpressionType.Quote)
                {
                    var argUnary = arg1 as UnaryExpression;
                    var getter = argUnary.Operand;
                    Expression setter = SetExpressionRewriter.CreateSetter(getter as LambdaExpression);
                    if (setter == null) setter = Expression.Constant(null, alternativeMethod.GetParameters()[3].ParameterType);
                    return Expression.Call(null, alternativeMethod, node.Arguments[0], arg1, getter, setter);
                }
                return node;
            }

            public static Expression RewriteWhereEnumerable<T>(MethodCallExpression node)
            {
                var where = ReflectionHelper.GetFunc((IEnumerableExpression<T> source, Expression<Func<T, bool>> predicate, Func<T, bool> predicateGetter) => Where(source, predicate, predicateGetter));
                return RewriteSinglePredicate(node, where);
            }

            public static Expression RewriteWhereCollection<T>(MethodCallExpression node)
            {
                var where = ReflectionHelper.GetFunc((ICollectionExpression<T> source, Expression<Func<T, bool>> predicate, Func<T, bool> predicateGetter, Action<T, bool> predicateSetter) => Where(source, predicate, predicateGetter, predicateSetter));
                return RewriteSinglePredicateWithSetter(node, where);
            }

            public static Expression LambdaMaxRewrite<TSource, TResult>(MethodCallExpression node)
                where TResult : IComparable<TResult>
            {
                var max = ReflectionHelper.GetFunc((IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, Func<TSource, TResult> selectorCompiled) => Max(source, selector, selectorCompiled));
                return RewriteSinglePredicate(node, max);
            }

            public static Expression RewriteSelect<TSource, TResult>(MethodCallExpression node)
            {
                var select = ReflectionHelper.GetFunc((IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, Func<TSource, TResult> selectorCompiled) => Select(source, selector, selectorCompiled));
                return RewriteSinglePredicate(node, select);
            }
        }
    }
}
