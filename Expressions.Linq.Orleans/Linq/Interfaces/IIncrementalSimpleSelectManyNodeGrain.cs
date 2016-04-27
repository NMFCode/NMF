using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalSimpleSelectManyNodeGrain<TSource, TResult, TModel> : IModelProcessingNodeGrain<TSource, TResult, TModel>, IObservingFuncProcessor<TSource, IEnumerable<TResult>>,
        IElementEnumeratorNode<TResult> where TModel : IResolvableModel
    {
    }
}