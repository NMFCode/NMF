using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Models;
using NMF.Models.Repository;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Endpoints;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class ModelContainer<T> : Grain, IElementEnumeratorNode<T>, IModelContainerGrain<T> where T : Models.Model
    {
        private const string StreamProviderName = "CollectionStreamProvider";
        protected T Model;
        protected StreamMessageSender OutputProducer;

        public override Task OnActivateAsync()
        {
            base.OnActivateAsync();
            OutputProducer = new StreamMessageSender(GetStreamProvider(StreamProviderName), this.GetPrimaryKey());
            return TaskDone.Done;
        }


        public async Task ExecuteSync(Action<T> action)
        {
            action(Model);
            await OutputProducer.SendMessagesFromQueue();
        }

        public Task<string> ModelToString(Func<Models.Model, IModelElement> elementSelectorFunc)
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

        public Task LoadModel(Func<T> modelLoadingFunc)
        {
            Model = modelLoadingFunc();
            Model.BubbledChange += ModelBubbledChange;

            return TaskDone.Done;
        }

        public Task<StreamIdentity> GetStreamIdentity()
        {
            return OutputProducer.GetStreamIdentity();
        }

        public Task<bool> IsTearedDown()
        {
            return OutputProducer.IsTearedDown();
        }

        public Task TearDown()
        {
            return OutputProducer.TearDown();
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

                OutputProducer.AddToMessageQueue(message);
            }
            else if (e.IsValueChangedEvent)
            {
                var propertyName = e.PropertyName;
                var propertyValue = e.SourceElement.GetType().GetProperty(propertyName).GetGetMethod(true).Invoke(e.SourceElement, null);

                var modelChangeValue = ModelRemoteValueFactory.CreateModelChangeValue(propertyValue);

                var message = new ModelPropertyChangedMessage(modelChangeValue, sourceUri, propertyName);
                OutputProducer.AddToMessageQueue(message);
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
            await OutputProducer.SendMessage(message);
            return transactionId.Value;
        }
    }
}