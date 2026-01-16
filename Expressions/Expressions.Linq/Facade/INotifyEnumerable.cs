using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an incremental collection
    /// </summary>
    public interface INotifyEnumerable : IEnumerable, INotifyCollectionChanged, INotifiable
    {
        /// <summary>
        /// Determines whether there is a dedicated order defined for this collection
        /// </summary>
        bool IsOrdered { get; }

        /// <summary>
        /// Defines whether an order is required for the given collection
        /// </summary>
        /// <param name="isOrderRequired"></param>
        void RequireOrder(bool isOrderRequired);
    }

    /// <summary>
    /// Denotes an incremental collection
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public interface INotifyEnumerable<out T> : IEnumerable<T>, INotifyEnumerable { }

    /// <summary>
    /// Denotes an orderable incremental collection
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public interface IOrderableNotifyEnumerable<T> : INotifyEnumerable<T>
    {
        /// <summary>
        /// Gets the sequences contained in this collection
        /// </summary>
        IEnumerable<IEnumerable<T>> Sequences { get; }

        /// <summary>
        /// Gets the sequence that contains the given item
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The sequence in which the item is contained</returns>
        IEnumerable<T> GetSequenceForItem(T item);
    }

    /// <summary>
    /// Denotes an incremental collection
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public interface INotifyCollection<T> : INotifyEnumerable<T>, ICollection<T> { }

    /// <summary>
    /// Denotes an incremental grouping
    /// </summary>
    /// <typeparam name="TKey">The type of the key</typeparam>
    /// <typeparam name="TItem">The type of items</typeparam>
    public interface INotifyGrouping<out TKey, out TItem> : INotifyEnumerable<TItem>, IGrouping<TKey, TItem> { }

    /// <summary>
    /// Denotes an incremental split
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public interface INotifySplit<T>
    {
        /// <summary>
        /// Gets the head of the split
        /// </summary>
        INotifyValue<T> Head { get; }

        /// <summary>
        /// Indicates whether the split is empty
        /// </summary>
        INotifyValue<bool> Empty { get; }

        /// <summary>
        /// Gets the tail of the split
        /// </summary>
        INotifyValue<INotifySplit<T>> Tail { get; }
    }
}
