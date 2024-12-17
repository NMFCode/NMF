using System;
using System.Collections.Generic;
using System.Linq;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;
using System.Collections.Specialized;
using System.Collections;

namespace NMF.Expressions
{
    internal class ConcatExpression<T> : IEnumerableExpression<T>, INotifyCollectionChanged, IList
    {
        public IEnumerableExpression<T> Source { get; private set; }
        public IEnumerable<T> Other { get; private set; }

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => true;

        int ICollection.Count => Source.Count() + Other.Count();

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => null;

        object IList.this[int index] { get => GetElementAt(index); set => throw new NotSupportedException(); }

        private object GetElementAt(int index)
        {
            foreach (var item in Source)
            {
                if (index == 0)
                {
                    return item;
                }
                index--;
            }
            foreach (var item in Other)
            {
                if (index == 0)
                {
                    return item;
                }
                index--;
            }
            throw new IndexOutOfRangeException();
        }

        private INotifyEnumerable<T> notifyEnumerable;

        private NotifyCollectionChangedEventHandler collectionChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                if (collectionChanged == null)
                {
                    AsNotifiable().CollectionChanged += NotifiableCollectionChanged;
                }
                collectionChanged += value;
            }
            remove
            {
                collectionChanged -= value;
                notifyEnumerable.CollectionChanged -= NotifiableCollectionChanged;
            }
        }

        private void NotifiableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            collectionChanged?.Invoke(this, e);
        }

        public ConcatExpression(IEnumerableExpression<T> source, IEnumerable<T> other)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (other == null) throw new ArgumentNullException(nameof(other));

            Source = source;
            Other = other;
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            if (notifyEnumerable == null)
            {
                IEnumerable<T> other = Other;
                if (Other is IEnumerableExpression<T> otherExpression)
                {
                    other = otherExpression.AsNotifiable();
                }
                notifyEnumerable = Source.AsNotifiable().Concat(other);
            }
            return notifyEnumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (notifyEnumerable != null) return notifyEnumerable.GetEnumerator();
            return SL.Concat(Source, Other).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object value)
        {
            if (value is T typed)
            {
                return Source.Contains(typed) || Other.Contains(typed);
            }
            return false;
        }

        int IList.IndexOf(object value)
        {
            if (value is T typecasted)
            {
                var index = 0;
                foreach (var item in Source)
                {
                    if (EqualityComparer<T>.Default.Equals(item, typecasted))
                    {
                        return index;
                    }
                    index++;
                }
                foreach (var item in Other)
                {
                    if (EqualityComparer<T>.Default.Equals(item, typecasted))
                    {
                        return index;
                    }
                    index++;
                }
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            foreach (var item in this)
            {
                array.SetValue(item, index);
                index++;
            }
        }
    }
}
