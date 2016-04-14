using System;
using System.Linq;
using System.Threading.Tasks;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans
{
    public abstract class IncrementalStreamProcessorAggregate<TSource, TResult, TNode> : StreamProcessorAggregate<TSource, TResult, TNode>,
        IElementEnumerator<ContainerElement<TResult>> where TNode : IStreamProcessorNodeGrain<TSource, TResult>, IElementEnumeratorNode<TResult>
    {
        public async Task<Guid> EnumerateToStream(params StreamIdentity[] streamIdentities)
        {
            var tId = Guid.NewGuid();
            var streamNodeTuples = streamIdentities.Zip(ProcessorNodes.Repeat(),
                (identity, processorNode) => new Tuple<StreamIdentity, IElementEnumeratorNode<TResult>>(identity, processorNode));
            await Task.WhenAll(streamNodeTuples.Select(t => t.Item2.EnumerateToStream(t.Item1, tId)));
            return tId;
        }

        public async Task<Guid> EnumerateToSubscribers(int batchSize = 2147483647)
        {
            var tId = Guid.NewGuid();
            await Task.WhenAll(ProcessorNodes.Select(n => n.EnumerateToSubscribers(tId)));
            return tId;

        }
    }
}