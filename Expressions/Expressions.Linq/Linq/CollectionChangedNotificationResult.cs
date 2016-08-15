using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal abstract class CollectionChangedNotificationResult : INotificationResult
    {
        public bool Changed { get { return true; } }

        public INotifiable Source { get; private set; }

        public bool IsReset { get; private set; }

        protected CollectionChangedNotificationResult(INotifiable source, bool isReset)
        {
            Source = source;
            IsReset = isReset;
        }

        public CollectionChangedNotificationResult Forward(INotifiable newSource)
        {
            Source = newSource;
            return this;
        }
    }

    internal class CollectionChangedNotificationResult<T> : CollectionChangedNotificationResult
    {
        public List<T> AddedItems { get; private set; }

        public List<T> RemovedItems { get; private set; }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems = null, List<T> removedItems = null) : base(source, false)
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
        }

        public CollectionChangedNotificationResult(INotifiable source) : base(source, true) { }
    }
}
