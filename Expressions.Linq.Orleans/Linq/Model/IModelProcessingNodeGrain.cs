using NMF.Models;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelProcessingNodeGrain<TIn, TOut, TModel> : IStreamProcessorNodeGrain<TIn, TOut>, IModelConsumer<TModel>, IModelLoader<TModel>,
        ITransactionalMultiplexingStreamProvider<TOut>
        where TModel : IResolvableModel
    {
    }
}