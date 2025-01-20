using NMF.Models.Repository;

namespace NMF.Analyses
{
    /// <summary>
    /// Denotes a base class for an analysis
    /// </summary>
    public abstract class Analysis
    {
        /// <summary>
        /// Runs the analysis on the given repository
        /// </summary>
        /// <param name="repository">The repository</param>
        public abstract void Run(IModelRepository repository);
    }
}
