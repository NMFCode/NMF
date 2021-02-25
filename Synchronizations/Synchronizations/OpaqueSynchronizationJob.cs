using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a synchronization job that is opque to the synchronization engine
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    internal class OpaqueSynchronizationJob<TLeft, TRight> : ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        /// <summary>
        /// Gets the action that should be performed
        /// </summary>
        public Func<TLeft, TRight, SynchronizationDirection, ISynchronizationContext, IDisposable> Action { get; private set;}

        /// <summary>
        /// Creates an opaque synchronization job
        /// </summary>
        /// <param name="action">The action that should be performed</param>
        public OpaqueSynchronizationJob(Func<TLeft, TRight, SynchronizationDirection, ISynchronizationContext, IDisposable> action)
        {
            Action = action;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public bool IsEarly
        {
            get { return false; }
        }
    }
}
