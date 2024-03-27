using NMF.Collections.ObjectModel;
using NMF.Expressions;

namespace NMF.Collections
{
    /// <summary>
    /// Contains extension methods for collection expressions
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Discards any update attempts for the given collection
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The inner collection expression</param>
        /// <returns>A collecvtion expression that discards any updates</returns>
        public static ICollectionExpression<T> IgnoreUpdates<T>(this IEnumerableExpression<T> source)
        {
            return new IgnoreUpdatesCollection<T>(source);
        }
    }
}
