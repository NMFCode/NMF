using System;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans.Interfaces
{
    public interface IIncrementalFunctionNodeGrain<TSource, TResult, TModel> : IModelProcessingNodeGrain<TSource, TResult, TModel>, IElementEnumeratorNode<TResult> where TModel : IResolvableModel
    {
        Task SetIncrementalFunc(Func<INotifyEnumerable<TSource>, INotifyEnumerable<TResult>> incrementalFunc);
    }
}