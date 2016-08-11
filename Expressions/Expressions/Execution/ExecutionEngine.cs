using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Execution
{
    public abstract class ExecutionEngine
    {
        private readonly IExecutionContext context;
        private HashSet<INotifiable> invalidNodes = new HashSet<INotifiable>();

        internal IExecutionContext Context { get { return context; } }

        public bool TransactionActive { get; private set; }

        public ExecutionEngine()
        {
            context = new ExecutionContext(this);
        }

        public void BeginTransaction()
        {
            TransactionActive = true;
        }

        public void CommitTransaction()
        {
            Execute(invalidNodes);
            invalidNodes.Clear();
            TransactionActive = false;
        }

        private void SetInvalidNode(INotifiable node)
        {
            if (TransactionActive)
                invalidNodes.Add(node);
            else
                ExecuteSingle(node);
        }

        protected abstract void Execute(IEnumerable<INotifiable> nodes);

        protected abstract void ExecuteSingle(INotifiable node);
        
        public static ExecutionEngine Current { get; set; } = new SequentialExecutionEngine();

        private class ExecutionContext : IExecutionContext
        {
            private readonly ExecutionEngine engine;
            private readonly Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler> propertyChangedHandler = new Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler>();
            private readonly Dictionary<Tuple<INotifiable, INotifyCollectionChanged>, NotifyCollectionChangedEventHandler> collectionChangedHandler = new Dictionary<Tuple<INotifiable, INotifyCollectionChanged>, NotifyCollectionChangedEventHandler>();

            public ExecutionContext(ExecutionEngine engine)
            {
                this.engine = engine;
            }

            public void AddChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
            {
                PropertyChangedEventHandler handler = (obj, e) =>
                {
                    if (e.PropertyName == propertyName)
                        engine.SetInvalidNode(node);
                };

                element.PropertyChanged += handler;
                propertyChangedHandler[new Tuple<INotifiable, INotifyPropertyChanged, string>(node, element, propertyName)] = handler;
            }

            public void AddChangeListener(INotifiable node, INotifyCollectionChanged collection)
            {
                NotifyCollectionChangedEventHandler handler = (obj, e) => engine.SetInvalidNode(node);
                collection.CollectionChanged += handler;
                collectionChangedHandler[new Tuple<INotifiable, INotifyCollectionChanged>(node, collection)] = handler;
            }

            public void RemoveChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
            {
                var key = new Tuple<INotifiable, INotifyPropertyChanged, string>(node, element, propertyName);
                var handler = propertyChangedHandler[key];
                element.PropertyChanged -= handler;
                propertyChangedHandler.Remove(key);
            }

            public void RemoveChangeListener(INotifiable node, INotifyCollectionChanged collection)
            {
                var key = new Tuple<INotifiable, INotifyCollectionChanged>(node, collection);
                var handler = collectionChangedHandler[key];
                collection.CollectionChanged -= handler;
                collectionChangedHandler.Remove(key);
            }
        }
    }
}
