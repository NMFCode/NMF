using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a context of a model synchronization run
    /// </summary>
    public interface ISynchronizationContext : ITransformationContext, ITransformationEngineContext, IDisposable
    {
        /// <summary>
        /// Gets the direction of the synchronization process
        /// </summary>
        SynchronizationDirection Direction { get; set; }

        /// <summary>
        /// Gets the change propagation mode of the model synchronization
        /// </summary>
        ChangePropagationMode ChangePropagation { get; }

        /// <summary>
        /// Gets a collection of inconsistencies found during the synchronization
        /// </summary>
        /// <remarks>This property is only used by the synchronization engine if the direction is set to CheckOnly</remarks>
        ICollectionExpression<IInconsistency> Inconsistencies { get; }
    }
}
