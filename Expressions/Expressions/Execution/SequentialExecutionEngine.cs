using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class SequentialExecutionEngine : ExecutionEngine
    {
        protected override void Execute(List<INotifiable> nodes)
        {
            foreach (var node in nodes)
                MarkNode(node);

            foreach (var source in nodes)
                NotifyNode(source);
        }

        private void NotifyNode(INotifiable node)
        {
            int currentValue = 1;
            bool evaluating = true;

            while (true)
            {
                var metaData = node.ExecutionMetaData;
                metaData.RemainingVisits -= currentValue;
                if (metaData.RemainingVisits > 0)
                    return;

                currentValue = metaData.TotalVisits;
                metaData.TotalVisits = 0;

                INotificationResult result = null;
                if (evaluating || metaData.Results.Count > 0)
                {
                    result = node.Notify(metaData.Results.Values);
                    evaluating = result.Changed;
                    metaData.Results.Clear();
                }

                if (node.Successors.HasSuccessors)
                {
                    node = node.Successors[0];
                    if (result != null && evaluating)
                        node.ExecutionMetaData.Results.UnsafeAdd(result);
                }
                else
                    break;
            }
        }

        private void MarkNode(INotifiable node)
        {
            do
            {
                var metaData = node.ExecutionMetaData;
                metaData.TotalVisits++;
                metaData.RemainingVisits++;
            }
            while (node.Successors.HasSuccessors && (node = node.Successors[0]) != null);
        }
    }
}
