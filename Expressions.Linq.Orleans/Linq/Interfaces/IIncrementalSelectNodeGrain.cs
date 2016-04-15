using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Interfaces
{
    public interface IIncrementalSelectNodeGrain<TSource, TResult> : IStreamProcessorNodeGrain<TSource, ContainerElement<TResult>>, IObservingFuncProcessor<ContainerElement<TSource>, TResult>, IElementEnumeratorNode<ContainerElement<TResult>>
    {
         
    }
}