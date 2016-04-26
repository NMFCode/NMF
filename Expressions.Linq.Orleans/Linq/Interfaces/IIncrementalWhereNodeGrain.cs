using NMF.Expressions.Linq.Orleans.Model;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalWhereNodeGrain<TSource> : IModelProcessingNodeGrain<TSource, TSource, Models.Model>, IObservingFuncProcessor<TSource, bool>, IElementEnumeratorNode<TSource>
    {

    }
}