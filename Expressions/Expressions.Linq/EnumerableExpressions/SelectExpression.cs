using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class SelectExpression<TSource, TResult> : IEnumerableExpression<TResult>
    {
        public IEnumerableExpression<TSource> Source { get; private set; }
        public Expression<Func<TSource, TResult>> SelectorExpression { get; private set; }
        public Func<TSource, TResult> SelectorCompiled { get; private set; }
        private INotifyEnumerable<TResult> notifyEnumerable;

        public SelectExpression(IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, Func<TSource, TResult> selectorCompiled)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (selectorCompiled == null) selectorCompiled = ExpressionCompileRewriter.Compile(selector);

            Source = source;
            SelectorExpression = selector;
            SelectorCompiled = selectorCompiled;
        }

        public INotifyEnumerable<TResult> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                notifyEnumerable = Source.AsNotifiable().Select(SelectorExpression, SelectorCompiled);
            }
            return notifyEnumerable;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.Select(Source, SelectorCompiled).GetEnumerator();
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
