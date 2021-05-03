using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Deotes an execution engine for incremental computation
    /// </summary>
    public abstract class ExecutionEngine
    {
        private readonly List<IChangeListener> changeListener = new List<IChangeListener>();
        private readonly List<INotifiable> changedNodes = new List<INotifiable>();
        private static ExecutionEngine _current = new SequentialExecutionEngine();

        /// <summary>
        /// Indicates whether the system is in a transaction
        /// </summary>
        public bool TransactionActive { get; private set; }

        /// <summary>
        /// Starts a new change transaction
        /// </summary>
        public void BeginTransaction()
        {
            TransactionActive = true;
        }

        /// <summary>
        /// Commits the transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (changeListener.Count > 0 || changedNodes.Count > 0)
            {
                var nodes = new List<INotifiable>(changeListener.Count + changedNodes.Count);
                nodes.AddRange(changedNodes);
                changedNodes.Clear();
                foreach (var listener in changeListener)
                {
                    var result = listener.AggregateChanges();
                    if (result == null)
                        nodes.Add(listener.Node);
                    else if (result.Changed)
                    {
                        listener.Node.ExecutionMetaData.Results.UnsafeAdd(result);
                        nodes.Add(listener.Node);
                    }
                }
                changeListener.Clear();

                if (nodes.Count == 1)
                    ExecuteSingle(nodes[0]);
                else
                    Execute(nodes);
            }
            TransactionActive = false;
        }

        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        public void RollbackTransaction()
        {
            changeListener.Clear();
            TransactionActive = false;
        }


        /// <summary>
        /// Invalidates the given DDG node
        /// </summary>
        /// <param name="node">The DDG node</param>
        public void InvalidateNode(INotifiable node)
        {
            if (TransactionActive)
            {
                changedNodes.Add(node);
            }
            else
            {
                ExecuteSingle(node);
            }
        }

        internal void InvalidateNode(IChangeListener listener)
        {
            if (TransactionActive)
            {
                changeListener.Add(listener);
            }
            else
            {
                var result = listener.AggregateChanges();
                if (result == null)
                    ExecuteSingle(listener.Node);
                else if (result.Changed)
                {
                    listener.Node.ExecutionMetaData.Results.UnsafeAdd(result);
                    ExecuteSingle(listener.Node);
                }
            }
        }

        /// <summary>
        /// Propagates the changes of a single DDG node
        /// </summary>
        /// <param name="source">The changed DDG node</param>
        protected virtual void ExecuteSingle(INotifiable source)
        {
            var stack = new Stack<INotifiable>();
            stack.Push(source);

            while (stack.Count > 0)
            {
                var node = stack.Pop();
                var result = node.Notify(node.ExecutionMetaData.Results);
                foreach (var item in node.ExecutionMetaData.Results)
                {
                    item.FreeReference();
                }
                node.ExecutionMetaData.Results.Clear();

                if (result.Changed && node.Successors.HasSuccessors)
                {
                    result.IncreaseReferences(node.Successors.Count);
                    for (int i = 0; i < node.Successors.Count; i++)
                    {
                        var succ = node.Successors.GetSuccessor(i);
                        succ.ExecutionMetaData.Results.UnsafeAdd(result);
                        stack.Push(succ);
                    }
                }
            }
        }

        /// <summary>
        /// Propagates changes of the given DDG nodes
        /// </summary>
        /// <param name="nodes">The changed DDG nodes</param>
        protected abstract void Execute(List<INotifiable> nodes);

        /// <summary>
        /// Gets or sets the current execution engine
        /// </summary>
        public static ExecutionEngine Current
        {
            get { return _current; }
            set {
                if (_current.TransactionActive)
                {
                    throw new Exception("Tried to change execution engine during transaction.");
                }
                else
                {
                    _current = value;
                }
            }
        }
    }
}