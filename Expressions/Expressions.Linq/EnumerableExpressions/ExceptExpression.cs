using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;

namespace NMF.Expressions
{
    internal abstract class SetExpression<T> : IEnumerableExpression<T>, ISQO
    {
        public IEnumerableExpression<T> Source { get; private set; }
        public IEnumerable<T> Other { get; private set; }
        public IEqualityComparer<T> Comparer { get; private set; }
        protected INotifyEnumerable<T> notifyEnumerable;

        public IEnumerableExpression OptSource => Source;

        public SetExpression(IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (other == null) throw new ArgumentNullException("other");

            Source = source;
            Other = other;
            Comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                notifyEnumerable = AsNotifiableCore();
            }
            return notifyEnumerable;
        }

        protected abstract INotifyEnumerable<T> AsNotifiableCore();

        public abstract IEnumerator<T> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }
    }

    internal class ExceptExpression<T> : SetExpression<T>
    {
        public ExceptExpression(IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer) : base(source, other, comparer) { }

        protected override INotifyEnumerable<T> AsNotifiableCore()
        {
            var otherExpression = Other as IEnumerableExpression<T>;
            IEnumerable<T> other = Other;
            if (otherExpression != null)
            {
                other = otherExpression.AsNotifiable();
            }
            return Source.AsNotifiable().Except(other, Comparer);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.Except(Source, Other, Comparer).GetEnumerator();
        }
    }

    internal class UnionExpression<T> : SetExpression<T>
    {
        public UnionExpression(IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer) : base(source, other, comparer) { }

        protected override INotifyEnumerable<T> AsNotifiableCore()
        {
            var otherExpression = Other as IEnumerableExpression<T>;
            IEnumerable<T> other = Other;
            if (otherExpression != null)
            {
                other = otherExpression.AsNotifiable();
            }
            return Source.AsNotifiable().Union(other, Comparer);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.Union(Source, Other, Comparer).GetEnumerator();
        }
    }

    internal class IntersectExpression<T> : SetExpression<T>
    {
        public IntersectExpression(IEnumerableExpression<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer) : base(source, other, comparer) { }

        protected override INotifyEnumerable<T> AsNotifiableCore()
        {
            var otherExpression = Other as IEnumerableExpression<T>;
            IEnumerable<T> other = Other;
            if (otherExpression != null)
            {
                other = otherExpression.AsNotifiable();
            }
            return Source.AsNotifiable().Intersect(other, Comparer);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.Intersect(Source, Other, Comparer).GetEnumerator();
        }
    }
}
