using System;
using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public abstract class IncrementalAggregateGrainBase<TSource, TResult, TNode, TModel> : StreamProcessorAggregate<TSource, TResult, TNode>,
        IModelProcessingAggregateGrain<TSource, TResult, TModel>
        where TNode : IModelProcessingNodeGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {
        protected IModelContainerGrain<TModel> ModelContainer;

        protected uint OutputMultiplexFactor { get; set; }

        public Task<IModelContainerGrain<TModel>> GetModelContainer()
        {
            return Task.FromResult(ModelContainer);
        }

        public Task SetModelContainer(IModelContainerGrain<TModel> modelContainer)
        {
            ModelContainer = modelContainer;
            return TaskDone.Done;
        }

        public Task SetOutputMultiplex(uint factor = 1)
        {
            if (ProcessorNodes.Count != 0)
            {
                throw new InvalidOperationException("Setting the value for output multiplexing must happen before processor nodes are initialized.");
            }
            OutputMultiplexFactor = factor;
            return TaskDone.Done;
        }
    }
}