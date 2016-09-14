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

        protected override void Schedule(HashSet<INotifiable> nodes)
        {
            var array = nodes.ToArray();
            var taskCount = Math.Min(MaxTasks, array.Length);
            var tasks = new Task[taskCount];
            int counter = taskCount - 1;

            Action<object> work = state =>
            {
                int startIndex = (int)state;
                NotifyNode(array[startIndex]);
                if (taskCount < array.Length)
                {
                    int index = Interlocked.Increment(ref counter);
                    while (index < array.Length)
                    {
                        NotifyNode(array[index]);
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
