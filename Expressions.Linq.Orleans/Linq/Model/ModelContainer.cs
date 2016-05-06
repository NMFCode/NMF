using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class ModelContainer<T> : Grain, IModelContainerGrain<T> where T : IResolvableModel, IModelElement
    { 

        private const string StreamProviderName = "CollectionStreamProvider";
        protected T Model;
        protected StreamMessageSender<Models.Model> OutputProducer;
        protected StreamMessageSender<Models.Model> ModelUpdateSender;
        private ILocalSendContext _sendContext;
        private string _modelPath;

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            _sendContext = new LocalSendContext();
            OutputProducer = new StreamMessageSender<Models.Model>(GetStreamProvider(StreamProviderName), this.GetPrimaryKey());
            ModelUpdateSender = new StreamMessageSender<Models.Model>(GetStreamProvider(StreamProviderName), new StreamIdentity(this.GetPrimaryKey(), "ModelUpdate"));
        }

        private async Task SendAllQueuedMessages()
        {
            await OutputProducer.FlushQueue();
            await ModelUpdateSender.FlushQueue();
        }


        public async Task ExecuteSync(Action<T> action, bool newModelElementCreated = false)
        {
            if (newModelElementCreated)
                Model.BubbledChange -= ModelBubbledChange;

            action(Model);

            if (newModelElementCreated)
            {
                Model.BubbledChange += ModelBubbledChange;
                ModelUpdateSender.EnqueueMessage(new ModelExecuteActionMessage<T>(action));
            }

            await SendAllQueuedMessages();
        }

        public async Task ExecuteSync(Action<T, object> action, object state, bool newModelElementCreated = false)
        {
            if (newModelElementCreated)
                Model.BubbledChange -= ModelBubbledChange;

            action(Model, state);

            if (newModelElementCreated)
            {
                Model.BubbledChange += ModelBubbledChange;
                ModelUpdateSender.EnqueueMessage(new ModelExecuteActionMessage<T>(action, state));
            }

            await SendAllQueuedMessages();

            // No flush pipeline

            await OutputProducer.SendMessage(new FlushMessage());
        }

        public async Task<StreamIdentity> GetModelUpdateStream()
        {
            return (await ModelUpdateSender.GetOutputStreams()).First();
        }

        public Task<string> GetModelPath()
        {
            return Task.FromResult(_modelPath);
        }

        public Task<RailwayContainer> NeverCallMe()
        {
            throw new InvalidOperationException();
        }

        public Task<string> ModelToString(Func<T, IModelElement> elementSelectorFunc)
        {
            var element = elementSelectorFunc(Model);

            ModelElement.EnforceModels = false; // TODO remove once bug fixed
            var result = element.ToXmlString();
            ModelElement.EnforceModels = true;

            return Task.FromResult(result);
        }

        public Task LoadModelFromPath(string modelPath)
        {
            ModelElement.EnforceModels = true;
            Assembly.LoadFrom("NMF.Models.Tests.dll");
            _modelPath = modelPath;

            Model = ModelUtil.LoadModelFromPath<T>(modelPath);

            Model.BubbledChange += ModelBubbledChange;

            return TaskDone.Done;
        }

        public async Task<IList<StreamIdentity>> GetOutputStreams()
        {
            return await OutputProducer.GetOutputStreams();
        }

        public async Task<bool> IsTearedDown()
        {
            return await OutputProducer.IsTearedDown() && await ModelUpdateSender.IsTearedDown();
        }

        public async Task TearDown()
        {
            await OutputProducer.TearDown();
            await ModelUpdateSender.TearDown();
        }

        private void ModelBubbledChange(object sender, BubbledChangeEventArgs e)
        {
            var sourceElement = ModelRemoteValueFactory.CreateModelRemoteValue(e.Element, _sendContext);
            if (e.IsCollectionChangeEvent)
            {
                ModelCollectionChangedMessage message = null;
                var eventArgs = (NotifyCollectionChangedEventArgs) e.OriginalEventArgs;
                switch (eventArgs.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        message = new ModelCollectionChangedMessage(NotifyCollectionChangedAction.Add, sourceElement,
                            CreateModelChanges(eventArgs.NewItems));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        message = new ModelCollectionChangedMessage(NotifyCollectionChangedAction.Remove, sourceElement,
                            CreateModelChanges(eventArgs.OldItems));
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        message = new ModelCollectionChangedMessage(NotifyCollectionChangedAction.Reset, sourceElement, null);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                ModelUpdateSender.EnqueueMessage(message);
            }
            else if (e.IsPropertyChangedEvent)
            {
                var eventArgs = (ValueChangedEventArgs) e.OriginalEventArgs;
                var propertyName = e.PropertyName;
                var propertyValue = eventArgs.NewValue;

                var modelChangeValue = ModelRemoteValueFactory.CreateModelRemoteValue(propertyValue, _sendContext);
                var oldValue = ModelRemoteValueFactory.CreateModelRemoteValue(eventArgs.OldValue, _sendContext);

                var message = new ModelPropertyChangedMessage(modelChangeValue, oldValue, sourceElement, propertyName);
                ModelUpdateSender.EnqueueMessage(message);
            }
        }

        private IObjectRemoteValue[] CreateModelChanges(IList modelElements)
        {
            var changes = new IObjectRemoteValue[modelElements.Count];
            for (var i = 0; i < modelElements.Count; i++)
            {
                changes[i] = ModelRemoteValueFactory.CreateModelRemoteValue(modelElements[i], _sendContext);
            }

            return changes;
        }

        public Task<Guid> EnumerateToStream(StreamIdentity streamIdentity, Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> EnumerateToSubscribers(Guid? transactionId = null)
        {
            var remoteModelValue = ModelRemoteValueFactory.CreateModelRemoteValue(Model, _sendContext);
            var message = new ModelItemAddMessage<T>(new List<IObjectRemoteValue<T>> { remoteModelValue });

            var tId = TransactionGenerator.GenerateTransactionId(transactionId);
            await OutputProducer.StartTransaction(tId);
            await OutputProducer.SendMessage(message);
            await OutputProducer.EndTransaction(tId);
            return tId;
        }
    }
}