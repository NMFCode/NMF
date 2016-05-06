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

        protected int OutputMultiplexFactor { get; set; } = 1;

        public Task<IModelContainerGrain<TModel>> GetModelContainer()
        {
            return Task.FromResult(ModelContainer);
        }

        public Task Setup(IModelContainerGrain<TModel> modelContainer, int outputMultiplexFactor = 1)
        {
            ModelContainer = modelContainer;
            OutputMultiplexFactor = outputMultiplexFactor;

            return TaskDone.Done;
        }
    }
}