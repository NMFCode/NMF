using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    public abstract class ObservableCollectionExtended<T> : ObservableCollection<T>, INotifyCollectionChanging
    {
        public event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;

        protected virtual void OnCollectionChanging(NotifyCollectionChangingEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        protected override void ClearItems()
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Reset));
            var items = this.ToArray();
            Items.Clear();
            BeforeClearPropagates(items);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected virtual void BeforeClearPropagates(T[] items) { }

        protected override void InsertItem(int index, T item)
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Add));
            Items.Insert(index, item);
            BeforeInsertPropagates(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected virtual void BeforeInsertPropagates(int index, T item) { }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Move));
            var item = this[oldIndex];
            Items.RemoveAt(oldIndex);
            Items.Insert(newIndex, item);
            BeforeMovePropagates(item, oldIndex, newIndex);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
        }

        protected virtual void BeforeMovePropagates(T item, int oldIndex, int newIndex) { }

        protected override void RemoveItem(int index)
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Remove));
            var item = this[index];
            Items.RemoveAt(index);
            BeforeRemovePropagates(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        protected virtual void BeforeRemovePropagates(int index, T item) { }

        protected override void SetItem(int index, T item)
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Replace));
            var oldItem = this[index];
            Items[index] = item;
            BeforeSetItemPropagates(index, oldItem, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index));
        }

        protected virtual void BeforeSetItemPropagates(int index, T oldItem, T item) { }
    }
}
