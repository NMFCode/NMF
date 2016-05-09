using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans
{
    public class IncrementalSelectManyAggregateGrain<TSource, TIntermediate, TResult, TModel> : IncrementalAggregateGrainBase<TSource, TResult, IIncrementalSelectManyNodeGrain<TSource, TIntermediate, TResult, TModel>, TModel>,
        IIncrementalSelectManyAggregateGrain<TSource, TIntermediate, TResult, TModel> where TModel : IResolvableModel
    {
        private SerializableFunc<TSource, IEnumerable<TIntermediate>> _collectionSelectorFunc;
        private SerializableFunc<TSource, TIntermediate, TResult> _resultSelectorFunc;


        protected override async Task<IIncrementalSelectManyNodeGrain<TSource, TIntermediate, TResult, TModel>> InitializeNode(StreamIdentity identity)
        {
            var node = GrainFactory.GetGrain<IIncrementalSelectManyNodeGrain<TSource, TIntermediate, TResult, TModel>>(Guid.NewGuid());
            await node.SetObservingFunc(_collectionSelectorFunc, _resultSelectorFunc);
            await node.Setup(ModelContainer, identity.SingleValueToList(), OutputMultiplexFactor);

            return node;
        }

        public Task SetObservingFunc(SerializableFunc<TSource, IEnumerable<TIntermediate>> collectionSelectorFunc, SerializableFunc<TSource, TIntermediate, TResult> resultSelectorFunc)
        {
            _collectionSelectorFunc = collectionSelectorFunc;
            _resultSelectorFunc = resultSelectorFunc;

            return TaskDone.Done;
        }
    }
}