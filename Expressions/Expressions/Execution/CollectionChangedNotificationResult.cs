using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public interface ICollectionChangedNotificationResult : INotificationResult
    {
        bool IsReset { get; }

        IList AddedItems { get; }

        IList RemovedItems { get; }

        IList MovedItems { get; }
    }

    public class CollectionChangedNotificationResult<T> : ICollectionChangedNotificationResult
    {
        private INotifiable source;
        private readonly bool isReset;
        private readonly List<T> addedItems;
        private readonly List<T> removedItems;
        private readonly List<T> movedItems;

        public bool Changed { get { return true; } }

        public INotifiable Source { get { return source; } }

        public bool IsReset { get { return isReset; } }

        public List<T> AddedItems { get { return addedItems; } }

        public List<T> RemovedItems { get { return removedItems; } }

        public List<T> MovedItems { get { return movedItems; } }

        IList ICollectionChangedNotificationResult.AddedItems { get { return addedItems; } }

        IList ICollectionChangedNotificationResult.RemovedItems { get { return removedItems; } }

        IList ICollectionChangedNotificationResult.MovedItems { get { return movedItems; } }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems) : this(source, addedItems, removedItems, null) { }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems, List<T> movedItems)
        {
            this.source = source;
            this.isReset = false;

            if (addedItems != null && addedItems.Count > 0)
                this.addedItems = addedItems;

            if (removedItems != null && removedItems.Count > 0)
                this.removedItems = removedItems;

            if (movedItems != null && movedItems.Count > 0)
                this.movedItems = movedItems;

            if (this.addedItems == null && this.removedItems == null && this.movedItems == null)
                throw new ArgumentException("A collection change that is not a reset must have at least one add, remove, move or replace.");
        }
        
        public CollectionChangedNotificationResult(INotifiable source)
        {
            this.source = source;
            this.isReset = true;
        }

        public static CollectionChangedNotificationResult<T> Transfer(ICollectionChangedNotificationResult oldResult, INotifiable newSource)
        {
            if (oldResult.IsReset)
                return new CollectionChangedNotificationResult<T>(newSource);

            var orig = oldResult as CollectionChangedNotificationResult<T>;
            if (orig != null)
                return new CollectionChangedNotificationResult<T>(newSource, orig.AddedItems, orig.RemovedItems, orig.MovedItems);
            
            return new CollectionChangedNotificationResult<T>(newSource,
                Cast(oldResult.AddedItems),
                Cast(oldResult.RemovedItems),
                Cast(oldResult.MovedItems));
        }

        private static List<T> Cast(IList list)
        {
            if (list == null || list.Count == 0)
                return null;
            
            var result = new List<T>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                result.Add((T)list[i]);
            }
            return result;
        }
    }
}
