using System;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Linq.Interfaces
{
    public interface IIncrementalFunctionAggregateGrain<TSource, TResult, TModel> :
        IModelProcessingAggregateGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {
        Task SetIncrementalFunc(Func<INotifyEnumerable<TSource>, INotifyEnumerable<TResult>> incrementalFunc);
    }
}