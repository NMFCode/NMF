using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using Orleans.Streams.Endpoints;

namespace NMF.Expressions.Linq.Orleans
{
    public class TransactionalStreamModelConsumer<T, TModel> : TransactionalStreamConsumer where TModel : IResolvableModel
    {
        public TModel Model { get; private set; }

        public LocalModelReceiveContext ModelReceiveContext { get; private set; }

        public IList<T> Items { get; set; }

        public TransactionalStreamModelConsumer(IStreamProvider streamProvider, IList<T> items = null) : base(streamProvider)
        {
            Items = items ?? new List<T>();
            ModelElement.EnforceModels = true;
        }

        public async Task SetModelContainer(IModelContainerGrain<TModel> modelContainer)
        {
            var modelPath = await modelContainer.GetModelPath();
            Model = ModelUtil.LoadModelFromPath<TModel>(modelPath);
            ModelReceiveContext = new LocalModelReceiveContext(Model);
            await MessageDispatcher.Subscribe(await modelContainer.GetModelUpdateStream());
        }

        protected override void SetupMessageDispatcher(StreamMessageDispatchReceiver dispatcher)
        {
            base.SetupMessageDispatcher(dispatcher);
            dispatcher.Register<ModelItemAddMessage<T>>(ProcessModelItemAddMessage);
            dispatcher.Register<ModelItemRemoveMessage<T>>(ProcessModelItemRemoveMessage);
            dispatcher.Register<ModelCollectionChangedMessage>(ProcessModelCollectionChangedMessage);
            dispatcher.Register<ModelPropertyChangedMessage>(ProcessModelPropertyChangedMessage);

            dispatcher.Register<ModelExecuteActionMessage<TModel>>(async message => { message.Execute(Model); });
        }

        private Task ProcessModelItemAddMessage(ModelItemAddMessage<T> message)
        {
            foreach (var item in message.Items)
            {
                var resultItem = item.Retrieve(ModelReceiveContext, ReceiveAction.LookupInsertIfNotFound);
                Items.Add(resultItem);
            }

            return TaskDone.Done;
        }

        private Task ProcessModelItemRemoveMessage(ModelItemRemoveMessage<T> message)
        {
            foreach (var item in message.Items)
            {
                var resultItem = item.Retrieve(ModelReceiveContext, ReceiveAction.Delete);
                Items.Remove(resultItem);
            }

            return TaskDone.Done;
        }

        private Task ProcessModelPropertyChangedMessage(ModelPropertyChangedMessage message)
        {
            var sourceItem = message.ElementAffected.Retrieve(ModelReceiveContext, ReceiveAction.LookupInsertIfNotFound);
            var newValue = message.Value.Retrieve(ModelReceiveContext, ReceiveAction.LookupInsertIfNotFound);
            var oldValue = message.Value.Retrieve(ModelReceiveContext, ReceiveAction.Delete); // Remove old value from lookup

            sourceItem.GetType().GetProperty(message.PropertyName).GetSetMethod(true).Invoke(sourceItem, new[] {newValue});

            return TaskDone.Done;
        }

        private Task ProcessModelCollectionChangedMessage(ModelCollectionChangedMessage message)
        {
            var sourceItem = (IList) message.ElementAffected.Retrieve(ModelReceiveContext, ReceiveAction.LookupInsertIfNotFound);
                // Model.Resolve(message.RelativeRootUri);

            switch (message.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var itemToAdd in message.Elements)
                    {
                        sourceItem.Add(itemToAdd.Retrieve(ModelReceiveContext, ReceiveAction.LookupInsertIfNotFound));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var itemToRemove in message.Elements)
                    {
                        sourceItem.Remove(itemToRemove.Retrieve(ModelReceiveContext, ReceiveAction.Delete));
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