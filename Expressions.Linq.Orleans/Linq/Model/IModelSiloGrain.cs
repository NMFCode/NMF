using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Concurrency;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelSiloGrain<T> : IGrainWithStringKey where T : IResolvableModel
    {
        Task<Immutable<T>> GetModel();

        Task SetModelContainer(IModelContainerGrain<T> modelContainer);

        Task<string> GetIdentity();
    }
}