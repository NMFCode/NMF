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
        private readonly ExecutionContext context;
        private HashSet<INotifiable> invalidNodes = new HashSet<INotifiable>();

        internal IExecutionContext Context
        {
            get { return context; }
        }

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
            foreach (var node in invalidNodes)
                AggregateCollectionChanges(node);
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
                AggregateCollectionChanges(nodes[0]);
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

        private void SetInvalidNode(INotifiable node)
        {
            if (TransactionActive)
                invalidNodes.Add(node);
            else
            {
                AggregateCollectionChanges(node);
                ExecuteSingle(node);
            }
        }

        private void AggregateCollectionChanges(INotifiable node)
        {
            INotifyCollectionChanged collection;
            if (context.TrackedCollections.TryGetValue(node, out collection))
            {
                ExecutionContext.ICollectionChangeTracker tracker;
                if (context.CollectionChanges.TryGetValue(collection, out tracker))
                    node.ExecutionMetaData.Sources.Add(tracker.GetResult());
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

            public readonly Dictionary<INotifyCollectionChanged, ICollectionChangeTracker> CollectionChanges = new Dictionary<INotifyCollectionChanged, ICollectionChangeTracker>();
            public readonly Dictionary<INotifiable, INotifyCollectionChanged> TrackedCollections = new Dictionary<INotifiable, INotifyCollectionChanged>();
            public ExecutionContext(ExecutionEngine engine)
            {
                this.engine = engine;
            }

            public void AddChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
            {
                PropertyChangedEventHandler handler = (obj, e) =>
                {
                    if (e.PropertyName == propertyName ||
                        e.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                        engine.SetInvalidNode(node);
                };

                element.PropertyChanged += handler;
                propertyChangedHandler[
                    new Tuple<INotifiable, INotifyPropertyChanged, string>(node, element, propertyName)] = handler;
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
                TrackedCollections[node] = collection;
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

                INotifyCollectionChanged trackedColl;
                if (TrackedCollections.TryGetValue(node, out trackedColl))
                {
                    if (trackedColl == collection)
                        TrackedCollections.Remove(node);
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
                ICollectionChangeTracker temp;
                if (!CollectionChanges.TryGetValue(collection, out temp))
                {
                    temp = new CollectionChangeTracker<T>();
                    CollectionChanges[collection] = temp;
                }

                var tracker = (CollectionChangeTracker<T>) temp;

                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        tracker.TrackAddAction(args.NewItems as IList<T>);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        tracker.TrackRemoveAction(args.OldItems as IList<T>);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        tracker.TrackMoveAction(args.OldItems as IList<T>);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        tracker.TrackReplaceAction(args.OldItems as IList<T>, args.NewItems as IList<T>);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        tracker.TrackResetAction();
                        break;
                    default:
                        throw new ArgumentException(
                            "{args.Action} is not a valid action for a NotifyCollectionChanged event.");
                }
            }
            
            internal interface ICollectionChangeTracker
            {
                ICollectionChangedNotificationResult GetResult();
            }
            private class CollectionChangeTracker<T> : ICollectionChangeTracker
            {
                private bool _isReset;
                private readonly List<T> _addedItems = new List<T>();
                private readonly List<T> _removedItems = new List<T>();
                private readonly List<T> _movedItems = new List<T>();
                private readonly List<T> _replaceAddedItems = new List<T>();
                private readonly List<T> _replaceRemovedItems = new List<T>();

                public CollectionChangeTracker()
                {
                }

                public CollectionChangeTracker(bool isReset)
                {
                    _isReset = isReset;
                }

                public void TrackAddAction(IEnumerable<T> addedItems)
                {
                    if (_isReset) return;
                    foreach (var item in addedItems)
                    {
                        if (_removedItems.Contains(item))
                            _removedItems.Remove(item);
                        else if (_replaceRemovedItems.Contains(item))
                            _replaceRemovedItems.Remove(item);
                        else
                            _addedItems.Add(item);
                    }
                }

                public void TrackRemoveAction(IEnumerable<T> removedItems)
                {
                    if (_isReset) return;
                    foreach (var item in removedItems)
                    {
                        if (_addedItems.Contains(item))
                            _addedItems.Remove(item);
                        else if (_replaceAddedItems.Contains(item))
                            _replaceAddedItems.Remove(item);
                        else
                            _removedItems.Add(item);
                    }
                }

                public void TrackMoveAction(IEnumerable<T> movedItems)
                {
                    if (_isReset) return;
                    foreach (var item in movedItems)
                        _movedItems.Add(item);
                }

                public void TrackReplaceAction(IEnumerable<T> replacedItems, IEnumerable<T> replacingItems)
                {
                    if (_isReset) return;
                    foreach (var item in replacingItems)
                    {
                        if (_removedItems.Contains(item))
                            _removedItems.Remove(item);
                        else if (_replaceRemovedItems.Contains(item))
                            _replaceRemovedItems.Remove(item);
                        else
                            _replaceAddedItems.Add(item);
                    }

                    foreach (var item in replacedItems)
                    {
                        if (_addedItems.Contains(item))
                            _addedItems.Remove(item);
                        else if (_replaceAddedItems.Contains(item))
                            _replaceAddedItems.Remove(item);
                        else
                            _replaceRemovedItems.Add(item);
                    }
                }

                public void TrackResetAction()
                {
                    _isReset = true;
                }

                public ICollectionChangedNotificationResult GetResult()
                {
                    if (_isReset)
                        return new CollectionChangedNotificationResult<T>(null);
                    else
                        return new CollectionChangedNotificationResult<T>
                        (
                            null,
                            _addedItems,
                            _removedItems,
                            _movedItems,
                            _replaceAddedItems,
                            _replaceRemovedItems
                        );
                }
            }
        }
    }
}