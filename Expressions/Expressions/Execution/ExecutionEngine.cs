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
            foreach (var node in invalidNodes)
                AggregateCollectionChanges(node);
            Execute(invalidNodes);
            context.CollectionChanges.Clear();
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
                AggregateCollectionChanges(nodes[0]);
                ExecuteSingle(nodes[0]);
                context.CollectionChanges.Clear();
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
                AggregateCollectionChanges(node);
                ExecuteSingle(node);
                context.CollectionChanges.Clear();
            }
        }

        private void AggregateCollectionChanges(INotifiable node)
        {
            INotifyCollectionChanged collection;
            if (!context.TrackedCollections.TryGetValue(node, out collection))
                return;

            ExecutionContext.ICollectionChangeTracker tracker;
            if (!context.CollectionChanges.TryGetValue(collection, out tracker))
                return;

            if (tracker.HasChanges())
                node.ExecutionMetaData.Sources.Add(tracker.GetResult());
            else
                node.ExecutionMetaData.Sources.Add(UnchangedNotificationResult.Instance);
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