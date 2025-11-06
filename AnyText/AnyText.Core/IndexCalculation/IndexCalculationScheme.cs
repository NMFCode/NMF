using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText.IndexCalculation
{
    /// <summary>
    /// Denotes an abstract base class to define a schema how to calculate a collection index
    /// </summary>
    public abstract class IndexCalculationScheme
    {
        /// <summary>
        /// Calculates the collection index for the given rule application
        /// </summary>
        /// <param name="ruleApplication">the rule application</param>
        /// <returns>the index in the parent collection or -1, if no such index can be calculated</returns>
        public abstract int CalculateIndex(RuleApplication ruleApplication);

        /// <summary>
        /// Denotes an index calculation scheme that simply looks up the index in the closest star rule application
        /// </summary>
        public static readonly IndexCalculationScheme Simple = new SimpleIndexCalculationScheme();

        /// <summary>
        /// Denotes an index calculation scheme that iterates the closest star rule application, respecting the rule stack
        /// </summary>
        public static readonly IndexCalculationScheme Detailed = new DetailedIndexCalculationScheme();

        /// <summary>
        /// Denotes an index calculation scheme that iterates all rule applications from the parent definition in a depth-first-search
        /// </summary>
        public static readonly IndexCalculationScheme Heterogeneous = new HeterogeneousIndexCalculationScheme(int.MaxValue);

        /// <summary>
        /// Denotes an index calculation scheme that iterates all rule applications from the parent definition in a depth-first-search
        /// </summary>
        /// <param name="maxDepth">the maximum depth of parse expressions to look at</param>
        /// <returns>An index calculation scheme with the desired max depth</returns>
        public static IndexCalculationScheme HeterogeneousWithMaxDepth(int maxDepth) => new HeterogeneousIndexCalculationScheme(maxDepth);
    }
}
