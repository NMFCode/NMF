﻿using System.Collections;
using System.Collections.Specialized;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a class that listenes for collection changes
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public class CollectionChangeListener<T> : IChangeListener
    {
        private INotifyCollectionChanged collection;
        private bool engineNotified;

        private CollectionChangedNotificationResult<T> notification = CollectionChangedNotificationResult<T>.Create(null);

        /// <inheritdoc />
        public INotifiable Node { get; private set; }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="node">the target node</param>
        public CollectionChangeListener(INotifiable node)
        {
            Node = node;
        }

        /// <summary>
        /// Subscribe to changes of the collection
        /// </summary>
        /// <param name="collection">The target collection</param>
        public void Subscribe(INotifyCollectionChanged collection)
        {
            if (this.collection != collection)
            {
                Unsubscribe();
                this.collection = collection;
                collection.CollectionChanged += OnCollectionChanged;
            }
        }

        /// <summary>
        /// Unsubscribe changes
        /// </summary>
        public void Unsubscribe()
        {
            if (collection != null)
            {
                collection.CollectionChanged -= OnCollectionChanged;
                collection = null;
            }
            engineNotified = false;
        }

        /// <inheritdoc />
        public INotificationResult AggregateChanges()
        {
            INotificationResult result;
            if (!HasChanges())
                result = UnchangedNotificationResult.Instance;
            else
                result = notification;
            
            engineNotified = false;
            notification = CollectionChangedNotificationResult<T>.Create(null);

            return result;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    TrackAddAction(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    TrackRemoveAction(e.OldItems, e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    TrackMoveAction(e.OldItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    TrackReplaceAction(e.OldItems, e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TrackResetAction();
                    break;
            }

            if (!engineNotified)
            {
                engineNotified = true;
                ExecutionEngine.Current.InvalidateNode(this);
            }
        }

        private void TrackAddAction(IList addedItems, int startIndex)
        {
            if (notification.IsReset)
                return;
            foreach (T item in addedItems)
            {
                if (!notification.RemovedItems.Remove(item))
                {
                    notification.AddedItems.Add(item);
                }
            }
            notification.UpdateNewStartIndex(startIndex);
        }

        private void TrackRemoveAction(IList removedItems, int startIndex)
        {
            if (notification.IsReset)
                return;
            foreach (T item in removedItems)
            {
                if (!notification.AddedItems.Remove(item))
                {
                    this.notification.RemovedItems.Add(item);
                }
            }
            notification.UpdateOldStartIndex(startIndex);
        }

        private void TrackMoveAction(IList movedItems, int startIndex)
        {
            if (notification.IsReset)
                return;
            foreach (T item in movedItems)
                this.notification.MovedItems.Add(item);
            notification.UpdateNewStartIndex(startIndex);
            notification.UpdateOldStartIndex(startIndex);
        }

        private void TrackReplaceAction(IList replacedItems, IList replacingItems, int startIndex)
        {
            if (notification.IsReset)
                return;
            foreach (T item in replacingItems)
            {
                if (!notification.RemovedItems.Remove(item))
                {
                    notification.AddedItems.Add(item);
                }
            }

            foreach (T item in replacedItems)
            {
                if (!notification.AddedItems.Remove(item))
                {
                    notification.RemovedItems.Add(item);
                }
            }

            notification.UpdateOldStartIndex(startIndex);
            notification.UpdateNewStartIndex(startIndex);
        }

        private void TrackResetAction()
        {
            notification.TurnIntoReset();
        }

        private bool HasChanges()
        {
            return
                notification.IsReset ||
                notification.AddedItems.Count > 0 ||
                notification.RemovedItems.Count > 0 ||
                notification.MovedItems.Count > 0;
        }
    }
}
