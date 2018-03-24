using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal interface ISQO
    {
        IEnumerableExpression OptSource { get; }
    }

    internal interface IOptimizableEnumerableExpression : ISQO
    {
        Expression OptSelectorExpression { get; }

        Expression PrevExpression { set; get; }

        /// <summary>
        /// Gets optimization for this collection
        /// </summary>
        /// <typeparam name="TOptimizedResult"></typeparam>
        /// <param name="prevOptExpression"></param>
        /// <returns></returns>
        IEnumerableExpression<TOptimizedResult> AsOptimized<TOptimizedResult>(IOptimizableEnumerableExpression prevOptExpression = null);

        IEnumerableExpression<TOptimizedResult> Merge<TOptimizedResult>(IOptimizableEnumerableExpression prevOptExpression);

        IOptimizableEnumerableExpression<TOptimizedResult> AsOptimized2<TOptimizedResult>(IQueryOptimizer queryOptimizer);
    }

    /// <summary>
    /// Represents a collection that can be optimized
    /// </summary>
    internal interface IOptimizableEnumerableExpression<TResult> : IOptimizableEnumerableExpression
    {
        /// <summary>
        /// Gets optimization for this collection
        /// </summary>
        /// <typeparam name="TOptimizedResult"></typeparam>
        /// <param name="prevOptExpression"></param>
        /// <returns></returns>
        //IEnumerableExpression<TOptimizedResult> AsOptimized<TOptimizedResult>(IOptimizableEnumerableExpression<TResult> prevOptExpression = null);

        //IEnumerableExpression<TOptimizedResult> Merge<TOptimizedResult>(IOptimizableEnumerableExpression<TResult> prevOptExpression);
    }

    internal interface IOptimizableEnumerableExpression<TSource, TResult> : IOptimizableEnumerableExpression<TResult>
    {
        new IOptimizableEnumerableExpression<TSource, TResult> AsOptimized2<TOptimizedResult>(IQueryOptimizer queryOptimizer);
    }

    internal interface IOptimizableEnumerableExpression<TSource, TIntermediate, TResult> : IOptimizableEnumerableExpression<TResult>
    {
        new IOptimizableEnumerableExpression<TSource, TIntermediate, TResult> AsOptimized2<TOptimizedResult>(IQueryOptimizer queryOptimizer);
    }

    internal interface IOptimizableEnumerableExpression<TOuter, TInner, TKey, TResult> : IOptimizableEnumerableExpression<TResult>
    {
        new IOptimizableEnumerableExpression<TOuter, TInner, TKey, TResult> AsOptimized2<TOptimizedResult>(IQueryOptimizer queryOptimizer);
    }

    /// <summary>
    /// Represents a collection that can be accessed incrementally, i.e. with change notifications
    /// </summary>
    /// <typeparam name="T">The type of the elements</typeparam>
    public interface IEnumerableExpression<out T> : IEnumerable<T>, IEnumerableExpression
    {
        /// <summary>
        /// Gets notifications for this collection
        /// </summary>
        /// <returns>A collection that will notify clients as new elements change</returns>
        new INotifyEnumerable<T> AsNotifiable();
    }

    /// <summary>
    /// Represents a (non-generoc) collection that can be accessed incrementally, i.e. with change notifications
    /// </summary>
    public interface IEnumerableExpression : IEnumerable
    {
        /// <summary>
        /// Gets notifications for this collection
        /// </summary>
        /// <returns>A collection that will notify clients as new elements change</returns>
        INotifyEnumerable AsNotifiable();
    }

    /// <summary>
    /// Represents an editable collection that can be accessed incrementally, i.e. with change notifications
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICollectionExpression<T> : IEnumerableExpression<T>, ICollection<T>
    {
        /// <summary>
        /// Gets notifications for this collection
        /// </summary>
        /// <returns>A collection that will notify clients as new elements change</returns>
        new INotifyCollection<T> AsNotifiable();
    }

    /// <summary>
    /// Represents an editable collection that can be accessed incrementally, i.e. with change notifications
    /// </summary>
    public interface ICollectionExpression : IEnumerableExpression, IList { }

    /// <summary>
    /// Represents an orderable collection that can be accessed incrementally, i.e. with change notifications
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOrderableEnumerableExpression<T> : IEnumerableExpression<T>, IOrderedEnumerable<T>
    {
        /// <summary>
        /// Gets notifications for this collection
        /// </summary>
        /// <returns>A collection that will notify clients as new elements change</returns>
        new IOrderableNotifyEnumerable<T> AsNotifiable();
    }

    /// <summary>
    /// Represents a list that can be accessed incrementally, i.e. with change notifications
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListExpression<T> : IList<T>, ICollectionExpression<T> { }

    /// <summary>
    /// Represents a set that can be accessed incrementally, i.e. with change notifications
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISetExpression<T> : ISet<T>, ICollectionExpression<T> { }

    /// <summary>
    /// Represents a group of elements sharing a common key
    /// </summary>
    /// <typeparam name="TKey">The type of the key</typeparam>
    /// <typeparam name="TElement">The type of the elements</typeparam>
    public interface IGroupingExpression<out TKey, out TElement> : IGrouping<TKey, TElement>, IEnumerableExpression<TElement> { }
}
