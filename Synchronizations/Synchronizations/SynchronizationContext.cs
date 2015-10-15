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
        private SynchronizationDirection direction;
        private ChangePropagationMode changePropagation;

        public SynchronizationContext(Synchronization synchronization, SynchronizationDirection direction, ChangePropagationMode changePropagation)
            : base(synchronization)
        {
            this.direction = direction;
            this.changePropagation = changePropagation;
        }

        public SynchronizationDirection Direction
        {
            get { return direction; }
        }

        public ChangePropagationMode ChangePropagation
        {
            get { return changePropagation; }
        }
    }
}
