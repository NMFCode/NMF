using System.Threading.Tasks;
using NMF.Models;
using Orleans;
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
    }
}