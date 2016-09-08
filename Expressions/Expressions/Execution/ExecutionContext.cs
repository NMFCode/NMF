using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    internal class ExecutionContext
    {
        private static readonly ExecutionContext instance = new ExecutionContext();
        public static ExecutionContext Instance => instance;

        private readonly Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler> propertyChangedHandler = new Dictionary<Tuple<INotifiable, INotifyPropertyChanged, string>, PropertyChangedEventHandler>();
        private readonly Dictionary<Tuple<INotifiable, INotifyCollectionChanged>, NotifyCollectionChangedEventHandler> collectionChangedHandler = new Dictionary<Tuple<INotifiable, INotifyCollectionChanged>, NotifyCollectionChangedEventHandler>();

        public readonly Dictionary<INotifyCollectionChanged, ICollectionChangeTracker> CollectionChanges = new Dictionary<INotifyCollectionChanged, ICollectionChangeTracker>();
        public readonly Dictionary<INotifiable, INotifyCollectionChanged> TrackedCollections = new Dictionary<INotifiable, INotifyCollectionChanged>();

        private ExecutionContext() { }

        public void AddChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
        {
            PropertyChangedEventHandler handler = (obj, e) =>
            {
                if (e.PropertyName == propertyName ||
                    e.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                    ExecutionEngine.Current.SetInvalidNode(node);
            };

            element.PropertyChanged += handler;
            propertyChangedHandler[
                new Tuple<INotifiable, INotifyPropertyChanged, string>(node, element, propertyName)] = handler;
        }

        public void AddChangeListener<T>(INotifiable node, INotifyCollectionChanged collection)
        {
            NotifyCollectionChangedEventHandler handler = (obj, e) =>
            {
                TrackCollectionChanges<T>(collection, e);
                ExecutionEngine.Current.SetInvalidNode(node);
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

        public void DetachAllChangeHandler()
        {
            foreach (var kvp in propertyChangedHandler)
                kvp.Key.Item2.PropertyChanged -= kvp.Value;
            propertyChangedHandler.Clear();

            foreach (var kvp in collectionChangedHandler)
                kvp.Key.Item2.CollectionChanged -= kvp.Value;
            collectionChangedHandler.Clear();
        }

        private void TrackCollectionChanges<T>(INotifyCollectionChanged collection, NotifyCollectionChangedEventArgs args)
        {
            ICollectionChangeTracker temp;
            if (!CollectionChanges.TryGetValue(collection, out temp))
            {
                temp = new CollectionChangeTracker<T>();
                CollectionChanges[collection] = temp;
            }

            var tracker = (CollectionChangeTracker<T>)temp;

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    tracker.TrackAddAction(args.NewItems.Cast<T>().ToList());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    tracker.TrackRemoveAction(args.OldItems.Cast<T>().ToList());
                    break;
                case NotifyCollectionChangedAction.Move:
                    tracker.TrackMoveAction(args.OldItems.Cast<T>().ToList());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    tracker.TrackReplaceAction(args.OldItems.Cast<T>().ToList(), args.NewItems.Cast<T>().ToList());
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
            bool HasChanges();
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

            public bool HasChanges()
            {
                return
                    _isReset ||
                    _addedItems.Count > 0 ||
                    _removedItems.Count > 0 ||
                    _movedItems.Count > 0 ||
                    _replaceAddedItems.Count > 0 ||
                    _replaceRemovedItems.Count > 0;
            }
        }
    }
}
