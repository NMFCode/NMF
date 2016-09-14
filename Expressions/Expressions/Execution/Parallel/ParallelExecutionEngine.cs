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
        
        protected void NotifyNode(INotifiable node)
        {
            int currentValue = 1;
            bool evaluating = true;
            bool isFirst = true;

            do
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

                    if (isFirst || metaData.Sources.Count > 0)
                    {
                        result = node.Notify(metaData.Sources);
                        evaluating = result.Changed;
                        if (evaluating && node.Successors.HasSuccessors)
                            metaData.Result = result;
                        currentValue = metaData.TotalVisits;
                        metaData.Sources.Clear();
                    }
                }

                metaData.TotalVisits = 0;
                isFirst = false;
            }
            while (node.Successors.HasSuccessors && (node = node.Successors[0]) != null);
        }
    }
}
