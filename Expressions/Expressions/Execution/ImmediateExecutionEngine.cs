using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Execution
{
    public class ImmediateExecutionEngine : ExecutionEngine
    {
        protected override void OnPropertyChanged(HashSet<INotifiable> handler)
        {
            foreach (var node in handler.ToList())
                NotifyLoop(node);
        }

        private void NotifyLoop(INotifiable source)
        {
            var node = source;
            while (node != null)
            {
                var result = node.Notify(Enumerable.Empty<INotifiable>());

                if (result && node.Successors.Count > 0)
                    node = node.Successors[0];
                else
                    break;
            }
        }
    }
}
