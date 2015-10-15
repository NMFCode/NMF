using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Transformations.Parallel.Tasks
{
    public class TaskParallelTransformationContext2 : TransformationContext
    {
        public TaskParallelTransformationContext2(Transformation transformation) : base(transformation) { }

        protected override ComputationContext CreateComputationContext(object[] input, GeneralTransformationRule rule)
        {
            return new TaskParallelComputationContext(this);
        }

        protected override void ExecuteLevel(IList<Computation> computationsOfLevel)
        {
            var allTasks = new List<Task>();
            foreach (var item in computationsOfLevel)
            {
                var cc = item.Context as TaskParallelComputationContext;
                if (cc != null)
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
