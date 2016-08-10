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
                int currentValue = 1;
                bool reevaluating = true;
                var node = source;

                while (node != null)
                {
                    node.ExecutionMetaData.RemainingVisits -= currentValue;
                    if (node.ExecutionMetaData.RemainingVisits == 0)
                    {
                        INotificationResult result = null;
                        if (reevaluating)
                        {
                            result = node.Notify(node.ExecutionMetaData.Sources);
                            reevaluating = result.Changed;
                            currentValue = node.ExecutionMetaData.TotalVisits;
                            node.ExecutionMetaData.TotalVisits = 0;
                            node.ExecutionMetaData.Sources.Clear();
                        }
                        
                        node = node.Successors[0];
                        if (node != null && result != null && reevaluating)
                            node.ExecutionMetaData.Sources.Add(result);
                    }
                    else
                        break;
                }
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
