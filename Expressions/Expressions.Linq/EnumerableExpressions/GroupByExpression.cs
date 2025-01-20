using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;
using System.Collections;
using System.Linq;

namespace NMF.Expressions
{
    internal class GroupByExpression<TElement, TKey> : IEnumerableExpression<IGroupingExpression<TKey, TElement>>, ISQO
    {
        private class GroupingExpression : IGroupingExpression<TKey, TElement>
        {
            public GroupingExpression(IGrouping<TKey, TElement> group, GroupByExpression<TElement, TKey> groupBy)
            {
                this.group = group;
                this.groupBy = groupBy;
            }

            private readonly IGrouping<TKey, TElement> group;
            private readonly GroupByExpression<TElement, TKey> groupBy;

            public TKey Key
            {
                get { return group.Key; }
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                return group.GetEnumerator();
            }

            public INotifyEnumerable<TElement> AsNotifiable()
            {
                groupBy.CreateIncremental();
                return groupBy.notifyEnumerable[Key];
            }
            public IEnumerableExpression OptSource => throw new InvalidOperationException();

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            INotifyEnumerable IEnumerableExpression.AsNotifiable()
            {
                return AsNotifiable();
            }
        }


        public IEnumerableExpression<TElement> Source { get; private set; }

        public IEnumerableExpression OptSource => Source;


        public Expression<Func<TElement, TKey>> Predicate { get; private set; }
        public Func<TElement, TKey> PredicateCompiled { get; private set; }
        public IEqualityComparer<TKey> Comparer { get; private set; }

        private ObservableGroupBy<TKey, TElement> notifyEnumerable;

        public GroupByExpression(IEnumerableExpression<TElement> source, Expression<Func<TElement, TKey>> keySelector, Func<TElement, TKey> keySelectorCompiled, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (keySelectorCompiled == null) keySelectorCompiled = ExpressionCompileRewriter.Compile(keySelector);

            Source = source;
            Predicate = keySelector;
            PredicateCompiled = keySelectorCompiled;
            Comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        public INotifyEnumerable<IGroupingExpression<TKey, TElement>> AsNotifiable()
        {
            CreateIncremental();
            return notifyEnumerable;
        }

        private void CreateIncremental()
        {
            if (notifyEnumerable == null)
            {
                notifyEnumerable = new ObservableGroupBy<TKey, TElement>(Source.AsNotifiable(), new ObservingFunc<TElement, TKey>(Predicate, PredicateCompiled), Comparer);
            }
        }

        public IEnumerator<IGroupingExpression<TKey, TElement>> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.GroupBy(Source, PredicateCompiled, Comparer).Select(g => new GroupingExpression(g, this)).GetEnumerator();
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
