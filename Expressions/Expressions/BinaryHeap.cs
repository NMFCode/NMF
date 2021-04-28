using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// A binary heap, useful for sorting data and priority queues.
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class BinaryHeap<T> : ICollection<T>
    {
        private const int initialSize = 4;
        private T[] data = new T[initialSize];
        private bool sorted = false;
        private int count = 0;
        private int capacity = initialSize;
        private readonly IComparer<T> comparer;


        /// <summary>
        /// Gets the number of values in the heap. 
        /// </summary>
        public int Count
        {
            get { return count; }
        }


        /// <summary>
        /// Gets or sets the capacity of the heap.
        /// </summary>
        public int Capacity
        {
            get { return capacity; }
            set
            {
                int previousCapacity = capacity;
                capacity = Math.Max(value, count);
                if (capacity != previousCapacity)
                {
                    T[] temp = new T[capacity];
                    Array.Copy(data, temp, count);
                    data = temp;
                }
            }
        }

        /// <summary>
        /// Creates a new binary heap
        /// </summary>
        public BinaryHeap()
            : this(null)
        {
        }

        /// <summary>
        /// Creates a new binary heap.
        /// </summary>
        /// <param name="comparer">The comparer used to compare the items</param>
        public BinaryHeap(IComparer<T> comparer)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
        }


        private BinaryHeap(T[] data, int count, IComparer<T> comparer)
        {
            Capacity = count;
            this.count = count;
            this.comparer = comparer;
            Array.Copy(data, this.data, count);
        }

        /// <summary>
        /// Gets the first value in the heap without removing it.
        /// </summary>
        /// <returns>The lowest value of type TValue.</returns>
        public T Peek()
        {
            return data[0];
        }

        /// <summary>
        /// Removes all items from the heap.
        /// </summary>
        public void Clear()
        {
            this.count = 0;
            data = new T[capacity];
        }

        /// <summary>
        /// Adds a key and value to the heap.
        /// </summary>
        /// <param name="item">The item to add to the heap.</param>
        public void Add(T item)
        {
            if (count == capacity)
            {
                Capacity *= 2;
            }
            data[count] = item;
            SiftUp();
            count++;
        }


        /// <summary>
        /// Removes and returns the first item in the heap.
        /// </summary>
        /// <returns>The next value in the heap.</returns>
        public T Remove()
        {
            if (this.count == 0)
            {
                throw new InvalidOperationException("Cannot remove item, heap is empty.");
            }
            T v = data[0];
            count--;
            data[0] = data[count];
            SiftDown();
            return v;
        }


        private void SiftUp()
        {
            sorted = false;
            int p = count;
            T item = data[p];
            int par = Parent(p);
            while (par > -1 && comparer.Compare(item, data[par]) < 0)
            {
                data[p] = data[par];
                p = par;
                par = Parent(p);
            }
            data[p] = item;
        }

        private void SiftDown()
        {
            sorted = false;
            int n;
            int p = 0;
            T item = data[p];
            while (true)
            {
                int ch1 = Child1(p);
                if (ch1 >= count) break;
                int ch2 = Child2(p);
                if (ch2 >= count)
                {
                    n = ch1;
                }
                else
                {
                    n = comparer.Compare(data[ch1], data[ch2]) < 0 ? ch1 : ch2;
                }
                if (comparer.Compare(item, data[n]) > 0)
                {
                    data[p] = data[n];
                    p = n;
                }
                else
                {
                    break;
                }
            }
            data[p] = item;
        }

        private void EnsureSort()
        {
            if (sorted) return;
            Array.Sort(data, 0, count, comparer);
            sorted = true;
        }

        private static int Parent(int index)
        {
            return (index - 1) >> 1;
        }


        private static int Child1(int index)
        {
            return (index << 1) + 1;
        }


        private static int Child2(int index)
        {
            return (index << 1) + 2;
        }

        /// <summary>
        /// Creates a new instance of an identical binary heap.
        /// </summary>
        /// <returns>A BinaryHeap.</returns>
        public BinaryHeap<T> Copy()
        {
            return new BinaryHeap<T>(data, count, comparer);
        }

        /// <summary>
        /// Gets an enumerator for the binary heap.
        /// </summary>
        /// <returns>An IEnumerator of type T.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return data[i];
            }
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Checks to see if the binary heap contains the specified item.
        /// </summary>
        /// <param name="item">The item to search the binary heap for.</param>
        /// <returns>A boolean, true if binary heap contains item.</returns>
        public bool Contains(T item)
        {
            EnsureSort();
            return Array.BinarySearch<T>(data, 0, count, item) >= 0;
        }

        /// <summary>
        /// Copies the binary heap to an array at the specified index.
        /// </summary>
        /// <param name="array">One dimensional array that is the destination of the copied elements.</param>
        /// <param name="arrayIndex">The zero-based index at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            EnsureSort();
            Array.Copy(data, array, count);
        }

        /// <summary>
        /// Gets whether or not the binary heap is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes an item from the binary heap. This utilizes the type T's Comparer and will not remove duplicates.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns>Boolean true if the item was removed.</returns>
        public bool Remove(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, Peek()))
            {
                Remove();
                return true;
            }
            EnsureSort();
            int i = Array.BinarySearch<T>(data, 0, count, item, comparer);
            if (i < 0) return false;
            Array.Copy(data, i + 1, data, i, count - i - 1);
            count--;
            return true;
        }

    }
}
