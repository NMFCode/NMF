using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a job that is executed when a new correspondence is found
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    public interface ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        /// <summary>
        /// True, if the job must be executed before any dependency, otherwise False
        /// </summary>
        bool IsEarly { get; }

        /// <summary>
        /// Performs the job
        /// </summary>
        /// <param name="computation">The correspondence</param>
        /// <param name="direction">The direction of the synchronization</param>
        /// <param name="context">The context of the synchronization</param>
        /// <returns>A disposable that will be disposed once the correspondence is broken or null</returns>
        IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context);
    }
}
