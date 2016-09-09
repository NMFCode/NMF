using System;
using System.Collections;
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

        private readonly Dictionary<INotifyCollectionChanged, CollectionChangeTracker> collectionChanges = new Dictionary<INotifyCollectionChanged, CollectionChangeTracker>();
        private readonly Dictionary<INotifiable, INotifyCollectionChanged> trackedCollections = new Dictionary<INotifiable, INotifyCollectionChanged>();

        private ExecutionContext() { }

        internal void AggregateCollectionChanges(IEnumerable<INotifiable> nodes)
        {
            foreach (var node in nodes)
            {
                INotifyCollectionChanged collection;
                if (!trackedCollections.TryGetValue(node, out collection))
                    return;

                CollectionChangeTracker tracker;
                if (!collectionChanges.TryGetValue(collection, out tracker))
                    return;

                if (tracker.HasChanges())
                    node.ExecutionMetaData.Sources.Add(tracker.GetResult());
            }
            collectionChanges.Clear();
        }

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

        public void AddChangeListener(INotifiable node, INotifyCollectionChanged collection)
        {
            NotifyCollectionChangedEventHandler handler = (obj, e) =>
            {
                TrackCollectionChanges(collection, e);
                ExecutionEngine.Current.SetInvalidNode(node);
            };
            collection.CollectionChanged += handler;
            collectionChangedHandler[new Tuple<INotifiable, INotifyCollectionChanged>(node, collection)] = handler;
            trackedCollections[node] = collection;
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

            trackedCollections.Remove(node);
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

        private void TrackCollectionChanges(INotifyCollectionChanged collection, NotifyCollectionChangedEventArgs args)
        {
            CollectionChangeTracker tracker;
            if (!collectionChanges.TryGetValue(collection, out tracker))
            {
                tracker = new CollectionChangeTracker();
                collectionChanges[collection] = tracker;
            }

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    tracker.TrackAddAction(args.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    tracker.TrackRemoveAction(args.OldItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    tracker.TrackMoveAction(args.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    tracker.TrackReplaceAction(args.OldItems, args.NewItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    tracker.TrackResetAction();
                    break;
                default:
                    throw new ArgumentException(
                        "{args.Action} is not a valid action for a NotifyCollectionChanged event.");
            }
        }
        
        private class CollectionChangeTracker
        {
            private bool _isReset;
            private readonly List<object> _addedItems = new List<object>();
            private readonly List<object> _removedItems = new List<object>();
            private readonly List<object> _movedItems = new List<object>();
            private readonly List<object> _replaceAddedItems = new List<object>();
            private readonly List<object> _replaceRemovedItems = new List<object>();

            public CollectionChangeTracker()
            {
            }

            public CollectionChangeTracker(bool isReset)
            {
                _isReset = isReset;
            }

            public void TrackAddAction(IEnumerable addedItems)
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

            public void TrackRemoveAction(IEnumerable removedItems)
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

            public void TrackMoveAction(IEnumerable movedItems)
            {
                if (_isReset) return;
                foreach (var item in movedItems)
                    _movedItems.Add(item);
            }

            public void TrackReplaceAction(IEnumerable replacedItems, IEnumerable replacingItems)
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
                    return new CollectionChangedNotificationResult<object>(null);
                else
                    return new CollectionChangedNotificationResult<object>
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
