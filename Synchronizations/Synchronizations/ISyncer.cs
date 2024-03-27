using System;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes an object that can synchronize properties
    /// </summary>
    /// <typeparam name="TLeft">The left type of elements</typeparam>
    /// <typeparam name="TRight">The right type of elements</typeparam>
    public interface ISyncer<TLeft, TRight>
    {
        /// <summary>
        /// Synchronizes the given instances
        /// </summary>
        /// <param name="left">The left element</param>
        /// <param name="right">The right element</param>
        /// <param name="context">The context in which to synchronize</param>
        /// <returns>A disposable object that can be used to terminate the synchronization</returns>
        IDisposable Sync(TLeft left, TRight right, ISynchronizationContext context);
    }
}
