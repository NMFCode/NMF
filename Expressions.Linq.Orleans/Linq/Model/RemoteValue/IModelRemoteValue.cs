using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{

    public interface IModelRemoteValue<T> : IModelRemoteValue
    {
        new T Retrieve(ILocalResolveContext resolveContext);
    }

    public interface IModelRemoteValue
    {
        object Retrieve(ILocalResolveContext resolveContext);

        /// <summary>
        /// Can be used to compare objects within a grain scope.
        /// </summary>
        object ReferenceComparable { get; }

        /// <summary>
        /// Can be used to identify objects in a global scope.
        /// </summary>
        object GlobalIdentifier { get;  }
    }
}