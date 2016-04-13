using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalSelectAggregateGrain<TSource, TResult> :
        IStreamProcessorAggregate<ContainerElement<TSource>, ContainerElement<TResult>, IIncrementalSelectNodeGrain<TSource, TResult>>, IObservingFuncProcessor<TSource, TResult>,
        IElementEnumerator<ContainerElement<TResult>>
    {
    }
}