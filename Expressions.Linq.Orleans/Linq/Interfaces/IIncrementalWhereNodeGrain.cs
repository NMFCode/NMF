using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans.Collections;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalWhereNodeGrain<TSource, TModel> : IModelProcessingNodeGrain<TSource, TSource, TModel>,
        IObservingFuncProcessor<TSource, bool>, IElementEnumeratorNode<TSource> where TModel : IResolvableModel
    {
    }
}