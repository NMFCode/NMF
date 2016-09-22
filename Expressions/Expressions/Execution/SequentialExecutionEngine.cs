using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class SequentialExecutionEngine : ExecutionEngine
    {
        protected override void ExecuteSingle(INotifiable source)
        {
            var node = source;
            INotificationResult lastResult = null;

            while (node != null)
            {
                if (lastResult != null)
                    node.ExecutionMetaData.Sources.Add(lastResult);

                lastResult = node.Notify(node.ExecutionMetaData.Sources);
                node.ExecutionMetaData.Sources.Clear();

                if (lastResult.Changed && node.Successors.HasSuccessors)
                    node = node.Successors[0];
                else
                    break;
            }
        }

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
#if DEBUG
                if (metaData.RemainingVisits < 0)
                    throw new InvalidOperationException("RemainingVisits < 0: This should never happen!");
#endif

                INotificationResult result = null;
                if (evaluating || metaData.Sources.Count > 0)
                {
                    result = node.Notify(metaData.Sources);
                    evaluating = result.Changed;
                }

                currentValue = metaData.TotalVisits;
                metaData.TotalVisits = 0;
                metaData.Sources.Clear();

                if (node.Successors.HasSuccessors)
                {
                    node = node.Successors[0];
                    if (result != null && evaluating)
                        node.ExecutionMetaData.Sources.Add(result);
                }
                else
                    break;
            }
        }

        protected void MarkNode(INotifiable node)
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
