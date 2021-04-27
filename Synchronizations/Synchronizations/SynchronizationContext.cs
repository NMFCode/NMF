using NMF.Collections.ObjectModel;
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
    public class SynchronizationContext : TransformationContext, ISynchronizationContext
    {
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
    }
}
