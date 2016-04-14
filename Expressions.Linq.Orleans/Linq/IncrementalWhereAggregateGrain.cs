using System;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans
{
    public class IncrementalWhereAggregateGrain<TSource> :
        IncrementalStreamProcessorAggregate<ContainerElement<TSource>, ContainerElement<TSource>, IIncrementalWhereNodeGrain<TSource>>,
                IIncrementalWhereAggregateGrain<TSource> 
    {
        private SerializableFunc<TSource, bool> _observingFunc;

        protected override async Task<IIncrementalWhereNodeGrain<TSource>> InitializeNode(StreamIdentity identity)
        {
            var node = GrainFactory.GetGrain<IIncrementalWhereNodeGrain<TSource>>(Guid.NewGuid());
            await node.SetObservingFunc(_observingFunc);
            await node.SetInput(identity);

            return node;
        }

        public Task SetObservingFunc(SerializableFunc<TSource, bool> observingFunc)
        {
            _observingFunc = observingFunc;
            return TaskDone.Done;
        }
    }
}