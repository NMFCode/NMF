using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalSelectAggregateGrain<TSource, TResult> :
        IStreamProcessorAggregate<ContainerElement<TSource>, ContainerElement<TResult>>, IObservingFuncProcessor<ContainerElement<TSource>, TResult>,
        IElementEnumerator<ContainerElement<TResult>>
    {
    }
}