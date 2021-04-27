using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes common useful extensions in the context of synchronizations
    /// </summary>
    public static class SynchronizationExtensions
    {
        /// <summary>
        /// Gets the synchronization rule of the given type
        /// </summary>
        /// <typeparam name="TRule">The type of synchronization rule</typeparam>
        /// <param name="rule">The transformation rule for which the synchronization rule is added</param>
        /// <returns>The synchronization rule or null, if it cannot be found</returns>
        public static TRule SyncRule<TRule>(this GeneralTransformationRule rule) where TRule : class
        {
            if(rule.Transformation is Synchronization synchronization)
            {
                return synchronization.GetSynchronizationRuleForType( typeof( TRule ) ) as TRule;
            }
            return null;
        }
    }
}
