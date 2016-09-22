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
    public abstract class ExecutionEngine
    {
        private readonly Collection<IChangeListener> changeListener = new Collection<IChangeListener>();

        public bool TransactionActive { get; private set; }

        public void BeginTransaction()
        {
            TransactionActive = true;
        }

        public void CommitTransaction()
        {
            if (changeListener.Count > 0)
            {
                var nodes = new List<INotifiable>(changeListener.Count);
                foreach (var listener in changeListener)
                {
                    var result = listener.AggregateChanges();
                    if (result == null)
                        nodes.Add(listener.Node);
                    else if (result.Changed)
                    {
                        listener.Node.ExecutionMetaData.Sources.Add(result);
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

        public void InvalidateNode(INotifiable node)
        {
            if (TransactionActive)
                throw new InvalidOperationException("A transaction is in progress. Commit or rollback first.");
            
            ExecuteSingle(node);
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
                    listener.Node.ExecutionMetaData.Sources.Add(result);
                    ExecuteSingle(listener.Node);
                }
            }
        }

        private void ExecuteSingle(INotifiable source)
        {
            var node = source;
            INotificationResult lastResult = null;

            while (node != null)
            {
                if (lastResult != null)
                    node.ExecutionMetaData.Sources.Add(lastResult);

                lastResult = node.Notify(node.ExecutionMetaData.Sources);
                node.ExecutionMetaData.Sources.Clear();

                if (lastResult.Changed && node.Successors.HasSuccessors)
                    node = node.Successors[0];
                else
                    break;
            }
        }

        protected abstract void Execute(List<INotifiable> nodes);
        
        public static ExecutionEngine Current { get; set; } = new SequentialExecutionEngine();
    }
}