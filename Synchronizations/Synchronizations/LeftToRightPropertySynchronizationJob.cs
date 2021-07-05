using NMF.Transformations.Core;
using System;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue> : OneWayPropertySynchronizationJob<TLeft, TRight, TValue>, ISynchronizationJob<TLeft, TRight>
    {
        public LeftToRightPropertySynchronizationJob(Expression<Func<TLeft, ITransformationContext, TValue>> leftGetter, Action<TRight, ITransformationContext, TValue> rightSetter, bool isEarly)
            : base(leftGetter, rightSetter, isEarly)
        { }

        public virtual IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            if (direction.IsLeftToRight())
            {
                return Perform(computation.Input, computation.Opposite.Input, context);
            }
            else if(context.ChangePropagation == Transformations.ChangePropagationMode.TwoWay)
            {
                return RegisterChangePropagationOnly( computation.Input, computation.Opposite.Input, context );
            }
            else
            {
                return null;
            }
        }
    }
}
