using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    public class ObservableCollectionExtended<T> : ObservableCollection<T>, INotifyCollectionChanging
    {
        public event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;

        protected virtual void OnCollectionChanging(NotifyCollectionChangingEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        protected override void ClearItems()
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Reset));
            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Add));
            base.InsertItem(index, item);
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Move));
            base.MoveItem(oldIndex, newIndex);
        }

        protected override void RemoveItem(int index)
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Remove));
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Replace));
            base.SetItem(index, item);
        }
    }
}
