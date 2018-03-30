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
    }
    public interface ICollectionChangedNotificationResult<T> : ICollectionChangedNotificationResult
    {
        new List<T> AddedItems { get; }

        new List<T> RemovedItems { get; }

        new List<T> MovedItems { get; }
    }

    public class CollectionChangedNotificationResult<T> : ICollectionChangedNotificationResult<T>
    {
        private static ConcurrentBag<CollectionChangedNotificationResult<T>> pool = new ConcurrentBag<CollectionChangedNotificationResult<T>>();

        private INotifiable source;
        private bool isReset;
        private readonly List<T> addedItems = new List<T>();
        private readonly List<T> removedItems = new List<T>();
        private readonly List<T> movedItems = new List<T>();
        private int references;

        public bool Changed { get { return isReset || addedItems.Count > 0 || removedItems.Count > 0 || movedItems.Count > 0; } }

        public INotifiable Source { get { return source; } }

        public bool IsReset { get { return isReset; } }

        public List<T> AddedItems { get { return addedItems; } }

        public List<T> RemovedItems { get { return removedItems; } }

        public List<T> MovedItems { get { return movedItems; } }

        IList ICollectionChangedNotificationResult.AddedItems { get { return addedItems; } }

        IList ICollectionChangedNotificationResult.RemovedItems { get { return removedItems; } }

        IList ICollectionChangedNotificationResult.MovedItems { get { return movedItems; } }
        
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
                item.removedItems.Clear();
                item.movedItems.Clear();
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
            result.RemovedItems.AddRange(Cast(oldResult.RemovedItems));
            result.MovedItems.AddRange(Cast(oldResult.MovedItems));
            result.isReset = oldResult.IsReset;
            return result;
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
