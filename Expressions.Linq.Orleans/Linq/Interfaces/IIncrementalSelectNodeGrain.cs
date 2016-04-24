using NMF.Expressions.Linq.Orleans.Model;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Interfaces
{
    public interface IIncrementalSelectNodeGrain<TSource, TResult> : IModelProcessingNodeGrain<TSource, TResult, NMF.Models.Model>, IObservingFuncProcessor<TSource, TResult>, IElementEnumeratorNode<TResult>
    {
         
    }
}