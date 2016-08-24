using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class ObservableSubsetOf<T> : ObservableSetComparer<T>
    {
        private int nDiff;

        public static INotifyValue<bool> Create(INotifyEnumerable<T> source, IEnumerable<T> other)
        {
            return new ObservableSetEquals<T>(source, other, null);
        }

        public static INotifyValue<bool> CreateWithComparer(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return new ObservableSetEquals<T>(source, other, comparer);
        }

        public static INotifyValue<bool> CreateExpression(IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return new ObservableSetEquals<T>(source.AsNotifiable(), other, null);
        }

        public static INotifyValue<bool> CreateExpressionWithComparer(IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return new ObservableSetEquals<T>(source.AsNotifiable(), other, comparer);
        }

        public ObservableSubsetOf(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
            : base(source, other, comparer) { }

        protected override void OnResetSource1(int entriesCount)
        {
            nDiff = 0;
        }

        protected override void OnResetSource2(int entriesCount)
        {
            nDiff = entriesCount;
        }

        protected override void OnAddSource1(bool isNew, bool isFirst)
        {
            if (isNew) nDiff++;
        }

        protected override void OnAddSource2(bool isNew, bool isFirst)
        {
            if (!isNew && isFirst) nDiff--;
        }

        protected override void OnRemoveSource1(bool isLast, bool removeEntry)
        {
            if (isLast && removeEntry) nDiff--;
        }

        protected override void OnRemoveSource2(bool isLast, bool removeEntry)
        {
            if (isLast && !removeEntry) nDiff++;
        }

        public override bool Value
        {
            get { return nDiff == 0; }
        }
    }

    internal class ObservableSupersetOf<T> : ObservableSetComparer<T>
    {
        private int nDiff;

        public static INotifyValue<bool> Create(INotifyEnumerable<T> source, IEnumerable<T> other)
        {
            return new ObservableSetEquals<T>(source, other, null);
        }

        public static INotifyValue<bool> CreateWithComparer(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return new ObservableSetEquals<T>(source, other, comparer);
        }

        public static INotifyValue<bool> CreateExpression(IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return new ObservableSetEquals<T>(source.AsNotifiable(), other, null);
        }

        public static INotifyValue<bool> CreateExpressionWithComparer(IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return new ObservableSetEquals<T>(source.AsNotifiable(), other, comparer);
        }

        public ObservableSupersetOf(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
            : base(source, other, comparer) { }

        protected override void OnResetSource2(int entriesCount)
        {
            nDiff = 0;
        }

        protected override void OnResetSource1(int entriesCount)
        {
            nDiff = entriesCount;
        }

        protected override void OnAddSource2(bool isNew, bool isFirst)
        {
            if (isNew) nDiff++;
        }

        protected override void OnAddSource1(bool isNew, bool isFirst)
        {
            if (!isNew && isFirst) nDiff--;
        }

        protected override void OnRemoveSource2(bool isLast, bool removeEntry)
        {
            if (isLast && removeEntry) nDiff--;
        }

        protected override void OnRemoveSource1(bool isLast, bool removeEntry)
        {
            if (isLast && !removeEntry) nDiff++;
        }

        public override bool Value
        {
            get { return nDiff == 0; }
        }
    }

}
