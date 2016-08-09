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
            INotificationResult lastResult = null;

            while (true)
            {
                var list = new ShortList<INotificationResult>();
                if (lastResult != null)
                    list.Add(lastResult);
                
                lastResult = node.Notify(list);

                if (lastResult.Changed && node.Successors.Count > 0)
                    node = node.Successors[0];
                else
                    break;
            }
        }
    }
}
