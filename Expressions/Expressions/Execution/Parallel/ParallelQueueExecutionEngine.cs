using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public class ParallelQueueExecutionEngine : ParallelExecutionEngine
    {
        private static readonly int MaxTaskCount = Environment.ProcessorCount;

        protected override void Schedule(List<INotifiable> nodes, Action<INotifiable> action)
        {
            var taskCount = Math.Min(MaxTaskCount, nodes.Count);
            var queue = new ConcurrentQueue<INotifiable>(nodes);
            var tasks = new Task[taskCount];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    INotifiable node;
                    while (!queue.IsEmpty)
                    {
                        if (queue.TryDequeue(out node))
                            action(node);
                    }
                });
            }
            Task.WaitAll(tasks);
        }
    }
}
