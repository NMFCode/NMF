using System;
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
        private readonly ExecutionContext context;
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
            if (invalidNodes.Count > 0)
            {
                Execute(invalidNodes);
                invalidNodes.Clear();
            }
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
            {
                BeginTransaction();
                foreach (var node in nodes)
                    SetInvalidNode(node);
                CommitTransaction();
            }
        }

        private void SetInvalidNode(INotifiable node)
        {
            if (TransactionActive)
                invalidNodes.Add(node);
            else
                ExecuteSingle(node);
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
            context.Dispose();
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

        private class ExecutionContext : IExecutionContext, IDisposable
        {
            private readonly ExecutionEngine engine;
            private readonly Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler> propertyChangedHandler = new Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler>();
            private readonly Dictionary<Tuple<INotifiable, INotifyCollectionChanged>, NotifyCollectionChangedEventHandler> collectionChangedHandler = new Dictionary<Tuple<INotifiable, INotifyCollectionChanged>, NotifyCollectionChangedEventHandler>();

            public readonly Dictionary<INotifyCollectionChanged, ICollectionChangedNotificationResult> CollectionChanges = new Dictionary<INotifyCollectionChanged, ICollectionChangedNotificationResult>();
            public ExecutionContext(ExecutionEngine engine)
            {
                this.engine = engine;
            }

            public void AddChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
            {
                PropertyChangedEventHandler handler = (obj, e) =>
                {
                    if (e.PropertyName == propertyName || e.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                        engine.SetInvalidNode(node);
                };

                element.PropertyChanged += handler;
                propertyChangedHandler[new Tuple<INotifiable, INotifyPropertyChanged, string>(node, element, propertyName)] = handler;
            }

            public void AddChangeListener<T>(INotifiable node, INotifyCollectionChanged collection)
            {
                NotifyCollectionChangedEventHandler handler = (obj, e) =>
                {
                    engine.SetInvalidNode(node);
                    TrackCollectionChanges<T>(collection, e);
                };
                collection.CollectionChanged += handler;
                collectionChangedHandler[new Tuple<INotifiable, INotifyCollectionChanged>(node, collection)] = handler;
            }

            

            public void RemoveChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
            {
                var key = new Tuple<INotifiable, INotifyPropertyChanged, string>(node, element, propertyName);
                PropertyChangedEventHandler handler;
                if (propertyChangedHandler.TryGetValue(key, out handler))
                {
                    element.PropertyChanged -= handler;
                    propertyChangedHandler.Remove(key);
                }
            }

            public void RemoveChangeListener(INotifiable node, INotifyCollectionChanged collection)
            {
                var key = new Tuple<INotifiable, INotifyCollectionChanged>(node, collection);
                NotifyCollectionChangedEventHandler handler;
                if (collectionChangedHandler.TryGetValue(key, out handler))
                {
                    collection.CollectionChanged -= handler;
                    collectionChangedHandler.Remove(key);
                }
            }

            public void Dispose()
            {
                foreach (var kvp in propertyChangedHandler)
                    kvp.Key.Item2.PropertyChanged -= kvp.Value;
                propertyChangedHandler.Clear();

                foreach (var kvp in collectionChangedHandler)
                    kvp.Key.Item2.CollectionChanged -= kvp.Value;
                collectionChangedHandler.Clear();

                GC.SuppressFinalize(this);
            }

            private void TrackCollectionChanges<T>(INotifyCollectionChanged collection, NotifyCollectionChangedEventArgs args)
            {
                ICollectionChangedNotificationResult changes;
                if (!CollectionChanges.TryGetValue(collection, out changes))
                {
                    changes = new CollectionChangedNotificationResult<T>(null, new List<T>(), new List<T>(), new List<T>(), new List<T>(), new List<T>());
                    CollectionChanges[collection] = changes;
                }

                if (changes.IsReset)
                    return;

                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in args.NewItems)
                        {
                            if (changes.RemovedItems.Contains(item))
                                changes.RemovedItems.Remove(item);
                            else if (changes.ReplaceRemovedItems.Contains(item))
                                changes.ReplaceRemovedItems.Remove(item);
                            else
                                changes.AddedItems.Add(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in args.OldItems)
                        {
                            if (changes.AddedItems.Contains(item))
                                changes.AddedItems.Remove(item);
                            else if (changes.ReplaceAddedItems.Contains(item))
                                changes.ReplaceAddedItems.Remove(item);
                            else
                                changes.RemovedItems.Add(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Move:
                        foreach (var item in args.OldItems)
                            changes.MovedItems.Add(item);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        foreach (var item in args.NewItems)
                        {
                            if (changes.RemovedItems.Contains(item))
                                changes.RemovedItems.Remove(item);
                            else if (changes.ReplaceRemovedItems.Contains(item))
                                changes.ReplaceRemovedItems.Remove(item);
                            else
                                changes.ReplaceAddedItems.Add(item);
                        }
                        foreach (var item in args.OldItems)
                        {
                            if (changes.AddedItems.Contains(item))
                                changes.AddedItems.Remove(item);
                            else if (changes.ReplaceAddedItems.Contains(item))
                                changes.ReplaceAddedItems.Remove(item);
                            else
                                changes.ReplaceRemovedItems.Add(item);
                        } 
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        CollectionChanges[collection] = new CollectionChangedNotificationResult<T>(null);
                        break;
                    default:
                        throw new ArgumentException("{args.Action} is not a valid action for a NotifyCollectionChanged event.");
                }
            }
        }
    }
}
