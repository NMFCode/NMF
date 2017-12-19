using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Text;
using NMF.Expressions.Linq;
using System.Linq.Expressions;

namespace NMF.Expressions
{

    internal class ThenByExpression<T, TKey> : IOrderableEnumerableExpression<T>
    {
        public IOrderableEnumerableExpression<T> Source { get; private set; }
        public Expression<Func<T, TKey>> Predicate { get; private set; }
        public Func<T, TKey> PredicateCompiled { get; private set; }
        public IComparer<TKey> Comparer { get; private set; }
        private IOrderableNotifyEnumerable<T> notifyEnumerable;

        public ThenByExpression(IOrderableEnumerableExpression<T> source, Expression<Func<T, TKey>> keySelector, Func<T, TKey> keySelectorCompiled, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (keySelectorCompiled == null) keySelectorCompiled = ExpressionCompileRewriter.Compile(keySelector);

            Source = source;
            Predicate = keySelector;
            PredicateCompiled = keySelectorCompiled;
            Comparer = comparer ?? Comparer<TKey>.Default;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.ThenBy(Source, PredicateCompiled, Comparer).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        public IOrderableNotifyEnumerable<T> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                notifyEnumerable = Source.AsNotifiable().ThenBy(Predicate, Comparer);
            }
            return notifyEnumerable;
        }

        public System.Linq.IOrderedEnumerable<T> CreateOrderedEnumerable<TKey2>(Func<T, TKey2> keySelector, IComparer<TKey2> comparer, bool descending)
        {
            return SL.ThenBy(Source, PredicateCompiled, Comparer).CreateOrderedEnumerable(keySelector, comparer, descending);
        }

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return AsNotifiable();
        }
    }
}
