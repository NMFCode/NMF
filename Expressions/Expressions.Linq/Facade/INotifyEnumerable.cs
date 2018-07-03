using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public interface INotifyEnumerable : IEnumerable, INotifyCollectionChanged, INotifiable { }

    public interface INotifyEnumerable<out T> : IEnumerable<T>, INotifyEnumerable { }

    public interface IOrderableNotifyEnumerable<T> : INotifyEnumerable<T>
    {
        IEnumerable<IEnumerable<T>> Sequences { get; }

        IEnumerable<T> GetSequenceForItem(T item);
    }

    public interface INotifyCollection<T> : INotifyEnumerable<T>, ICollection<T> { }

    public interface INotifyGrouping<out TKey, out TItem> : INotifyEnumerable<TItem>, IGrouping<TKey, TItem> { }

    public interface INotifySplit<T>
    {
        INotifyValue<T> Head { get; }
        INotifyValue<bool> Empty { get; }
        INotifyEnumerable<T> Tail { get; }
    }
}
