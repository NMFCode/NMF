using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public class ParallelArrayExecutionEngine : ParallelExecutionEngine
    {
        private static readonly int MaxTasks = Math.Min(64, Environment.ProcessorCount * 2);

        protected override void Schedule(List<INotifiable> nodes, Action<INotifiable> action)
        {
            var taskCount = Math.Min(MaxTasks, nodes.Count);
            var tasks = new Task[taskCount - 1];
            int counter = taskCount - 1;

            Action<object> work = state =>
            {
                int startIndex = (int)state;
                action(nodes[startIndex]);
                if (taskCount < nodes.Count)
                {
                    int index = Interlocked.Increment(ref counter);
                    while (index < nodes.Count)
                    {
                        action(nodes[index]);
                        index = Interlocked.Increment(ref counter);
                    }
                }
            };
            
            for (int i = 0; i < taskCount - 1; i++)
            {
                tasks[i] = Task.Factory.StartNew(work, i);
            }
            work(taskCount - 1);

            Task.WaitAll(tasks);
        }
    }
}
