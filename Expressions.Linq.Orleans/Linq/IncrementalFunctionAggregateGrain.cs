using System;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans
{
    public class IncrementalFunctionAggregateGrain<TSource, TResult, TModel> : ModelAggregateGrainBase<TSource, TResult, IIncrementalFunctionNodeGrain<TSource, TResult, TModel>, TModel>,
        IIncrementalFunctionAggregateGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {
        private Func<INotifyEnumerable<TSource>, INotifyEnumerable<TResult>> _incrementalFunc;

        protected override async Task<IIncrementalFunctionNodeGrain<TSource, TResult, TModel>> InitializeNode(StreamIdentity identity)
        {
            var node = GrainFactory.GetGrain<IIncrementalFunctionNodeGrain<TSource, TResult, TModel>>(Guid.NewGuid());
            await node.SetIncrementalFunc(_incrementalFunc);
            await node.Setup(ModelContainer, identity.SingleValueToList(), OutputMultiplexFactor);

            return node;
        }

        public Task SetIncrementalFunc(Func<INotifyEnumerable<TSource>, INotifyEnumerable<TResult>> incrementalFunc)
        {
            _incrementalFunc = incrementalFunc;
            return TaskDone.Done;
        }
    }
}