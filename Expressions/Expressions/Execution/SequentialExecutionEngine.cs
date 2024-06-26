﻿using System;
using System.Collections.Generic;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a standard sequential execution engine
    /// </summary>
    public class SequentialExecutionEngine : ExecutionEngine
    {
        /// <inheritdoc />
        protected override void Execute(List<INotifiable> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                MarkNode(nodes[i]);
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                NotifyNode(nodes[i]);
            }
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
                    foreach (var item in metaData.Results)
                    {
                        item.FreeReference();
                    }
                    metaData.Results.Clear();
                }

                if (node.Successors.HasSuccessors)
                {
                    ProcessSuccessors(evaluating, stack, node, metaData, result);
                }

                metaData.TotalVisits = 0;
            }
        }

        private static void ProcessSuccessors(bool evaluating, Stack<Tuple<INotifiable, int>> stack, INotifiable node, ExecutionMetaData metaData, INotificationResult result)
        {
            if (evaluating)
            {
                result.IncreaseReferences(node.Successors.Count);
                for (int i = 0; i < node.Successors.Count; i++)
                {
                    var succ = node.Successors.GetSuccessor(i);
                    succ.ExecutionMetaData.Results.UnsafeAdd(result);
                    stack.Push(new Tuple<INotifiable, int>(succ, metaData.TotalVisits));
                }
            }
            else
            {
                for (int i = 0; i < node.Successors.Count; i++)
                {
                    var succ = node.Successors.GetSuccessor(i);
                    stack.Push(new Tuple<INotifiable, int>(succ, metaData.TotalVisits));
                }
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
            while (node.Successors.HasSuccessors && (node = node.Successors.GetSuccessor(0)) != null);
        }
    }
}
