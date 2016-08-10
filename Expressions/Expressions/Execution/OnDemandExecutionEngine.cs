using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Execution
{
    public class OnDemandExecutionEngine : ExecutionEngine
    {
        private HashSet<INotifiable> changeSources = new HashSet<INotifiable>();

        public void Execute()
        {
            NotifyLoop(changeSources);
            changeSources.Clear();
        }

        protected override void OnPropertyChanged(HashSet<INotifiable> handler)
        {
            foreach (var source in handler)
                changeSources.Add(source);
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

            node.ExecutionMetaData.RemainingVisits -= currentValue;
            if (node.ExecutionMetaData.RemainingVisits > 0)
                return;
            
            INotificationResult result = null;
            if (evaluating)
            {
                result = node.Notify(node.ExecutionMetaData.Sources);
                evaluating = result.Changed;
                currentValue = node.ExecutionMetaData.TotalVisits;
                node.ExecutionMetaData.TotalVisits = 0;
                node.ExecutionMetaData.Sources.Clear();
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
            if (node == null)
                return;

            node.ExecutionMetaData.TotalVisits++;
            node.ExecutionMetaData.RemainingVisits++;
            
            MarkNode(node.Successors[0]);
        }
    }
}
