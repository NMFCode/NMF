using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class ObservableContains<TSource> : ObservableAggregate<TSource, int, bool>
    {
        public override string ToString()
        {
            return $"[Contains {searchItem}]";
        }

        public static ObservableContains<TSource> Create(INotifyEnumerable<TSource> source, TSource searchItem)
        {
            return CreateWithComparer(source, searchItem, null);
        }

        public static ObservableContains<TSource> CreateWithComparer(INotifyEnumerable<TSource> source, TSource searchItem, IEqualityComparer<TSource> comparer)
        {
            var observable = new ObservableContains<TSource>(source, searchItem, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableContains<TSource> CreateExpression(IEnumerableExpression<TSource> source, TSource searchItem)
        {
            return CreateWithComparer(source.AsNotifiable(), searchItem, null);
        }

        public static ObservableContains<TSource> CreateExpressionWithComparer(IEnumerableExpression<TSource> source, TSource searchItem, IEqualityComparer<TSource> comparer)
        {
            return CreateWithComparer(source.AsNotifiable(), searchItem, comparer);
        }

        private readonly TSource searchItem;
        private readonly IEqualityComparer<TSource> comparer;

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
