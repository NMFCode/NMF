using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IObservableSelectAggregateGrain<TSource, TResult> : IStreamProcessorAggregate<ContainerHostedElement<TSource>, ContainerHostedElement<TResult>>, IObservingFuncProcessor<TSource, TResult>
    {
         
    }
}