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
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ObservableSet<T> : DecoratedSet<T>, ISet<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, INotifyCollectionChanged, INotifyCollectionChanging, INotifyPropertyChanged, ISetExpression<T>
    {
        public override bool Add(T item)
        {
            if (!RequireEvents())
            {
                if (base.Add(item))
                {
                    return true;
                }
                return false;
            }
            if (item != null && !base.Contains(item))
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item);
                OnCollectionChanging(e);
                SilentAdd(item);
                OnCollectionChanged(e);
                return true;
            }
            return false;
        }

        protected bool RequireEvents()
        {
            return CollectionChanged != null || PropertyChanged != null || CollectionChanging != null;
        }

        protected bool SilentAdd(T item)
        {
            return base.Add(item);
        }

        public override void Clear()
        {
            if (!RequireEvents())
            {
                base.Clear();
            }
            else
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanging(e);
                SilentClear();
                OnCollectionChanged(e);
            }
        }

        protected void SilentClear()
        {
            base.Clear();
        }

        public override bool Remove(T item)
        {
            if (CollectionChanged == null && PropertyChanged == null && CollectionChanging == null)
            {
                return base.Remove(item);
            }
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item);
            OnCollectionChanging(e);
            if (SilentRemove(item))
            {
                OnCollectionChanged(e);
                return true;
            }
            return false;
        }

        protected bool SilentRemove(T item)
        {
            return base.Remove(item);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanging;

        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Count");
            CollectionChanged?.Invoke(this, e);
        }

        protected void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }
        
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

        public override string ToString()
        {
            return $"[Set Count={Count}]";
        }
    }
}
