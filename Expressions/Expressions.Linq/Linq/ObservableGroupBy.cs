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
        private Dictionary<TItem, TaggedObservableValue<TKey, TItem>> keys = new Dictionary<TItem, TaggedObservableValue<TKey, TItem>>();

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

        private bool AttachItem(TItem item)
        {
            var key = keySelector.InvokeTagged(item, item);
            key.Successors.Set(this);
            keys[item] = key;

            var add = false;
            ObservableGroup<TKey, TItem> group;
            if (!groups.TryGetValue(key.Value, out group))
            {
                group = new ObservableGroup<TKey, TItem>(key.Value);
                groups.Add(key.Value, group);
                add = true;
            }
            group.Items.Add(item);

            return add;
        }

        private ObservableGroup<TKey, TItem> DetachItem(TItem item)
        {
            var key = keys[item];
            key.Successors.Unset(this);
            keys.Remove(item);
            var group = groups[key.Value];
            group.Items.Remove(item);
            if (group.Count == 0)
            {
                groups.Remove(key.Value);
            }
            return group;
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
                key.Successors.Unset(this);
            }

            keys.Clear();
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
                    var tagged = (TaggedObservableValue<TKey, TItem>)keyChange.Source;

                    ObservableGroup<TKey, TItem> group;
                    if (groups.TryGetValue(keyChange.OldValue, out group))
                    {
                        group.Items.Remove(tagged.Tag);
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
                    group.Items.Add(tagged.Tag);
                }
            }

            if (added.Count == 0 && removed.Count == 0)
                return UnchangedNotificationResult.Instance;

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
                    if (groups.ContainsValue(group))
                        added.Add(group);
                    removed.Add(group);
                }
            }

            if (sourceChange.AddedItems != null)
            {
                foreach (var item in sourceChange.AddedItems)
                {
                    var add = AttachItem(item);
                    var group = groups[keys[item].Value];
                    if (!add)
                        removed.Add(group);
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
                        if (!groups.ContainsValue(removedGroup))
                            removed.Add(removedGroup);
                        if (AttachItem(newItem))
                            added.Add(groups[keys[newItem].Value]);
                    }
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
