using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Collections.Generic
{
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ReadOnlyListSelection<TSource, T> : IList<T>
    {
        private readonly IList<TSource> source;
        private readonly Func<TSource, T> selector;

        public ReadOnlyListSelection(IList<TSource> source, Func<TSource, T> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public int IndexOf(T item)
        {
            return source.IndexOf(source.FirstOrDefault(o => object.Equals(selector(o), item)));
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new InvalidOperationException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new InvalidOperationException();
        }

        public T this[int index]
        {
            get
            {
                return selector(source[index]);
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public void Add(T item)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(T item)
        {
            return source.Any(o => object.Equals(selector(o), item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            for (int i = 0; i < source.Count; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        public int Count
        {
            get { return source.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(T item)
        {
            throw new InvalidOperationException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return source.Select(selector).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
