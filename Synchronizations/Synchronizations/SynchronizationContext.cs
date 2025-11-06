using NMF.Collections.ObjectModel;
using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Linq;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a context in which a synchroniation takes place
    /// </summary>
    public class SynchronizationContext : TransformationContext, ISynchronizationContext, IDisposable
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="synchronization">The synchronization that should be executed</param>
        /// <param name="direction">The direction of the synchronization</param>
        /// <param name="changePropagation">The change propagation mode of the synchronization</param>
        public SynchronizationContext(Synchronization synchronization, SynchronizationDirection direction, ChangePropagationMode changePropagation)
            : base(synchronization)
        {
            Direction = direction;
            ChangePropagation = changePropagation;
            Inconsistencies = new ObservableSet<IInconsistency>();
        }

        /// <inheritdoc />
        public SynchronizationDirection Direction
        {
            get; set;
        }

        /// <inheritdoc />
        public ChangePropagationMode ChangePropagation
        {
            get;
        }

        /// <inheritdoc />
        public ICollectionExpression<IInconsistency> Inconsistencies { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var item in Trace.All.OfType<IDisposable>())
            {
                item.Dispose();
            }
        }
    }
}
