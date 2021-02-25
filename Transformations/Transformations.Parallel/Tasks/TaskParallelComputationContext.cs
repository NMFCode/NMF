using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Transformations.Parallel.Tasks
{
    /// <summary>
    /// Denotes a computation context that uses the TPL for parallel task execution
    /// </summary>
    public class TaskParallelComputationContext : ComputationContext
    {
        private readonly List<Computation> computations = new List<Computation>();
        private List<Task> transformationRequirements;
        private readonly Task transformTask;

        private void Transform()
        {
            foreach (var item in computations)
            {
                item.Transform();
            }
        }

        /// <summary>
        /// Waits until all transformations are computed
        /// </summary>
        /// <returns></returns>
        public async Task Complete()
        {
            if (transformationRequirements != null)
            {
                await Task.WhenAll(transformationRequirements);
            }
            Transform();
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context"></param>
        public TaskParallelComputationContext(ITransformationContext context)
            : base(context)
        {
            transformTask = new Task(Transform);
        }

        /// <inheritdoc />
        public override void ConnectWith(Computation computation)
        {
            computations.Add(computation);
        }

        /// <inheritdoc />
        public override void MarkRequire(Computation other, bool isRequired)
        {
            base.MarkRequire(other, isRequired);
            var context = other.Context as TaskParallelComputationContext;
            if (isRequired && context != null && context != this)
            {
                if (transformationRequirements == null) transformationRequirements = new List<Task>();
                transformationRequirements.Add(context.transformTask);
            }
        }
    }
}
