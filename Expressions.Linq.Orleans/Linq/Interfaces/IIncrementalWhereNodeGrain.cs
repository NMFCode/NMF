using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalWhereNodeGrain<TSource> : IStreamProcessorNodeGrain<ContainerElement<TSource>, ContainerElement<TSource>>, IObservingFuncProcessor<TSource, bool>, IElementEnumeratorNode<TSource>
    {

    }
}