using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NMF.Expressions
{
    internal class NotificationResultCollection : IList<INotificationResult>
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
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(INotificationResult item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(INotificationResult[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(INotificationResult item)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(INotificationResult item)
        {
            throw new NotImplementedException();
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

        private struct Enumerator : IEnumerator<INotificationResult>
        {
            private readonly NotificationResultCollection collection;
            private Entry currentEntry;

            public Enumerator(NotificationResultCollection collection)
            {
                this.collection = collection;
                this.currentEntry = collection.head;
            }

            public INotificationResult Current { get { return currentEntry.Item; } }

            object IEnumerator.Current { get { return Current; } }

            public void Dispose() { }

            public bool MoveNext()
            {
                if (currentEntry.Next != null)
                {
                    currentEntry = currentEntry.Next;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                currentEntry = collection.head;
            }
        }
    }
}
