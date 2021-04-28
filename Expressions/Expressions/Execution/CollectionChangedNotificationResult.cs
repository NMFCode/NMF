using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a notification result for a collection
    /// </summary>
    public interface ICollectionChangedNotificationResult : INotificationResult
    {
        /// <summary>
        /// True, if the collection was reset, otherwise False
        /// </summary>
        bool IsReset { get; }

        /// <summary>
        /// Gets a list of added items
        /// </summary>
        IList AddedItems { get; }

        /// <summary>
        /// Gets a list of removed items
        /// </summary>
        IList RemovedItems { get; }

        /// <summary>
        /// Gets a list of moved items
        /// </summary>
        IList MovedItems { get; }

        /// <summary>
        /// Gets the first index of old items or -1
        /// </summary>
        int OldItemsStartIndex { get; }

        /// <summary>
        /// Gets the first index of new items or -1
        /// </summary>
        int NewItemsStartIndex { get; }
    }

    /// <summary>
    /// Denotes a notification result for a collection
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public interface ICollectionChangedNotificationResult<T> : ICollectionChangedNotificationResult
    {
        /// <summary>
        /// Gets a list of added items
        /// </summary>
        new List<T> AddedItems { get; }

        /// <summary>
        /// Gets a list of moved items
        /// </summary>
        new List<T> MovedItems { get; }

        /// <summary>
        /// Gets a list of removed items
        /// </summary>
        new List<T> RemovedItems { get; }
    }

    /// <summary>
    /// Denotes the standard implementation of a collection result
    /// </summary>
    /// <typeparam name="T">the type of elements</typeparam>
    public class CollectionChangedNotificationResult<T> : ICollectionChangedNotificationResult<T>
    {
        private static readonly ConcurrentBag<CollectionChangedNotificationResult<T>> pool = new ConcurrentBag<CollectionChangedNotificationResult<T>>();

        private INotifiable source;
        private bool isReset;
        private readonly List<T> addedItems = new List<T>();
        private readonly List<T> movedItems = new List<T>();
        private readonly List<T> removedItems = new List<T>();
        private int references;

        /// <inheritdoc />
        public bool Changed { get { return isReset || addedItems.Count > 0 || removedItems.Count > 0 || movedItems.Count > 0; } }

        /// <inheritdoc />
        public INotifiable Source { get { return source; } }

        /// <inheritdoc />
        public bool IsReset { get { return isReset; } }

        /// <inheritdoc />
        public List<T> AddedItems { get { return addedItems; } }

        /// <inheritdoc />
        public List<T> MovedItems { get { return movedItems; } }

        /// <inheritdoc />
        public List<T> RemovedItems { get { return removedItems; } }

        IList ICollectionChangedNotificationResult.AddedItems { get { return addedItems; } }

        IList ICollectionChangedNotificationResult.RemovedItems { get { return removedItems; } }

        IList ICollectionChangedNotificationResult.MovedItems { get { return movedItems; } }

        /// <inheritdoc />
        public int OldItemsStartIndex { get; set; } = -1;

        /// <inheritdoc />
        public int NewItemsStartIndex { get; set; } = -1;

        private CollectionChangedNotificationResult(INotifiable source, bool isReset)
        {
            this.source = source;
            this.isReset = isReset;
        }

        /// <summary>
        /// Creates a new instance of a collection notification
        /// </summary>
        /// <param name="source">the source DDG node</param>
        /// <param name="isReset">True, if this is a reset, otherwise False</param>
        /// <returns>A collection notification instance</returns>
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

        /// <summary>
        /// Turns this notification into a reset
        /// </summary>
        public void TurnIntoReset()
        {
            this.isReset = true;
        }

        /// <summary>
        /// Transfers the provided notification to a new change source
        /// </summary>
        /// <param name="oldResult">The old notification</param>
        /// <param name="newSource">The new change source</param>
        /// <returns>An updated notification</returns>
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

        /// <summary>
        /// Updates the old start index
        /// </summary>
        /// <param name="startIndex">the new start index of old items</param>
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

        /// <summary>
        /// Updates the new start index
        /// </summary>
        /// <param name="startIndex">the new start index of new items</param>
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

        /// <inheritdoc />
        public void IncreaseReferences(int references)
        {
            Interlocked.Add(ref this.references, references);
        }

        /// <inheritdoc />
        public void FreeReference()
        {
            if (Interlocked.Decrement(ref references) == 0)
            {
                pool.Add(this);
            }
        }
    }
}
