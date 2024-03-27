namespace NMF.Transformations.Core
{
    /// <summary>
    /// Denotes the transformation context of a parallel transformation engine
    /// </summary>
    public interface ITransformationEngineContext : ITransformationContext
    {
        /// <summary>
        /// Waits until all pending actions have been processed
        /// </summary>
        void ExecutePending();
    }
}
