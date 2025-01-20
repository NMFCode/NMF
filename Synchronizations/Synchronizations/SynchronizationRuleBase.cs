using NMF.Transformations.Core;
using System;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes an abstract synchronization rule
    /// </summary>
    public abstract class SynchronizationRuleBase
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected internal SynchronizationRuleBase() { }

        /// <summary>
        /// Gets the LHS type of the synchronization rule
        /// </summary>
        public abstract Type LeftType { get; }

        /// <summary>
        /// Gets the RHS type of the synchronization rule
        /// </summary>
        public abstract Type RightType { get; }

        /// <summary>
        /// Declares the jobs and synchronization blocks that make up the synchronization rule
        /// </summary>
        public abstract void DeclareSynchronization();

        internal abstract GeneralTransformationRule LTR { get; }

        internal abstract GeneralTransformationRule RTL { get; }

        /// <summary>
        /// Gets the context synchronization
        /// </summary>
        public Synchronization Synchronization { get; internal set; }

        /// <summary>
        /// Gets the synchroniation rule of the given type
        /// </summary>
        /// <typeparam name="TRule">The type of the synchronization rule</typeparam>
        /// <returns>The synchronization rule instance of the given type or null, if no such rule exists</returns>
        public TRule SyncRule<TRule>() where TRule : SynchronizationRuleBase
        {
            return Synchronization.GetSynchronizationRuleForType(typeof(TRule)) as TRule;
        }

        /// <summary>
        /// Gets the transformation rule type of the given type
        /// </summary>
        /// <typeparam name="TRule">The type of the transformation rule</typeparam>
        /// <returns>The transformation rule instance of the given type or null, if no such rule exists</returns>
        public TRule Rule<TRule>() where TRule : GeneralTransformationRule
        {
            return Synchronization.GetRuleForRuleType(typeof(TRule)) as TRule;
        }
    }
}
