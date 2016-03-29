using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans
{
    public class ObservableSelectAggregateGrain<TSource, TResult> : StreamProcessorAggregate<ContainerElement<TSource>, ContainerElement<TResult>>, IObservableSelectAggregateGrain<TSource, TResult>
    {
        private Func<TSource, TResult> _observingFunc;

        protected override async Task<IStreamProcessorNodeGrain<ContainerElement<TSource>, ContainerElement<TResult>>> InitializeNode(StreamIdentity identity)
        {
            var node = GrainFactory.GetGrain<IObservableSelectNodeGrain<TSource, TResult>>(Guid.NewGuid());
            await node.SetObservingFunc(_observingFunc);
            await node.SetInput(identity);

            return node;
        }

        public Task SetObservingFunc(Func<TSource, TResult> observingFunc)
        {
            _observingFunc = observingFunc;
            return TaskDone.Done;
        }
    }
}