using NMF.Transformations.Core;
using System;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue> : OneWayPropertySynchronizationJob<TLeft, TRight, TValue>, ISynchronizationJob<TLeft, TRight>, ISyncer<TLeft, TRight>
    {
        public LeftToRightPropertySynchronizationJob(Expression<Func<TLeft, ITransformationContext, TValue>> leftGetter, Action<TRight, ITransformationContext, TValue> rightSetter, bool isEarly)
            : base(leftGetter, rightSetter, isEarly)
        { }

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
            if (direction.IsLeftToRight())
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
    }
}
