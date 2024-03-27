using System;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a synchronization job that is opque to the synchronization engine
    /// </summary>
    internal class OpaqueSynchronizationJob<TLeft, TRight> : ISynchronizationJob<TLeft, TRight>
    {
        /// <summary>
        /// Gets the action that should be performed
        /// </summary>
        public Func<TLeft, TRight, SynchronizationDirection, ISynchronizationContext, IDisposable> Action { get; private set;}

        /// <summary>
        /// Creates an opaque synchronization job
        /// </summary>
        /// <param name="action">The action that should be performed</param>
        /// <param name="isEarly">Determines whether the job should be executed early</param>
        public OpaqueSynchronizationJob(Func<TLeft, TRight, SynchronizationDirection, ISynchronizationContext, IDisposable> action, bool isEarly)
        {
            Action = action;
            IsEarly = isEarly;
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
            get;
        }
    }
}
