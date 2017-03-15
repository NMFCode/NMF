using NMF.Collections.Generic;
using NMF.Expressions;
using NMF.Expressions.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ObservableSet<T> : DecoratedSet<T>, ISet<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, INotifyCollectionChanged, INotifyCollectionChanging, INotifyPropertyChanged, ISetExpression<T>
    {
        public override bool Add(T item)
        {
            if (item != null && !base.Contains(item))
            {
                OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Add));
                base.Add(item);
                OnInsertItem(item);
                return true;
            }
            return false;
        }

        public override void Clear()
        {
            OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Reset));
            base.Clear();
            OnClear();
        }

        public override bool Remove(T item)
        {
            if (item != null && base.Contains(item))
            {
                OnCollectionChanging(new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Remove));
                base.Remove(item);
                OnRemoveItem(item);
                return true;
            }
            return false;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Count");
            CollectionChanged?.Invoke(this, e);
        }

        protected void OnCollectionChanging(NotifyCollectionChangingEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        protected virtual void OnClear()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected virtual void OnInsertItem(T item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        protected virtual void OnRemoveItem(T item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        protected virtual void OnReplaceItem(T oldItem, T newItem)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public INotifyCollection<T> AsNotifiable()
        {
            return this.WithUpdates();
        }

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return AsNotifiable();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }
    }
}
