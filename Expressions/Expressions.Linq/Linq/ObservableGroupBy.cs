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
        private Dictionary<TItem, Stack<Entry>> keys = new Dictionary<TItem, Stack<Entry>>();

        public ObservableGroupBy(INotifyEnumerable<TItem> source, ObservingFunc<TItem, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            this.groups = new Dictionary<TKey, ObservableGroup<TKey, TItem>>(comparer);

            this.source = source;
            this.keySelector = keySelector;

            Attach();
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
                {
                    foreach (TItem item in e.OldItems)
                    {
                        Stack<Entry> entryStack = keys[item];
                        var entry = entryStack.Pop();
                        if (entryStack.Count == 0) keys.Remove(item);
                        DetachItem(item, entry);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
                {
                    foreach (TItem item in e.NewItems)
                    {
                        var key = keySelector.Observe(item);
                        AttachItem(item, key);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    var count = Math.Min(e.NewItems.Count, e.OldItems.Count);
                    for (int i = 0; i < count; i++)
                    {
                        var oldItem = (TItem)e.OldItems[i];
                        var newItem = (TItem)e.NewItems[i];
                        ReplaceItem(oldItem, newItem);
                    }
                }
            }
            else
            {
                DetachCore();
                OnCleared();
            }
        }

        private void AttachItem(TItem item, INotifyValue<TKey> key)
        {
            var entry = new Entry(key, item, this);
            Stack<Entry> entryStack;
            if (!keys.TryGetValue(item, out entryStack))
            {
                entryStack = new Stack<Entry>();
                keys.Add(item, entryStack);
            }
            entryStack.Push(entry);
            var add = false;
            ObservableGroup<TKey, TItem> group;
            if (!groups.TryGetValue(key.Value, out group))
            {
                group = new ObservableGroup<TKey, TItem>(key.Value);
                groups.Add(key.Value, group);
                add = true;
            }
            group.ItemsInternal.Add(item);
            key.ValueChanged += entry.KeyChanged;
            if (add) OnAddItem(group);
        }

        private void ReplaceItem(TItem old, TItem newItem)
        {
            Stack<Entry> entryStack = keys[old];
            var entry = entryStack.Pop();
            var key = keySelector.Observe(newItem);

            if (EqualityComparer<TKey>.Default.Equals(entry.Key.Value, key.Value))
            {
                var group = groups[entry.Key.Value];
                entry.Item = newItem;
                int itemIdx = group.ItemsInternal.IndexOf(old);
                group.ItemsInternal[itemIdx] = newItem;
            }
            else
            {
                DetachItem(old, entry);
                AttachItem(newItem, key);
            }
        }

        private void DetachItem(TItem item, Entry entry)
        {
            entry.Key.ValueChanged -= entry.KeyChanged;
            var group = groups[entry.Key.Value];
            group.ItemsInternal.Remove(item);
            if (group.Count == 0)
            {
                groups.Remove(entry.Key.Value);
                OnRemoveItem(group);
            }
        }

        private class Entry
        {
            public Entry(INotifyValue<TKey> key, TItem item, ObservableGroupBy<TKey, TItem> parent)
            {
                Key = key;
                Item = item;
                Parent = parent;
            }

            public INotifyValue<TKey> Key { get; private set; }

            public ObservableGroupBy<TKey, TItem> Parent { get; private set; }

            public TItem Item { get; set; }

            public void KeyChanged(object sender, ValueChangedEventArgs e)
            {
                ObservableGroup<TKey, TItem> group;
                var oldKey = (TKey)e.OldValue;
                if (Parent.groups.TryGetValue(oldKey, out group))
                {
                    group.ItemsInternal.Remove(Item);
                    if (group.Count == 0)
                    {
                        Parent.groups.Remove(oldKey);
                        Parent.OnRemoveItem(group);
                    }
                }
                var newKey = (TKey)e.NewValue;
                if (!Parent.groups.TryGetValue(newKey, out group))
                {
                    group = new ObservableGroup<TKey, TItem>(newKey);
                    Parent.groups.Add(newKey, group);
                    Parent.OnAddItem(group);
                }
                group.ItemsInternal.Add(Item);
            }
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

        public override int Count
        {
            get
            {
                return groups.Count;
            }
        }

        protected override void AttachCore()
        {
            foreach (var item in source)
            {
                var key = keySelector.Observe(item);
                AttachItem(item, key);
            }
            source.CollectionChanged += SourceCollectionChanged;
        }

        protected override void DetachCore()
        {
            foreach (var entries in keys.Values)
            {
                foreach (var entry in entries)
                {
                    entry.Key.ValueChanged -= entry.KeyChanged;
                    entry.Key.Detach();
                }
            }
            keys.Clear();
            groups.Clear();
            source.CollectionChanged -= SourceCollectionChanged;
        }
    }
}
