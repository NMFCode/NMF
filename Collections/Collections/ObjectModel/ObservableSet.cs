using NMF.Collections.Generic;
using NMF.Expressions;
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
    public class ObservableSet<T> : DecoratedSet<T>, ISet<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, INotifyCollectionChanged, INotifyEnumerable<T>, INotifyPropertyChanged, INotifyCollection<T>, ISetExpression<T>
    {
        public override bool Add(T item)
        {
            if (base.Add(item))
            {
                OnInsertItem(item);
                return true;
            }
            return false;
        }

        public override void Clear()
        {
            base.Clear();
            OnClear();
        }

        public override bool Remove(T item)
        {
            if (base.Remove(item))
            {
                OnRemoveItem(item);
                return true;
            }
            return false;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Count");
            if (CollectionChanged != null) CollectionChanged(this, e);
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
