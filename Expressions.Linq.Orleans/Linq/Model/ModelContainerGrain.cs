using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Models;
using NMF.Models.Tests.Railway;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.Streams.Messages;
using Orleans.Streams.Stateful;
using Orleans.Streams.Stateful.Messages;

namespace NMF.Expressions.Linq.Orleans.Model
{
    /// <summary>
    ///     Contains and owns a model instance.
    /// </summary>
    /// <typeparam name="T">Type of the model.</typeparam>
    public class ModelContainerGrain<T> : Grain, IModelContainerGrain<T> where T : IResolvableModel, IModelElement
    {
        private const string StreamProviderName = "CollectionStreamProvider";
        protected T Model;
        protected StreamMessageSender<Models.Model> OutputProducer;
        protected StreamMessageSender<Models.Model> ModelUpdateSender;
        private ILocalSendContext _sendContext;
        private string _modelPath;

        /// <summary>
        /// Transform the model into an xml string.
        /// </summary>
        /// <param name="elementSelectorFunc">Element to start serializing from.</param>
        /// <returns>XML serialization of the model.</returns>
        public Task<string> ModelToString(Func<T, IModelElement> elementSelectorFunc)
        {
            var element = elementSelectorFunc(Model);

            ModelElement.EnforceModels = false; // TODO remove once bug fixed
            var result = element.ToXmlString();
            ModelElement.EnforceModels = true;

            return Task.FromResult(result);
        }

        public Task<Guid> EnumerateToStream(StreamIdentity streamIdentity, Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> EnumerateToSubscribers(Guid? transactionId = null)
        {
            var remoteModelValue = ModelRemoteValueFactory.CreateModelRemoteValue(Model, _sendContext);
            var message = new RemoteItemAddMessage<T>(new List<IObjectRemoteValue<T>> {remoteModelValue});

            var tId = TransactionGenerator.GenerateTransactionId(transactionId);
            await OutputProducer.StartTransaction(tId);
            await OutputProducer.SendMessage(message);
            await OutputProducer.EndTransaction(tId);
            return tId;
        }

        /// <summary>
        /// Gets the path to the loaded model.
        /// </summary>
        /// <returns>Path to the loaded model.</returns>
        public Task<string> GetModelPath()
        {
            return Task.FromResult(_modelPath);
        }

        /// <summary>
        /// Executes an action on the model. If a new model element is created forwarding of changes is done via forwarding the action
        /// and not via messages.
        /// </summary>
        /// <param name="action">Action to execute on the model.</param>
        /// <param name="newModelElementCreated">Needs to be set to true if a new model element is created during the action.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Executes an action on the model. If a new model element is created forwarding of changes is done via forwarding the action
        /// and not via messages.
        /// </summary>
        /// <param name="action">Action to execute on the model and state.</param>
        /// <param name="state">State that is passed through the action.</param>
        /// <param name="newModelElementCreated">Needs to be set to true if a new model element is created during the action.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets StreamIdentity of the stream that distributes model updates.
        /// </summary>
        /// <returns>Stream identity.</returns>
        public async Task<StreamIdentity> GetModelUpdateStream()
        {
            return (await ModelUpdateSender.GetOutputStreams()).First();
        }

        /// <summary>
        /// Loads a model from the given path.
        /// </summary>
        /// <param name="modelPath">Path to the model.</param>
        /// <returns></returns>
        public Task LoadModelFromPath(string modelPath)
        {
            ModelElement.EnforceModels = true;
            _modelPath = modelPath;

            Model = ModelUtil.LoadModelFromPath<T>(modelPath);

            Model.BubbledChange += ModelBubbledChange;

            return TaskDone.Done;
        }

        /// <summary>
        /// This method is just here to ensure assembly loading of RailwayContainer by Orleans. 
        /// To be removed once NMF assembly registration has been changed.
        /// </summary>
        /// <returns></returns>
        public Task<RailwayContainer> NeverCallMe()
        {
            throw new InvalidOperationException();
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

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            _sendContext = new LocalSendContext();
            OutputProducer = new StreamMessageSender<Models.Model>(GetStreamProvider(StreamProviderName), this.GetPrimaryKey());
            ModelUpdateSender = new StreamMessageSender<Models.Model>(GetStreamProvider(StreamProviderName),
                new StreamIdentity(this.GetPrimaryKey(), "ModelUpdate"));
        }

        private async Task SendAllQueuedMessages()
        {
            await OutputProducer.FlushQueue();
            await ModelUpdateSender.FlushQueue();
        }

        private void ModelBubbledChange(object sender, BubbledChangeEventArgs e)
        {
            var sourceElement = ModelRemoteValueFactory.CreateModelRemoteValue(e.Element, _sendContext);
            if (e.IsCollectionChangeEvent)
            {
                RemoteCollectionChangedMessage message = null;
                var eventArgs = (NotifyCollectionChangedEventArgs) e.OriginalEventArgs;
                switch (eventArgs.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        message = new RemoteCollectionChangedMessage(NotifyCollectionChangedAction.Add, sourceElement,
                            CreateModelChanges(eventArgs.NewItems));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        message = new RemoteCollectionChangedMessage(NotifyCollectionChangedAction.Remove, sourceElement,
                            CreateModelChanges(eventArgs.OldItems));
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        message = new RemoteCollectionChangedMessage(NotifyCollectionChangedAction.Reset, sourceElement, null);
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

                var message = new RemotePropertyChangedMessage(modelChangeValue, oldValue, sourceElement, propertyName);
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
    }
}