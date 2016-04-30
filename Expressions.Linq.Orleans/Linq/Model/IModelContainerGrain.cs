using System;
using System.Threading.Tasks;
using NMF.Models;
using NMF.Models.Repository;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelContainerGrain<T> : IGrainWithGuidKey, ITransactionalStreamProvider<T>, IElementEnumeratorNode<T>, IModelLoader<T> where T : IResolvableModel
    {
        Task ExecuteSync(Action<T> action);
        Task ExecuteSync(Action<T, object> action, object state);

        Task<StreamIdentity> GetModelUpdateStream();

        Task<string> GetModelPath();
    }
}