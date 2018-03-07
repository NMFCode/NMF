using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class SelectExpression<TSource, TResult> : IEnumerableExpression<TResult>, IOptimizableEnumerableExpression
    {
        public IEnumerableExpression<TSource> Source { get; private set; }
        public Expression<Func<TSource, TResult>> SelectorExpression { get; private set; }
        public Func<TSource, TResult> SelectorCompiled { get; private set; }

        private INotifyEnumerable<TResult> notifyEnumerable;
        public Expression OptSelectorExpression => SelectorExpression;

        public SelectExpression(IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, Func<TSource, TResult> selectorCompiled)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (selectorCompiled == null) selectorCompiled = ExpressionCompileRewriter.Compile(selector);

            Source = source;
            SelectorExpression = selector;
            SelectorCompiled = selectorCompiled;
#if DEBUG
            QueryExpressionDgmlVisualizer.AddNode(this);

#endif
        }

        public INotifyEnumerable<TResult> AsNotifiable()
        {
            //TODO: Wie oben erst eine IsOptimizable Methode aufrufen und falls true Optimize aufrufen. Dadurch spart man sich hier wieder den != this Vergleich
            IEnumerableExpression<TResult> optimizedExpression = AsOptimized<TResult>();

            if (optimizedExpression != this)
                return optimizedExpression.AsNotifiable();

            if (notifyEnumerable == null)
            {
                notifyEnumerable = Source.AsNotifiable().Select(SelectorExpression);
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

        public IEnumerableExpression<TOptimizedResult> AsOptimized<TOptimizedResult>(IOptimizableEnumerableExpression expression = null)
        {
#if DEBUG
            VisitForDebugging(SelectorExpression);
#endif

            //TODO: Aufruf hier weglassen
            IEnumerableExpression<TResult> optimizedExpression = this.Optimize<TResult>(Source);

            //Wenn expression == null : Ausgangspunkt der Optimierung
            if (expression != null)
            {
                //TODO: Hier eine IsOptimizable() Methode in einem if aufrufen (prüft nur den Typ)

                //TODO: Dadurch kann man sich die Überprüfung mit != this sparen
                //if return value of optimize() is the caller of this method
                //nothing can be optimized
                if (optimizedExpression != this)
                {

                    //TODO: Dann hier erst Optimize Extension Method Aufrufen und den Rückgabetyp ändern um anschließenden Cast zu sparen
                    //TODO: bzw. wieso muss man hier nochmal AsOptimized aufrufen? Wurde doch oben in der Optimize ExtMethod gemacht wenn Typ passt!

                    //test != this: eine bereit neu erstellte Expression soll weiter optimiert werden
                    var castedOptimizedExpression = optimizedExpression as IOptimizableEnumerableExpression;
                    return castedOptimizedExpression.AsOptimized<TOptimizedResult>(expression);
                }

                //Create new Expression
                return Merge<TOptimizedResult>(expression);
            }

            return (IEnumerableExpression<TOptimizedResult>)optimizedExpression;
        }

        public IEnumerableExpression<TOptimizedResult> Merge<TOptimizedResult>(IOptimizableEnumerableExpression prevExpr)
        {
            var mergedSelectorExpression = QueryOptimizer.Optimize<TSource, TResult, TOptimizedResult>(prevExpr.OptSelectorExpression, OptSelectorExpression) as Expression<Func<TSource, TOptimizedResult>>;
            return new SelectExpression<TSource, TOptimizedResult>(Source, mergedSelectorExpression, null);
        }

        private void VisitForDebugging(dynamic expression)
        {
            //Ausgabe überprüfen
            DebugVisitor debugVisitor = new DebugVisitor();
            debugVisitor.Visit(expression);
        }
    }
}
