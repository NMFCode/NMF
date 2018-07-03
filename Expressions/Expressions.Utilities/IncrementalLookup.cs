using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{

    public abstract class IncrementalLookupBase<TSource, TKey> : INotifyEnumerable<TKey>
    {
        private INotifyEnumerable<TSource> source;
        private ObservingFunc<TSource, TKey> keySelector;
        private Dictionary<TSource, TaggedObservableValue<TKey, (TSource, int)>> keyValueCache = new Dictionary<TSource, TaggedObservableValue<TKey, (TSource, int)>>();
        private Dictionary<TKey, IncrementalLookupSlave> slaves = new Dictionary<TKey, IncrementalLookupSlave>();
        private Notification notification;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IncrementalLookupBase(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TKey> keySelector)
        {
            this.source = source;
            this.keySelector = keySelector;
            notification = new Notification(this);

            Successors.Attached += Attach;
            Successors.Detached += Detach;
        }

        protected virtual void Detach(object sender, EventArgs e)
        {
            source.Successors.Unset(this);
            foreach (var incKey in keyValueCache.Values)
            {
                incKey.Successors.Unset(this);
            }
        }

        protected virtual void Attach(object sender, EventArgs e)
        {
            source.Successors.Set(this);
            foreach (var item in source)
            {
                var incKey = AttachItem(item);
                GetLookup(incKey.Value).Items.Add(item);
            }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                return keyValueCache.Values.Concat<INotifiable>(Enumerable.Repeat(source, 1));
            }
        }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

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

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            notification.Reset();
            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var collectionChange = change as ICollectionChangedNotificationResult<TSource>;
                    if (!collectionChange.IsReset)
                    {
                        if (collectionChange.RemovedItems != null)
                        {
                            foreach (var item in collectionChange.RemovedItems)
                            {
                                var incKey = keyValueCache[item];
                                var key = incKey.Value;
                                incKey.Tag = (incKey.Tag.Item1, incKey.Tag.Item2 - 1);
                                if (incKey.Tag.Item2 == 0)
                                {
                                    incKey.Successors.Unset(this);
                                }
                                var slaveNotification = GetSlaveNotification(incKey.Value);
                                slaveNotification.RemovedItems.Add(item);
                                
                            }
                        }
                        if (collectionChange.AddedItems != null)
                        {
                            foreach (var item in collectionChange.AddedItems)
                            {
                                var incKey = AttachItem(item);

                                var slaveNotification = GetSlaveNotification(incKey.Value);
                                slaveNotification.AddedItems.Add(item);
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
                else
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
            }
            return notification;
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
            }
            else
            {
                incKey.Tag = (incKey.Tag.Item1, incKey.Tag.Item2 + 1);
            }
            return incKey;
        }

        public void Dispose()
        {
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            return slaves.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected internal class IncrementalLookupSlave : ObservableEnumerable<TSource>
        {
            private IncrementalLookupBase<TSource, TKey> parent;
            private TKey key;

            public TKey Key { get { return key; } }

            public List<TSource> Items { get; } = new List<TSource>();

            public IncrementalLookupSlave(IncrementalLookupBase<TSource, TKey> parent, TKey key)
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

        protected class Notification : INotificationResult
        {
            private IncrementalLookupBase<TSource, TKey> parent;
            private Dictionary<TKey, CollectionChangedNotificationResult<TSource>> notifications = new Dictionary<TKey, CollectionChangedNotificationResult<TSource>>();

            public Notification(IncrementalLookupBase<TSource, TKey> parent)
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
            }

            public INotifiable Source { get { return parent; } }

            public bool Changed { get { return true; } }

            public void FreeReference()
            {
            }

            public void IncreaseReferences(int references)
            {
            }
        }
    }
}
