﻿using System;
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
    public abstract class ExecutionEngine
    {
        private readonly List<IChangeListener> changeListener = new List<IChangeListener>();
        private readonly List<INotifiable> changedNodes = new List<INotifiable>();
        private static ExecutionEngine _current = new SequentialExecutionEngine();
        public bool TransactionActive { get; private set; }

        public void BeginTransaction()
        {
            TransactionActive = true;
        }

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

        public void RollbackTransaction()
        {
            changeListener.Clear();
            TransactionActive = false;
        }

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
                    foreach (var succ in node.Successors)
                    {
                        succ.ExecutionMetaData.Results.UnsafeAdd(result);
                        stack.Push(succ);
                    }
                }
            }
        }

        protected abstract void Execute(List<INotifiable> nodes);

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