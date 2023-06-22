using NMF.Transformations.Core;
using System;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue> : OneWayPropertySynchronizationJob<TRight, TLeft, TValue>, ISynchronizationJob<TLeft, TRight>, ISyncer<TLeft, TRight>
    {
        public RightToLeftPropertySynchronizationJob(Action<TLeft, ITransformationContext, TValue> leftSetter, Expression<Func<TRight, ITransformationContext, TValue>> rightGetter, bool isEarly)
            : base(rightGetter, leftSetter, isEarly)
        { }

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
    }
}
