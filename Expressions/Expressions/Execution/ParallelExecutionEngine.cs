using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an incrementalization system where changes are propagated in parallel
    /// </summary>
    public class ParallelExecutionEngine : ExecutionEngine
    {
        private static readonly int MaxTasks = Math.Min(64, Environment.ProcessorCount * 2);

        /// <inheritdoc />
        protected override void Execute(List<INotifiable> nodes)
        {
            ScheduleMark(nodes);
            ScheduleNotify(nodes);
        }

        private void ScheduleMark(List<INotifiable> nodes)
        {
            var taskCount = Math.Min(MaxTasks, nodes.Count);
            var tasks = new Task[taskCount - 1];
            int counter = taskCount - 1;

            Action<object> work = state =>
            {
                int startIndex = (int)state;
                MarkNode(nodes[startIndex]);
                if (taskCount < nodes.Count)
                {
                    int index = Interlocked.Increment(ref counter);
                    while (index < nodes.Count)
                    {
                        MarkNode(nodes[index]);
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


        private void ScheduleNotify(List<INotifiable> nodes)
        {
            var taskCount = Math.Min(MaxTasks, nodes.Count);
            var tasks = new Task[taskCount - 1];
            int counter = taskCount - 1;

            Action<object> work = state =>
            {
                int startIndex = (int)state;
                NotifyNode(nodes[startIndex], true, 1);
                if (taskCount < nodes.Count)
                {
                    int index = Interlocked.Increment(ref counter);
                    while (index < nodes.Count)
                    {
                        NotifyNode(nodes[index], true, 1);
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

        private static bool ShouldProceed(ExecutionMetaData metadata, int visits)
        {
            if (metadata.RemainingVisits == 0)
            {
                return false;
            }

            if (metadata.RemainingVisits != visits)
            {
                int remaining = Interlocked.Add(ref metadata.RemainingVisits, -visits);
                if (remaining != 0)
                    return false;
            }

            metadata.RemainingVisits = 0;
            return true;
        }

        private void NotifyNode(INotifiable node, bool evaluating, int visits)
        {            
            while (node != null)
            {
                var metaData = node.ExecutionMetaData;
                if (!ShouldProceed(metaData, visits)) return;

                INotificationResult result = null;
                VisitNode(node, ref evaluating, metaData, ref result);

                visits = metaData.TotalVisits;
                metaData.RemainingVisits = 0;
                metaData.TotalVisits = 0;

                if (node.Successors.HasSuccessors)
                {
                    if (node.Successors.Count == 1)
                    {
                        node = MoveToSingleSuccessor(node, evaluating, result);
                    }
                    else
                    {
                        NotifyChildrenInParallel(node, visits, evaluating ? result : null);
                        return;
                    }
                }
            }
        }

        private static INotifiable MoveToSingleSuccessor(INotifiable node, bool evaluating, INotificationResult result)
        {
            var successor = node.Successors.GetSuccessor(0);
            if (evaluating)
            {
                result.IncreaseReferences(node.Successors.Count);
                successor.ExecutionMetaData.Results.Add(result);
            }
            return successor;
        }

        private void NotifyChildrenInParallel(INotifiable node, int visits, INotificationResult result)
        {
            var childTasks = new Task[node.Successors.Count];
            for (int i = 0; i < node.Successors.Count && i < childTasks.Length; i++)
            {
                var successor = node.Successors.GetSuccessor(i);
                if (result != null)
                {
                    successor.ExecutionMetaData.Results.Add(result);
                }
                childTasks[i] = Task.Factory.StartNew(() => NotifyNode(successor, true, visits));
            }

            Task.WaitAll(childTasks);
        }

        private static void VisitNode(INotifiable node, ref bool evaluating, ExecutionMetaData metaData, ref INotificationResult result)
        {
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
        }

        private void MarkNode(object state)
        {
            if (state is INotifiable notifiable)
            {
                MarkNode(notifiable);
            }
        }

        private void MarkNode(INotifiable node)
        {
            List<Task> tasksToWait = null;
            do
            {
                MarkNodeCore(ref node, ref tasksToWait);
            }
            while (node != null);
            if (tasksToWait != null)
            {
                Task.WaitAll(tasksToWait.ToArray());
            }
        }

        private void MarkNodeCore(ref INotifiable node, ref List<Task> tasksToWait)
        {
            var metaData = node.ExecutionMetaData;
            Interlocked.Increment(ref metaData.RemainingVisits);
            Interlocked.Increment(ref metaData.TotalVisits);
            var succesors = node.Successors;
            if (succesors.HasSuccessors)
            {
                node = succesors.GetSuccessor(0);
                if (succesors.Count > 1)
                {
                    if (tasksToWait == null)
                    {
                        tasksToWait = new List<Task>();
                    }
                    for (int i = 1; i < succesors.Count; i++)
                    {
                        tasksToWait.Add(Task.Factory.StartNew(MarkNode, succesors.GetSuccessor(i)));
                    }
                }
            }
            else
            {
                node = null;
            }
        }
    }
}
