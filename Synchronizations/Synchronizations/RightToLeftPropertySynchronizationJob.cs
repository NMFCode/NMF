using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations.Core;
using System;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue> : OneWayPropertySynchronizationJob<TRight, TLeft, TValue>,
        ISynchronizationJob<TLeft, TRight>, ISyncer<TLeft, TRight>,
        IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue>,
        IInconsistencyDescriptor<TLeft, TRight, TValue, TValue>
    {
        public RightToLeftPropertySynchronizationJob(Func<TLeft, ITransformationContext, TValue> leftGetter, Action<TLeft, ITransformationContext, TValue> leftSetter, Expression<Func<TRight, ITransformationContext, TValue>> rightGetter, bool isEarly)
            : base(rightGetter, leftGetter, leftSetter, isEarly)
        { }

        private Func<TLeft, TRight, TValue, TValue, string> _descriptor;

        public string DescribeLeft(TLeft left, TRight right, TValue depLeft, TValue depRight)
        {
            if (_descriptor != null)
            {
                return _descriptor(left, right, depLeft, depRight);
            }
            if (depLeft != null)
            {
                return $"Add {depLeft} to {right}";
            }
            else
            {
                return $"Remove {depRight} (missing in {left})";
            }
        }

        public IInconsistencyDescriptorSyntaxLeft<TLeft, TRight, TValue, TValue> DescribeLeftChange(Func<TLeft, TRight, TValue, TValue, string> descriptor)
        {
            _descriptor = descriptor;
            return this;
        }

        public string DescribeRight(TLeft left, TRight right, TValue depLeft, TValue depRight)
        {
            return null;
        }

        public IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            return SyncInternal(computation.Input, computation.Opposite.Input, context, direction);
        }

        public IDisposable Sync(TLeft left, TRight right, ISynchronizationContext context)
        {
            return SyncInternal(left, right, context, context.Direction);
        }

        private IDisposable SyncInternal(TLeft left, TRight right, ISynchronizationContext context, SynchronizationDirection direction)
        {
            if (direction == SynchronizationDirection.CheckOnly)
            {
                return PerformCheckOnly(right, left, context, CreateInconsistency);
            }
            if (direction.IsRightToLeft())
            {
                return Perform(right, left, context);
            }
            else if (context.ChangePropagation == Transformations.ChangePropagationMode.TwoWay)
            {
                return RegisterChangePropagationOnly(right, left, context);
            }
            else
            {
                return null;
            }
        }

        private IInconsistency CreateInconsistency(TRight source, TLeft target, TValue sourceValue, TValue targetValue, ISynchronizationContext context)
        {
            return new PropertyInequality<TLeft, TRight, TValue>(target, (t, v) => targetSetter(t, context, v), targetValue, source, null, sourceValue, this);
        }
    }
}
