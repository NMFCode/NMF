namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Denotes an interface for a lookup
    /// </summary>
    /// <typeparam name="TSource">The source type of the lookup</typeparam>
    /// <typeparam name="TKey">The type of key elements</typeparam>
    public interface ILookupExpression<TSource, TKey>
    {
        /// <summary>
        /// Gets the elements for the provided key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The elements for this key</returns>
        IEnumerableExpression<TSource> this[TKey key] { get; }

        /// <summary>
        /// Gets a collection of keys
        /// </summary>
        IEnumerableExpression<TKey> Keys { get; }

        /// <summary>
        /// Gets an incremental version of the lookup
        /// </summary>
        /// <returns></returns>
        INotifyLookup<TSource, TKey> AsNotifiable();
    }

    /// <summary>
    /// Denotes an incremental lookup
    /// </summary>
    /// <typeparam name="TSource">The source type of the lookup</typeparam>
    /// <typeparam name="TKey">The key type for which the elements should be looked up</typeparam>
    public interface INotifyLookup<TSource, TKey>
    {
        /// <summary>
        /// Gets the collection of elements for the given key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>A collection of associated elements</returns>
        INotifyEnumerable<TSource> this[TKey key] { get; }

        /// <summary>
        /// Gets a collection of keys
        /// </summary>
        INotifyEnumerable<TKey> Keys { get; }
    }
}
