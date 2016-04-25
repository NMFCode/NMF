using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public interface IModelProcessingAggregateGrain<TSource, TResult, TModel> : IStreamProcessorAggregate<TSource, TResult>, IModelConsumer<TModel>,
        IModelLoader<TModel>
        where TModel : Models.Model

    {
    }
}