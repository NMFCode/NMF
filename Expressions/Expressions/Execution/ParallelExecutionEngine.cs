using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public class ParallelExecutionEngine : ExecutionEngine
    {
        private static readonly int MaxTasks = Math.Min(64, Environment.ProcessorCount * 2);

        protected override void Execute(List<INotifiable> nodes)
        {
            Schedule(nodes, MarkNode);
            Schedule(nodes, NotifyNode);
        }

        private void Schedule(List<INotifiable> nodes, Action<INotifiable> action)
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

        private void NotifyNode(INotifiable source)
        {
            bool evaluating = true;
            var stack = new Stack<Tuple<INotifiable, int>>();
            stack.Push(new Tuple<INotifiable, int>(source, 1));

            while (stack.Count > 0)
            {
                var tuple = stack.Pop();
                var node = tuple.Item1;
                var metaData = node.ExecutionMetaData;

                if (metaData.RemainingVisits == 0)
                {
                    continue;
                }
                if (metaData.RemainingVisits != tuple.Item2)
                {
                    int remaining = Interlocked.Add(ref metaData.RemainingVisits, -tuple.Item2);
                    if (remaining != 0)
                        continue;
                }
                else
                {
                    metaData.RemainingVisits = 0;
                }

                INotificationResult result = null;
                if (evaluating || metaData.Results.Count > 0)
                {
                    result = node.Notify(metaData.Results);
                    evaluating = result.Changed;
                    foreach (var item in metaData.Results)
                    {
                        item.FreeReference();
                    }
                    metaData.Results.Clear();
                }

                if (node.Successors.HasSuccessors)
                {
                    if (evaluating)
                    {
                        result.IncreaseReferences(node.Successors.Count);
                        foreach (var succ in node.Successors)
                        {
                            if (result != null)
                                succ.ExecutionMetaData.Results.Add(result);
                            stack.Push(new Tuple<INotifiable, int>(succ, metaData.TotalVisits));
                        }
                    }
                    else
                    {
                        foreach (var succ in node.Successors)
                        {
                            stack.Push(new Tuple<INotifiable, int>(succ, metaData.TotalVisits));
                        }
                    }
                }

                metaData.TotalVisits = 0;
                metaData.RemainingVisits = 0;
            }
        }

        private void MarkNode(INotifiable node)
        {
            do
            {
                var metaData = node.ExecutionMetaData;
                Interlocked.Increment(ref metaData.RemainingVisits);
                Interlocked.Increment(ref metaData.TotalVisits);
            }
            while (node.Successors.HasSuccessors && (node = node.Successors[0]) != null);
        }
    }
}
