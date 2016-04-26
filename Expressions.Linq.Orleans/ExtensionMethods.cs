using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Endpoints;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.Streams.Linq;

namespace NMF.Expressions.Linq.Orleans
{
    public static class ExtensionMethods
    {
        #region ClientConsumer

        public static async Task<MultiStreamListConsumer<TIn>> ToListConsumer<TOldIn, TIn, TFactory>(
            this Task<StreamProcessorChain<TOldIn, TIn, TFactory>> previousNodeTask)
            where TFactory : IStreamProcessorAggregateFactory
        {
            var previousNode = await previousNodeTask;
            return await ToListConsumer(previousNode);
        }

        public static async Task<MultiStreamListConsumer<TIn>> ToListConsumer<ToldIn, TIn, TFactory>(
            this StreamProcessorChain<ToldIn, TIn, TFactory> previousNode)
            where TFactory : IStreamProcessorAggregateFactory
        {
            var clientConsumer = new MultiStreamListConsumer<TIn>(GrainClient.GetStreamProvider("CollectionStreamProvider"));
            await clientConsumer.SetInput(await previousNode.GetOutputStreams());

            return clientConsumer;
        }

        #endregion

        #region Select

        public static async Task<StreamProcessorChain<TIn, TOut, TFactory>> SelectIncremental<TIn, TOut, TFactory>
            (
            this ITransactionalStreamProvider<TIn> source, Expression<Func<TIn, TOut>> selectionFunc,
            TFactory factory) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var processorAggregate = await factory.CreateSelect(selectionFunc, await source.GetOutputStreams());
            var processorChain = new StreamProcessorChainStart<TIn, TOut, TFactory>(processorAggregate, source,
                factory);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<TIn, TOut, TFactory>> SelectIncremental
            <TOldIn, TIn, TOut, TFactory>(
            this StreamProcessorChain<TOldIn, TIn, TFactory> previousNode,
            Expression<Func<TIn, TOut>> selectionFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var processorAggregate =
                await previousNode.Factory.CreateSelect(selectionFunc, await previousNode.Aggregate.GetOutputStreams());
            var processorChain = new StreamProcessorChain<TIn, TOut, TFactory>(processorAggregate, previousNode);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<TIn, TOut, TFactory>> SelectIncremental
            <TOldIn, TIn, TOut, TFactory>(
            this Task<StreamProcessorChain<TOldIn, TIn, TFactory>> previousNodeTask,
            Expression<Func<TIn, TOut>> selectionFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var previousNode = await previousNodeTask;
            return await SelectIncremental(previousNode, selectionFunc);
        }

        #endregion

        //#region Where

        //public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TIn>, TFactory>> WhereIncremental<TIn, TFactory>
        //    (
        //    this ITransactionalStreamProvider<TIn> source, Expression<Func<ContainerElement<TIn>, bool>> filterFunc,
        //    TFactory factory) where TFactory : IncrementalStreamProcessorAggregateFactory
        //{
        //    var processorAggregate = await factory.CreateWhere(filterFunc, await source.GetOutputStreams());
        //    var processorChain = new StreamProcessorChainStart<TIn, TIn, TFactory>(processorAggregate, source,
        //        factory);

        //    return processorChain;
        //}

        //public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TIn>, TFactory>> WhereIncremental
        //    <TOldIn, TIn, TFactory>(
        //    this StreamProcessorChain<ContainerElement<TOldIn>, ContainerElement<TIn>, TFactory> previousNode,
        //    Expression<Func<ContainerElement<TIn>, bool>> filterFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        //{
        //    var processorAggregate =
        //        await previousNode.Factory.CreateWhere(filterFunc, await previousNode.Aggregate.GetOutputStreams());
        //    var processorChain = new StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TIn>, TFactory>(processorAggregate, previousNode);

        //    return processorChain;
        //}

        //public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TIn>, TFactory>> WhereIncremental
        //    <TOldIn, TIn, TFactory>(
        //    this Task<StreamProcessorChain<ContainerElement<TOldIn>, ContainerElement<TIn>, TFactory>> previousNodeTask,
        //    Expression<Func<ContainerElement<TIn>, bool>> filterFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        //{
        //    var previousNode = await previousNodeTask;
        //    return await WhereIncremental(previousNode, filterFunc);
        //}

        #endregion
    }
}