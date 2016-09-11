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

        IList ReplaceAddedItems { get; }

        IList ReplaceRemovedItems { get; }
    }

    public class CollectionChangedNotificationResult<T> : ICollectionChangedNotificationResult
    {
        private INotifiable source;
        private readonly bool isReset;
        private readonly List<T> addedItems;
        private readonly List<T> removedItems;
        private readonly List<T> movedItems;
        private readonly List<T> replaceAddedItems;
        private readonly List<T> replaceRemovedItems;

        public bool Changed { get { return true; } }

        public INotifiable Source { get { return source; } }

        public bool IsReset { get { return isReset; } }

        public List<T> AddedItems { get { return addedItems; } }

        public List<T> RemovedItems { get { return removedItems; } }

        public List<T> MovedItems { get { return movedItems; } }

        public List<T> ReplaceAddedItems { get { return replaceAddedItems; } }

        public List<T> ReplaceRemovedItems { get { return replaceRemovedItems; } }

        public IEnumerable<T> AllAddedItems
        {
            get
            {
                if (addedItems == null)
                {
                    if (replaceAddedItems == null)
                        return Enumerable.Empty<T>();
                    else
                        return replaceAddedItems;
                }
                else
                {
                    if (replaceAddedItems == null)
                        return addedItems;
                    else
                        return addedItems.Concat(replaceAddedItems);
                }
            }
        }

        public IEnumerable<T> AllRemovedItems
        {
            get
            {
                if (removedItems == null)
                {
                    if (replaceRemovedItems == null)
                        return Enumerable.Empty<T>();
                    else
                        return replaceRemovedItems;
                }
                else
                {
                    if (replaceRemovedItems == null)
                        return removedItems;
                    else
                        return removedItems.Concat(replaceRemovedItems);
                }
            }
        }

        IList ICollectionChangedNotificationResult.AddedItems { get { return addedItems; } }

        IList ICollectionChangedNotificationResult.RemovedItems { get { return removedItems; } }

        IList ICollectionChangedNotificationResult.MovedItems { get { return movedItems; } }

        IList ICollectionChangedNotificationResult.ReplaceAddedItems { get { return replaceAddedItems; } }

        IList ICollectionChangedNotificationResult.ReplaceRemovedItems { get { return replaceRemovedItems; } }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems) : this(source, addedItems, removedItems, null, null, null) { }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems, List<T> movedItems) : this(source, addedItems, removedItems, movedItems, null, null) { }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems, List<T> replaceAddedItems, List<T> replaceRemovedItems) : this(source, addedItems, removedItems, null, replaceAddedItems, replaceRemovedItems) { }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems, List<T> movedItems, List<T> replaceAddedItems, List<T> replaceRemovedItems)
        {
            this.source = source;
            this.isReset = false;

            if (addedItems != null && addedItems.Count > 0)
                this.addedItems = addedItems;

            if (removedItems != null && removedItems.Count > 0)
                this.removedItems = removedItems;

            if (movedItems != null && movedItems.Count > 0)
                this.movedItems = movedItems;

            if (replaceAddedItems != null && replaceAddedItems.Count > 0)
                this.replaceAddedItems = replaceAddedItems;

            if (replaceRemovedItems != null && replaceRemovedItems.Count > 0)
                this.replaceRemovedItems = replaceRemovedItems;

            if (this.replaceAddedItems != null || this.replaceRemovedItems != null)
            {
                if ((this.replaceAddedItems == null || this.replaceRemovedItems == null)
                    || this.replaceAddedItems.Count != this.replaceRemovedItems.Count)
                {
                    throw new ArgumentException("The replace lists must have the same number of items.");
                }
            }

            if (this.addedItems == null && this.removedItems == null && this.movedItems == null && this.replaceAddedItems == null && this.replaceRemovedItems == null)
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
                return new CollectionChangedNotificationResult<T>(newSource, orig.AddedItems, orig.RemovedItems, orig.MovedItems, orig.ReplaceAddedItems, orig.ReplaceRemovedItems);
            
            return new CollectionChangedNotificationResult<T>(newSource,
                Cast(oldResult.AddedItems),
                Cast(oldResult.RemovedItems),
                Cast(oldResult.MovedItems),
                Cast(oldResult.ReplaceAddedItems),
                Cast(oldResult.ReplaceRemovedItems));
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
