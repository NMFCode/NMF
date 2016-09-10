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
    public abstract class ExecutionEngine
    {
        private readonly ExecutionContext context = ExecutionContext.Instance;

        public bool TransactionActive { get; private set; }

        public void BeginTransaction()
        {
            TransactionActive = true;
        }

        public void CommitTransaction()
        {
            AggregateAndExecute();
            TransactionActive = false;
        }

        public void ManualInvalidation(params INotifiable[] nodes)
        {
            if (nodes.Length == 0)
                return;

            if (TransactionActive)
                throw new InvalidOperationException("A transaction is in progress. Commit or rollback first.");

            if (nodes.Length == 1)
                ExecuteSingle(nodes[0]);
            else
                Execute(new HashSet<INotifiable>(nodes));
        }

        internal void OnNodesInvalidated()
        {
            if (!TransactionActive)
                AggregateAndExecute();
        }

        private void AggregateAndExecute()
        {
            var invalids = context.AggregateInvalidNodes();
            if (invalids.Count == 0)
                return;

            if (invalids.Count == 1)
                ExecuteSingle(invalids.First());
            else
                Execute(invalids);
        }

        protected abstract void Execute(HashSet<INotifiable> nodes);

        protected abstract void ExecuteSingle(INotifiable node);
        
        public static ExecutionEngine Current { get; set; } = new SequentialExecutionEngine();
    }
}