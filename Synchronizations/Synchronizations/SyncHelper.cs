using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a utility class to create synchronization helpers
    /// </summary>
    public class SyncHelper<TLeft, TRight>
    {
        /// <summary>
        /// Create a new synchronizer that synchronizes instances along given predicates
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftExpression">The expression to select values on the left side</param>
        /// <param name="rightExpression">The expression to select values on the right side</param>
        /// <returns>An object that synchronizes the objects</returns>
        public static ISyncer<TLeft, TRight> Synchronize<TValue>(Expression<Func<TLeft, TValue>> leftExpression, Expression<Func<TRight, TValue>> rightExpression)
        {
            return new PropertySynchronizationJob<TLeft, TRight, TValue>(leftExpression, rightExpression, true);
        }

        /// <summary>
        /// Create a new synchronizer that synchronizes instances along given predicates
        /// </summary>
        /// <typeparam name="TValue">The type of the values</typeparam>
        /// <param name="leftExpression">The expression to select values on the left side</param>
        /// <param name="rightExpression">The expression to select values on the right side</param>
        /// <returns>An object that synchronizes the objects</returns>
        public static ISyncer<TLeft, TRight> SynchronizeMany<TValue>(Func<TLeft, ICollectionExpression<TValue>> leftExpression, Func<TRight, ICollectionExpression<TValue>> rightExpression)
        {
            return new CollectionSynchronizationJob<TLeft, TRight, TValue>(leftExpression, rightExpression, true);
        }
    }
}
