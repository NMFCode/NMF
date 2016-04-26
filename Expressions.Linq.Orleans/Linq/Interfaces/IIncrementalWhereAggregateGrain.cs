using NMF.Expressions.Linq.Orleans.Model;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalWhereAggregateGrain<TSource> : IModelProcessingAggregateGrain<TSource, TSource, Models.Model>,
        IObservingFuncProcessor<TSource, bool>
    {
    }
}