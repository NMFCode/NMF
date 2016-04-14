using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalWhereAggregateGrain<TSource> : IStreamProcessorAggregate<TSource, TSource>, IObservingFuncProcessor<TSource, bool>,
        IElementEnumerator<ContainerElement<TSource>>
    {
         
    }
}