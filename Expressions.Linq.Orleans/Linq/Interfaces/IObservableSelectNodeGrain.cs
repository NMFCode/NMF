using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Interfaces
{
    public interface IObservableSelectNodeGrain<TSource, TResult> : IStreamProcessorNodeGrain<ContainerElement<TSource>, ContainerElement<TResult>>, IObservingFuncProcessor<TSource, TResult>, IElementEnumeratorNode<TResult>
    {
         
    }
}