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

        private void NotifyNode(INotifiable node, bool evaluating, int visits)
        {            
            while (node != null)
            {
                var metaData = node.ExecutionMetaData;

                if (metaData.RemainingVisits == 0)
                {
                    return;
                }
                if (metaData.RemainingVisits != visits)
                {
                    int remaining = Interlocked.Add(ref metaData.RemainingVisits, -visits);
                    if (remaining != 0)
                        return;
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

                visits = metaData.TotalVisits;
                metaData.RemainingVisits = 0;
                metaData.TotalVisits = 0;

                if (node.Successors.HasSuccessors)
                {
                    if (evaluating)
                    {
                        result.IncreaseReferences(node.Successors.Count);
                        if (node.Successors.Count == 1)
                        {
                            node = node.Successors.GetSuccessor(0);
                            node.ExecutionMetaData.Results.Add(result);
                        }
                        else
                        {
                            var childTasks = new Task[node.Successors.Count];
                            for (int i = 0; i < node.Successors.Count && i < childTasks.Length; i++)
                            {
                                var successor = node.Successors.GetSuccessor(i);
                                successor.ExecutionMetaData.Results.Add(result);
                                childTasks[i] = Task.Factory.StartNew(() => NotifyNode(successor, true, visits));
                            }
                            Task.WaitAll(childTasks);
                            return;
                        }
                    }
                    else
                    {
                        if (node.Successors.Count == 1)
                        {
                            node = node.Successors.GetSuccessor(0);
                        }
                        else
                        {
                            var childTasks = new Task[node.Successors.Count];
                            for (int i = 0; i < node.Successors.Count; i++)
                            {
                                var successor = node.Successors.GetSuccessor(i);
                                childTasks[i] = Task.Factory.StartNew(() => NotifyNode(successor, true, visits));
                            }
                            Task.WaitAll(childTasks);
                            return;
                        }

                    }
                }
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
            while (node != null);
            if (tasksToWait != null)
            {
                Task.WaitAll(tasksToWait.ToArray());
            }
        }
    }
}
