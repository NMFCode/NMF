using System;
using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Streams;
using Orleans.Streams.Linq;

namespace NMF.Expressions.Linq.Orleans
{
    public static class ExtensionMethods
    {
        #region ModelConsumer for NMF - hack to walk around two-level type inference issues.

        /// <summary>
        /// Obtain query results with a model consumer.
        /// </summary>
        /// <typeparam name="TOldIn"></typeparam>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="previousNodeTask"></param>
        /// <returns></returns>
        public static async Task<TransactionalStreamModelConsumer<TIn, NMF.Models.Model>> ToNmfModelConsumer<TOldIn, TIn, TFactory>(
            this Task<StreamProcessorChain<TOldIn, TIn, TFactory>> previousNodeTask)
            where TFactory : IncrementalNmfModelStreamProcessorAggregateFactory
        {
            var previousNode = await previousNodeTask;
            return await ToModelConsumer<TOldIn, TIn, TFactory, NMF.Models.Model>(previousNode);
        }

        /// <summary>
        /// Obtain query results with a model consumer.
        /// </summary>
        /// <typeparam name="TOldIn"></typeparam>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="previousNode"></param>
        /// <returns></returns>
        public static Task<TransactionalStreamModelConsumer<TIn, NMF.Models.Model>> ToNmfModelConsumer<TOldIn, TIn, TFactory>(
            this StreamProcessorChain<TOldIn, TIn, TFactory> previousNode)
            where TFactory : IncrementalNmfModelStreamProcessorAggregateFactory
        {
            return ToModelConsumer<TOldIn, TIn, TFactory, NMF.Models.Model>(previousNode);
        }

        #endregion

        #region ModelConsumer

        public static async Task<TransactionalStreamModelConsumer<TIn, TModel>> ToModelConsumer<TOldIn, TIn, TFactory, TModel>(
            this Task<StreamProcessorChain<TOldIn, TIn, TFactory>> previousNodeTask)
            where TFactory : IncrementalStreamProcessorAggregateFactory<TModel> where TModel : IResolvableModel
        {
            var previousNode = await previousNodeTask;
            return await ToModelConsumer<TOldIn, TIn, TFactory, TModel>(previousNode);
        }

        public static async Task<TransactionalStreamModelConsumer<TIn, TModel>> ToModelConsumer<TOldIn, TIn, TFactory, TModel>(
            this StreamProcessorChain<TOldIn, TIn, TFactory> previousNode)
            where TFactory : IncrementalStreamProcessorAggregateFactory<TModel> where TModel : IResolvableModel
        {
            var clientConsumer = new TransactionalStreamModelConsumer<TIn, TModel>(GrainClient.GetStreamProvider("CollectionStreamProvider"));
            await clientConsumer.SetModelContainer(previousNode.Factory.ModelContainerGrain);
            await clientConsumer.SetInput(await previousNode.GetOutputStreams());

            return clientConsumer;
        }

        #endregion

        #region Processor

        public static async Task<StreamProcessorChain<TIn, TOut, TFactory>> ProcessLocal<TIn, TOut, TFactory>(
            this ITransactionalStreamProvider<TIn> source, Func<INotifyEnumerable<TIn>, INotifyEnumerable<TOut>> processingFunc, TFactory factory,
            int scatterFactor = 1) where TFactory : IncrementalNmfModelStreamProcessorAggregateFactory
        {
            var aggregateConfiguration = new StreamProcessorAggregateConfiguration(await source.GetOutputStreams(), scatterFactor);
            var processorAggregate = await factory.CreateProcessor(processingFunc, aggregateConfiguration);
            var processorChain = new StreamProcessorChainStart<TIn, TOut, TFactory>(processorAggregate, source, factory);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<TIn, TOut, TFactory>> ProcessLocal<TOldIn, TIn, TOut, TFactory>(
            this StreamProcessorChain<TOldIn, TIn, TFactory> previousNode, Func<INotifyEnumerable<TIn>, INotifyEnumerable<TOut>> processingFunc,
            int scatterFactor = 1) where TFactory : IncrementalNmfModelStreamProcessorAggregateFactory
        {
            var aggregateConfiguration = new StreamProcessorAggregateConfiguration(await previousNode.Aggregate.GetOutputStreams(), scatterFactor);
            var processorAggregate = await previousNode.Factory.CreateProcessor(processingFunc, aggregateConfiguration);
            var processorChain = new StreamProcessorChain<TIn, TOut, TFactory>(processorAggregate, previousNode.Factory);

            return processorChain;
        }

        public static async Task<StreamProcessorChain<TIn, TOut, TFactory>> ProcessLocal<TOldIn, TIn, TOut, TFactory>(
            this Task<StreamProcessorChain<TOldIn, TIn, TFactory>> previousNodeTask,
            Func<INotifyEnumerable<TIn>, INotifyEnumerable<TOut>> processingFunc, int scatterFactor = 1)
            where TFactory : IncrementalNmfModelStreamProcessorAggregateFactory
        {
            return await ProcessLocal(await previousNodeTask, processingFunc, scatterFactor);
        }

        #endregion
    }
}