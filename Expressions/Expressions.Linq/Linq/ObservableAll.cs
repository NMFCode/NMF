using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class ObservableAll<TItem> : ObservableAggregate<bool, int, bool>
    {
        public static ObservableAll<TItem> Create(INotifyEnumerable<TItem> source, Expression<Func<TItem, bool>> predicate)
        {
            var observable =  new ObservableAll<TItem>(source, predicate);
            observable.Successors.SetDummy();
            return observable;
        }

        public static ObservableAll<TItem> CreateExpression(IEnumerableExpression<TItem> source, Expression<Func<TItem, bool>> predicate)
        {
            return Create(source.AsNotifiable(), predicate);
        }

        public ObservableAll(INotifyEnumerable<TItem> source, Expression<Func<TItem, bool>> predicate)
            : base(new ObservableSelect<TItem, bool>(source, predicate), 0) { }

        protected override void ResetAccumulator()
        {
            Accumulator = 0;
        }

        protected override void RemoveItem(bool item)
        {
            if (!item)
            {
                Accumulator--;
            }
        }

        protected override void AddItem(bool item)
        {
            if (!item)
            {
                Accumulator++;
            }
        }

        public override bool Value
        {
            get { return Accumulator == 0; }
        }
    }
}
