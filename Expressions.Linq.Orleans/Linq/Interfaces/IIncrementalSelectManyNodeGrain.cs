using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalSelectManyNodeGrain<TSource, TIntermediate, TResult, TModel> : IModelProcessingNodeGrain<TSource, TResult, TModel>,
        IElementEnumeratorNode<TResult> where TModel : IResolvableModel
    {
        Task SetObservingFunc(SerializableFunc<TSource, IEnumerable<TIntermediate>> collectionSelectorFunc,
            SerializableFunc<TSource, TIntermediate, TResult> resultSelectorFunc);
    }
}