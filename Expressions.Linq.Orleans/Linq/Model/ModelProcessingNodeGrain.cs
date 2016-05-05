using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Models;
using NMF.Models.Repository;
using Orleans;
using Orleans.Streams;
using Orleans.Streams.Linq.Nodes;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class ModelProcessingNodeGrain<TIn, TOut, TModel> : StreamProcessorNodeGrain<TIn, TOut>, IModelProcessingNodeGrain<TIn, TOut, TModel>
        where TModel : IResolvableModel
    {
        protected TModel Model { get; private set; }
        protected IModelContainerGrain<TModel> ModelContainer { get; private set; }

        protected ILocalResolveContext ResolveContext;

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            StreamSender = new MappingStreamMessageSenderComposite<TOut>(GetStreamProvider(StreamProviderNamespace));
        }

        protected new MappingStreamMessageSenderComposite<TOut> StreamSender
        {
            get { return (MappingStreamMessageSenderComposite<TOut>) base.StreamSender; }
            set { base.StreamSender = value; }
        }

        public Task LoadModelFromPath(string modelPath)
        {
            ModelElement.EnforceModels = false;
            Model = ModelUtil.LoadModelFromPath<TModel>(modelPath);
            ModelElement.EnforceModels = true;
            ResolveContext = new LocalResolveContext(Model);
            return TaskDone.Done;
        }

        public Task<string> ModelToString(Func<TModel, IModelElement> elementSelectorFunc)
        {
            var element = elementSelectorFunc(Model);

            ModelElement.EnforceModels = false; // TODO remove once bug fixed
            var result = element.ToXmlString();
            ModelElement.EnforceModels = true;

            return Task.FromResult(result);
        }

        public async Task SetModelContainer(IModelContainerGrain<TModel> modelContainer)
        {
            ModelContainer = modelContainer;
            var loadModelTask = LoadModelFromPath(await modelContainer.GetModelPath());
            var subscribeTask = SubscribeToStreams((await modelContainer.GetModelUpdateStream()).SingleValueToList());

            await Task.WhenAll(loadModelTask, subscribeTask);
        }

        public Task<IModelContainerGrain<TModel>> GetModelContainer()
        {
            return Task.FromResult(ModelContainer);
        }

        protected override void RegisterMessages()
        {
            base.RegisterMessages();
            StreamConsumer.MessageDispatcher.Register<ModelCollectionChangedMessage>(ProcessModelCollectionChangedMessage);
            StreamConsumer.MessageDispatcher.Register<ModelPropertyChangedMessage>(ProcessModelPropertyChangedMessage);

            StreamConsumer.MessageDispatcher.Register<ModelExecuteActionMessage<TModel>>(async message =>
            {
                message.Execute(Model);
                //await StreamSender.FlushQueue();
            });
        }

        private async Task ProcessModelPropertyChangedMessage(ModelPropertyChangedMessage message)
        {
            var sourceItem = Model.Resolve(message.RelativeRootUri);
            var newValue = message.Value.Retrieve(ResolveContext);

            sourceItem.GetType().GetProperty(message.PropertyName).GetSetMethod(true).Invoke(sourceItem, new[] {newValue});

            await StreamSender.FlushQueue();
        }

        private async Task ProcessModelCollectionChangedMessage(ModelCollectionChangedMessage message)
        {
            var sourceItem = (IList) Model.Resolve(message.RelativeRootUri);

            switch (message.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var itemToAdd in message.Elements)
                    {
                        sourceItem.Add(itemToAdd.Retrieve(ResolveContext));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var itemToRemove in message.Elements)
                    {
                        sourceItem.Remove(itemToRemove.Retrieve(ResolveContext));
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    sourceItem.Clear();
                    break;
                default:
                    throw new NotImplementedException();
            }

            // TODO maybe in the feature just apply changes if necessary because updated item might or might not be removed
            await StreamSender.FlushQueue();
        }


        public Task SetOutputMultiplex(uint factor = 1)
        {
            if (StreamConsumer.MessageDispatcher.SubscriptionCount != 0)
            {
                throw new InvalidOperationException("Input stream was already set.");
            }
            StreamSender = new MappingStreamMessageSenderComposite<TOut>(GetStreamProvider(StreamProviderNamespace), (int) factor);
            return TaskDone.Done;
        }
    }
}