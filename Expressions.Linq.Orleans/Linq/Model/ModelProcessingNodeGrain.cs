using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Models;
using NMF.Models.Repository;
using Orleans;
using Orleans.Streams.Linq.Nodes;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public class ModelProcessingNodeGrain<TIn, TOut> : StreamProcessorNodeGrain<TIn, TOut>, IModelProcessingNodeGrain<TIn, TOut, Models.Model>
    {
        protected Models.Model Model { get; private set; }

        public Task LoadModel(Func<Models.Model> modelLoadingFunc)
        {
            Model = modelLoadingFunc();
            return TaskDone.Done;
        }

        public Task<string> ModelToString(Func<Models.Model, IModelElement> elementSelectorFunc)
        {
            var element = elementSelectorFunc(Model);

            var serializer = MetaRepository.Instance.Serializer;

            var stream = new NonImplicitDisposableMemoryStream();
            serializer.SerializeFragment(element, stream);

            var result = Encoding.UTF8.GetString(stream.ToArray(), 0, (int) stream.Length);
            stream.CanDispose = true;
            stream.Dispose();

            return Task.FromResult(result);
        }

        public async Task SetModelContainer(IModelContainerGrain<Models.Model> modelContainer)
        {
            await SetInput(await modelContainer.GetStreamIdentity());
        }

        protected override void RegisterMessages()
        {
            base.RegisterMessages();
            StreamMessageDispatchReceiver.Register<ModelCollectionChangedMessage>(ProcessModelCollectionChangedMessage);
            StreamMessageDispatchReceiver.Register<ModelPropertyChangedMessage>(ProcessModelPropertyChangedMessage);
        }

        private Task ProcessModelPropertyChangedMessage(ModelPropertyChangedMessage message)
        {
            var sourceItem = Model.Resolve(message.RelativeRootUri);
            var newValue = message.Value.Retrieve(Model);

            sourceItem.GetType().GetProperty(message.PropertyName).GetSetMethod(true).Invoke(sourceItem, new[] {newValue});
            return TaskDone.Done;
        }

        private Task ProcessModelCollectionChangedMessage(ModelCollectionChangedMessage message)
        {
            var sourceItem = (IList) Model.Resolve(message.RelativeRootUri);

            switch (message.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var itemToAdd in message.Elements)
                    {
                        sourceItem.Add(itemToAdd.Retrieve(Model));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var itemToRemove in message.Elements)
                    {
                        sourceItem.Remove(itemToRemove.Retrieve(Model));
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    sourceItem.Clear();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return TaskDone.Done;
        }
    }
}