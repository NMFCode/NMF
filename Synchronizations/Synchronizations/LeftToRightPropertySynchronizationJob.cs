using System;
using System.Linq.Expressions;

namespace NMF.Synchronizations
{
    internal class LeftToRightPropertySynchronizationJob<TLeft, TRight, TValue> : OneWayPropertySynchronizationJob<TLeft, TRight, TValue>, ISynchronizationJob<TLeft, TRight>
    {
        public LeftToRightPropertySynchronizationJob(Expression<Func<TLeft, TValue>> leftGetter, Action<TRight, TValue> rightSetter, bool isEarly)
            : base(leftGetter, rightSetter, isEarly)
        { }

        public virtual IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            if (direction.IsLeftToRight())
            {
                return Perform(computation.Input, computation.Opposite.Input, context);
            }
            else
            {
                return null;
            }
        }
    }
}
