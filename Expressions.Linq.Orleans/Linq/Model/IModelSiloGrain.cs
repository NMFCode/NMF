using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Concurrency;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelSiloGrain<T> : IGrainWithGuidKey, ITransactionalStreamTearDown where T : IResolvableModel
    {
        Task<Immutable<T>> GetModel();

        Task SetModelContainer(IModelContainerGrain<T> modelContainer, string modelPath = "");

        Task<string> GetIdentity();
    }
}