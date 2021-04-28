using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Transformations.Parallel.Tasks
{
    /// <summary>
    /// An alternative implementation of a transformation context parallelized using the TPL
    /// </summary>
    public class TaskParallelTransformationContext2 : TransformationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="transformation"></param>
        public TaskParallelTransformationContext2(Transformation transformation) : base(transformation) { }

        /// <inheritdoc />
        protected override ComputationContext CreateComputationContext(object[] input, GeneralTransformationRule rule)
        {
            return new TaskParallelComputationContext(this);
        }

        
        /// <inheritdoc />
        protected override void ExecuteLevel(IList<Computation> computationsOfLevel)
        {
            var allTasks = new List<Task>();
            foreach (var item in computationsOfLevel)
            {
                if (item.Context is TaskParallelComputationContext cc)
                {
                    allTasks.Add(cc.Complete());
                }
                else
                {
                    throw new NotSupportedException();
                }
                OnComputationCompleted(new ComputationEventArgs(item));
            }
            Task.WaitAll(allTasks.ToArray());
        }
    }
}
