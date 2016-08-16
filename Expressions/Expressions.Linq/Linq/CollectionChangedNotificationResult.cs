using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal interface ICollectionChangedNotificationResult : INotificationResult
    {
        bool IsReset { get; }

        IList AddedItems { get; }

        IList RemovedItems { get; }

        int AddedIndex { get; }

        int RemovedIndex { get; }

        new INotifiable Source { get; set; }
    }

    internal class CollectionChangedNotificationResult<T> : ICollectionChangedNotificationResult
    {
        public bool Changed { get { return true; } }

        public INotifiable Source { get; set; }

        public bool IsReset { get; private set; }

        public List<T> AddedItems { get; private set; }

        public List<T> RemovedItems { get; private set; }

        public int AddedIndex { get; private set; }

        public int RemovedIndex { get; private set; }

        IList ICollectionChangedNotificationResult.AddedItems { get { return AddedItems; } }

        IList ICollectionChangedNotificationResult.RemovedItems { get { return RemovedItems; } }

        private CollectionChangedNotificationResult(INotifiable source, bool isReset)
        {
            Source = source;
            IsReset = isReset;
        }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems, int addedIndex = -1, int removedIndex = -1) : this(source, false)
        {
            if (addedItems != null && addedItems.Count == 0)
                AddedItems = null;
            else
                AddedItems = addedItems;

            if (removedItems != null && removedItems.Count == 0)
                removedItems = null;
            else
                RemovedItems = removedItems;

            if (AddedItems == null && RemovedItems == null)
                throw new ArgumentException("A collection change that is not a reset must have at least one add or one remove.");

            AddedIndex = addedIndex;
            RemovedIndex = removedIndex;
        }

        public CollectionChangedNotificationResult(INotifiable source) : this(source, true) { }
    }
}
