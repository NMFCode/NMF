using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public abstract class IncrementalAggregateGrainBase<TSource, TResult, TNode, TModel> : StreamProcessorAggregate<TSource, TResult, TNode>, IModelProcessingAggregateGrain<TSource, TResult, TModel>
         where TNode : IModelProcessingNodeGrain<TSource, TResult, TModel> where TModel : IResolvableModel
    {
        protected IModelContainerGrain<TModel> ModelContainer;

        public Task SetModelContainer(IModelContainerGrain<TModel> modelContainer)
        {
            ModelContainer = modelContainer;
            return TaskDone.Done;
        }

        public Task<IModelContainerGrain<TModel>> GetModelContainer()
        {
            return Task.FromResult(ModelContainer);
        }
    }
}