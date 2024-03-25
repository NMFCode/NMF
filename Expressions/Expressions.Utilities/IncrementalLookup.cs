using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    /// <summary>
    /// Implements an incremental lookup
    /// </summary>
    /// <typeparam name="TSource">The source type of elements</typeparam>
    /// <typeparam name="TKey">The type along which the elements are grouped</typeparam>
    public class IncrementalLookup<TSource, TKey> : ObservableEnumerable<TKey>, INotifyLookup<TSource, TKey>
    {
        private readonly INotifyEnumerable<TSource> source;
        private readonly ObservingFunc<TSource, TKey> keySelector;
        private readonly Dictionary<TSource, TaggedObservableValue<TKey, (TSource, int)>> keyValueCache = new Dictionary<TSource, TaggedObservableValue<TKey, (TSource, int)>>();
        private readonly Dictionary<TKey, IncrementalLookupSlave> slaves = new Dictionary<TKey, IncrementalLookupSlave>();
        private readonly Notification notification;

        /// <summary>
        /// Creates an incremental lookup
        /// </summary>
        /// <param name="source">The source of elements</param>
        /// <param name="keySelector">A function that selects the keys for an element</param>
        public IncrementalLookup(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TKey> keySelector)
        {
            this.source = source;
            this.keySelector = keySelector;
            notification = new Notification(this);
        }

        /// <inheritdoc />
        protected override void OnAttach()
        {
            foreach (var item in source)
            {
                var incKey = AttachItem(item);
                GetLookup(incKey.Value).Items.Add(item);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                return keyValueCache.Values.Concat<INotifiable>(Enumerable.Repeat(source, 1));
            }
        }

        /// <inheritdoc />
        public INotifyEnumerable<TKey> Keys
        {
            get { return this; }
        }

        /// <inheritdoc />
        public INotifyEnumerable<TSource> this[TKey key]
        {
            get
            {
                return slaves[key];
            }
        }

        internal IncrementalLookupSlave GetLookup(TKey key)
        {
            if (!slaves.TryGetValue(key, out IncrementalLookupSlave slave))
            {
                slave = new IncrementalLookupSlave(this, key);
                slaves.Add(key, slave);
                Successors.Set(slave);
            }
            return slave;
        }

        private IEnumerable<IncrementalLookupSlave> GetAllSlaves()
        {
            return slaves.Values;
        }

        /// <inheritdoc />
        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            notification.Reset();
            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    NotifySourceChange(change);
                }
                else
                {
                    NotifyValueChange(change);
                }
            }
            return notification;
        }

        private void NotifyValueChange(INotificationResult change)
        {
            var valueHost = change.Source as TaggedObservableValue<TKey, (TSource, int)>;
            var valueChange = change as IValueChangedNotificationResult<TKey>;

            var oldLookup = GetSlaveNotification(valueChange.OldValue);
            var newLookup = GetSlaveNotification(valueChange.NewValue);
            var item = valueHost.Tag.Item1;
            for (int i = 0; i < valueHost.Tag.Item2; i++)
            {
                oldLookup.RemovedItems.Add(item);
                newLookup.AddedItems.Add(item);
            }
        }

        private void NotifySourceChange(INotificationResult change)
        {
            var collectionChange = change as ICollectionChangedNotificationResult<TSource>;
            if (!collectionChange.IsReset)
            {
                if (collectionChange.RemovedItems != null)
                {
                    foreach (var item in collectionChange.RemovedItems)
                    {
                        ProcessRemovedItem(item);
                    }
                }
                if (collectionChange.AddedItems != null)
                {
                    foreach (var item in collectionChange.AddedItems)
                    {
                        ProcessAddedItem(item);
                    }
                }
            }
            else
            {
                foreach (var slave in GetAllSlaves())
                {
                    notification[slave.Key] = CollectionChangedNotificationResult<TSource>.Create(slave, true);
                }
            }
        }

        private void ProcessAddedItem(TSource item)
        {
            var incKey = AttachItem(item);

            var slaveNotification = GetSlaveNotification(incKey.Value);
            slaveNotification.AddedItems.Add(item);
        }

        private void ProcessRemovedItem(TSource item)
        {
            var incKey = keyValueCache[item];
            incKey.Tag = (incKey.Tag.Item1, incKey.Tag.Item2 - 1);
            if (incKey.Tag.Item2 == 0)
            {
                incKey.Successors.Unset(this);
            }
            var slaveNotification = GetSlaveNotification(incKey.Value);
            slaveNotification.RemovedItems.Add(item);
        }

        private CollectionChangedNotificationResult<TSource> GetSlaveNotification(TKey key)
        {
            var lookupSlave = GetLookup(key);
            var slaveNotification = notification[key];
            if (slaveNotification == null)
            {
                slaveNotification = CollectionChangedNotificationResult<TSource>.Create(lookupSlave);
                notification[key] = slaveNotification;
            }

            return slaveNotification;
        }

        private TaggedObservableValue<TKey, (TSource, int)> AttachItem(TSource item)
        {
            TaggedObservableValue<TKey, (TSource, int)> incKey;
            if (!keyValueCache.TryGetValue(item, out incKey))
            {
                incKey = keySelector.InvokeTagged(item, (item, 1));
                keyValueCache.Add(item, incKey);
                incKey.Successors.Set(this);
                notification.AddedItems.Add(incKey.Value);
            }
            else
            {
                incKey.Tag = (incKey.Tag.Item1, incKey.Tag.Item2 + 1);
            }
            return incKey;
        }

        /// <inheritdoc />
        public override IEnumerator<TKey> GetEnumerator()
        {
            return slaves.Keys.GetEnumerator();
        }

        internal class IncrementalLookupSlave : ObservableEnumerable<TSource>
        {
            private readonly IncrementalLookup<TSource, TKey> parent;
            private readonly TKey key;

            public TKey Key { get { return key; } }

            public List<TSource> Items { get; } = new List<TSource>();

            public IncrementalLookupSlave(IncrementalLookup<TSource, TKey> parent, TKey key)
            {
                this.parent = parent;
                this.key = key;
            }

            public override IEnumerable<INotifiable> Dependencies
            {
                get
                {
                    yield return parent;
                }
            }

            public override IEnumerator<TSource> GetEnumerator()
            {
                return Items.GetEnumerator();
            }

            public override INotificationResult Notify(IList<INotificationResult> sources)
            {
                var notification = (sources[0] as Notification)[key];
                if (notification != null)
                {
                    if (notification.IsReset)
                    {
                        OnCleared();
                    }
                    else
                    {
                        if (notification.RemovedItems.Count > 0)
                        {
                            foreach (var item in notification.RemovedItems)
                            {
                                Items.Remove(item);
                            }
                            OnRemoveItems(notification.RemovedItems);
                        }
                        if (notification.AddedItems.Count > 0)
                        {
                            Items.AddRange(notification.AddedItems);
                            OnAddItems(notification.AddedItems);
                        }
                    }
                    return notification;
                }
                else
                {
                    return UnchangedNotificationResult.Instance;
                }
            }
        }

        internal class Notification : ICollectionChangedNotificationResult<TKey>
        {
            private readonly IncrementalLookup<TSource, TKey> parent;
            private readonly Dictionary<TKey, CollectionChangedNotificationResult<TSource>> notifications = new Dictionary<TKey, CollectionChangedNotificationResult<TSource>>();
            private readonly List<TKey> addedKeys = new List<TKey>();
            private readonly List<TKey> removedKeys = new List<TKey>();
            private bool isReset = false;

            public Notification(IncrementalLookup<TSource, TKey> parent)
            {
                this.parent = parent;
            }

            public CollectionChangedNotificationResult<TSource> this[TKey key]
            {
                get
                {
                    if (notifications.TryGetValue(key, out CollectionChangedNotificationResult<TSource> notification))
                    {
                        return notification;
                    }
                    else
                    {
                        return null;
                    }
                }
                set
                {
                    notifications.Add(key, value);
                }
            }

            public void Reset()
            {
                notifications.Clear();
                addedKeys.Clear();
                removedKeys.Clear();
                isReset = false;
            }

            public INotifiable Source { get { return parent; } }

            public bool Changed { get { return true; } }

            public List<TKey> AddedItems
            {
                get { return addedKeys; }
            }

            public List<TKey> RemovedItems
            {
                get { return removedKeys; }
            }

            public List<TKey> MovedItems
            {
                get { return null; }
            }

            public bool IsReset
            {
                get { return isReset; }
            }

            IList ICollectionChangedNotificationResult.AddedItems
            {
                get { return AddedItems; }
            }

            IList ICollectionChangedNotificationResult.RemovedItems
            {
                get { return RemovedItems; }
            }

            IList ICollectionChangedNotificationResult.MovedItems
            {
                get { return MovedItems; }
            }

            public int OldItemsStartIndex => 0;

            public int NewItemsStartIndex => 0;

            public void FreeReference()
            {
            }

            public void IncreaseReferences(int references)
            {
            }
        }
    }
}
