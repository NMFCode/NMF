using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    public abstract class ExecutionEngine : IDisposable
    {
        private readonly ExecutionContext context = ExecutionContext.Instance;
        private HashSet<INotifiable> invalidNodes = new HashSet<INotifiable>();

        public bool TransactionActive { get; private set; }

        public void BeginTransaction()
        {
            TransactionActive = true;
        }

        public void CommitTransaction()
        {
            context.AggregateCollectionChanges(invalidNodes);
            Execute(invalidNodes);
            invalidNodes.Clear();
            TransactionActive = false;
        }

        public void ManualInvalidation(params INotifiable[] nodes)
        {
            if (nodes.Length == 0)
                return;

            if (TransactionActive)
                throw new InvalidOperationException("A transaction is in progress. Commit or rollback first.");

            if (nodes.Length == 1)
            {
                context.AggregateCollectionChanges(nodes);
                ExecuteSingle(nodes[0]);
            }
            else
            {
                BeginTransaction();
                foreach (var node in nodes)
                    SetInvalidNode(node);
                CommitTransaction();
            }
        }

        internal void SetInvalidNode(INotifiable node)
        {
            if (TransactionActive)
                invalidNodes.Add(node);
            else
            {
                context.AggregateCollectionChanges(new[] { node });
                ExecuteSingle(node);
            }
        }

        protected abstract void Execute(HashSet<INotifiable> nodes);

        protected abstract void ExecuteSingle(INotifiable node);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            invalidNodes.Clear();
        }

        private static ExecutionEngine current = new SequentialExecutionEngine();

        public static ExecutionEngine Current
        {
            get { return current; }
            set
            {
                if (value != null)
                {
                    current.Dispose();
                    current = value;
                }
            }
        }
    }
}