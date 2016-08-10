using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Execution
{
    public class OnDemandExecutionEngine : ExecutionEngine
    {
        private HashSet<INotifiable> invalidNodes = new HashSet<INotifiable>();

        public void Execute()
        {
            NotifyLoop(invalidNodes);
            invalidNodes.Clear();
        }

        protected override void SetInvalidNode(INotifiable node)
        {
            invalidNodes.Add(node);
        }

        private void NotifyLoop(HashSet<INotifiable> sources)
        {
            MarkAffectedNodes(sources);

            foreach (var source in sources)
            {
                NotifyNode(source, 1, true);
            }
        }

        private void NotifyNode(INotifiable node, int currentValue, bool evaluating)
        {
            if (node == null)
                return;

            var metaData = node.ExecutionMetaData;
            metaData.RemainingVisits -= currentValue;
            if (metaData.RemainingVisits > 0)
                return;
            
            INotificationResult result = null;
            if (evaluating)
            {
                result = node.Notify(metaData.Sources);
                evaluating = result.Changed;
                currentValue = metaData.TotalVisits;
                metaData.TotalVisits = 0;
                metaData.Sources.Clear();
            }

            var nextNode = node.Successors[0];
            if (nextNode != null)
            {
                if (result != null && evaluating)
                    nextNode.ExecutionMetaData.Sources.Add(result);

                NotifyNode(nextNode, currentValue, evaluating);
            }
        }

        private void MarkAffectedNodes(HashSet<INotifiable> sources)
        {
            foreach (var node in sources)
                MarkNode(node);
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
