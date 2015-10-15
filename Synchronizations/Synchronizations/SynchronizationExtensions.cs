using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Synchronizations
{
    public static class SynchronizationExtensions
    {
        public static TRule SyncRule<TRule>(this GeneralTransformationRule rule) where TRule : class
        {
            var synchronization = rule.Transformation as Synchronization;
            if (synchronization != null)
            {
                return synchronization.GetSynchronizationRuleForType(typeof(TRule)) as TRule;
            }
            return null;
        }
    }
}
