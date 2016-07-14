using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalSelectManyNodeGrain<TSource, TIntermediate, TResult, TModel> : IModelProcessingNodeGrain<TSource, TResult, TModel>,
        IElementEnumeratorNode<TResult> where TModel : IResolvableModel
    {
        /// <summary>
        /// Sets the func to apply.
        /// </summary>
        /// <param name="collectionSelectorFunc"></param>
        /// <param name="resultSelectorFunc"></param>
        /// <returns></returns>
        Task SetObservingFunc(SerializableFunc<TSource, IEnumerable<TIntermediate>> collectionSelectorFunc,
            SerializableFunc<TSource, TIntermediate, TResult> resultSelectorFunc);
    }
}