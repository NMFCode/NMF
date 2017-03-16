using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class SequentialExecutionEngine : ExecutionEngine
    {
        protected override void Execute(List<INotifiable> nodes)
        {
            foreach (var node in nodes)
                MarkNode(node);

            foreach (var source in nodes)
                NotifyNode(source);
        }

        private void NotifyNode(INotifiable source)
        {
            bool evaluating = true;
            var stack = new Stack<Tuple<INotifiable, int>>();
            stack.Push(new Tuple<INotifiable, int>(source, 1));

            while (stack.Count > 0)
            {
                var tuple = stack.Pop();
                var node = tuple.Item1;
                var metaData = node.ExecutionMetaData;
                metaData.RemainingVisits -= tuple.Item2;
                if (metaData.RemainingVisits > 0)
                    continue;

                INotificationResult result = null;
                if (evaluating || metaData.Results.Count > 0)
                {
                    result = node.Notify(metaData.Results);
                    evaluating = result.Changed;
                    metaData.Results.Clear();
                }

                if (node.Successors.HasSuccessors)
                {
                    foreach (var succ in node.Successors)
                    {
                        if (result != null && evaluating)
                            succ.ExecutionMetaData.Results.UnsafeAdd(result);
                        stack.Push(new Tuple<INotifiable, int>(succ, metaData.TotalVisits));
                    }
                }
                
                metaData.TotalVisits = 0;
            }
        }

        private void MarkNode(INotifiable node)
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
