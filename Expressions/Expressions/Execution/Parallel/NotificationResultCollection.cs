using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NMF.Expressions
{
    internal class NotificationResultCollection
    {
        private INotificationResult[] array = new INotificationResult[2];
        private int count = -1;

        public int Count { get { return count + 1; } }

        public IList<INotificationResult> Values { get { return new ArraySegment<INotificationResult>(array, 0, Count); } }

        public void Add(INotificationResult item)
        {
            INotificationResult[] oldArray, newArray;
            var index = Interlocked.Increment(ref count);

            do
            {
                oldArray = array;
                newArray = array;
                EnsureCapacity(ref newArray, index + 1);
                newArray[index] = item;
            } while (Interlocked.CompareExchange(ref array, newArray, oldArray) != newArray);
        }

        public void UnsafeAdd(INotificationResult item)
        {
            count++;
            EnsureCapacity(ref array, count + 1);
            array[count] = item;
        }

        public void Clear()
        {
            for (int i = 0; i < Count; i++)
                array[i] = null;
            count = -1;
        }

        private void EnsureCapacity(ref INotificationResult[] array, int capacity)
        {
            if (capacity > array.Length)
            {
                var newArray = new INotificationResult[Math.Max(capacity, array.Length * 2)];
                Array.Copy(array, newArray, array.Length);
                array = newArray;
            }
        }
    }
}
