using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public class OpaqueSynchronizationJob<TLeft, TRight> : ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        public Action<TLeft, TRight, SynchronizationDirection, ISynchronizationContext> Action { get; private set;}

        public OpaqueSynchronizationJob(Action<TLeft, TRight, SynchronizationDirection, ISynchronizationContext> action)
        {
            Action = action;
        }

        public void Perform(TLeft left, TRight right, SynchronizationDirection direction, ISynchronizationContext context)
        {
            if (Action != null)
            {
                Action(left, right, direction, context);
            }
        }

        public void Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            if (Action != null)
            {
                Action(computation.Input, computation.Opposite.Input, direction, context);
            }
        }

        public bool IsEarly
        {
            get { return false; }
        }
    }
}
