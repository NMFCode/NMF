using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Text;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Defines a set of extension methods on the <see cref="INotifyValue{T}">INotifyValue</see> monad
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Gets or sets a global flag indicating whether projections and filters should maintain the element order
        /// </summary>
        public static bool KeepOrder { get; set; }

        /// <summary>
        /// Gets a value indicating whether all items in the given collection match the given predicate
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">A custom predicate that is applied to all items in the collection</param>
        /// <returns>True, if all items in the collection match the given predicate</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableAll<>), "Create")]
        public static bool All<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
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
        [ObservableProxy(typeof(ObservableAny<>), "Create")]
        public static bool Any<TSource>(this INotifyEnumerable<TSource> source)
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
        [ObservableProxy(typeof(ObservableLambdaAny<>), "Create")]
        public static bool Any<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
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
        [ObservableProxy(typeof(ObservableIntAverage), "Create")]
        public static double Average(this INotifyEnumerable<int> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableLongAverage), "Create")]
        public static double Average(this INotifyEnumerable<long> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableFloatAverage), "Create")]
        public static float Average(this INotifyEnumerable<float> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableDoubleAverage), "Create")]
        public static double Average(this INotifyEnumerable<double> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableDecimalAverage), "Create")]
        public static decimal Average(this INotifyEnumerable<decimal> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableIntAverage), "Create")]
        public static double? Average(this INotifyEnumerable<int?> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableLongAverage), "Create")]
        public static double? Average(this INotifyEnumerable<long?> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableFloatAverage), "Create")]
        public static float? Average(this INotifyEnumerable<float?> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableDoubleAverage), "Create")]
        public static double? Average(this INotifyEnumerable<double?> source)
        {
            return SL.Average(source);
        }

        /// <summary>
        /// Gets the average of the given collection of numbers
        /// </summary>
        /// <param name="source">A collection of numbers</param>
        /// <returns>The average of the given collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableNullableDecimalAverage), "Create")]
        public static decimal? Average(this INotifyEnumerable<decimal?> source)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateInt")]
        public static double Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, int>> predicate)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateLong")]
        public static double Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, long>> predicate)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateFloat")]
        public static float Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, float>> predicate)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateDouble")]
        public static double Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, double>> predicate)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateDecimal")]
        public static decimal Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal>> predicate)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableInt")]
        public static double? Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, int?>> predicate)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableLong")]
        public static double? Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, long?>> predicate)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableFloat")]
        public static float? Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, float?>> predicate)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableDouble")]
        public static double? Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, double?>> predicate)
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
        [ObservableProxy(typeof(ObservableAverage), "CreateNullableDecimal")]
        public static decimal? Average<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal?>> predicate)
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
        /// <remarks>If any item in the source collection is not of type <typeparamref name="TResult"/>, an exception is thrown. Consider using <see cref="OfType{TResult}(INotifyEnumerable)"/> in this scenario.</remarks>
        public static INotifyEnumerable<TResult> Cast<TResult>(this INotifyEnumerable source)
        {
            if (source is ObservableEnumerable<TResult> casted)
            {
                return casted;
            }
            var observable = new ObservableCast<TResult>(source);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Concats the given notifying enumerables
        /// </summary>
        /// <typeparam name="TSource">The type of the items</typeparam>
        /// <param name="source">The first source</param>
        /// <param name="source2">The second source</param>
        /// <returns>The concatenation of both sources</returns>
        /// <remarks>The second collection does not have to be a notifying collection, but if it is not, it must not change its contents.</remarks>
        public static INotifyEnumerable<TSource> Concat<TSource>(this INotifyEnumerable<TSource> source, IEnumerable<TSource> source2)
        {
            var observable = new ObservableConcat<TSource>(source, source2);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Searches the given collection for the given item
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="item">The item that needs to be checked</param>
        /// <returns>True, if the given source collection contains the provided item, otherwise false</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableContains<>), "Create")]
        public static bool Contains<TSource>(this INotifyEnumerable<TSource> source, TSource item)
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
        [ObservableProxy(typeof(ObservableContains<>), "CreateWithComparer")]
        public static bool Contains<TSource>(this INotifyEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
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
        [ObservableProxy(typeof(ObservableCount<>), "Create")]
        public static int Count<TSource>(this INotifyEnumerable<TSource> source)
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
        [ObservableProxy(typeof(ObservableCount<>), "CreateWithComparer")]
        public static int Count<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
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
        public static INotifyEnumerable<TSource> Distinct<TSource>(this INotifyEnumerable<TSource> source)
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
        public static INotifyEnumerable<TSource> Distinct<TSource>(this INotifyEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            var observable = new ObservableDistinct<TSource>(source, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Returns the given source collection without the elements from the second collection
        /// </summary>
        /// <typeparam name="TSource">The type of the items</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="exceptions">The exceptions. Can be a static collection, but in that case must not change</param>
        /// <returns>The source collection without the exceptions</returns>
        /// <remarks>If the exceptions collection will ever change, it must implement <see cref="INotifyCollectionChanged"/>, otherwise the implementation will get corrupted.</remarks>
        public static INotifyEnumerable<TSource> Except<TSource>(this INotifyEnumerable<TSource> source, IEnumerable<TSource> exceptions)
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
        /// <remarks>If the exceptions collection will ever change, it must implement <see cref="INotifyCollectionChanged"/>, otherwise the implementation will get corrupted.</remarks>
        public static INotifyEnumerable<TSource> Except<TSource>(this INotifyEnumerable<TSource> source, IEnumerable<TSource> exceptions, IEqualityComparer<TSource> comparer)
        {
            var observable = new ObservableExcept<TSource>(source, exceptions, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Gets the first item of the given source collection or the item type default value, if the collection is empty
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <returns>The first item of the collection or the type default value, if the collection is empty</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableFirstOrDefault<>), "Create")]
        [SetExpressionRewriter(typeof(ObservableFirstOrDefault<>), "CreateSetExpression")]
        public static TSource FirstOrDefault<TSource>(this INotifyEnumerable<TSource> source)
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
        [ObservableProxy(typeof(ObservableFirstOrDefault<>), "CreateForPredicate")]
        [SetExpressionRewriter(typeof(ObservableFirstOrDefault<>), "CreateSetExpressionWithPredicate")]
        public static TSource FirstOrDefault<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
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
        public static INotifyEnumerable<INotifyGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return GroupBy(source, keySelector, null, null);
        }

        /// <summary>
        /// Groups the given collection by the given predicate
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <typeparam name="TKey">The type of keys used for grouping</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">The predicate expression selecting the keys for grouping</param>
        /// <param name="keySelectorCompiled">A compiled version of keySelector</param>
        /// <returns>A collection of groups</returns>
        public static INotifyEnumerable<INotifyGrouping<TKey, TSource>> GroupBy<TSource, TKey>( this INotifyEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Func<TSource, TKey> keySelectorCompiled )
        {
            return GroupBy( source, keySelector, keySelectorCompiled, null );
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
        public static INotifyEnumerable<INotifyGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            return GroupBy( source, keySelector, null, comparer );
        }

        /// <summary>
        /// Groups the given collection by the given predicate
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <typeparam name="TKey">The type of keys used for grouping</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">The predicate expression selecting the keys for grouping</param>
        /// <param name="keySelectorCompiled">A compiled version of keySelector</param>
        /// <param name="comparer">A comparer that decides whether items are identical</param>
        /// <returns>A collection of groups</returns>
        public static INotifyEnumerable<INotifyGrouping<TKey, TSource>> GroupBy<TSource, TKey>( this INotifyEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Func<TSource, TKey> keySelectorCompiled, IEqualityComparer<TKey> comparer )
        {
            var observable = new ObservableGroupBy<TKey, TSource>( source, new ObservingFunc<TSource, TKey>( keySelector, keySelectorCompiled ), comparer );
            observable.Successors.SetDummy();
            return observable;
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
        public static INotifyEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector)
        {
            return GroupBy(source, keySelector, resultSelector, null);
        }

        /// <summary>
        /// Groups the given collection by the given predicate into the given result
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <typeparam name="TKey">The type of keys used for grouping</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">The predicate expression selecting the keys for grouping</param>
        /// <param name="keySelectorCompiled">A compiled version of keySelector</param>
        /// <param name="resultSelector">A function to get the result element for a group</param>
        /// <param name="resultSelectorCompiled">A compiled version of resultSelector</param>
        /// <returns>A collection of groups</returns>
        public static INotifyEnumerable<TResult> GroupBy<TSource, TKey, TResult>( this INotifyEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Func<TSource, TKey> keySelectorCompiled, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelectorCompiled )
        {
            return GroupBy( source, keySelector, keySelectorCompiled, resultSelector, resultSelectorCompiled, null );
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
        public static INotifyEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return GroupBy( source, keySelector, null, resultSelector, null, comparer );
        }

        /// <summary>
        /// Groups the given collection by the given predicate into the given result
        /// </summary>
        /// <typeparam name="TSource">The element type of the source collection</typeparam>
        /// <typeparam name="TKey">The type of keys used for grouping</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="keySelector">The predicate expression selecting the keys for grouping</param>
        /// <param name="keySelectorCompiled">A compiled version of keySelector</param>
        /// <param name="resultSelector">A function to get the result element for a group</param>
        /// <param name="resultSelectorCompiled">A compiled version of resultSelector</param>
        /// <param name="comparer">A comparer that decides whether items are identical</param>
        /// <returns>A collection of groups</returns>
        public static INotifyEnumerable<TResult> GroupBy<TSource, TKey, TResult>( this INotifyEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Func<TSource, TKey> keySelectorCompiled, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelectorCompiled, IEqualityComparer<TKey> comparer )
        {
            var newP = Expression.Parameter( typeof( INotifyGrouping<TKey, TSource> ) );
            var dict = new Dictionary<ParameterExpression, Expression>();
            dict.Add( resultSelector.Parameters[0], newP );
            dict.Add( resultSelector.Parameters[1], Expression.Property( newP, "Key" ) );
            var visitor = new ReplaceParametersVisitor( dict );
            var lambda = Expression.Lambda<Func<INotifyGrouping<TKey, TSource>, TResult>>( visitor.Visit( resultSelector.Body ), newP );
            Func<INotifyGrouping<TKey, TSource>, TResult> lambdaCompiled = null;
            if (resultSelectorCompiled != null)
            {
                lambdaCompiled = g => resultSelectorCompiled( g.Key, g );
            }
            return GroupBy( source, keySelector, keySelectorCompiled, comparer ).Select( lambda, lambdaCompiled );
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
        public static INotifyEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this INotifyEnumerable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
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
        public static INotifyEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this INotifyEnumerable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            var observable = new ObservableGroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Intersects two collections
        /// </summary>
        /// <typeparam name="TSource">The element type of the collections</typeparam>
        /// <param name="source">The first collection</param>
        /// <param name="source2">The second collection</param>
        /// <returns>The intersection of both collections</returns>
        /// <remarks>No deduplication is done</remarks>
        public static INotifyEnumerable<TSource> Intersect<TSource>(this INotifyEnumerable<TSource> source, IEnumerable<TSource> source2)
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
        public static INotifyEnumerable<TSource> Intersect<TSource>(this INotifyEnumerable<TSource> source, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
        {
            var observable = new ObservableIntersect<TSource>(source, source2, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Gets a value indicating whether the given collection is a proper subset of the current collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="other">The collection that is compared to</param>
        /// <returns>True, if all elements of the current collection are contained in the given collection but not inverse, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableProperSubsetOf<>), "Create")]
        public static bool IsProperSubsetOf<T>(this INotifyEnumerable<T> source, IEnumerable<T> other)
        {
            return IsProperSubsetOf(source, other, null);
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
        [ObservableProxy(typeof(ObservableProperSubsetOf<>), "CreateWithComparer")]
        public static bool IsProperSubsetOf<T>(this INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
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
        [ObservableProxy(typeof(ObservableProperSupersetOf<>), "Create")]
        public static bool IsProperSupersetOf<T>(this INotifyEnumerable<T> source, IEnumerable<T> other)
        {
            return IsProperSupersetOf(source, other, null);
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
        [ObservableProxy(typeof(ObservableProperSupersetOf<>), "CreateWithComparer")]
        public static bool IsProperSupersetOf<T>(this INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
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
        [ObservableProxy(typeof(ObservableSubsetOf<>), "Create")]
        public static bool IsSubsetOf<T>(this INotifyEnumerable<T> source, IEnumerable<T> other)
        {
            return IsSubsetOf(source, other, null);
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
        [ObservableProxy(typeof(ObservableSubsetOf<>), "CreateWithComparer")]
        public static bool IsSubsetOf<T>(this INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
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
        [ObservableProxy(typeof(ObservableSupersetOf<>), "Create")]
        public static bool IsSupersetOf<T>(this INotifyEnumerable<T> source, IEnumerable<T> other)
        {
            return IsSupersetOf(source, other, null);
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
        [ObservableProxy(typeof(ObservableSupersetOf<>), "CreateWithComparer")]
        public static bool IsSupersetOf<T>(this INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
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
        public static INotifyEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this INotifyEnumerable<TOuter> outerSource, IEnumerable<TInner> innerSource, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
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
        public static INotifyEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this INotifyEnumerable<TOuter> outerSource, IEnumerable<TInner> innerSource, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            var observable = new ObservableJoin<TOuter, TInner, TKey, TResult>(outerSource, innerSource, outerKeySelector, innerKeySelector, resultSelector, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Gets the maximum element of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <param name="source">The collection</param>
        /// <returns>An element which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "Max")]
        public static TSource Max<TSource>(this INotifyEnumerable<TSource> source)
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
        [ObservableProxy(typeof(ObservableComparisons), "MaxWithComparer")]
        public static TSource Max<TSource>(this INotifyEnumerable<TSource> source, IComparer<TSource> comparer)
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
        /// <typeparam name="TResult">The result type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <returns>An elements feature which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "LambdaMax")]
        public static TResult Max<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector)
            where TResult : IComparable<TResult>
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return SL.Max(source, selector.Compile());
        }

        /// <summary>
        /// Gets the maximum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The result type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An elements feature which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "LambdaMaxWithComparer")]
        public static TResult Max<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector, IComparer<TResult> comparer)
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
        [ObservableProxy(typeof(ObservableComparisons), "NullableMax")]
        public static TSource? Max<TSource>(this INotifyEnumerable<TSource?> source)
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
        [ObservableProxy(typeof(ObservableComparisons), "NullableMaxWithComparer")]
        public static TSource? Max<TSource>(this INotifyEnumerable<TSource?> source, IComparer<TSource> comparer)
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
        /// <typeparam name="TResult">The result type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <returns>An elements feature which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "LambdaNullableMax")]
        public static TResult? Max<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult?>> selector)
            where TResult : struct, IComparable<TResult>
        {
            return Max(source, selector, Comparer<TResult>.Default);
        }

        /// <summary>
        /// Gets the maximum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The result type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An elements feature which is maximal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "LambdaNullableMaxWithComparer")]
        public static TResult? Max<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult?>> selector, IComparer<TResult> comparer)
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
        [ObservableProxy(typeof(ObservableComparisons), "Min")]
        public static TSource Min<TSource>(this INotifyEnumerable<TSource> source)
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
        [ObservableProxy(typeof(ObservableComparisons), "MinWithComparer")]
        public static TSource Min<TSource>(this INotifyEnumerable<TSource> source, IComparer<TSource> comparer)
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
        /// <typeparam name="TResult">The result type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <returns>An elements feature which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "LambdaMin")]
        public static TResult Min<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector)
            where TResult : IComparable<TResult>
        {
            if (selector == null) throw new ArgumentNullException("selector");
            return SL.Min(source, selector.Compile());
        }

        /// <summary>
        /// Gets the minimum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The result type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An elements feature which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "LambdaMinWithComparer")]
        public static TResult Min<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector, IComparer<TResult> comparer)
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
        [ObservableProxy(typeof(ObservableComparisons), "NullableMin")]
        public static TSource? Min<TSource>(this INotifyEnumerable<TSource?> source)
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
        [ObservableProxy(typeof(ObservableComparisons), "NullableMinWithComparer")]
        public static TSource? Min<TSource>(this INotifyEnumerable<TSource?> source, IComparer<TSource> comparer)
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
        /// <typeparam name="TResult">The result type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <returns>An elements feature which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "LambdaNullableMin")]
        public static TResult? Min<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult?>> selector)
            where TResult : struct, IComparable<TResult>
        {
            return Min(source, selector, Comparer<TResult>.Default);
        }

        /// <summary>
        /// Gets the minimum feature of the given collection
        /// </summary>
        /// <typeparam name="TSource">The element type</typeparam>
        /// <typeparam name="TResult">The result type of the comparison</typeparam>
        /// <param name="selector">A lambda expression to obtain the elements feature in quest</param>
        /// <param name="source">The collection</param>
        /// <param name="comparer">A comparer for custom comparison</param>
        /// <returns>An elements feature which is minimal</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableComparisons), "LambdaNullableMinWithComparer")]
        public static TResult? Min<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult?>> selector, IComparer<TResult> comparer)
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
        public static INotifyEnumerable<TResult> OfType<TResult>(this INotifyEnumerable source)
        {
            var observable = new ObservableOfType<TResult>(source);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Filters the given collection for elements of the given type
        /// </summary>
        /// <typeparam name="TSource">The type of the original collection</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="source">The current collection</param>
        /// <returns>A collection containing the elements of the given type</returns>
        public static INotifyCollection<TResult> OfType<TSource, TResult>(this INotifyCollection<TSource> source)
            where TResult : TSource
        {
            var observable = new ObservableOfTypeCollection<TSource, TResult>(source);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Orders the given collection ascending by the given predicate
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The type of the keys used for ordering</typeparam>
        /// <param name="source">The collection that should be sorted</param>
        /// <param name="keySelector">A lambda expression selecting the sorting keys for the given collection</param>
        /// <returns>A collection with the elements contained in the current collection sorted by the given predicate</returns>
        public static IOrderableNotifyEnumerable<TItem> OrderBy<TItem, TKey>(this INotifyEnumerable<TItem> source, Expression<Func<TItem, TKey>> keySelector)
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
        public static IOrderableNotifyEnumerable<TItem> OrderBy<TItem, TKey>(this INotifyEnumerable<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
        {
            var observable = new ObservableOrderBy<TItem, TKey>(source, keySelector, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Orders the given collection descending by the given predicate
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The type of the keys used for ordering</typeparam>
        /// <param name="source">The collection that should be sorted</param>
        /// <param name="keySelector">A lambda expression selecting the sorting keys for the given collection</param>
        /// <returns>A collection with the elements contained in the current collection sorted by the given predicate</returns>
        public static IOrderableNotifyEnumerable<TItem> OrderByDescending<TItem, TKey>(this INotifyEnumerable<TItem> source, Expression<Func<TItem, TKey>> keySelector)
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
        public static IOrderableNotifyEnumerable<TItem> OrderByDescending<TItem, TKey>(this INotifyEnumerable<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return OrderBy(source, keySelector, new ReverseComparer<TKey>(comparer));
        }

        /// <summary>
        /// Maps the current collection to the given lambda expression
        /// </summary>
        /// <typeparam name="TSource">The elements type</typeparam>
        /// <typeparam name="TResult">The result element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="selector">A lambda expression representing the mapping result for a given item</param>
        /// <returns>A collection with the mapping results</returns>
        public static INotifyEnumerable<TResult> Select<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return Select( source, selector, null );
        }

        /// <summary>
        /// Maps the current collection to the given lambda expression
        /// </summary>
        /// <typeparam name="TSource">The elements type</typeparam>
        /// <typeparam name="TResult">The result element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="selector">A lambda expression representing the mapping result for a given item</param>
        /// <param name="selectorCompiled">A compiled form of the selector</param>
        /// <returns>A collection with the mapping results</returns>
        public static INotifyEnumerable<TResult> Select<TSource, TResult>( this INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector, Func<TSource, TResult> selectorCompiled )
        {
            var observable = new ObservableSelect<TSource, TResult>( source, new ObservingFunc<TSource, TResult>(selector, selectorCompiled));
            observable.Successors.SetDummy();
            return observable;
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
        public static INotifyEnumerable<TResult> SelectMany<TSource, TIntermediate, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, IEnumerable<TIntermediate>>> func, Expression<Func<TSource, TIntermediate, TResult>> selector)
        {
            return SelectMany( source, func, null, selector, null );
        }

        /// <summary>
        /// Flattens the given collection of collections where the subsequent collections are selected by a predicate
        /// </summary>
        /// <typeparam name="TSource">The source element type</typeparam>
        /// <typeparam name="TIntermediate">The element type of the subsequent collection</typeparam>
        /// <typeparam name="TResult">The result element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="func">A lambda expression to select subsequent collections</param>
        /// <param name="funcCompiled">A compiled version of func</param>
        /// <param name="selector">A lambda expression that determines the result element given the element of the source collection and the element of the subsequent collection</param>
        /// <param name="selectorCompiled">A compiled version of selector</param>
        /// <returns>A collection with the results</returns>
        public static INotifyEnumerable<TResult> SelectMany<TSource, TIntermediate, TResult>( this INotifyEnumerable<TSource> source, Expression<Func<TSource, IEnumerable<TIntermediate>>> func, Func<TSource, IEnumerable<TIntermediate>> funcCompiled, Expression<Func<TSource, TIntermediate, TResult>> selector, Func<TSource, TIntermediate, TResult> selectorCompiled )
        {
            var observable = new ObservableSelectMany<TSource, TIntermediate, TResult>( source, new ObservingFunc<TSource, IEnumerable<TIntermediate>>(func, funcCompiled), new ObservingFunc<TSource, TIntermediate, TResult>(selector, selectorCompiled) );
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Flattens the given collection of collections where the subsequent collections are selected by a predicate
        /// </summary>
        /// <typeparam name="TSource">The source element type</typeparam>
        /// <typeparam name="TResult">The result element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="selector">A lambda expression to select subsequent collections</param>
        /// <returns>A collection with the results</returns>
        public static INotifyEnumerable<TResult> SelectMany<TSource, TResult>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
        {
            var observable = new ObservableSimpleSelectMany<TSource, TResult>(source, selector);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Flattens the given collection of collections where the subsequent collections are selected by a predicate
        /// </summary>
        /// <typeparam name="TSource">The source element type</typeparam>
        /// <typeparam name="TResult">The result element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="selector">A lambda expression to select subsequent collections</param>
        /// <param name="selectorCompiled">A compiled version of the selector</param>
        /// <returns>A collection with the results</returns>
        public static INotifyEnumerable<TResult> SelectMany<TSource, TResult>( this INotifyEnumerable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector, Func<TSource, IEnumerable<TResult>> selectorCompiled )
        {
            var observable = new ObservableSimpleSelectMany<TSource, TResult>( source, new ObservingFunc<TSource, IEnumerable<TResult>>(selector, selectorCompiled) );
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Gets a value indicating whether the current collection and the given collection contain the same set of elements, regardless of their order
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="other">The given other collection</param>
        /// <returns>True, if both collections contain the same set of elements, otherwise False</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSetEquals<>), "Create")]
        public static bool SetEquals<T>(this INotifyEnumerable<T> source, IEnumerable<T> other)
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
        [ObservableProxy(typeof(ObservableSetEquals<>), "CreateWithComparer")]
        public static bool SetEquals<T>(this INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
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
        [ObservableProxy(typeof(ObservableSingleOrDefault<>), "Create")]
        [SetExpressionRewriter(typeof(ObservableSingleOrDefault<>), "CreateSetExpression")]
        public static TSource SingleOrDefault<TSource>(this INotifyEnumerable<TSource> source)
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
        [ObservableProxy(typeof(ObservableSingleOrDefault<>), "CreateForPredicate")]
        [SetExpressionRewriter(typeof(ObservableSingleOrDefault<>), "CreateSetExpressionWithPredicate")]
        public static TSource SingleOrDefault<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            return SL.SingleOrDefault(source, predicate.Compile());
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumInt")]
        public static int Sum(this INotifyEnumerable<int> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumLong")]
        public static long Sum(this INotifyEnumerable<long> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumFloat")]
        public static float Sum(this INotifyEnumerable<float> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumDouble")]
        public static double Sum(this INotifyEnumerable<double> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumDecimal")]
        public static decimal Sum(this INotifyEnumerable<decimal> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableInt")]
        public static int? Sum(this INotifyEnumerable<int?> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableLong")]
        public static long? Sum(this INotifyEnumerable<long?> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableFloat")]
        public static float? Sum(this INotifyEnumerable<float?> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableDouble")]
        public static double? Sum(this INotifyEnumerable<double?> source)
        {
            return SL.Sum(source);
        }

        /// <summary>
        /// Gets the sum of the current collection
        /// </summary>
        /// <param name="source">The collection of numbers</param>
        /// <returns>The sum of the numbers contained in this collection</returns>
        /// <remarks>This method has an observable proxy, i.e. it can be used in a observable expression</remarks>
        [ObservableProxy(typeof(ObservableSum), "SumNullableDecimal")]
        public static decimal? Sum(this INotifyEnumerable<decimal?> source)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaInt")]
        public static int Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, int>> selector)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaLong")]
        public static long Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, long>> selector)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaFloat")]
        public static float Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, float>> selector)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaDouble")]
        public static double Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, double>> selector)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaDecimal")]
        public static decimal Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal>> selector)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableInt")]
        public static int? Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, int?>> selector)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableLong")]
        public static long? Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, long?>> selector)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableFloat")]
        public static float? Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, float?>> selector)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableDouble")]
        public static double? Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, double?>> selector)
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
        [ObservableProxy(typeof(ObservableSum), "SumLambdaNullableDecimal")]
        public static decimal? Sum<TSource>(this INotifyEnumerable<TSource> source, Expression<Func<TSource, decimal?>> selector)
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
        public static IOrderableNotifyEnumerable<TItem> ThenBy<TItem, TKey>(this IOrderableNotifyEnumerable<TItem> source, Expression<Func<TItem, TKey>> keySelector)
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
        public static IOrderableNotifyEnumerable<TItem> ThenBy<TItem, TKey>(this IOrderableNotifyEnumerable<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
        {
            var observable = new ObservableThenBy<TItem, TKey>(source, keySelector, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Orders the given orderable collection by the given predicate descending
        /// </summary>
        /// <typeparam name="TItem">The elements type</typeparam>
        /// <typeparam name="TKey">The ordering key type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="keySelector">A lambda expression to select the features used for ordering</param>
        /// <returns>A collection with the elements of the current collection but ordered in lower priority for the given predicate</returns>
        public static IOrderableNotifyEnumerable<TItem> ThenByDescending<TItem, TKey>(this IOrderableNotifyEnumerable<TItem> source, Expression<Func<TItem, TKey>> keySelector)
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
        public static IOrderableNotifyEnumerable<TItem> ThenByDescending<TItem, TKey>(this IOrderableNotifyEnumerable<TItem> source, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
        {
            return ThenBy(source, keySelector, new ReverseComparer<TKey>(comparer));
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
        [ObservableProxy(typeof(ObservableTopX<,>), "CreateSelector")]
        public static KeyValuePair<TItem, TKey>[] TopX<TItem, TKey>(this INotifyEnumerable<TItem> source, int x, Expression<Func<TItem, TKey>> keySelector)
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
        [ObservableProxy(typeof(ObservableTopX<,>), "CreateSelectorComparer")]
        public static KeyValuePair<TItem, TKey>[] TopX<TItem, TKey>(this INotifyEnumerable<TItem> source, int x, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey> comparer)
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
        public static INotifyEnumerable<TSource> Union<TSource>(this INotifyEnumerable<TSource> source, IEnumerable<TSource> source2)
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
        public static INotifyEnumerable<TSource> Union<TSource>(this INotifyEnumerable<TSource> source, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
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
        public static INotifyEnumerable<T> Where<T>(this INotifyEnumerable<T> source, Expression<Func<T, bool>> filter)
        {
            return Where( source, filter, null );
        }

        /// <summary>
        /// Filters the given collection with the given predicate
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="filter">The predicate used for filtering</param>
        /// <param name="filterCompiled">A compiled version of filter</param>
        /// <returns>A collection containing the elements that passed the filter</returns>
        public static INotifyEnumerable<T> Where<T>( this INotifyEnumerable<T> source, Expression<Func<T, bool>> filter, Func<T, bool> filterCompiled )
        {
            var observable = new ObservableWhere<T>( source, new ObservingFunc<T, bool>(filter, filterCompiled) );
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Filters the given collection with the given predicate
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="filter">The predicate used for filtering</param>
        /// <returns>A collection containing the elements that passed the filter</returns>
        public static INotifyCollection<T> Where<T>(this INotifyCollection<T> source, Expression<Func<T, bool>> filter)
        {
            return Where( source, filter, null );
        }

        /// <summary>
        /// Filters the given collection with the given predicate
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <param name="filter">The predicate used for filtering</param>
        /// <param name="filterCompiled">A compiled version of filter</param>
        /// <returns>A collection containing the elements that passed the filter</returns>
        public static INotifyCollection<T> Where<T>( this INotifyCollection<T> source, Expression<Func<T, bool>> filter, Func<T, bool> filterCompiled )
        {
            var observable = new ObservableWhere<T>( source, new ObservingFunc<T, bool>( filter, filterCompiled ) );
            observable.Successors.SetDummy();
            return observable;
        }

        /// <summary>
        /// Fetches updates of the given collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <returns>The same collection as INotifyEnumerable</returns>
        public static INotifyEnumerable<T> WithUpdates<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (!(source is INotifyCollectionChanged)) throw new ArgumentException("The provided collection does not implement INotifyCollectionChanged", "source");

            if (source is not INotifyEnumerable<T> collection)
            {
                var observable = new ObservableCollectionProxy<T>(source);
                observable.Successors.SetDummy();
                return observable;
            }
            else
            {
                return collection;
            }
        }

        /// <summary>
        /// Fetches updates of the given collection
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="source">The current collection</param>
        /// <returns>The same collection as INotifyEnumerable</returns>
        public static INotifyCollection<T> WithUpdates<T>(this ICollection<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            if (source is not INotifyCollection<T> collection)
            {
                var observable = new ObservableCollectionProxy<T>(source);
                observable.Successors.SetDummy();
                return observable;
            }
            else
            {
                return collection;
            }
        }
    }
}
