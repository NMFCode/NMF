using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class ModelContainer<T> : Grain, IModelContainerGrain<T> where T : IModelElement
    {
        private const string StreamProviderName = "CollectionStreamProvider";
        protected T Model;
        protected StreamMessageSender OutputProducer;
        protected StreamMessageSender ModelUpdateSender;
        private Func<string, T> _modelLoadingFunc;
        private string _modelPath;

        public override Task OnActivateAsync()
        {
            base.OnActivateAsync();
            OutputProducer = new StreamMessageSender(GetStreamProvider(StreamProviderName), this.GetPrimaryKey());
            ModelUpdateSender = new StreamMessageSender(GetStreamProvider(StreamProviderName), new StreamIdentity("ModelUpdate", this.GetPrimaryKey()));
            return TaskDone.Done;
        }


        public async Task ExecuteSync(Action<T> action)
        {
            action(Model);

            await ModelUpdateSender.SendMessagesFromQueue();
            await OutputProducer.SendMessagesFromQueue();
        }

        public async Task ExecuteSync(Action<T, object> action, object state)
        {
            action(Model, state);

            await ModelUpdateSender.SendMessagesFromQueue();
            await OutputProducer.SendMessagesFromQueue();
        }

        public Task<StreamIdentity> GetModelUpdateStream()
        {
            return ModelUpdateSender.GetStreamIdentity();
        }

        public Task<string> GetModelPath()
        {
            return Task.FromResult(_modelPath);
        }

        public Task<string> ModelToString(Func<T, IModelElement> elementSelectorFunc)
        {
            var element = elementSelectorFunc(Model);

            var serializer = MetaRepository.Instance.Serializer;

            var stream = new NonImplicitDisposableMemoryStream();
            serializer.SerializeFragment(element, stream);

            var result = System.Text.Encoding.UTF8.GetString(stream.ToArray(), 0, (int) stream.Length);
            stream.CanDispose = true;
            stream.Dispose();

            return Task.FromResult(result);
        }

        public Task LoadModelFromPath(string modelPath)
        {
            Assembly.LoadFrom("NMF.Models.Tests.dll");
            _modelPath = modelPath;

            Model = ModelUtil.LoadModelFromPath<T>(modelPath);
            Model.BubbledChange += ModelBubbledChange;

            return TaskDone.Done;
        }

        public async Task<IList<StreamIdentity>> GetOutputStreams()
        {
            return new List<StreamIdentity> { await OutputProducer.GetStreamIdentity() };
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
            var sourceUri = e.SourceElement.RelativeUri;
            if (e.IsCollectionChangeEvent)
            {
                ModelCollectionChangedMessage message = null;
                switch (e.OriginalEventArgs.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        message = new ModelCollectionChangedMessage(NotifyCollectionChangedAction.Add, sourceUri,
                            CreateModelChanges(e.OriginalEventArgs.NewItems));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        message = new ModelCollectionChangedMessage(NotifyCollectionChangedAction.Remove, sourceUri,
                            CreateModelChanges(e.OriginalEventArgs.OldItems));
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        message = new ModelCollectionChangedMessage(NotifyCollectionChangedAction.Reset, sourceUri, null);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                ModelUpdateSender.AddToMessageQueue(message);
            }
            else if (e.IsValueChangedEvent)
            {
                var propertyName = e.PropertyName;
                var propertyValue = e.SourceElement.GetType().GetProperty(propertyName).GetGetMethod(true).Invoke(e.SourceElement, null);

                var modelChangeValue = ModelRemoteValueFactory.CreateModelChangeValue(propertyValue);

                var message = new ModelPropertyChangedMessage(modelChangeValue, sourceUri, propertyName);
                ModelUpdateSender.AddToMessageQueue(message);
            }
        }

        private IModelRemoteValue[] CreateModelChanges(IList modelElements)
        {
            var changes = new IModelRemoteValue[modelElements.Count];
            for (var i = 0; i < modelElements.Count; i++)
            {
                changes[i] = ModelRemoteValueFactory.CreateModelChangeValue(modelElements[i]);
            }

            return changes;
        }

        public Task<Guid> EnumerateToStream(StreamIdentity streamIdentity, Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> EnumerateToSubscribers(Guid? transactionId = null)
        {
            var remoteModelValue = ModelRemoteValueFactory.CreateModelChangeValue(Model);
            var message = new ModelItemAddMessage<T>(new List<IModelRemoteValue<T>> { remoteModelValue });

            var tId = TransactionGenerator.GenerateTransactionId(transactionId);
            await OutputProducer.StartTransaction(tId);
            await OutputProducer.SendMessage(message);
            await OutputProducer.EndTransaction(tId);
            return tId;
        }
    }
}