using System;

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
            if (sender is Computation computation)
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
