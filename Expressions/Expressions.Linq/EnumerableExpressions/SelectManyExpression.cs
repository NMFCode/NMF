using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class SelectManyExpression<TSource, TIntermediate, TResult> : IEnumerableExpression<TResult>, IOptimizableEnumerableExpression<TResult>
    {
        public IEnumerableExpression<TSource> Source { get; private set; }
        public Expression<Func<TSource, IEnumerable<TIntermediate>>> FuncExpression { get; private set; }
        public Func<TSource, IEnumerable<TIntermediate>> FuncCompiled { get; private set; }
        public Expression<Func<TSource, TIntermediate, TResult>> ResultSelector { get; private set; }
        public Func<TSource, TIntermediate, TResult> ResultSelectorCompiled { get; private set; }
        private INotifyEnumerable<TResult> notifyEnumerable;

        public Expression OptSelectorExpression => ResultSelector;

        public IEnumerableExpression OptSource => Source;


        public Expression PrevExpression { get; set; }

        public SelectManyExpression(IEnumerableExpression<TSource> source, Expression<Func<TSource, IEnumerable<TIntermediate>>> func, Func<TSource, IEnumerable<TIntermediate>> funcCompiled, Expression<Func<TSource, TIntermediate, TResult>> resultSelector, Func<TSource, TIntermediate, TResult> resultSelectorCompiled)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            Source = source;
            FuncExpression = func;
            FuncCompiled = funcCompiled ?? ExpressionCompileRewriter.Compile(func);
            ResultSelector = resultSelector;
            ResultSelectorCompiled = resultSelectorCompiled ?? ExpressionCompileRewriter.Compile(resultSelector);
#if DEBUG
            QueryExpressionDgmlVisualizer.AddNode(this);
#endif
        }

        public INotifyEnumerable<TResult> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                notifyEnumerable = Source.AsNotifiable().SelectMany(FuncExpression, FuncCompiled, ResultSelector, ResultSelectorCompiled);
            }
            return notifyEnumerable;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.SelectMany(Source, FuncCompiled, ResultSelectorCompiled).GetEnumerator();
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
#if DEBUG
            VisitForDebugging(ResultSelector);
#endif
            if(expression != null)
                return Merge<TOptimizedResult>(expression);
            return (IEnumerableExpression<TOptimizedResult>) this;
        }

        public IEnumerableExpression<TOptimizedResult> Merge<TOptimizedResult>(IOptimizableEnumerableExpression prevExpr)
        {
            var mergedSelectorExpression = new ProjectionMergeQueryOptimizer().Optimize<TSource, TResult, TOptimizedResult>(prevExpr.OptSelectorExpression, OptSelectorExpression) as Expression<Func<TSource, TIntermediate, TOptimizedResult>>;
            return new SelectManyExpression<TSource, TIntermediate, TOptimizedResult>(Source, FuncExpression, null, mergedSelectorExpression, null);
        }

        IOptimizableEnumerableExpression<TOptimizedResult> IOptimizableEnumerableExpression.AsOptimized2<TOptimizedResult>(IQueryOptimizer queryOptimizer)
        {
            return queryOptimizer.OptimizeExpression<TSource, TIntermediate, TResult, TOptimizedResult>(this);
        }

        private void VisitForDebugging(Expression expression)
        {
            //Ausgabe überprüfen
            DebugVisitor debugVisitor = new DebugVisitor();
            debugVisitor.Visit(expression);
        }
    }
}
