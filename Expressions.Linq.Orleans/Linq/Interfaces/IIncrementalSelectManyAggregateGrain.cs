using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalSelectManyAggregateGrain<TSource, TIntermediate, TResult, TModel> :
        IModelProcessingAggregateGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {
        Task SetObservingFunc(SerializableFunc<TSource, IEnumerable<TIntermediate>> collectionSelectorFunc,
                   SerializableFunc<TSource, TIntermediate, TResult> resultSelectorFunc);
    }
}