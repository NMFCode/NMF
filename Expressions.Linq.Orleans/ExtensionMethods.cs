using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq;

namespace NMF.Expressions.Linq.Orleans
{
    public static class ExtensionMethods
    {
        #region Select

        //public static async Task<StreamProcessorChain<TIn, TOut, TFactory>> Select<TIn, TOut, TFactory>(
        //    this ITransactionalStreamProviderAggregate<TIn> source, Expression<Func<TIn, TOut>> selectionFunc, IGrainFactory factory) where TFactory : IStreamProcessorAggregateFactory
        //{
        //    return await Select(source, selectionFunc, new DefaultStreamProcessorAggregateFactory(factory));
        //}

        public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>> SelectIncremental<TIn, TOut, TFactory>(
            this ITransactionalStreamProviderAggregate<ContainerElement<TIn>> source, Expression<Func<ContainerElement<TIn>, TOut>> selectionFunc,
            TFactory factory) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var processorAggregate = await factory.CreateSelect<TIn, TOut>(selectionFunc, await source.GetStreamIdentities());
            var processorChain = new StreamProcessorChainStart<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>(processorAggregate, source, factory);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>> SelectIncremental<TOldIn, TIn, TOut, TFactory>(
            this StreamProcessorChain<ContainerElement<TOldIn>, ContainerElement<TIn>, TFactory> previousNode, Expression<Func<ContainerElement<TIn>, TOut>> selectionFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var processorAggregate =
                await previousNode.Factory.CreateSelect<TIn, TOut>(selectionFunc, await previousNode.Aggregate.GetStreamIdentities());
            var processorChain = new StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>(processorAggregate, previousNode);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<ContainerElement<TIn>, ContainerElement<TOut>, TFactory>> SelectIncremental<TOldIn, TIn, TOut, TFactory>(
            this Task<StreamProcessorChain<ContainerElement<TOldIn>, ContainerElement<TIn>, TFactory>> previousNodeTask, Expression<Func<ContainerElement<TIn>, TOut>> selectionFunc) where TFactory : IncrementalStreamProcessorAggregateFactory
        {
            var previousNode = await previousNodeTask;
            return await SelectIncremental(previousNode, selectionFunc);
        }

        #endregion

        #region Where

        //public static async Task<StreamProcessorChain<TIn, TIn>> Where<TIn>(
        //    this ITransactionalStreamProviderAggregate<TIn> source, Expression<Func<TIn, bool>> filterFunc, IGrainFactory factory)
        //{
        //    return await Where(source, filterFunc, new DefaultStreamProcessorAggregateFactory(factory));
        //}

        //public static async Task<StreamProcessorChain<TIn, TIn, TFactory>> Where<TIn, TFactory>(this ITransactionalStreamProviderAggregate<TIn> source, Expression<Func<TIn, bool>> filterFunc,
        //    TFactory factory) where TFactory : DefaultStreamProcessorAggregateFactory
        //{
        //    var processorAggregate = await factory.CreateWhere(filterFunc, await source.GetStreamIdentities());
        //    var processorChain = new StreamProcessorChainStart<TIn, TIn, TFactory>(processorAggregate, source, factory);

        //    return processorChain;
        //}

        //public static async Task<StreamProcessorChain<TIn, TIn, TFactory>> Where<TOldIn, TIn, TFactory>(
        //    this StreamProcessorChain<TOldIn, TIn, TFactory> previousNode, Expression<Func<TIn, bool>> filterFunc) where TFactory : DefaultStreamProcessorAggregateFactory
        //{
        //    var processorAggregate = await previousNode.Factory.CreateWhere(filterFunc, await previousNode.Aggregate.GetStreamIdentities());
        //    var processorChain = new StreamProcessorChain<TIn, TIn, TFactory>(processorAggregate, previousNode);

        //    return processorChain;
        //}

        //public static async Task<StreamProcessorChain<TIn, TIn, TFactory>> Where<TOldIn, TIn, TFactory>(
        //    this Task<StreamProcessorChain<TOldIn, TIn, TFactory>> previousNodeTask, Expression<Func<TIn, bool>> filterFunc) where TFactory : DefaultStreamProcessorAggregateFactory
        //{
        //    var previousNode = await previousNodeTask;
        //    return await Where(previousNode, filterFunc);
        //}

        #endregion
    }
}