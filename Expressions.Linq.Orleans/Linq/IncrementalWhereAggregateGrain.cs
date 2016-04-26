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
    public class IncrementalWhereAggregateGrain<TSource> : ModelProcessingAggregateGrain<TSource, TSource, IIncrementalWhereNodeGrain<TSource>>,
        IIncrementalWhereAggregateGrain<TSource>
    {
        private SerializableFunc<TSource, bool> _observingFunc;

        public Task SetObservingFunc(SerializableFunc<TSource, bool> observingFunc)
        {
            _observingFunc = observingFunc;
            return TaskDone.Done;
        }

        protected override async Task<IIncrementalWhereNodeGrain<TSource>> InitializeNode(StreamIdentity identity)
        {
            var node = GrainFactory.GetGrain<IIncrementalWhereNodeGrain<TSource>>(Guid.NewGuid());
            await node.SetObservingFunc(_observingFunc);
            await node.SetModelContainer(ModelContainer);
            await node.SubscribeToStreams(identity.SingleValueToList());

            return node;
        }

    }
}