using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class represents a (non-calling) dependency
    /// </summary>
    public abstract class Dependency : ITransformationRuleDependency
    {
        internal GeneralTransformationRule BaseTransformation { get; set; }
        internal GeneralTransformationRule DependencyTransformation { get; set; }
        internal Predicate<Computation> Filter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dependency needs the output of the base computation set
        /// </summary>
        public bool NeedOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dependency is executed before or after the base computation
        /// </summary>
        public bool ExecuteBefore { get; set;}

        /// <summary>
        /// Calls the transformation dependency for the given computation
        /// </summary>
        /// <param name="computation">The computation that this dependency is to be called</param>
        public abstract void HandleDependency(Computation computation);

        bool ITransformationRuleDependency.ExecuteBefore { get { return ExecuteBefore; } }
    }
}
