using System;
using System.Threading.Tasks;
using NMF.Models;
using NMF.Models.Repository;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelContainerGrain<T> : IGrainWithGuidKey, ITransactionalStreamProvider, IElementEnumeratorNode<T>, IModelLoader<T> where T : Models.Model
    {
        Task ExecuteSync(Action<T> action);
    }
}