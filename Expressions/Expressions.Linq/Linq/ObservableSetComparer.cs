﻿using System;
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
        private INotifyEnumerable<T> observableSource2;

        private Dictionary<T, Entry> entries;

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source1;
                if (observableSource2 != null)
                    yield return observableSource2;
            }
        }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        protected ObservableSetComparer(INotifyEnumerable<T> source1, IEnumerable<T> source2, IEqualityComparer<T> comparer)
        {
            if (source1 == null) throw new ArgumentNullException("source1");
            if (source2 == null) throw new ArgumentNullException("source2");

            this.source1 = source1;
            this.source2 = source2;

            this.observableSource2 = source2 as INotifyEnumerable<T>;
            if (observableSource2 == null)
                observableSource2 = (source2 as IEnumerableExpression<T>)?.AsNotifiable();
            this.entries = new Dictionary<T, Entry>(comparer);

            Successors.Attached += (obj, e) => Attach();
            Successors.Detached += (obj, e) => Detach();
        }

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

        public abstract bool Value { get; }

        protected abstract void OnAddSource1(bool isNew, bool isFirst);

        protected abstract void OnAddSource2(bool isNew, bool isFirst);

        protected abstract void OnRemoveSource1(bool isLast, bool removeEntry);

        protected abstract void OnRemoveSource2(bool isLast, bool removeEntry);

        protected abstract void OnResetSource1(int entriesCount);

        protected abstract void OnResetSource2(int entriesCount);

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        private void OnValueChanged(ValueChangedEventArgs e)
        {
            if (ValueChanged != null) ValueChanged(this, e);
        }

        public void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);

            foreach (var item in source1)
            {
                AddSource1(item);
            }

            foreach (var item in source2)
            {
                AddSource2(item);
            }
        }

        public void Detach()
        {
            entries.Clear();

            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        public void Dispose()
        {
            Detach();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            bool oldValue = Value;

            foreach (ICollectionChangedNotificationResult<T> change in sources)
            {
                if (change.Source == source1)
                    NotifySource1(change);
                else
                    NotifySource2(change);
            }

            bool newValue = Value;

            if (oldValue == newValue)
                return UnchangedNotificationResult.Instance;
            else
                return new ValueChangedNotificationResult<bool>(this, oldValue, newValue);
        }

        private void NotifySource1(ICollectionChangedNotificationResult<T> change)
        {
            if (change.IsReset)
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
                OnResetSource1(entries.Count);
                foreach (var item in source1)
                {
                    AddSource1(item);
                }
            }
            else
            {
                if (change.RemovedItems != null)
                {
                    foreach (var item in change.RemovedItems)
                    {
                        RemoveSource1(item);
                    }
                }

                if (change.AddedItems != null)
                {
                    foreach (var item in change.AddedItems)
                    {
                        AddSource1(item);
                    }
                }
            }
        }

        private void NotifySource2(ICollectionChangedNotificationResult<T> change)
        {
            if (change.IsReset)
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
                OnResetSource2(entries.Count);
                foreach (var item in source2)
                {
                    AddSource2(item);
                }
            }
            else
            {
                if (change.RemovedItems != null)
                {
                    foreach (var item in change.RemovedItems)
                    {
                        RemoveSource2(item);
                    }
                }

                if (change.AddedItems != null)
                {
                    foreach (var item in change.AddedItems)
                    {
                        AddSource2(item);
                    }
                }
            }
        }

        private class Entry
        {
            public int Source1Count { get; set; }

            public int Source2Count { get; set; }
        }
    }
}