using System;
using System.Linq;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans
{
    public class IncrementalSelectAggregateGrain<TSource, TResult> : ModelProcessingAggregateGrain<TSource, TResult, IIncrementalSelectNodeGrain<TSource, TResult>>,
        IIncrementalSelectAggregateGrain<TSource, TResult>
    {
        private SerializableFunc<TSource, TResult> _observingFunc;

        public Task SetObservingFunc(SerializableFunc<TSource, TResult> observingFunc)
        {
            _observingFunc = observingFunc;
            return TaskDone.Done;
        }

        protected override async Task<IIncrementalSelectNodeGrain<TSource, TResult>> InitializeNode(StreamIdentity identity)
        {
            var node = GrainFactory.GetGrain<IIncrementalSelectNodeGrain<TSource, TResult>>(Guid.NewGuid());
            await node.SetObservingFunc(_observingFunc);
            await node.SetModelContainer(ModelContainer);
            await node.SubscribeToStreams(identity.SingleValueToList());

            return node;
        }

    }
}