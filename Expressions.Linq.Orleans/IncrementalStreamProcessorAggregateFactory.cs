using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans
{

    /// <summary>
    /// Factory for NMF models. Used for Extension method ToNmfModelConsumer() in order to not make the caller specify all generic arguments.
    /// </summary>
    public class IncrementalNmfModelStreamProcessorAggregateFactory : IncrementalStreamProcessorAggregateFactory<Models.Model>
    {
        public IncrementalNmfModelStreamProcessorAggregateFactory(IGrainFactory factory, IModelContainerGrain<Models.Model> modelContainer) : base(factory, modelContainer)
        {
        }
    }

    public class IncrementalStreamProcessorAggregateFactory<TModel> : IStreamProcessorAggregateFactory where TModel : IResolvableModel
    {
        public IncrementalStreamProcessorAggregateFactory(IGrainFactory factory, IModelContainerGrain<TModel> modelContainer)
        {
            GrainFactory = factory;
            ModelContainerGrain = modelContainer;
        }

        public IModelContainerGrain<TModel> ModelContainerGrain { get; private set; }

        public async Task<IStreamProcessorAggregate<TIn, TOut>> CreateSimpleSelectMany<TIn, TOut>(Expression<Func<TIn, IEnumerable<TOut>>> selectionFunc, IList<StreamIdentity> streamIdentities)
        {
            var processorAggregate = GrainFactory.GetGrain<IIncrementalSimpleSelectManyAggregateGrain<TIn, TOut, TModel>>(Guid.NewGuid());

            await processorAggregate.SetObservingFunc(selectionFunc);
            await processorAggregate.SetModelContainer(ModelContainerGrain);
            await processorAggregate.SetInput(streamIdentities);

            return processorAggregate;
        }

        public IGrainFactory GrainFactory { get; }

        public async Task<IStreamProcessorAggregate<TIn, TOut>> CreateSelect<TIn, TOut>(Expression<Func<TIn, TOut>> selectionFunc, IList<StreamIdentity> streamIdentities)
        {
            var processorAggregate = GrainFactory.GetGrain<IIncrementalSelectAggregateGrain<TIn, TOut, TModel>>(Guid.NewGuid());

            await processorAggregate.SetObservingFunc(selectionFunc);
            await processorAggregate.SetModelContainer(ModelContainerGrain);
            await processorAggregate.SetInput(streamIdentities);

            return processorAggregate;
        }

        public async Task<IStreamProcessorAggregate<TIn, TIn>> CreateWhere<TIn>(Expression<Func<TIn, bool>> filterFunc, IList<StreamIdentity> streamIdentities)
        {
            var processorAggregate = GrainFactory.GetGrain<IIncrementalWhereAggregateGrain<TIn, TModel>>(Guid.NewGuid());

            await processorAggregate.SetObservingFunc(filterFunc);
            await processorAggregate.SetModelContainer(ModelContainerGrain);
            await processorAggregate.SetInput(streamIdentities);

            return processorAggregate;
        }
    }
}