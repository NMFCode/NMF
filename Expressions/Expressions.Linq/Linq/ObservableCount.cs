using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class ObservableCount<TSource> : ObservableAggregate<TSource, int, int>
    {
        public override string ToString()
        {
            return "[Count]";
        }

        public static ObservableCount<TSource> Create(INotifyEnumerable<TSource> source)
        {
            var observable = new ObservableCount<TSource>(source);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableCount<TSource> CreateWithComparer(INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return Create(source.Where(predicate));
        }

        public static ObservableCount<TSource> CreateExpression(IEnumerableExpression<TSource> source)
        {
            return Create(source.AsNotifiable());
        }

        public static ObservableCount<TSource> CreateExpressionWithComparer(IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return Create(source.AsNotifiable().Where(predicate));
        }

        public ObservableCount(INotifyEnumerable<TSource> source)
            : base(source, 0)
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = 0;
        }

        protected override void RemoveItem(TSource item)
        {
            Accumulator--;
        }

        protected override void AddItem(TSource item)
        {
            Accumulator++;
        }

        public override int Value
        {
            get { return Accumulator; }
        }
    }
}
