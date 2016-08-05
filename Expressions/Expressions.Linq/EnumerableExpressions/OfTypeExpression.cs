using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SL = System.Linq.Enumerable;
using NMF.Expressions.Linq;
using System.Collections;

namespace NMF.Expressions
{
    internal class OfTypeExpression<T> : IEnumerableExpression<T>
    {
        public IEnumerableExpression Source { get; private set; }

        public OfTypeExpression(IEnumerableExpression source)
        {
            if (source == null) throw new ArgumentNullException("source");

            Source = source;
        }

        public INotifyEnumerable<T> AsNotifiable()
        {
            return Source.AsNotifiable().OfType<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return SL.OfType<T>(Source).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        INotifyEnumerable IEnumerableExpression.AsNotifiable()
        {
            return AsNotifiable();
        }
    }

    internal class OfTypeCollectionExpression<TSource, T> : OfTypeExpression<T>, ICollectionExpression<T>
        where T : TSource
    {
        private ICollectionExpression<TSource> casted;
        public OfTypeCollectionExpression(ICollectionExpression<TSource> source) : base(source)
        {
            casted = source;
        }

        public int Count
        {
            get
            {
                return SL.OfType<T>(Source).Count();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return casted.IsReadOnly;
            }
        }

        public void Add(T item)
        {
            casted.Add(item);
        }

        public void Clear()
        {
            foreach (var item in SL.OfType<T>(Source).ToArray())
            {
                casted.Remove(item);
            }
        }

        public bool Contains(T item)
        {
            return casted.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        public bool Remove(T item)
        {
            return casted.Remove(item);
        }

        INotifyCollection<T> ICollectionExpression<T>.AsNotifiable()
        {
            return casted.AsNotifiable().OfType<TSource, T>();
        }
    }

}
