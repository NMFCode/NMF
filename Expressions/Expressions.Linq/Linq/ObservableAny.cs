using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class ObservableAny<TSource> : ObservableAggregate<TSource, int, bool>
    {
        public static ObservableAny<TSource> Create(INotifyEnumerable<TSource> source)
        {
            return new ObservableAny<TSource>(source);
        }

        public static ObservableAny<TSource> CreateExpression(IEnumerableExpression<TSource> source)
        {
            return new ObservableAny<TSource>(source.AsNotifiable());
        }

        public ObservableAny(INotifyEnumerable<TSource> source)
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

        public override bool Value
        {
            get { return Accumulator > 0; }
        }
    }


    internal class ObservableLambdaAny<TSource> : ObservableAggregate<bool, int, bool>
    {
        public static ObservableLambdaAny<TSource> Create(INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> selector)
        {
            return new ObservableLambdaAny<TSource>(source, selector);
        }

        public static ObservableLambdaAny<TSource> CreateExpression(IEnumerableExpression<TSource> source, Expression<Func<TSource, bool>> selector)
        {
            return new ObservableLambdaAny<TSource>(source.AsNotifiable(), selector);
        }

        public ObservableLambdaAny(INotifyEnumerable<TSource> source, Expression<Func<TSource, bool>> selector)
            : base(new ObservableSelect<TSource, bool>(source, selector), 0)
        {
        }

        protected override void ResetAccumulator()
        {
            Accumulator = 0;
        }

        protected override void RemoveItem(bool item)
        {
            if (item)
            {
                Accumulator--;
            }
        }

        protected override void AddItem(bool item)
        {
            if (item)
            {
                Accumulator++;
            }
        }

        public override bool Value
        {
            get { return Accumulator != 0; }
        }
    }
}
