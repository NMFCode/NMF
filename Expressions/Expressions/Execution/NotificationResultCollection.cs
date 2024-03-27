using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a collection of results
    /// </summary>
    public class NotificationResultCollection : IList<INotificationResult>
    {
        private readonly Entry head = new Entry(null);
        private Entry tail;
        private int count = 0;

        /// <inheritdoc />
        public int Count { get { return count; } }

        /// <inheritdoc />
        public bool IsReadOnly { get { return false; } }

        /// <inheritdoc />
        public INotificationResult this[int index]
        {
            get
            {
                if (count == 0 || index < 0)
                    throw new IndexOutOfRangeException();
                var current = head.Next;
                for (int i = 0; i < index; i++)
                {
                    current = current.Next;
                    if (current == null)
                        throw new IndexOutOfRangeException();
                }
                return current.Item;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        public NotificationResultCollection()
        {
            tail = head;
        }

        /// <inheritdoc />
        public void Add(INotificationResult item)
        {
            Interlocked.Increment(ref count);
            var entry = new Entry(item);
            var oldTail = Interlocked.Exchange(ref tail, entry);
            oldTail.Next = entry;
        }

        /// <summary>
        /// Add an item not threadsafe
        /// </summary>
        /// <param name="item">The item to add</param>
        public void UnsafeAdd(INotificationResult item)
        {
            count++;
            var entry = new Entry(item);
            tail.Next = entry;
            tail = entry;
        }

        /// <inheritdoc />
        public void Clear()
        {
            tail = head;
            head.Next = null;
            count = 0;
        }

        /// <inheritdoc />
        public IEnumerator<INotificationResult> GetEnumerator()
        {
            var current = head.Next;
            while (current != null)
            {
                yield return current.Item;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public bool Contains(INotificationResult item)
        {
            foreach (var it in this)
            {
                if (item == it) return true;
            }
            return false;
        }

        /// <inheritdoc />
        public void CopyTo(INotificationResult[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        /// <inheritdoc />
        public bool Remove(INotificationResult item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public int IndexOf(INotificationResult item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Insert(int index, INotificationResult item)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        private sealed class Entry
        {
            public readonly INotificationResult Item;

            public Entry Next;

            public Entry(INotificationResult item)
            {
                Item = item;
            }
        }
    }
}
