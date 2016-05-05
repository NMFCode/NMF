using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{

    public interface IModelRemoteValue<T> : IModelRemoteValue
    {
        new T Retrieve(IResolvableModel lookupModel);
    }

    public interface IModelRemoteValue
    {
        object Retrieve(IResolvableModel lookupModel);

        object ReferenceComparable { get; }
    }
}