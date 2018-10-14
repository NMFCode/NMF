using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class ObservableSubsetOf<T> : ObservableSetComparer<T>
    {
        public override string ToString()
        {
            return "[SubsetOf]";
        }

        private int nDiff;

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
        public override string ToString()
        {
            return "[SupersetOf]";
        }

        private int nDiff;

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
