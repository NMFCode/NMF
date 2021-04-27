using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes the context of a synchronization
    /// </summary>
    public interface ISynchronizationContext : ITransformationContext
    {
        /// <summary>
        /// Gets or sets the direction of the synchronization
        /// </summary>
        SynchronizationDirection Direction { get; set; }

        /// <summary>
        /// Gets the change propagation mode of the synchronization
        /// </summary>
        ChangePropagationMode ChangePropagation { get; }

        /// <summary>
        /// Gets a collection of inconsistencies found during the synchronization
        /// </summary>
        /// <remarks>This property is only used by the synchronization engine if the direction is set to CheckOnly</remarks>
        ICollectionExpression<IInconsistency> Inconsistencies { get; }
    }
}
