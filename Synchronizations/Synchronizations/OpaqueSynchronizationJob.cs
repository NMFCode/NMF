using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    internal class OpaqueSynchronizationJob<TLeft, TRight> : ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        public Func<TLeft, TRight, SynchronizationDirection, ISynchronizationContext, IDisposable> Action { get; private set;}

        public OpaqueSynchronizationJob(Func<TLeft, TRight, SynchronizationDirection, ISynchronizationContext, IDisposable> action)
        {
            Action = action;
        }

        public IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            if (Action != null)
            {
                return Action(computation.Input, computation.Opposite.Input, direction, context);
            }
            else
            {
                return null;
            }
        }

        public bool IsEarly
        {
            get { return false; }
        }
    }
}
