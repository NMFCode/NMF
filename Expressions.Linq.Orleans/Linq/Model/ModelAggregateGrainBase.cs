using System;
using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans.Model
{
    /// <summary>
    /// Transforms data that can be based on a model from TIn to TOut using multiple IStreamProcessorNode.
    /// </summary>
    /// <typeparam name="TSource">Data input type.</typeparam>
    /// <typeparam name="TResult">Data output type.</typeparam>
    /// <typeparam name="TModel">Data type of the model.</typeparam>
    public abstract class ModelAggregateGrainBase<TSource, TResult, TNode, TModel> : StreamProcessorAggregate<TSource, TResult, TNode>,
        IModelProcessingAggregateGrain<TSource, TResult, TModel>
        where TNode : IModelProcessingNodeGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {
        protected IModelContainerGrain<TModel> ModelContainer;

        protected int OutputMultiplexFactor { get; set; } = 1;

        /// <summary>
        /// Setup the aggregate.
        /// </summary>
        /// <param name="modelContainer">Model container to use.</param>
        /// <param name="outputMultiplexFactor">Multiplexing from input to output streams. Values in range [1, int32.max] are allowed.</param>
        /// <returns></returns>
        public Task Setup(IModelContainerGrain<TModel> modelContainer, int outputMultiplexFactor = 1)
        {
            ModelContainer = modelContainer;
            OutputMultiplexFactor = outputMultiplexFactor;

            return TaskDone.Done;
        }
    }
}