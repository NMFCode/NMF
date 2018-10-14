using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableSetEquals<T> : ObservableSetComparer<T>
    {
        public override string ToString()
        {
            return "[SetEquals]";
        }

        private int not_balanced;

        public static INotifyValue<bool> Create(INotifyEnumerable<T> source, IEnumerable<T> other)
        {
            return CreateWithComparer(source, other, null);
        }

        public static INotifyValue<bool> CreateWithComparer(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            var observable = new ObservableSetEquals<T>(source, other, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        public static INotifyValue<bool> CreateExpression(IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return CreateWithComparer(source.AsNotifiable(), other, null);
        }

        public static INotifyValue<bool> CreateExpressionWithComparer(IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return CreateWithComparer(source.AsNotifiable(), other, comparer);
        }

        public ObservableSetEquals(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
            : base(source, other, comparer) { }

        protected override void OnResetSource1(int entriesCount)
        {
            not_balanced = entriesCount;
        }

        protected override void OnResetSource2(int entriesCount)
        {
            not_balanced = entriesCount;
        }

        protected override void OnAddSource1(bool isNew, bool isFirst)
        {
            if (isNew)
            {
                not_balanced++;
            }
            else if (isFirst)
            {
                not_balanced--;
            }
        }

        protected override void OnAddSource2(bool isNew, bool isFirst)
        {
            if (isNew)
            {
                not_balanced++;
            }
            else if (isFirst)
            {
                not_balanced--;
            }
        }

        protected override void OnRemoveSource1(bool isLast, bool removeEntry)
        {
            if (isLast)
            {
                if (removeEntry)
                {
                    not_balanced--;
                }
                else
                {
                    not_balanced++;
                }
            }
        }

        protected override void OnRemoveSource2(bool isLast, bool removeEntry)
        {
            if (isLast)
            {
                if (removeEntry)
                {
                    not_balanced--;
                }
                else
                {
                    not_balanced++;
                }
            }
        }

        public override bool Value
        {
            get { return not_balanced == 0; }
        }
    }
}
