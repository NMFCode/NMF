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
        protected abstract void HandleReadyComputation(Computation computation);

        void computation_OutputInitialized(object sender, EventArgs e)
        {
            var computation = sender as Computation;
            if (computation != null)
            {
                HandleReadyComputation(computation);
                computation.OutputInitialized -= computation_OutputInitialized;
            }
        }

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
                    computation.OutputInitialized += computation_OutputInitialized;
                }
                else
                {
                    HandleReadyComputation(computation);
                }
            }
        }
    }
}
