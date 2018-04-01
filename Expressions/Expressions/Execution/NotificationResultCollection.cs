using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NMF.Expressions
{
    public class NotificationResultCollection : IList<INotificationResult>
    {
        private readonly Entry head = new Entry(null);
        private Entry tail;
        private int count = 0;
        
        public int Count { get { return count; } }

        public bool IsReadOnly { get { return false; } }

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

        public NotificationResultCollection()
        {
            tail = head;
        }

        public void Add(INotificationResult item)
        {
            Interlocked.Increment(ref count);
            var entry = new Entry(item);
            var oldTail = Interlocked.Exchange(ref tail, entry);
            oldTail.Next = entry;
        }

        public void UnsafeAdd(INotificationResult item)
        {
            count++;
            var entry = new Entry(item);
            tail.Next = entry;
            tail = entry;
        }

        public void Clear()
        {
            tail = head;
            head.Next = null;
            count = 0;
        }

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

        public bool Contains(INotificationResult item)
        {
            foreach (var it in this)
            {
                if (item == it) return true;
            }
            return false;
        }

        public void CopyTo(INotificationResult[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        public bool Remove(INotificationResult item)
        {
            throw new NotSupportedException();
        }

        public int IndexOf(INotificationResult item)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, INotificationResult item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        private class Entry
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
