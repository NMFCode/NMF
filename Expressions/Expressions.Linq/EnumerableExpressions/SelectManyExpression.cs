using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class SelectManyExpression<TSource, TIntermediate, TResult> : IEnumerableExpression<TResult>
    {
        public IEnumerableExpression<TSource> Source { get; private set; }
        public Expression<Func<TSource, IEnumerable<TIntermediate>>> FuncExpression { get; private set; }
        public Func<TSource, IEnumerable<TIntermediate>> FuncCompiled { get; private set; }
        public Expression<Func<TSource, TIntermediate, TResult>> ResultSelector { get; private set; }
        public Func<TSource, TIntermediate, TResult> ResultSelectorCompiled { get; private set; }

        public SelectManyExpression(IEnumerableExpression<TSource> source, Expression<Func<TSource, IEnumerable<TIntermediate>>> func, Func<TSource, IEnumerable<TIntermediate>> funcCompiled, Expression<Func<TSource, TIntermediate, TResult>> resultSelector, Func<TSource, TIntermediate, TResult> resultSelectorCompiled)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (func == null) throw new ArgumentNullException("func");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            Source = source;
            FuncExpression = func;
            FuncCompiled = funcCompiled ?? ExpressionCompileRewriter.Compile(func);
            ResultSelector = resultSelector;
            ResultSelectorCompiled = resultSelectorCompiled ?? ExpressionCompileRewriter.Compile(resultSelector);
        }

        public INotifyEnumerable<TResult> AsNotifiable()
        {
            return Source.AsNotifiable().SelectMany(FuncExpression, ResultSelector);
        }

        public IEnumerator<TResult> GetEnumerator()
        {
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
    }
}
