using NMF.Collections.Generic;
using NMF.Expressions;
using NMF.Expressions.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace NMF.Collections.ObjectModel
{
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ObservableOrderedSet<T> : OrderedSet<T>, INotifyCollectionChanged, INotifyCollectionChanging, INotifyPropertyChanged, IOrderedSetExpression<T>, ICollectionExpression
    {
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            CollectionChanged?.Invoke(this, e);
        }

        protected void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;
        
        public event PropertyChangedEventHandler PropertyChanged;

        private INotifyCollection<T> proxy;

        public INotifyCollection<T> AsNotifiable()
        {
            if (proxy == null) proxy = this.WithUpdates();
            return proxy;
        }

        INotifyEnumerable<T> IEnumerableExpression<T>.AsNotifiable()
        {
            return AsNotifiable();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        protected bool RequireEvents()
        {
            return CollectionChanged != null || PropertyChanged != null || CollectionChanging != null;
        }

        public override bool Add(T item)
        {
            if (!RequireEvents())
            {
                return SilentAdd(item);
            }
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count);
            OnCollectionChanging(e);
            if (SilentAdd(item))
            {
                OnCollectionChanged(e);
                return true;
            }
            return false;
        }

        protected bool SilentAdd(T item)
        {
            return base.Add(item);
        }

        public override void Insert(int index, T item)
        {
            if (!RequireEvents())
            {
                base.Insert(index, item);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
                OnCollectionChanging(e);
                SilentInsert(index, item);
                OnCollectionChanged(e);
            }
        }

        protected void SilentInsert(int index, T item)
        {
            base.Insert(index, item);
        }

        public override void Clear()
        {
            if (RequireEvents())
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                SilentClear();
                OnCollectionChanged(e);
            }
            else
            {
                SilentClear();
            }
        }

        protected void SilentClear()
        {
            base.Clear();
        }

        protected override bool Remove(T item, int index)
        {
            if (!RequireEvents())
            {
                return SilentRemove(item, index);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
                OnCollectionChanging(e);
                if (SilentRemove(item, index))
                {
                    OnCollectionChanged(e);
                    return true;
                }
                return false;
            }
        }

        protected bool SilentRemove(T item, int index)
        {
            return base.Remove(item, index);
        }

        protected override void Replace(int index, T oldValue, T newValue)
        {
            if (!RequireEvents())
            {
                SilentReplace(index, oldValue, newValue);
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newValue, oldValue, index);
                OnCollectionChanging(e);
                SilentReplace(index, oldValue, newValue);
                OnCollectionChanged(e);
            }
        }

        protected void SilentReplace(int index, T oldValue, T newValue)
        {
            base.Replace(index, oldValue, newValue);
        }

        public override string ToString()
        {
            return $"[OrderedSet Count={Count}]";
        }
    }
}
