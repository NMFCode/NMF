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

#if DEBUG
                if (node.Successors.Count > 1)
                    throw new InvalidOperationException("Node has more than one successor: This should never happen!");
#endif

                if (lastResult.Changed && node.Successors.Count > 0)
                    node = node.Successors[0];
                else
                    break;
            }
        }

        protected override void Execute(HashSet<INotifiable> nodes)
        {
            foreach (var node in nodes)
                MarkNode(node);

            foreach (var source in nodes)
                NotifyNode(source, 1, true);
        }

        private void NotifyNode(INotifiable node, int currentValue, bool evaluating)
        {
            if (node == null)
                return;

            var metaData = node.ExecutionMetaData;
            metaData.RemainingVisits -= currentValue;
            if (metaData.RemainingVisits > 0)
                return;
#if DEBUG
            if (metaData.RemainingVisits < 0)
                throw new InvalidOperationException("RemainingVisits < 0: This should never happen!");
#endif

            INotificationResult result = null;
            if (evaluating)
            {
                result = node.Notify(metaData.Sources);
                evaluating = result.Changed;
                currentValue = metaData.TotalVisits;
                metaData.TotalVisits = 0;
                metaData.Sources.Clear();
            }

#if DEBUG
            if (node.Successors.Count > 1)
                throw new InvalidOperationException("Node has more than one successor: This should never happen!");
#endif
            var nextNode = node.Successors[0];
            if (nextNode != null)
            {
                if (result != null && evaluating)
                    nextNode.ExecutionMetaData.Sources.Add(result);

                NotifyNode(nextNode, currentValue, evaluating);
            }
        }

        private void MarkNode(INotifiable node)
        {
            var metaData = node.ExecutionMetaData;
            metaData.TotalVisits++;
            metaData.RemainingVisits++;

            var nextNode = node.Successors[0];
            if (nextNode != null)
                MarkNode(nextNode);
        }
    }
}
