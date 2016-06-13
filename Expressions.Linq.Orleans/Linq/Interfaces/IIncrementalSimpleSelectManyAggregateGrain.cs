using System.Collections.Generic;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalSimpleSelectManyAggregateGrain<TSource, TResult, TModel> :
        IModelProcessingAggregateGrain<TSource, TResult, TModel>, IObservingFuncProcessor<TSource, IEnumerable<TResult>> where TModel : IResolvableModel
    {
    }
}