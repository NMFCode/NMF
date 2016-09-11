using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public abstract class ParallelExecutionEngine : SequentialExecutionEngine
    {
        protected abstract void Schedule(HashSet<INotifiable> nodes);

        protected override void Execute(HashSet<INotifiable> nodes)
        {
            foreach (var node in nodes)
                MarkNode(node);

            Schedule(nodes);
        }

        protected void NotifyNode(INotifiable node, int currentValue, bool evaluating)
        {
            var metaData = node.ExecutionMetaData;
            if (metaData.RemainingVisits != currentValue)
            {
                int remaining = Interlocked.Add(ref metaData.RemainingVisits, -currentValue);
                if (remaining > 0)
                    return;
            }
            else
            {
                metaData.RemainingVisits = 0;
            }

            INotificationResult result = null;
            if (evaluating)
            {
                foreach (var dep in node.Dependencies)
                {
                    if (dep.ExecutionMetaData.Result != null)
                    {
                        metaData.Sources.Add(dep.ExecutionMetaData.Result);
                        dep.ExecutionMetaData.Result = null;
                    }
                }
                
                if (metaData.Sources.Count > 0)
                {
                    result = node.Notify(metaData.Sources);
                    evaluating = result.Changed;
                    if (evaluating)
                        metaData.Result = result;
                    currentValue = metaData.TotalVisits;
                    metaData.Sources.Clear();
                }
            }

            metaData.TotalVisits = 0;

            if (node.Successors.HasSuccessors)
            {
                var nextNode = node.Successors[0];
                NotifyNode(nextNode, currentValue, evaluating);
            }
        }

        private void MarkNode(INotifiable node)
        {
            var metaData = node.ExecutionMetaData;
            metaData.RemainingVisits++;
            metaData.TotalVisits++;

            if (node.Successors.HasSuccessors)
                MarkNode(node.Successors[0]);
        }
    }
}
