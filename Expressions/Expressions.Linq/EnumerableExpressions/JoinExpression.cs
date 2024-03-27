using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class JoinExpression<TOuter, TInner, TKey, TResult> : IEnumerableExpression<TResult>
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

        public JoinExpression(IEnumerableExpression<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Func<TOuter, TKey> outerKeySelectorCompiled, Expression<Func<TInner, TKey>> innerKeySelector, Func<TInner, TKey> innerKeySelectorCompiled, Expression<Func<TOuter, TInner, TResult>> resultSelector, Func<TOuter, TInner, TResult> resultSelectorCompiled, IEqualityComparer<TKey> comparer)
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
    }
}
