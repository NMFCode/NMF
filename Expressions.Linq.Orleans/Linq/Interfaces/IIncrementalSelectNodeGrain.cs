using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans.Interfaces
{
    public interface IIncrementalSelectNodeGrain<TSource, TResult, TModel> : IModelProcessingNodeGrain<TSource, TResult, TModel>, IObservingFuncProcessor<TSource, TResult>, IElementEnumeratorNode<TResult> where TModel : IResolvableModel
    {
         
    }
}