using System;
using System.Threading.Tasks;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelContainerGrain<T> : IGrainWithGuidKey, ITransactionalStreamProvider<T>, IElementEnumeratorNode<T>, IModelLoader<T>, IContainsModel<T> where T : IResolvableModel
    {
        Task ExecuteSync(Action<T> action, bool newModelElementCreated = false);
        Task ExecuteSync(Action<T, object> action, object state, bool newModelElementCreated = false);

        Task<StreamIdentity> GetModelUpdateStream();

        Task<string> GetModelPath();

        // This method is just here to ensure assembly loading of RailwayContainer by Orleans. To be removed once NMF assembly registration has been changed.
        Task<RailwayContainer> NeverCallMe();
    }
}