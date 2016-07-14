using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalWhereAggregateGrain<TSource, TModel> : IModelProcessingAggregateGrain<TSource, TSource, TModel>,
        IObservingFuncProcessor<TSource, bool> where TModel : IResolvableModel
    {
    }
}