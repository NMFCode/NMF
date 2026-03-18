using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations.Core;
using System;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue> : OneWayPropertySynchronizationJob<TLeft, TRight, TValue>, 
        ISynchronizationJob<TLeft, TRight>, ISyncer<TLeft, TRight>,
        IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue>,
        IInconsistencyDescriptor<TLeft, TRight, TValue, TValue>
    {
        public LeftToRightPropertySynchronizationJob(Expression<Func<TLeft, ITransformationContext, TValue>> leftGetter, Func<TRight, ITransformationContext, TValue> rightGetter, Action<TRight, ITransformationContext, TValue> rightSetter, bool isEarly)
            : base(leftGetter, rightGetter, rightSetter, isEarly)
        { }

        private Func<TLeft, TRight, TValue, TValue, string> _descriptor;

        public string DescribeLeft(TLeft left, TRight right, TValue depLeft, TValue depRight)
        {
            return null;
        }

        public string DescribeRight(TLeft left, TRight right, TValue depLeft, TValue depRight)
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

        public IInconsistencyDescriptorSyntaxRight<TLeft, TRight, TValue, TValue> DescribeRightChange(Func<TLeft, TRight, TValue, TValue, string> descriptor)
        {
            _descriptor = descriptor;
            return this;
        }

        public virtual IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
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
                return PerformCheckOnly(left, right, context, CreateInconsistency);
            }
            else if (direction.IsLeftToRight())
            {
                return Perform(left, right, context);
            }
            else if (context.ChangePropagation == Transformations.ChangePropagationMode.TwoWay)
            {
                return RegisterChangePropagationOnly(left, right, context);
            }
            else
            {
                return null;
            }
        }

        private IInconsistency CreateInconsistency(TLeft source, TRight target, TValue sourceValue, TValue targetValue, ISynchronizationContext context)
        {
            return new PropertyInequality<TLeft, TRight, TValue>(source, null, sourceValue, target, (t, v) => targetSetter(t, context, v), targetValue, this);
        }
    }
}
