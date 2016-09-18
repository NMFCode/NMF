using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public class ParallelArrayExecutionEngine : ParallelExecutionEngine
    {
        private static readonly int MaxTasks = Math.Min(64, Environment.ProcessorCount);

        protected override void Schedule(List<INotifiable> nodes)
        {
            var taskCount = Math.Min(MaxTasks, nodes.Count);
            var tasks = new Task[taskCount];
            int counter = taskCount - 1;

            Action<object> work = state =>
            {
                int startIndex = (int)state;
                NotifyNode(nodes[startIndex]);
                if (taskCount < nodes.Count)
                {
                    int index = Interlocked.Increment(ref counter);
                    while (index < nodes.Count)
                    {
                        NotifyNode(nodes[index]);
                        index = Interlocked.Increment(ref counter);
                    }
                }
            };
            
            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Factory.StartNew(work, i);
            }

            Task.WaitAll(tasks);
        }
    }
}
