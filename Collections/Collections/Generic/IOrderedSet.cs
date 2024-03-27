using NMF.Expressions;
using System.Collections.Generic;

namespace NMF.Collections.Generic
{
    /// <summary>
    /// Defines an ordered Set
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public interface IOrderedSet<T> : IList<T>, ISet<T>, ICollection<T>, IEnumerable<T>
    {
    }

    /// <summary>
    /// Represents an ordered set that can be accessed incrementally, i.e. with change notifications
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOrderedSetExpression<T> : IListExpression<T>, ISetExpression<T>, IOrderedSet<T> { }
}
