using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    public abstract class ObservableCollectionExtended<T> : Collection<T>, INotifyCollectionChanging, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e, bool countAffected)
        {
            CollectionChanged?.Invoke(this, e);
            if (countAffected)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }

        protected bool RequireEvents()
        {
            return CollectionChanged != null || CollectionChanging != null || PropertyChanged != null;
        }

        protected override void ClearItems()
        {
            if (RequireEvents())
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                Items.Clear();
                OnCollectionChanged(e, true);
            }
            else
            {
                Items.Clear();
            }
        }

        protected override void InsertItem(int index, T item)
        {
            if (RequireEvents())
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                OnCollectionChanging(e);
                Items.Insert(index, item);
                OnCollectionChanged(e, true);
            }
            else
            {
                Items.Insert(index, item);
            }
        }

        public virtual void MoveItem(int oldIndex, int newIndex)
        {
            var item = this[oldIndex];
            if (RequireEvents())
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
                OnCollectionChanging(e);
                Items.RemoveAt(oldIndex);
                Items.Insert(newIndex, item);
                OnCollectionChanged(e, false);
            }
            else
            {
                Items.RemoveAt(oldIndex);
                Items.Insert(newIndex, item);
            }
        }

        protected override void RemoveItem(int index)
        {
            if (RequireEvents())
            {
                var item = this[index];
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
                OnCollectionChanging(e);
                Items.RemoveAt(index);
                OnCollectionChanged(e, true);
            }
            else
            {
                Items.RemoveAt(index);
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (RequireEvents())
            {
                var oldItem = this[index];
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index);
                OnCollectionChanging(e);
                Items[index] = item;
                OnCollectionChanged(e, false);
            }
            else
            {
                Items[index] = item;
            }
        }
    }
}
