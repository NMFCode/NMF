using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Endpoints;
using Orleans.Streams;
using Orleans.Streams.Linq;

namespace NMF.Expressions.Linq.Orleans
{
    public static class ExtensionMethods
    {
        #region ClientConsumer

        public static async Task<ContainerElementListConsumer<TIn>> ToListConsumer<TOldIn, TIn, TFactory>(
            this Task<StreamProcessorChain<ContainerElement<TOldIn>, ContainerElement<TIn>, TFactory>> previousNodeTask)
            where TFactory : IStreamProcessorAggregateFactory
        {
            var previousNode = await previousNodeTask;
            return await ToListConsumer(previousNode);
        }

        public static async Task<ContainerElementListConsumer<TIn>> ToListConsumer<ToldIn, TIn, TFactory>(
            this StreamProcessorChain<ContainerElement<ToldIn>, ContainerElement<TIn>, TFactory> previousNode)
            where TFactory : IStreamProcessorAggregateFactory
        {
            var clientConsumer = new ContainerElementListConsumer<TIn>(GrainClient.GetStreamProvider("CollectionStreamProvider"));
            await clientConsumer.SetInput(await previousNode.GetStreamIdentities());

            return clientConsumer;
        }

        #endregion

        #region Select

        public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>> SelectIncremental<TIn, TOut, TFactory>
            (
            this ITransactionalStreamProviderAggregate<ContainerElement<TIn>> source, Expression<Func<ContainerElement<TIn>, TOut>> selectionFunc,
            TFactory factory) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var processorAggregate = await factory.CreateSelect(selectionFunc, await source.GetStreamIdentities());
            var processorChain = new StreamProcessorChainStart<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>(processorAggregate, source,
                factory);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>> SelectIncremental
            <TOldIn, TIn, TOut, TFactory>(
            this StreamProcessorChain<ContainerElement<TOldIn>, ContainerElement<TIn>, TFactory> previousNode,
            Expression<Func<ContainerElement<TIn>, TOut>> selectionFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var processorAggregate =
                await previousNode.Factory.CreateSelect(selectionFunc, await previousNode.Aggregate.GetStreamIdentities());
            var processorChain = new StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>(processorAggregate, previousNode);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>> SelectIncremental
            <TOldIn, TIn, TOut, TFactory>(
            this Task<StreamProcessorChain<ContainerElement<TOldIn>, ContainerElement<TIn>, TFactory>> previousNodeTask,
            Expression<Func<ContainerElement<TIn>, TOut>> selectionFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var previousNode = await previousNodeTask;
            return await SelectIncremental(previousNode, selectionFunc);
        }

        #endregion

        #region Where

        public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TIn>, TFactory>> WhereIncremental<TIn, TFactory>
            (
            this ITransactionalStreamProviderAggregate<ContainerElement<TIn>> source, Expression<Func<ContainerElement<TIn>, bool>> filterFunc,
            TFactory factory) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var processorAggregate = await factory.CreateWhere(filterFunc, await source.GetStreamIdentities());
            var processorChain = new StreamProcessorChainStart<ContainerElement<TIn>, ContainerElement<TIn>, TFactory>(processorAggregate, source,
                factory);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TIn>, TFactory>> WhereIncremental
            <TOldIn, TIn, TFactory>(
            this StreamProcessorChain<ContainerElement<TOldIn>, ContainerElement<TIn>, TFactory> previousNode,
            Expression<Func<ContainerElement<TIn>, bool>> filterFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var processorAggregate =
                await previousNode.Factory.CreateWhere(filterFunc, await previousNode.Aggregate.GetStreamIdentities());
            var processorChain = new StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TIn>, TFactory>(processorAggregate, previousNode);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TIn>, TFactory>> WhereIncremental
            <TOldIn, TIn, TFactory>(
            this Task<StreamProcessorChain<ContainerElement<TOldIn>, ContainerElement<TIn>, TFactory>> previousNodeTask,
            Expression<Func<ContainerElement<TIn>, bool>> filterFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var previousNode = await previousNodeTask;
            return await WhereIncremental(previousNode, filterFunc);
        }

        #endregion
    }
}