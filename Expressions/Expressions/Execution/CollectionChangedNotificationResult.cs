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
        public bool Changed { get { return true; } }

        public INotifiable Source { get; set; }

        public bool IsReset { get; private set; }

        public List<T> AddedItems { get; private set; }

        public List<T> RemovedItems { get; private set; }

        public List<T> MovedItems { get; private set; }

        public List<T> ReplaceAddedItems { get; private set; }

        public List<T> ReplaceRemovedItems { get; private set; }

        public IEnumerable<T> AllAddedItems
        {
            get
            {
                if (AddedItems == null)
                {
                    if (ReplaceAddedItems == null)
                        return Enumerable.Empty<T>();
                    else
                        return ReplaceAddedItems;
                }
                else
                {
                    if (ReplaceAddedItems == null)
                        return AddedItems;
                    else
                        return AddedItems.Concat(ReplaceAddedItems);
                }
            }
        }

        public IEnumerable<T> AllRemovedItems
        {
            get
            {
                if (RemovedItems == null)
                {
                    if (ReplaceRemovedItems == null)
                        return Enumerable.Empty<T>();
                    else
                        return ReplaceRemovedItems;
                }
                else
                {
                    if (ReplaceRemovedItems == null)
                        return RemovedItems;
                    else
                        return RemovedItems.Concat(ReplaceRemovedItems);
                }
            }
        }

        IList ICollectionChangedNotificationResult.AddedItems { get { return AddedItems; } }

        IList ICollectionChangedNotificationResult.RemovedItems { get { return RemovedItems; } }

        IList ICollectionChangedNotificationResult.MovedItems { get { return MovedItems; } }

        IList ICollectionChangedNotificationResult.ReplaceAddedItems { get { return ReplaceAddedItems; } }

        IList ICollectionChangedNotificationResult.ReplaceRemovedItems { get { return ReplaceRemovedItems; } }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems) : this(source, addedItems, removedItems, null, null, null) { }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems, List<T> movedItems) : this(source, addedItems, removedItems, movedItems, null, null) { }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems, List<T> replaceAddedItems, List<T> replaceRemovedItems) : this(source, addedItems, removedItems, null, replaceAddedItems, replaceRemovedItems) { }

        public CollectionChangedNotificationResult(INotifiable source, List<T> addedItems, List<T> removedItems, List<T> movedItems, List<T> replaceAddedItems, List<T> replaceRemovedItems)
        {
            Source = source;
            IsReset = false;

            if (addedItems != null && addedItems.Count > 0)
                AddedItems = addedItems;

            if (removedItems != null && removedItems.Count > 0)
                RemovedItems = removedItems;

            if (movedItems != null && movedItems.Count > 0)
                MovedItems = movedItems;

            if (replaceAddedItems != null && replaceAddedItems.Count > 0)
                ReplaceAddedItems = replaceAddedItems;

            if (replaceRemovedItems != null && replaceRemovedItems.Count > 0)
                ReplaceRemovedItems = replaceRemovedItems;

            if (ReplaceAddedItems != null || ReplaceRemovedItems != null)
            {
                if ((ReplaceAddedItems == null || ReplaceRemovedItems == null)
                    || ReplaceAddedItems.Count != ReplaceRemovedItems.Count)
                {
                    throw new ArgumentException("The replace lists must have the same number of items.");
                }
            }

            if (AddedItems == null && RemovedItems == null && MovedItems == null && ReplaceAddedItems == null && ReplaceRemovedItems == null)
                throw new ArgumentException("A collection change that is not a reset must have at least one add, remove, move or replace.");
        }
        
        public CollectionChangedNotificationResult(INotifiable source)
        {
            Source = source;
            IsReset = true;
        }

        public static CollectionChangedNotificationResult<T> Transfer(ICollectionChangedNotificationResult oldResult, INotifiable newSource)
        {
            var orig = oldResult as CollectionChangedNotificationResult<T>;
            if (orig != null)
            {
                orig.Source = newSource;
                return orig;
            }

            if (oldResult.IsReset)
                return new CollectionChangedNotificationResult<T>(newSource);

            return new CollectionChangedNotificationResult<T>(newSource,
                oldResult.AddedItems?.Cast<T>().ToList(),
                oldResult.RemovedItems?.Cast<T>().ToList(),
                oldResult.MovedItems?.Cast<T>().ToList(),
                oldResult.ReplaceAddedItems?.Cast<T>().ToList(),
                oldResult.ReplaceRemovedItems?.Cast<T>().ToList());
        }
    }
}
