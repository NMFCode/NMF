using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Transformations.Parallel.Tasks
{
    public class TaskParallelComputationContext : ComputationContext
    {
        private List<Computation> computations = new List<Computation>();
        private List<Task> transformationRequirements;
        private Task transformTask;

        private void Transform()
        {
            foreach (var item in computations)
            {
                item.Transform();
            }
        }

        public async Task Complete()
        {
            if (transformationRequirements != null)
            {
                await Task.WhenAll(transformationRequirements);
            }
            Transform();
        }

        public TaskParallelComputationContext(ITransformationContext context)
            : base(context)
        {
            transformTask = new Task(Transform);
        }

        public override void ConnectWith(Computation computation)
        {
            computations.Add(computation);
        }

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
