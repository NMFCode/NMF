using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// Represents an interface for dependencies for transformation rules
    /// </summary>
    public interface ITransformationRuleDependency
    {
        /// <summary>
        /// Calls the transformation dependency for the given computation
        /// </summary>
        /// <param name="computation">The computation that this dependency is to be called</param>
        void HandleDependency(Computation computation);

        /// <summary>
        /// Gets a value indicating whether the dependency should be executed before or after the computation is added to the computation order
        /// </summary>
        bool ExecuteBefore { get; }
    }
}
