using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal class OrderByExpression<T, TKey> : IOrderableEnumerableExpression<T>
    {
        public IEnumerableExpression<T> Source { get; private set; }
        public Expression<Func<T, TKey>> Predicate { get; private set; }
        public Func<T, TKey> PredicateCompiled { get; private set; }
        public IComparer<TKey> Comparer { get; private set; }
        private IOrderableNotifyEnumerable<T> notifyEnumerable;

        public OrderByExpression(IEnumerableExpression<T> source, Expression<Func<T, TKey>> keySelector, Func<T, TKey> keySelectorCompiled, IComparer<TKey> comparer)
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
            return SL.OrderBy(Source, PredicateCompiled, Comparer).GetEnumerator();
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
                notifyEnumerable = Source.AsNotifiable().OrderBy(Predicate, Comparer);
            }
            return notifyEnumerable;
        }

        public System.Linq.IOrderedEnumerable<T> CreateOrderedEnumerable<TKey2>(Func<T, TKey2> keySelector, IComparer<TKey2> comparer, bool descending)
        {
            return SL.OrderBy(Source, PredicateCompiled, Comparer).CreateOrderedEnumerable(keySelector, comparer, descending);
        }

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return AsNotifiable();
        }
    }
}
