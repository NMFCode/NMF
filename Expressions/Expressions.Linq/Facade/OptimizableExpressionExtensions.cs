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
        internal static IEnumerableExpression<TOptimizedResult> Optimize<TOptimizedResult>(this IOptimizableEnumerableExpression optimizableEnumerableExpression, IEnumerableExpression source)
        {
            if (source is IOptimizableEnumerableExpression) 
            {
                var castedSource = source as IOptimizableEnumerableExpression;
                return castedSource.AsOptimized<TOptimizedResult>(optimizableEnumerableExpression); ;
            }
            return (IEnumerableExpression<TOptimizedResult>)optimizableEnumerableExpression;
        }
    }
}