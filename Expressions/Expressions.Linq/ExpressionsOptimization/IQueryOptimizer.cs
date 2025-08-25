namespace NMF.Expressions
{
    internal interface IQueryOptimizer
    {
        IEnumerableExpression Optimize<TResult>(ISQO expression);

        IOptimizableEnumerableExpression<TOptimizedResult> OptimizeExpression<TSource, TResult, TOptimizedResult>(SelectExpression<TSource, TResult> expression);

        IOptimizableEnumerableExpression<TOptimizedResult> OptimizeExpression<TSource, TIntermediate, TResult, TOptimizedResult>(SelectManyExpression<TSource, TIntermediate, TResult> expression);

        IOptimizableEnumerableExpression<TOptimizedResult> OptimizeExpression<TOuter, TInner, TKey, TResult, TOptimizedResult>(JoinExpression<TOuter, TInner, TKey, TResult> expression);

        IOptimizableEnumerableExpression<TOptimizedResult> OptimizeExpression<TOuter, TInner, TKey, TResult, TOptimizedResult>(GroupJoinExpression<TOuter, TInner, TKey, TResult> expression);
    }
}