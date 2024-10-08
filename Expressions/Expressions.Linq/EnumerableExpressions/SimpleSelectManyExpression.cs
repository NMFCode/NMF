﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class SimpleSelectManyExpression<TSource, TResult> : IEnumerableExpression<TResult>
    {
        public IEnumerableExpression<TSource> Source { get; private set; }
        public Expression<Func<TSource, IEnumerable<TResult>>> SelectorExpression { get; private set; }
        public Func<TSource, IEnumerable<TResult>> SelectorCompiled { get; private set; }
        private INotifyEnumerable<TResult> notifyEnumerable;

        public SimpleSelectManyExpression(IEnumerableExpression<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector, Func<TSource, IEnumerable<TResult>> selectorCompiled)
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
                notifyEnumerable = Source.AsNotifiable().SelectMany(SelectorExpression, SelectorCompiled);
            }
            return notifyEnumerable;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.SelectMany(Source, SelectorCompiled).GetEnumerator();
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
