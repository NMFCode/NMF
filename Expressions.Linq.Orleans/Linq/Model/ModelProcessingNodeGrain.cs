using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public abstract class ModelProcessingNodeGrain<TIn, TOut, TModel> : Grain, IModelProcessingNodeGrain<TIn, TOut, TModel>
        where TModel : IResolvableModel
    {
        protected const string StreamProviderNamespace = "CollectionStreamProvider"; // TODO replace with config value

        protected LocalModelReceiveContext ReceiveContext;
        protected ILocalSendContext SendContext;

        protected TransactionalStreamModelConsumer<TIn, TModel> StreamConsumer;

        protected MappingStreamMessageSenderComposite<TOut> StreamSender;
        protected IModelContainerGrain<TModel> ModelContainer { get; private set; }

        public int OutputMultiplexFactor { get; set; }

        public Task<string> ModelToString(Func<TModel, IModelElement> elementSelectorFunc)
        {
            var element = elementSelectorFunc(StreamConsumer.Model);
            var result = element.ToXmlString();

            return Task.FromResult(result);
        }

        public Task<IModelContainerGrain<TModel>> GetModelContainer()
        {
            return Task.FromResult(ModelContainer);
        }

        public virtual async Task Setup(IModelContainerGrain<TModel> modelContainer, IEnumerable<StreamIdentity> inputStreams = null,
            int outputMultiplexFactor = 1)
        {
            await StreamConsumer.SetModelContainer(modelContainer);
            OutputMultiplexFactor = outputMultiplexFactor;

            int inputStreamCount = inputStreams?.Count() ?? 0;
            if (inputStreamCount > 0)
                await Task.WhenAll(inputStreams.Select(s => StreamConsumer.MessageDispatcher.Subscribe(s)));

            StreamSender = new MappingStreamMessageSenderComposite<TOut>(GetStreamProvider(StreamProviderNamespace),
                OutputMultiplexFactor * inputStreamCount);
        }

        public async Task SubscribeToStreams(IEnumerable<StreamIdentity> inputStreams)
        {
            await StreamSender.SetNumberOfSenders(inputStreams.Count() * OutputMultiplexFactor);
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