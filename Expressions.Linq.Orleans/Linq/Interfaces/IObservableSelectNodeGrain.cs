using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Interfaces
{
    public interface IObservableSelectNodeGrain<TSource, TResult> : IStreamProcessorNodeGrain<ContainerHostedElement<TSource>, ContainerHostedElement<TResult>>, IObservingFuncProcessor<TSource, TResult>
    {
         
    }
}