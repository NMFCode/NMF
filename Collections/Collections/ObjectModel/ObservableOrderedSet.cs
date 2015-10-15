using NMF.Collections.Generic;
using NMF.Expressions;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace NMF.Collections.ObjectModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ObservableOrderedSet<T> : OrderedSet<T>, INotifyCollectionChanged, INotifyEnumerable<T>, INotifyPropertyChanged, INotifyCollection<T>, IOrderedSetExpression<T>
    {
        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            if (CollectionChanged != null) CollectionChanged(this, e);
        }

        protected override void OnCleared()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void OnInsertItem(T item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        protected override void OnRemoveItem(T item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        protected override void OnReplaceItem(T oldItem, T newItem)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        void INotifyEnumerable.Attach() { }

        void INotifyEnumerable.Detach() { }

        bool INotifyEnumerable.IsAttached
        {
            get { return true; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public INotifyCollection<T> AsNotifiable()
        {
            return this;
        }

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return this;
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return this;
        }
    }
}
