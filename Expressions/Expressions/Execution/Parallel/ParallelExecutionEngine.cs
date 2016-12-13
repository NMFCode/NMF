using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public abstract class ParallelExecutionEngine : ExecutionEngine
    {
        protected abstract void Schedule(List<INotifiable> nodes, Action<INotifiable> action);

        protected override void Execute(List<INotifiable> nodes)
        {
            Schedule(nodes, MarkNode);
            Schedule(nodes, NotifyNode);
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

                if (metaData.RemainingVisits != tuple.Item2)
                {
                    int remaining = Interlocked.Add(ref metaData.RemainingVisits, -tuple.Item2);
                    if (remaining > 0)
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
                    metaData.Results.Clear();
                }

                if (node.Successors.HasSuccessors)
                {
                    foreach (var succ in node.Successors)
                    {
                        if (result != null && evaluating)
                            succ.ExecutionMetaData.Results.Add(result);
                        stack.Push(new Tuple<INotifiable, int>(succ, metaData.TotalVisits));
                    }
                }

                metaData.TotalVisits = 0;
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
