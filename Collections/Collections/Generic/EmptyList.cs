using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using NMF.Expressions;
using NMF.Expressions.Linq;
using System.Collections.ObjectModel;

namespace NMF.Collections.Generic
{
    public sealed class EmptyList<T> : IList<T>, ICollection<T>, IEnumerableExpression<T>, ICollectionExpression<T>, IListExpression<T>
    {
        private static readonly EmptyList<T> instance = new EmptyList<T>();

        public static EmptyList<T> Instance
        {
            get
            {
                return instance;
            }
        }
        
        int IList<T>.IndexOf(T item)
        {
            throw new IndexOutOfRangeException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        T IList<T>.this[int index]
        {
            get
            {
                throw new IndexOutOfRangeException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            return false;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
        }

        int ICollection<T>.Count
        {
            get { return 0; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }
        
        bool ICollection<T>.Remove(T item)
        {
            return false;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Enumerable.Empty<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Enumerable.Empty<T>().GetEnumerator();
        }

        public INotifyCollection<T> AsNotifiable()
        {
            return new Collection<T>().WithUpdates();
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
