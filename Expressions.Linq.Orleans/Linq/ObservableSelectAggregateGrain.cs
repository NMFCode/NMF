using System;
using System.Linq;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans
{
    public class ObservableSelectAggregateGrain<TSource, TResult> :
        StreamProcessorAggregate<ContainerElement<TSource>, ContainerElement<TResult>, IObservableSelectNodeGrain<TSource, TResult>>,
        IObservableSelectAggregateGrain<TSource, TResult>
    {
        private SerializableFunc<TSource, TResult> _observingFunc;

        public async Task<Guid> EnumerateToStream(params StreamIdentity[] streamIdentities)
        {
            var tId = Guid.NewGuid();
            var streamNodeTuples = streamIdentities.Zip(ProcessorNodes.Repeat(), (identity, processorNode) => new Tuple<StreamIdentity, IElementEnumeratorNode<TResult>>(identity, processorNode));
            await Task.WhenAll(streamNodeTuples.Select(t => t.Item2.EnumerateToStream(t.Item1, tId)));
            return tId;
        }

        public async Task<Guid> EnumerateToSubscribers(int batchSize = 2147483647)
        {
            var tId = Guid.NewGuid();
            await Task.WhenAll(ProcessorNodes.Select(n => n.EnumerateToSubscribers(tId)));
            return tId;
        }

        public Task SetObservingFunc(SerializableFunc<TSource, TResult> observingFunc)
        {
            _observingFunc = observingFunc;
            return TaskDone.Done;
        }

        protected override async Task<IObservableSelectNodeGrain<TSource, TResult>> InitializeNode(StreamIdentity identity)
        {
            var node = GrainFactory.GetGrain<IObservableSelectNodeGrain<TSource, TResult>>(Guid.NewGuid());
            await node.SetObservingFunc(_observingFunc);
            await node.SetInput(identity);

            return node;
        }

    }
}