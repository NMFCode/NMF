using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class GroupJoinExpression<TOuter, TInner, TKey, TResult> : IEnumerableExpression<TResult>, IOptimizableEnumerableExpression
    {
        public IEnumerableExpression<TOuter> Source { get; set; }
        public IEnumerable<TInner> Inner { get; set; }
        public Expression<Func<TOuter, TKey>> OuterKeySelector { get; set; }
        public Func<TOuter, TKey> OuterKeySelectorCompiled { get; set; }
        public Expression<Func<TInner, TKey>> InnerKeySelector { get; set; }
        public Func<TInner, TKey> InnerKeySelectorCompiled { get; set; }
        public Expression<Func<TOuter, IEnumerable<TInner>, TResult>> ResultSelector { get; set; }
        public Func<TOuter, IEnumerable<TInner>, TResult> ResultSelectorCompiled { get; set; }
        public IEqualityComparer<TKey> Comparer { get; set; }
        private INotifyEnumerable<TResult> notifyEnumerable;

        public Expression OptSelectorExpression => ResultSelector;

        public GroupJoinExpression(IEnumerableExpression<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Func<TOuter, TKey> outerKeySelectorCompiled, Expression<Func<TInner, TKey>> innerKeySelector, Func<TInner, TKey> innerKeySelectorCompiled, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelectorCompiled, IEqualityComparer<TKey> comparer)
        {
            if (outer == null) throw new ArgumentNullException("outer");
            if (inner == null) throw new ArgumentNullException("inner");
            if (outerKeySelector == null) throw new ArgumentNullException("outerKeySelector");
            if (innerKeySelector == null) throw new ArgumentNullException("innerKeySelector");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            Source = outer;
            Inner = inner;
            OuterKeySelector = outerKeySelector;
            OuterKeySelectorCompiled = outerKeySelectorCompiled ?? ExpressionCompileRewriter.Compile(outerKeySelector);
            InnerKeySelector = innerKeySelector;
            InnerKeySelectorCompiled = innerKeySelectorCompiled ?? ExpressionCompileRewriter.Compile(innerKeySelector);
            ResultSelector = resultSelector;
            ResultSelectorCompiled = resultSelectorCompiled ?? ExpressionCompileRewriter.Compile(resultSelector);
            Comparer = comparer;
#if DEBUG
            AddDgmlNode();
#endif
        }

        public INotifyEnumerable<TResult> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                notifyEnumerable = Source.AsNotifiable().GroupJoin(Inner, OuterKeySelector, InnerKeySelector, ResultSelector, Comparer);
            }
            return notifyEnumerable;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.GroupJoin(Source, Inner, OuterKeySelectorCompiled, InnerKeySelectorCompiled, ResultSelectorCompiled, Comparer).GetEnumerator();
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

            return this.Merge<TOptimizedResult>(expression);
        }

        public IEnumerableExpression<TOptimizedResult> Merge<TOptimizedResult>(IOptimizableEnumerableExpression prevExpr)
        {
            var mergedSelectorExpression = QueryOptimizer.Optimize<TOuter, TResult, TOptimizedResult>(prevExpr.OptSelectorExpression, OptSelectorExpression) as Expression<Func<TOuter, IEnumerable<TInner>, TOptimizedResult>>;
            return new GroupJoinExpression<TOuter, TInner, TKey, TOptimizedResult>(Source, Inner, OuterKeySelector, null, InnerKeySelector, null, mergedSelectorExpression, null, Comparer);
        }

        public void AddDgmlNode()
        {
            DmglVisualizer.AddNode(this);
        }

        private void VisitForDebugging(dynamic expression)
        {
            //Ausgabe überprüfen
            DebugVisitor debugVisitor = new DebugVisitor();
            debugVisitor.Visit(expression);
        }
    }
}
