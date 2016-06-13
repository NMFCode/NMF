﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans.Model
{
    /// <summary>
    ///     Executes operations on streaming data based on a model and forwards it to its output stream.
    /// </summary>
    public abstract class ModelProcessingNodeGrain<TIn, TOut, TModel> : Grain, IModelProcessingNodeGrain<TIn, TOut, TModel>
        where TModel : IResolvableModel
    {
        protected const string StreamProviderNamespace = "CollectionStreamProvider"; // TODO replace with config value

        protected ILocalSendContext SendContext;

        protected TransactionalStreamModelConsumer<TIn, TModel> StreamConsumer;

        protected MappingStreamMessageSenderComposite<TOut> StreamSender;
        protected IModelContainerGrain<TModel> ModelContainer { get; private set; }
        private IModelSiloGrainManagerGrain<TModel> _localModelManagerGrain = null;
        protected int OutputMultiplexFactor { get; set; }

        /// <summary>
        /// Transform the model into an xml string.
        /// </summary>
        /// <param name="elementSelectorFunc">Element to start serializing from.</param>
        /// <returns>XML serialization of the model.</returns>
        public Task<string> ModelToString(Func<TModel, IModelElement> elementSelectorFunc)
        {
            var element = elementSelectorFunc(StreamConsumer.Model);
            var result = element.ToXmlString();

            return Task.FromResult(result);
        }

        public virtual async Task Setup(IModelContainerGrain<TModel> modelContainer,
            int outputMultiplexFactor = 1)
        {
            var managementKey = modelContainer.GetPrimaryKey();
            var siloModelManagerGrain = GrainFactory.GetGrain<IModelSiloGrainManagerGrain<TModel>>(managementKey);
            var localModelGrain = await siloModelManagerGrain.GetModelSiloGrainForSilo(RuntimeIdentity);
            _localModelManagerGrain = siloModelManagerGrain;

            var identity = await localModelGrain.GetIdentity();
            await StreamConsumer.SetLocalModel(localModelGrain);
            //await StreamConsumer.SetModelContainer(modelContainer);
            OutputMultiplexFactor = outputMultiplexFactor;
        }

        public async Task SubscribeToStreams(IEnumerable<StreamIdentity> inputStreams)
        {
            await StreamSender.SetNumberOfSenders(inputStreams.Count()*OutputMultiplexFactor);
            await Task.WhenAll(inputStreams.Select(s => StreamConsumer.MessageDispatcher.Subscribe(s)));
        }

        public async Task TransactionComplete(Guid transactionId)
        {
            await StreamConsumer.TransactionComplete(transactionId);
        }

        public async Task<IList<StreamIdentity>> GetOutputStreams()
        {
            return await StreamSender.GetOutputStreams();
        }

        public async Task<bool> IsTearedDown()
        {
            var consumerTearDownState = (StreamConsumer == null) || await StreamConsumer.IsTearedDown();
            var providerTearDownState = (StreamSender == null) || await StreamSender.IsTearedDown();

            return consumerTearDownState && providerTearDownState;
        }

        public async Task<IList<SiloLocationStreamIdentity>> GetOutputStreamsWithSourceLocation()
        {
            var outputStreams = await GetOutputStreams();
            return outputStreams.Select(os => new SiloLocationStreamIdentity(os.Guid, os.Namespace, RuntimeIdentity)).ToList();
        }

        public async Task TearDown()
        {
            if (StreamConsumer != null)
            {
                await StreamConsumer.TearDown();
                StreamConsumer = null;
            }

            if (StreamSender != null)
            {
                await StreamSender.TearDown();
                StreamSender = null;
            }
            if (_localModelManagerGrain != null)
            {
                await _localModelManagerGrain.TearDownModelSiloGrain(RuntimeIdentity);
            }
            DeactivateOnIdle();
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            ModelElement.EnforceModels = true;
            StreamSender = new MappingStreamMessageSenderComposite<TOut>(GetStreamProvider(StreamProviderNamespace));
            SendContext = new LocalSendContext();
        }

        public async Task SetModelContainer(IModelContainerGrain<TModel> modelContainer)
        {
            await StreamConsumer.SetModelContainer(modelContainer);
        }
    }
}