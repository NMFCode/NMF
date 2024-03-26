using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Collections.Generic
{
    /// <summary>
    /// Denotes a readonly view on a list selection
    /// </summary>
    /// <typeparam name="TSource">The element type of the source collection</typeparam>
    /// <typeparam name="T">The element type</typeparam>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(EnumerableDebuggerProxy<>))]
    public class ReadOnlyListSelection<TSource, T> : IList<T>
    {
        private readonly IList<TSource> source;
        private readonly Func<TSource, T> selector;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="source">The source list</param>
        /// <param name="selector">The selector</param>
        public ReadOnlyListSelection(IList<TSource> source, Func<TSource, T> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Add(T item)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public void Clear()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return source.Any(o => object.Equals(selector(o), item));
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            for (int i = 0; i < source.Count; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        /// <inheritdoc />
        public int Count
        {
            get { return source.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
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
