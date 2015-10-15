using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class ObservableContains<TSource> : ObservableAggregate<TSource, int, bool>
    {
        public static ObservableContains<TSource> Create(INotifyEnumerable<TSource> source, TSource searchItem)
        {
            return new ObservableContains<TSource>(source, searchItem);
        }

        public static ObservableContains<TSource> CreateWithComparer(INotifyEnumerable<TSource> source, TSource searchItem, IEqualityComparer<TSource> comparer)
        {
            return new ObservableContains<TSource>(source, searchItem, comparer);
        }

        public static ObservableContains<TSource> CreateExpression(IEnumerableExpression<TSource> source, TSource searchItem)
        {
            return new ObservableContains<TSource>(source.AsNotifiable(), searchItem);
        }

        public static ObservableContains<TSource> CreateExpressionWithComparer(IEnumerableExpression<TSource> source, TSource searchItem, IEqualityComparer<TSource> comparer)
        {
            return new ObservableContains<TSource>(source.AsNotifiable(), searchItem, comparer);
        }

        private TSource searchItem;
        private IEqualityComparer<TSource> comparer;

        public ObservableContains(INotifyEnumerable<TSource> source, TSource searchItem) : this(source, searchItem, null) { }

        public ObservableContains(INotifyEnumerable<TSource> source, TSource searchItem, IEqualityComparer<TSource> comparer)
            : base(source, 0)
        {
            this.searchItem = searchItem;
            this.comparer = comparer ?? EqualityComparer<TSource>.Default;
        }

        protected override void ResetAccumulator()
        {
            Accumulator = 0;
        }

        protected override void RemoveItem(TSource item)
        {
            if (comparer.Equals(item, searchItem))
            {
                Accumulator--;
            }
        }

        protected override void AddItem(TSource item)
        {
            if (comparer.Equals(item, searchItem))
            {
                Accumulator++;
            }
        }

        public override bool Value
        {
            get { return Accumulator > 0; }
        }
    }
}
