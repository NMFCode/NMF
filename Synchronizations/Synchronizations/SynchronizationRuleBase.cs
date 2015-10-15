using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public abstract class SynchronizationRuleBase
    {
        protected internal SynchronizationRuleBase() { }

        public abstract Type LeftType { get; }

        public abstract Type RightType { get; }

        public abstract void DeclareSynchronization();

        internal abstract GeneralTransformationRule LTR { get; }

        internal abstract GeneralTransformationRule RTL { get; }

        public Synchronization Synchronization { get; internal set; }

        public TRule SyncRule<TRule>() where TRule : SynchronizationRuleBase
        {
            return Synchronization.GetSynchronizationRuleForType(typeof(TRule)) as TRule;
        }

        public TRule Rule<TRule>() where TRule : GeneralTransformationRule
        {
            return Synchronization.GetRuleForRuleType(typeof(TRule)) as TRule;
        }
    }
}
