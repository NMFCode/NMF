using NMF.Transformations.Core;
using System;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class RightToLeftPropertySynchronizationJob<TLeft, TRight, TValue> : OneWayPropertySynchronizationJob<TRight, TLeft, TValue>, ISynchronizationJob<TLeft, TRight>
    {
        public RightToLeftPropertySynchronizationJob(Action<TLeft, ITransformationContext, TValue> leftSetter, Expression<Func<TRight, ITransformationContext, TValue>> rightGetter, bool isEarly)
            : base(rightGetter, leftSetter, isEarly)
        { }

        public virtual IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            if (direction.IsRightToLeft())
            {
                return Perform(computation.Opposite.Input, computation.Input, context);
            }
            else if (context.ChangePropagation == Transformations.ChangePropagationMode.TwoWay)
            {
                return RegisterChangePropagationOnly( computation.Opposite.Input, computation.Input, context );
            }
            else
            {
                return null;
            }
        }
    }
}
