using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace NMF.Collections.ObjectModel
{
    /// <summary>
    /// Denotes an abstract extension of observable collections
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ObservableCollectionExtended<T> : Collection<T>, INotifyCollectionChanging, INotifyPropertyChanged, INotifyCollectionChanged
    {
        /// <inheritdoc />
        public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets called when an attempt is made to change the collection
        /// </summary>
        /// <param name="e">the event args</param>
        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        /// <summary>
        /// Gets called when the collection was changed
        /// </summary>
        /// <param name="e">the event args</param>
        /// <param name="countAffected">True, if the Count is also affected, otherwise False</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e, bool countAffected)
        {
            CollectionChanged?.Invoke(this, e);
            if (countAffected)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }

        /// <summary>
        /// Determines whether events are required
        /// </summary>
        /// <returns>True, if there is any subscriber to CollectionChanged, CollectionChanging or PropertyChanged events</returns>
        protected bool RequireEvents()
        {
            return CollectionChanged != null || CollectionChanging != null || PropertyChanged != null;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <summary>
        /// Moves a given item
        /// </summary>
        /// <param name="oldIndex">the old index</param>
        /// <param name="newIndex">the new index</param>
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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
