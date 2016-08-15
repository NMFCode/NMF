using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal class CollectionChangedNotificationResult<T> : INotificationResult
    {
        public bool Changed { get { return true; } }

        public INotifiable Source { get; private set; }

        public List<T> AddedItems { get; private set; }

        public List<T> RemovedItems { get; private set; }

        public bool IsReset { get; private set; }

        private CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems, bool isReset)
        {
            Source = source;
            AddedItems = addedItems;
            RemovedItems = removedItems;
            IsReset = isReset;
        }

        public static CollectionChangedNotificationResult<T> Reset(INotifiable source)
        {
            return new CollectionChangedNotificationResult<T>(source, null, null, true);
        }

        public static CollectionChangedNotificationResult<T> AddRemove(INotifiable source, List<T> addedItems, List<T> removedItems)
        {
            if (addedItems != null && addedItems.Count == 0)
                addedItems = null;
            if (removedItems != null && removedItems.Count == 0)
                removedItems = null;
            return new CollectionChangedNotificationResult<T>(source, addedItems, removedItems, false);
        }
    }
}
