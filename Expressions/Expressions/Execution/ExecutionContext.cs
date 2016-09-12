using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private readonly HashSet<Tuple<INotifiable, CollectionChangeTracker>> invalidNodes = new HashSet<Tuple<INotifiable, CollectionChangeTracker>>();

        private readonly List<CollectionChangeTracker> collectionChanges = new List<CollectionChangeTracker>();

        private readonly Dictionary<INotifyCollectionChanged, Collection<INotifiable>> collectionSubscribers = new Dictionary<INotifyCollectionChanged, Collection<INotifiable>>();
        private readonly Dictionary<INotifyPropertyChanged, Collection<Tuple<INotifiable, string>>> propertySubscribers = new Dictionary<INotifyPropertyChanged, Collection<Tuple<INotifiable, string>>>();

        private ExecutionContext() { }

        internal HashSet<INotifiable> AggregateInvalidNodes()
        {
            var results = new HashSet<INotifiable>();

            foreach (var node in invalidNodes)
            {
                if (node.Item2 == null)
                    results.Add(node.Item1);
                else if (node.Item2.HasChanges())
                {
                    results.Add(node.Item1);
                    node.Item1.ExecutionMetaData.Sources.Add(node.Item2.GetResult());
                }
            }
            invalidNodes.Clear();

            return results;
        }

        public void DetachAllChangeHandler()
        {
            invalidNodes.Clear();

            foreach (var element in propertySubscribers.Keys)
                element.PropertyChanged -= OnPropertyChanged;
            propertySubscribers.Clear();

            foreach (var collection in collectionSubscribers.Keys)
                collection.CollectionChanged -= OnCollectionChanged;
            collectionSubscribers.Clear();
            collectionChanges.Clear();
        }

        #region Properties

        public void AddChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
        {
            Collection<Tuple<INotifiable, string>> subscribers;
            if (!propertySubscribers.TryGetValue(element, out subscribers))
            {
                subscribers = new Collection<Tuple<INotifiable, string>>();
                propertySubscribers.Add(element, subscribers);
                element.PropertyChanged += OnPropertyChanged;
            }
            subscribers.Add(new Tuple<INotifiable, string>(node, propertyName));
        }

        public void RemoveChangeListener(INotifiable node, INotifyPropertyChanged element, string propertyName)
        {
            var subscribers = propertySubscribers[element];
            subscribers.Remove(new Tuple<INotifiable, string>(node, propertyName));
            if (subscribers.Count == 0)
            {
                element.PropertyChanged -= OnPropertyChanged;
                propertySubscribers.Remove(element);
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var element = (INotifyPropertyChanged)sender;

            foreach (var subscriber in propertySubscribers[element])
            {
                if (subscriber.Item2 == e.PropertyName)
                    invalidNodes.Add(new Tuple<INotifiable, CollectionChangeTracker>(subscriber.Item1, null));
            }

            ExecutionEngine.Current.OnNodesInvalidated();
        }
        
        #endregion

        #region Collections

        public void AddChangeListener(INotifiable node, INotifyCollectionChanged collection)
        {
            Collection<INotifiable> subscribers;
            if (!collectionSubscribers.TryGetValue(collection, out subscribers))
            {
                subscribers = new Collection<INotifiable>();
                collectionSubscribers.Add(collection, subscribers);
                collection.CollectionChanged += OnCollectionChanged;
            }
            subscribers.Add(node);
        }

        public void RemoveChangeListener(INotifiable node, INotifyCollectionChanged collection)
        {
            var subscribers = collectionSubscribers[collection];
            subscribers.Remove(node);
            if (subscribers.Count == 0)
            {
                collection.CollectionChanged -= OnCollectionChanged;
                collectionSubscribers.Remove(collection);
                collectionChanges.RemoveAll(t => t.Collection == collection);
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = (INotifyCollectionChanged)sender;
            TrackCollectionChanges(collection, e);
            ExecutionEngine.Current.OnNodesInvalidated();
        }

        private void TrackCollectionChanges(INotifyCollectionChanged collection, NotifyCollectionChangedEventArgs args)
        {
            var tracker = collectionChanges.Find(t => t.Collection == collection);
            if (tracker == null)
            {
                tracker = new CollectionChangeTracker(collection);
                collectionChanges.Add(tracker);
            }

            foreach (var node in collectionSubscribers[collection])
                invalidNodes.Add(new Tuple<INotifiable, CollectionChangeTracker>(node, tracker));

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
            private List<object> _addedItems;
            private List<object> _removedItems;
            private List<object> _movedItems;
            private List<object> _replaceAddedItems;
            private List<object> _replaceRemovedItems;

            public INotifyCollectionChanged Collection { get; private set; }

            public CollectionChangeTracker(INotifyCollectionChanged collection)
            {
                Collection = collection;
            }

            public void TrackAddAction(IEnumerable addedItems)
            {
                if (_isReset)
                    return;
                foreach (var item in addedItems)
                {
                    if ((!_removedItems?.Remove(item) ?? true) && (!_replaceRemovedItems?.Remove(item) ?? true))
                    {
                        if (_addedItems == null)
                            _addedItems = new List<object>();
                        _addedItems.Add(item);
                    }
                }
            }

            public void TrackRemoveAction(IEnumerable removedItems)
            {
                if (_isReset)
                    return;
                foreach (var item in removedItems)
                {
                    if ((!_addedItems?.Remove(item) ?? true) && (!_replaceAddedItems?.Remove(item) ?? true))
                    {
                        if (_removedItems == null)
                            _removedItems = new List<object>();
                        _removedItems.Add(item);
                    }
                }
            }

            public void TrackMoveAction(IEnumerable movedItems)
            {
                if (_isReset)
                    return;
                if (_movedItems == null)
                    _movedItems = new List<object>();
                foreach (var item in movedItems)
                    _movedItems.Add(item);
            }

            public void TrackReplaceAction(IEnumerable replacedItems, IEnumerable replacingItems)
            {
                if (_isReset)
                    return;
                foreach (var item in replacingItems)
                {
                    if ((!_removedItems?.Remove(item) ?? true) && (!_replaceRemovedItems?.Remove(item) ?? true))
                    {
                        if (_replaceAddedItems == null)
                            _replaceAddedItems = new List<object>();
                        _replaceAddedItems.Add(item);
                    }
                }

                foreach (var item in replacedItems)
                {
                    if ((!_addedItems?.Remove(item) ?? true) && (!_replaceAddedItems?.Remove(item) ?? true))
                    {
                        if (_replaceRemovedItems == null)
                            _replaceRemovedItems = new List<object>();
                        _replaceRemovedItems.Add(item);
                    }
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
                    _addedItems?.Count > 0 ||
                    _removedItems?.Count > 0 ||
                    _movedItems?.Count > 0 ||
                    _replaceAddedItems?.Count > 0 ||
                    _replaceRemovedItems?.Count > 0;
            }
        }

        #endregion
    }
}
