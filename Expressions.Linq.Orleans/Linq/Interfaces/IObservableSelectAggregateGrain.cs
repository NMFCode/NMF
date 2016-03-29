using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IObservableSelectAggregateGrain<TSource, TResult> : IStreamProcessorAggregate<ContainerElement<TSource>, ContainerElement<TResult>>, IObservingFuncProcessor<TSource, TResult>
    {
         
    }
}