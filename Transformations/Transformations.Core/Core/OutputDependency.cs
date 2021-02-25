using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// Represents a default implementation for a dependency that requires a computation to have its output initialized
    /// </summary>
    public abstract class OutputDependency : ITransformationRuleDependency
    {
        /// <summary>
        /// Handles the computation that is ready (i.e. the output is clear)
        /// </summary>
        /// <param name="computation"></param>
        protected abstract void HandleReadyComputation(Computation computation);

        void Computation_OutputInitialized(object sender, EventArgs e)
        {
            var computation = sender as Computation;
            if (computation != null)
            {
                HandleReadyComputation(computation);
                computation.OutputInitialized -= Computation_OutputInitialized;
            }
        }

        /// <inheritdoc />
        public bool ExecuteBefore
        {
            get { return true; }
        }

        void ITransformationRuleDependency.HandleDependency(Computation computation)
        {
            if (computation != null)
            {
                if (computation.IsDelayed)
                {
                    computation.OutputInitialized += Computation_OutputInitialized;
                }
                else
                {
                    HandleReadyComputation(computation);
                }
            }
        }
    }
}
