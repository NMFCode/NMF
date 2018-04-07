﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class CollectionChangeListener<T> : IChangeListener
    {
        private INotifyCollectionChanged collection;
        private bool engineNotified;

        private CollectionChangedNotificationResult<T> notification = CollectionChangedNotificationResult<T>.Create(null);

        public INotifiable Node { get; private set; }

        public CollectionChangeListener(INotifiable node)
        {
            Node = node;
        }

        public void Subscribe(INotifyCollectionChanged collection)
        {
            if (this.collection != collection)
            {
                Unsubscribe();
                this.collection = collection;
                collection.CollectionChanged += OnCollectionChanged;
            }
        }

        public void Unsubscribe()
        {
            if (collection != null)
            {
                collection.CollectionChanged -= OnCollectionChanged;
                collection = null;
            }
            engineNotified = false;
        }

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
                    TrackAddAction(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    TrackRemoveAction(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    TrackMoveAction(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    TrackReplaceAction(e.OldItems, e.NewItems);
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

        private void TrackAddAction(IList addedItems)
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
        }

        private void TrackRemoveAction(IList removedItems)
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
        }

        private void TrackMoveAction(IList movedItems)
        {
            if (notification.IsReset)
                return;
            foreach (T item in movedItems)
                this.notification.MovedItems.Add(item);
        }

        private void TrackReplaceAction(IList replacedItems, IList replacingItems)
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
