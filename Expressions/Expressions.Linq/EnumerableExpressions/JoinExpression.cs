using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class JoinExpression<TOuter, TInner, TKey, TResult> : IEnumerableExpression<TResult>, IOptimizableEnumerableExpression<TResult>
    {
        public IEnumerableExpression<TOuter> Source { get; set; }
        public IEnumerable<TInner> Inner { get; set; }
        public Expression<Func<TOuter, TKey>> OuterKeySelector { get; set; }
        public Func<TOuter, TKey> OuterKeySelectorCompiled { get; set; }
        public Expression<Func<TInner, TKey>> InnerKeySelector { get; set; }
        public Func<TInner, TKey> InnerKeySelectorCompiled { get; set; }
        public Expression<Func<TOuter, TInner, TResult>> ResultSelector { get; set; }
        public Func<TOuter, TInner, TResult> ResultSelectorCompiled { get; set; }
        public IEqualityComparer<TKey> Comparer { get; set; }
        private INotifyEnumerable<TResult> notifyEnumerable;


        public IEnumerableExpression OptSource => Source;


        public Expression OptSelectorExpression => ResultSelector;
        public Expression PrevExpression { get; set; }

        public JoinExpression(IEnumerableExpression<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Func<TOuter, TKey> outerKeySelectorCompiled, Expression<Func<TInner, TKey>> innerKeySelector, Func<TInner, TKey> innerKeySelectorCompiled, Expression<Func<TOuter, TInner, TResult>> resultSelector, Func<TOuter, TInner, TResult> resultSelectorCompiled, IEqualityComparer<TKey> comparer)
        {
            if (outer == null) throw new ArgumentNullException(nameof(outer));
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            Source = outer;
            Inner = inner;
            OuterKeySelector = outerKeySelector;
            OuterKeySelectorCompiled = outerKeySelectorCompiled ?? ExpressionCompileRewriter.Compile(outerKeySelector);
            InnerKeySelector = innerKeySelector;
            InnerKeySelectorCompiled = innerKeySelectorCompiled ?? ExpressionCompileRewriter.Compile(innerKeySelector);
            ResultSelector = resultSelector;
            ResultSelectorCompiled = resultSelectorCompiled ?? ExpressionCompileRewriter.Compile(resultSelector);
            Comparer = comparer;
        }

        public INotifyEnumerable<TResult> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                IEnumerable<TInner> inner = Inner;
                if (Inner is IEnumerableExpression<TInner> innerExpression)
                {
                    inner = innerExpression.AsNotifiable();
                }
                notifyEnumerable = Source.AsNotifiable().Join(inner, OuterKeySelector, InnerKeySelector, ResultSelector, Comparer);
            }
            return notifyEnumerable;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.Join(Source, Inner, OuterKeySelectorCompiled, InnerKeySelectorCompiled, ResultSelectorCompiled, Comparer).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        public IEnumerableExpression<TOptimizedResult> AsOptimized<TOptimizedResult>(IOptimizableEnumerableExpression expression = null)
        {
            if (expression != null)
                return Merge<TOptimizedResult>(expression);
            return (IEnumerableExpression<TOptimizedResult>)this;
        }

        public IEnumerableExpression<TOptimizedResult> Merge<TOptimizedResult>(IOptimizableEnumerableExpression prevExpr)
        {
            var mergedSelectorExpression = new ProjectionMergeQueryOptimizer().Optimize<TOuter, TResult, TOptimizedResult>(prevExpr.OptSelectorExpression, OptSelectorExpression) as Expression<Func<TOuter, TInner, TOptimizedResult>>;
            return new JoinExpression<TOuter, TInner, TKey, TOptimizedResult>(Source, Inner, OuterKeySelector, null, InnerKeySelector, null, mergedSelectorExpression, null, Comparer);
        }

        IOptimizableEnumerableExpression<TOptimizedResult> IOptimizableEnumerableExpression.AsOptimized2<TOptimizedResult>(IQueryOptimizer queryOptimizer)
        {
            return queryOptimizer.OptimizeExpression<TOuter, TInner, TKey, TResult, TOptimizedResult>(this);
        }
    }
}
