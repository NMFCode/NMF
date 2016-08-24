using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableProperSubsetOf<T> : ObservableSubsetOf<T>
    {
        private int diffCount;

        public new static INotifyValue<bool> Create(INotifyEnumerable<T> source, IEnumerable<T> other)
        {
            return new ObservableSetEquals<T>(source, other, null);
        }

        public new static INotifyValue<bool> CreateWithComparer(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return new ObservableSetEquals<T>(source, other, comparer);
        }

        public new static INotifyValue<bool> CreateExpression(IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return new ObservableSetEquals<T>(source.AsNotifiable(), other, null);
        }

        public new static INotifyValue<bool> CreateExpressionWithComparer(IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return new ObservableSetEquals<T>(source.AsNotifiable(), other, comparer);
        }

        public ObservableProperSubsetOf(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
            : base(source, other, comparer) { }

        protected override void OnAddSource1(bool isNew, bool isFirst)
        {
            base.OnAddSource1(isNew, isFirst);
            if (!isNew && isFirst) diffCount--;
        }

        protected override void OnAddSource2(bool isNew, bool isFirst)
        {
            base.OnAddSource2(isNew, isFirst);
            if (isNew) diffCount++;
        }

        protected override void OnRemoveSource1(bool isLast, bool removeEntry)
        {
            base.OnRemoveSource1(isLast, removeEntry);
            if (isLast && !removeEntry) diffCount++;
        }

        protected override void OnRemoveSource2(bool isLast, bool removeEntry)
        {
            base.OnRemoveSource2(isLast, removeEntry);
            if (isLast && removeEntry) diffCount--;
        }

        protected override void OnResetSource1(int entriesCount)
        {
            base.OnResetSource1(entriesCount);
            diffCount = entriesCount;
        }

        protected override void OnResetSource2(int entriesCount)
        {
            base.OnResetSource2(entriesCount);
            diffCount = 0;
        }

        public override bool Value
        {
            get
            {
                return base.Value && diffCount > 0;
            }
        }
    }

    internal sealed class ObservableProperSupersetOf<T> : ObservableSubsetOf<T>
    {
        private int diffCount;

        public new static INotifyValue<bool> Create(INotifyEnumerable<T> source, IEnumerable<T> other)
        {
            return new ObservableSetEquals<T>(source, other, null);
        }

        public new static INotifyValue<bool> CreateWithComparer(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return new ObservableSetEquals<T>(source, other, comparer);
        }

        public new static INotifyValue<bool> CreateExpression(IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            return new ObservableSetEquals<T>(source.AsNotifiable(), other, null);
        }

        public new static INotifyValue<bool> CreateExpressionWithComparer(IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            return new ObservableSetEquals<T>(source.AsNotifiable(), other, comparer);
        }

        public ObservableProperSupersetOf(INotifyEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
            : base(source, other, comparer) { }

        protected override void OnAddSource2(bool isNew, bool isFirst)
        {
            base.OnAddSource1(isNew, isFirst);
            if (!isNew && isFirst) diffCount--;
        }

        protected override void OnAddSource1(bool isNew, bool isFirst)
        {
            base.OnAddSource2(isNew, isFirst);
            if (isNew) diffCount++;
        }

        protected override void OnRemoveSource2(bool isLast, bool removeEntry)
        {
            base.OnRemoveSource1(isLast, removeEntry);
            if (isLast && !removeEntry) diffCount++;
        }

        protected override void OnRemoveSource1(bool isLast, bool removeEntry)
        {
            base.OnRemoveSource2(isLast, removeEntry);
            if (isLast && removeEntry) diffCount--;
        }

        protected override void OnResetSource2(int entriesCount)
        {
            base.OnResetSource1(entriesCount);
            diffCount = entriesCount;
        }

        protected override void OnResetSource1(int entriesCount)
        {
            base.OnResetSource2(entriesCount);
            diffCount = 0;
        }

        public override bool Value
        {
            get
            {
                return base.Value && diffCount > 0;
            }
        }
    }
}
