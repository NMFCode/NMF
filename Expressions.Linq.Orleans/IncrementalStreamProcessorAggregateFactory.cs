﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using NMF.Models.Tests.Railway;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Linq.Aggregates;

namespace NMF.Expressions.Linq.Orleans
{

    /// <summary>
    /// Factory for NMF models. Used for Extension method ToNmfModelConsumer() in order to not make the caller specify all generic arguments.
    /// </summary>
    public class IncrementalNmfModelStreamProcessorAggregateFactory : IncrementalStreamProcessorAggregateFactory<NMF.Models.Model>
    {
        public IncrementalNmfModelStreamProcessorAggregateFactory(IGrainFactory factory, IModelContainerGrain<NMF.Models.Model> modelContainer) : base(factory, modelContainer)
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

        public async Task<IStreamProcessorAggregate<TIn, TOut>> CreateSimpleSelectMany<TIn, TOut>(Expression<Func<TIn, IEnumerable<TOut>>> selectionFunc, StreamProcessorAggregateConfiguration configuration)
        {
            var processorAggregate = GrainFactory.GetGrain<IIncrementalSimpleSelectManyAggregateGrain<TIn, TOut, TModel>>(Guid.NewGuid());

            await processorAggregate.SetObservingFunc(selectionFunc);
            await processorAggregate.Setup(ModelContainerGrain, configuration.ScatterFactor);
            await processorAggregate.SetInput(configuration.InputStreams);

            return processorAggregate;
        }

        public async Task<IStreamProcessorAggregate<TIn, TOut>> CreateSelectMany<TIn, TIntermediate, TOut>(Expression<Func<TIn, IEnumerable<TIntermediate>>> collectionSelectorFunc, Expression<Func<TIn, TIntermediate, TOut>> resultSelectorFunc, StreamProcessorAggregateConfiguration configuration)
        {
            var processorAggregate = GrainFactory.GetGrain<IIncrementalSelectManyAggregateGrain<TIn, TIntermediate, TOut, TModel>>(Guid.NewGuid());

            await processorAggregate.SetObservingFunc(collectionSelectorFunc, resultSelectorFunc);
            await processorAggregate.Setup(ModelContainerGrain, configuration.ScatterFactor);
            await processorAggregate.SetInput(configuration.InputStreams);

            return processorAggregate;
        }

        public IGrainFactory GrainFactory { get; }

        public async Task<IStreamProcessorAggregate<TIn, TOut>> CreateSelect<TIn, TOut>(Expression<Func<TIn, TOut>> selectionFunc, StreamProcessorAggregateConfiguration configuration)
        {
            var processorAggregate = GrainFactory.GetGrain<IIncrementalSelectAggregateGrain<TIn, TOut, TModel>>(Guid.NewGuid());

            await processorAggregate.SetObservingFunc(selectionFunc);
            await processorAggregate.Setup(ModelContainerGrain, configuration.ScatterFactor);
            await processorAggregate.SetInput(configuration.InputStreams);

            return processorAggregate;
        }

        public async Task<IStreamProcessorAggregate<TIn, TIn>> CreateWhere<TIn>(Expression<Func<TIn, bool>> filterFunc, StreamProcessorAggregateConfiguration configuration)
        {
            var processorAggregate = GrainFactory.GetGrain<IIncrementalWhereAggregateGrain<TIn, TModel>>(Guid.NewGuid());

            await processorAggregate.SetObservingFunc(filterFunc);
            await processorAggregate.Setup(ModelContainerGrain, configuration.ScatterFactor);
            await processorAggregate.SetInput(configuration.InputStreams);

            return processorAggregate;
        }

        public async Task<IStreamProcessorAggregate<TIn, TOut>> CreateProcessor<TIn, TOut>(
            Func<INotifyEnumerable<TIn>, INotifyEnumerable<TOut>> processingFunc, StreamProcessorAggregateConfiguration configuration)
        {
            var processorAggregate = GrainFactory.GetGrain<IIncrementalFunctionAggregateGrain<TIn, TOut, TModel>>(Guid.NewGuid());

            await processorAggregate.SetIncrementalFunc(processingFunc);
            await processorAggregate.Setup(ModelContainerGrain, configuration.ScatterFactor);
            await processorAggregate.SetInput(configuration.InputStreams);

            return processorAggregate;
        }
    }
}