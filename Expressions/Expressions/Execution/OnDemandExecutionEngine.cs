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
            throw new NotImplementedException("OnDemand will not work until introduction of ExecutionContext.");
            MarkAffectedNodes(sources);

            foreach (var source in sources)
            {
                int currentValue = 1;
                bool reevaluating = true;
                var node = source;

                while (node != null)
                {
                    node.RemainingVisits -= currentValue;
                    if (node.RemainingVisits == 0)
                    {
                        if (reevaluating)
                        {
                            var result = node.Notify(null);
                            reevaluating = result.Changed;
                            currentValue = node.TotalVisits;
                            node.TotalVisits = 0;
                        }

                        if (node.Successors.Count == 0)
                            break;
                        node = node.Successors[0];
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
                node.TotalVisits++;
                node.RemainingVisits++;

                if (node.Successors.Count == 0)
                    break;
                stack.Push(node.Successors[0]);
            }
        }
    }
}
