using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NMF.Expressions
{
    public interface ICollectionChangedNotificationResult : INotificationResult
    {
        bool IsReset { get; }

        IList AddedItems { get; }

        IList RemovedItems { get; }

        IList MovedItems { get; }

        int OldItemsStartIndex { get; }

        int NewItemsStartIndex { get; }
    }
    public interface ICollectionChangedNotificationResult<T> : ICollectionChangedNotificationResult
    {
        new List<T> AddedItems { get; }

        new List<T> MovedItems { get; }

        new List<T> RemovedItems { get; }
    }

    public class CollectionChangedNotificationResult<T> : ICollectionChangedNotificationResult<T>
    {
        private static ConcurrentBag<CollectionChangedNotificationResult<T>> pool = new ConcurrentBag<CollectionChangedNotificationResult<T>>();

        private INotifiable source;
        private bool isReset;
        private readonly List<T> addedItems = new List<T>();
        private readonly List<T> movedItems = new List<T>();
        private readonly List<T> removedItems = new List<T>();
        private int references;

        public bool Changed { get { return isReset || addedItems.Count > 0 || removedItems.Count > 0 || movedItems.Count > 0; } }

        public INotifiable Source { get { return source; } }

        public bool IsReset { get { return isReset; } }

        public List<T> AddedItems { get { return addedItems; } }

        public List<T> MovedItems { get { return movedItems; } }

        public List<T> RemovedItems { get { return removedItems; } }

        IList ICollectionChangedNotificationResult.AddedItems { get { return addedItems; } }

        IList ICollectionChangedNotificationResult.RemovedItems { get { return removedItems; } }

        IList ICollectionChangedNotificationResult.MovedItems { get { return movedItems; } }

        public int OldItemsStartIndex { get; set; } = -1;

        public int NewItemsStartIndex { get; set; } = -1;

        private CollectionChangedNotificationResult(INotifiable source, bool isReset)
        {
            this.source = source;
            this.isReset = isReset;
        }

        public static CollectionChangedNotificationResult<T> Create(INotifiable source, bool isReset = false)
        {
            CollectionChangedNotificationResult<T> item;
            if (pool.TryTake(out item))
            {
                item.source = source;
                item.addedItems.Clear();
                item.movedItems.Clear();
                item.removedItems.Clear();
                item.NewItemsStartIndex = -1;
                item.OldItemsStartIndex = -1;
                item.isReset = isReset;
            }
            else
            {
                item = new CollectionChangedNotificationResult<T>(source, isReset);
            }
            return item;
        }

        public void TurnIntoReset()
        {
            this.isReset = true;
        }

        public static CollectionChangedNotificationResult<T> Transfer(ICollectionChangedNotificationResult oldResult, INotifiable newSource)
        {
            var result = Create(newSource);
            result.AddedItems.AddRange(Cast(oldResult.AddedItems));
            result.MovedItems.AddRange(Cast(oldResult.MovedItems));
            result.RemovedItems.AddRange(Cast(oldResult.RemovedItems));
            result.isReset = oldResult.IsReset;
            result.NewItemsStartIndex = oldResult.NewItemsStartIndex;
            result.OldItemsStartIndex = oldResult.OldItemsStartIndex;
            return result;
        }


        public void UpdateOldStartIndex(int startIndex)
        {
            if (OldItemsStartIndex == -1)
            {
                OldItemsStartIndex = startIndex;
            }
            else
            {
                OldItemsStartIndex = Math.Min(OldItemsStartIndex, startIndex);
            }
        }

        public void UpdateNewStartIndex(int startIndex)
        {
            if (NewItemsStartIndex == -1)
            {
                NewItemsStartIndex = startIndex;
            }
            else
            {
                NewItemsStartIndex = Math.Min(NewItemsStartIndex, startIndex);
            }
        }

        private static IEnumerable<T> Cast(IList list)
        {
            return list.Cast<T>();
        }

        public void IncreaseReferences(int references)
        {
            Interlocked.Add(ref this.references, references);
        }

        public void FreeReference()
        {
            if (Interlocked.Decrement(ref references) == 0)
            {
                pool.Add(this);
            }
        }
    }
}
