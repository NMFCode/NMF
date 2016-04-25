using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalSelectAggregateGrain<TSource, TResult> :
        IModelProcessingAggregateGrain<TSource, TResult, NMF.Models.Model>, IObservingFuncProcessor<TSource, TResult>
    {
    }
}