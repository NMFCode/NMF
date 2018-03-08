using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Defines a set of extension methods
    /// </summary>
    internal  static class OptimizableEnumerableExpressionExtensions
    {
        internal static bool IsSourceOptimizable(this IOptimizableEnumerableExpression optimizableEnumerableExpression, IEnumerableExpression source)
        {
            if (source is IOptimizableEnumerableExpression)
                return true;
            return false;
        }

        internal static IOptimizableEnumerableExpression Optimize<TOptimizedResult>(this IOptimizableEnumerableExpression optimizableEnumerableExpression, IOptimizableEnumerableExpression source)
        {
            return (IOptimizableEnumerableExpression) source.AsOptimized<TOptimizedResult>(optimizableEnumerableExpression);
        }
    }
}