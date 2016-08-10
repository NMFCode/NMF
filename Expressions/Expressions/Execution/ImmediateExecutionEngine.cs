using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Execution
{
    public class ImmediateExecutionEngine : ExecutionEngine
    {
        protected override void SetInvalidNode(INotifiable node)
        {
            NotifyLoop(node);
        }

        private void NotifyLoop(INotifiable source)
        {
            var node = source;
            INotificationResult lastResult = null;

            while (node != null)
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
