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
                    var metaData = (OnDemandMetaData)node.ExecutionMetaData;
                    metaData.RemainingVisits -= currentValue;
                    if (metaData.RemainingVisits == 0)
                    {
                        INotificationResult result = null;
                        if (reevaluating)
                        {
                            result = node.Notify(metaData.Sources);
                            reevaluating = result.Changed;
                            currentValue = metaData.TotalVisits;
                            node.ExecutionMetaData = null;
                        }

                        if (node.Successors.Count == 0)
                            break;
                        node = node.Successors[0];
                        if (result != null)
                            ((OnDemandMetaData)node.ExecutionMetaData).Sources.Add(result);
                    }
                    else
                        break;
                }
            }
        }

        private void MarkAffectedNodes(HashSet<INotifiable> sources)
        {
            var stack = new Stack<INotifiable>(sources);
            
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (node.ExecutionMetaData == null)
                    node.ExecutionMetaData = new OnDemandMetaData { RemainingVisits = 1, TotalVisits = 1 };
                else
                {
                    var metaData = (OnDemandMetaData)node.ExecutionMetaData;
                    metaData.TotalVisits++;
                    metaData.RemainingVisits++;
                }

                if (node.Successors.Count == 0)
                    break;
                stack.Push(node.Successors[0]);
            }
        }

        private class OnDemandMetaData
        {
            public int TotalVisits { get; set; }
            public int RemainingVisits { get; set; }
            public List<INotificationResult> Sources { get; private set; }

            public OnDemandMetaData()
            {
                Sources = new List<INotificationResult>();
            }
        }
    }
}
