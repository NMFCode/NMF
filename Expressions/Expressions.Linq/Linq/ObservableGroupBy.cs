using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableGroupBy<TKey, TItem> : ObservableEnumerable<ObservableGroup<TKey, TItem>>
    {
        private INotifyEnumerable<TItem> source;
        private ObservingFunc<TItem, TKey> keySelector;

        private Dictionary<TKey, ObservableGroup<TKey, TItem>> groups;
        private Dictionary<TItem, INotifyValue<TKey>> keys = new Dictionary<TItem, INotifyValue<TKey>>();
        private Dictionary<INotifyValue<TKey>, TItem> items = new Dictionary<INotifyValue<TKey>, TItem>();

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                foreach (var keySelectorInstance in keys.Values)
                {
                    yield return keySelectorInstance;
                }
            }
        }

        public ObservableGroupBy(INotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            this.groups = new Dictionary<TKey, ObservableGroup<TKey, TItem>>(comparer);

            this.source = source;
            this.keySelector = keySelector;
        }

        private ObservableGroup<TKey, TItem> AttachItem(TItem item)
        {
            var key = keySelector.Observe(item);
            key.Successors.Add(this);
            keys[item] = key;
            items[key] = item;

            var add = false;
            ObservableGroup<TKey, TItem> group;
            if (!groups.TryGetValue(key.Value, out group))
            {
                group = new ObservableGroup<TKey, TItem>(key.Value);
                groups.Add(key.Value, group);
                add = true;
            }
            group.Items.Add(item);

            return add ? group : null;
        }

        private ObservableGroup<TKey, TItem> DetachItem(TItem item)
        {
            var key = keys[item];
            key.Successors.Remove(this);
            keys.Remove(item);
            items.Remove(key);
            var group = groups[key.Value];
            group.Items.Remove(item);
            if (group.Count == 0)
            {
                groups.Remove(key.Value);
                return group;
            }
            return null;
        }

        private bool ReplaceItem(TItem old, TItem newItem)
        {
            var oldKey = keys[old];
            var newKey = keySelector.Evaluate(newItem);

            if (EqualityComparer<TKey>.Default.Equals(oldKey.Value, newKey))
            {
                var group = groups[newKey];
                int itemIdx = group.Items.IndexOf(old);
                group.Items[itemIdx] = newItem;
                return true;
            }
            return false;
        }

        public override IEnumerator<ObservableGroup<TKey, TItem>> GetEnumerator()
        {
            return groups.Values.GetEnumerator();
        }

        public ObservableGroup<TKey, TItem> this[TKey key]
        {
            get
            {
                return groups[key];
            }
        }

        public bool TryGetGroup(TKey key, out INotifyGrouping<TKey, TItem> group)
        {
            ObservableGroup<TKey, TItem> _group;
            var ret = groups.TryGetValue(key, out _group);
            group = _group;
            return ret;
        }

        public override bool Contains(ObservableGroup<TKey, TItem> item)
        {
            if (item != null)
            {
                return groups.ContainsValue(item);
            }
            else
            {
                return false;
            }
        }

        public override int Count { get { return groups.Count; } }
        
        protected override void OnAttach()
        {
            foreach (var item in source)
            {
                AttachItem(item);
            }
        }

        protected override void OnDetach()
        {
            foreach (var key in keys.Values)
            {
                key.Successors.Remove(this);
            }

            keys.Clear();
            items.Clear();
            groups.Clear();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var added = new List<INotifyGrouping<TKey, TItem>>();
            var removed = new List<INotifyGrouping<TKey, TItem>>();

            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var sourceChange = (CollectionChangedNotificationResult<TItem>)change;
                    if (sourceChange.IsReset)
                    {
                        OnDetach();
                        OnAttach();
                        OnCleared();
                        return new CollectionChangedNotificationResult<INotifyGrouping<TKey, TItem>>(this);
                    }
                    else
                    {
                        NotifySource(sourceChange, added, removed);
                    }
                }
                else
                {
                    var keyChange = (ValueChangedNotificationResult<TKey>)change;
                    var key = (INotifyValue<TKey>)keyChange.Source;
                    var item = items[key];

                    ObservableGroup<TKey, TItem> group;
                    if (groups.TryGetValue(keyChange.OldValue, out group))
                    {
                        group.Items.Remove(item);
                        if (group.Count == 0)
                        {
                            groups.Remove(keyChange.OldValue);
                            removed.Add(group);
                        }
                    }
                    
                    if (!groups.TryGetValue(keyChange.NewValue, out group))
                    {
                        group = new ObservableGroup<TKey, TItem>(keyChange.NewValue);
                        groups.Add(keyChange.NewValue, group);
                        added.Add(group);
                    }
                    group.Items.Add(item);
                }
            }

            if (added.Count == 0 && removed.Count == 0)
                return new UnchangedNotificationResult(this);

            OnRemoveItems(removed.Cast<ObservableGroup<TKey, TItem>>());
            OnAddItems(added.Cast<ObservableGroup<TKey, TItem>>());
            return new CollectionChangedNotificationResult<INotifyGrouping<TKey, TItem>>(this, added, removed);
        }

        private void NotifySource(CollectionChangedNotificationResult<TItem> sourceChange, List<INotifyGrouping<TKey, TItem>> added, List<INotifyGrouping<TKey, TItem>> removed)
        {
            if (sourceChange.RemovedItems != null)
            {
                foreach (var item in sourceChange.RemovedItems)
                {
                    var group = DetachItem(item);
                    if (group != null)
                        removed.Add(group);
                }
            }

            if (sourceChange.AddedItems != null)
            {
                foreach (var item in sourceChange.AddedItems)
                {
                    var group = AttachItem(item);
                    if (group != null)
                        added.Add(group);
                }
            }

            if (sourceChange.ReplaceAddedItems != null)
            {
                for (int i = 0; i < sourceChange.ReplaceAddedItems.Count; i++)
                {
                    var oldItem = sourceChange.ReplaceRemovedItems[i];
                    var newItem = sourceChange.ReplaceAddedItems[i];
                    if (!ReplaceItem(oldItem, newItem))
                    {
                        var removedGroup = DetachItem(oldItem);
                        if (removedGroup != null)
                            removed.Add(removedGroup);
                        var addedGroup = AttachItem(newItem);
                        if (addedGroup != null)
                            added.Add(addedGroup);
                    }
                }
            }
        }
    }
}
