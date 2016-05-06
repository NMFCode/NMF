using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelProcessingNodeGrain<TIn, TOut, TModel> : IStreamProcessorNodeGrain<TIn, TOut>, IModelConsumer<TModel>, IContainsModel<TModel>
        where TModel : IResolvableModel
    {
        Task Setup(IModelContainerGrain<TModel> modelContainer, IEnumerable<StreamIdentity> inputStreams = null, int outputMultiplexFactor = 1);
    }
}