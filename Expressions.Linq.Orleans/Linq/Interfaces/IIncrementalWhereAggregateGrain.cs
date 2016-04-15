using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalWhereAggregateGrain<TSource> : IStreamProcessorAggregate<ContainerElement<TSource>, ContainerElement<TSource>>, IObservingFuncProcessor<ContainerElement<TSource>, bool>,
        IElementEnumerator<ContainerElement<TSource>>
    {
         
    }
}