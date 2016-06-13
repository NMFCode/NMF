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
    public class IncrementalSimpleSelectManyAggregateGrain<TSource, TResult, TModel> : ModelAggregateGrainBase<TSource, TResult, IIncrementalSimpleSelectManyNodeGrain<TSource, TResult, TModel>, TModel>,
        IIncrementalSimpleSelectManyAggregateGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {
        private SerializableFunc<TSource, IEnumerable<TResult>> _observingFunc;

        public Task SetObservingFunc(SerializableFunc<TSource, IEnumerable<TResult>> observingFunc)
        {
            _observingFunc = observingFunc;
            return TaskDone.Done;
        }

        protected override async Task<IIncrementalSimpleSelectManyNodeGrain<TSource, TResult, TModel>> InitializeNode(Tuple<IIncrementalSimpleSelectManyNodeGrain<TSource, TResult, TModel>, StreamIdentity> nodeStreamPair)
        {
            var node = nodeStreamPair.Item1;
            await node.SetObservingFunc(_observingFunc);
            await node.Setup(ModelContainer, OutputMultiplexFactor);
            await node.SubscribeToStreams(nodeStreamPair.Item2.SingleValueToList());

            return node;
        }

    }
}