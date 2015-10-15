   using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;

namespace NMF.Expressions.Linq
{
    internal abstract class ObservableSetComparer<T> : INotifyValue<bool>
    {
        private INotifyEnumerable<T> source1;
        private IEnumerable<T> source2;

        private Dictionary<T, Entry> entries;
        private int attachedCount;

        protected ObservableSetComparer(INotifyEnumerable<T> source1, IEnumerable<T> source2, IEqualityComparer<T> comparer)
        {
            if (source1 == null) throw new ArgumentNullException("source1");
            if (source2 == null) throw new ArgumentNullException("source2");

            this.source1 = source1;
            this.source2 = source2;

            if (source1 != source2)
            {
                this.entries = new Dictionary<T, Entry>(comparer);
            }

            Attach();
        }

        private void Source2CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move) return;

            bool oldValue = Value;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                List<T> toRemove = new List<T>();
                foreach (var entry in entries)
                {
                    entry.Value.Source2Count = 0;
                    if (entry.Value.Source1Count == 0)
                    {
                        toRemove.Add(entry.Key);
                    }
                }
                foreach (var item in toRemove)
                {
                    entries.Remove(item);
                }
                OnSource2Reset(entries.Count);
                foreach (var item in source2)
                {
                    AddSource2(item);
                }
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (T item in e.OldItems)
                    {
                        RemoveSource2(item);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (T item in e.NewItems)
                    {
                        AddSource2(item);
                    }
                }
            }

            if (oldValue != Value)
            {
                OnValueChanged(new ValueChangedEventArgs(oldValue, Value));
            }
        }

        protected abstract void OnSource1Reset(int entriesCount);

        protected abstract void OnSource2Reset(int entriesCount);

        private void Source1CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move) return;

            bool oldValue = Value;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                List<T> toRemove = new List<T>();
                foreach (var entry in entries)
                {
                    entry.Value.Source1Count = 0;
                    if (entry.Value.Source2Count == 0)
                    {
                        toRemove.Add(entry.Key);
                    }
                }
                foreach (var item in toRemove)
                {
                    entries.Remove(item);
                }
                OnSource1Reset(entries.Count);
                foreach (var item in source1)
                {
                    AddSource1(item);
                }
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (T item in e.OldItems)
                    {
                        RemoveSource1(item);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (T item in e.NewItems)
                    {
                        AddSource1(item);
                    }
                }
            }

            if (oldValue != Value)
            {
                OnValueChanged(new ValueChangedEventArgs(oldValue, Value));
            }
        }

        protected abstract void OnAddSource1(bool isNew, bool isFirst);

        protected abstract void OnAddSource2(bool isNew, bool isFirst);

        protected abstract void OnRemoveSource1(bool isLast, bool removeEntry);

        protected abstract void OnRemoveSource2(bool isLast, bool removeEntry);

        private void AddSource2(T item)
        {
            Entry entry;
            if (!entries.TryGetValue(item, out entry))
            {
                entry = new Entry();
                OnAddSource2(true, true);
                entries.Add(item, entry);
            }
            else
            {
                OnAddSource2(false, entry.Source2Count == 0);
            }
            entry.Source2Count++;
        }

        private void RemoveSource2(T item)
        {
            Entry entry = entries[item];
            entry.Source2Count--;
            if (entry.Source2Count == 0)
            {
                if (entry.Source1Count == 0)
                {
                    entries.Remove(item);
                    OnRemoveSource2(true, true);
                }
                else
                {
                    OnRemoveSource2(true, false);
                }
            }
            else
            {
                OnRemoveSource2(false, false);
            }
        }

        private void AddSource1(T item)
        {
            Entry entry;
            if (!entries.TryGetValue(item, out entry))
            {
                entry = new Entry();
                OnAddSource1(true, true);
                entries.Add(item, entry);
            }
            else
            {
                OnAddSource1(false, entry.Source1Count == 0);
            }
            entry.Source1Count++;
        }

        private void RemoveSource1(T item)
        {
            Entry entry = entries[item];
            entry.Source1Count--;
            if (entry.Source1Count == 0)
            {
                if (entry.Source2Count == 0)
                {
                    entries.Remove(item);
                    OnRemoveSource1(true, true);
                }
                else
                {
                    OnRemoveSource1(true, false);
                }
            }
            else
            {
                OnRemoveSource1(false, false);
            }
        }

        public abstract bool Value
        {
            get;
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        private void OnValueChanged(ValueChangedEventArgs e)
        {
            if (ValueChanged != null) ValueChanged(this, e);
        }

        public void Detach()
        {
            if (attachedCount == 1)
            {
                DetachCore();
            }
            attachedCount--;
        }

        private void DetachCore()
        {
            if (entries == null) return;
            source1.CollectionChanged -= Source1CollectionChanged;
            var notifier = source2 as INotifyEnumerable<T>;
            if (notifier != null)
            {
                notifier.CollectionChanged -= Source2CollectionChanged;
            }
        }

        public void Attach()
        {
            if (attachedCount == 0)
            {
                AttachCore();
            }
            attachedCount++;
        }

        private void AttachCore()
        {
            if (entries == null) return;
            entries.Clear();
            foreach (var item in source1)
            {
                AddSource1(item);
            }

            foreach (var item in source2)
            {
                AddSource2(item);
            }

            source1.CollectionChanged += Source1CollectionChanged;
            var notifier = source2 as INotifyEnumerable<T>;
            if (notifier != null)
            {
                notifier.CollectionChanged += Source2CollectionChanged;
            }
        }

        private class Entry
        {
            public int Source1Count { get; set; }

            public int Source2Count { get; set; }
        }


        public bool IsAttached
        {
            get { return attachedCount > 0; }
        }
    }
}