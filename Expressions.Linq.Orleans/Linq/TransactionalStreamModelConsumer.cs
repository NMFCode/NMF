using System.Linq;
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
    public class TransactionalStreamModelConsumer<T, TModel> : TransactionalStreamListConsumer<T> where TModel : IResolvableModel
    {
        protected TModel Model;

        public LocalModelReceiveContext ModelReceiveContext { get; }

        public TransactionalStreamModelConsumer(IStreamProvider streamProvider, TModel model) : base(streamProvider)
        {
            Model = model;
            ModelReceiveContext = new LocalModelReceiveContext(model);
            ModelElement.EnforceModels = true;
        }

        public async Task SetModelContainer(IModelContainerGrain<TModel> modelContainer)
        {
            await MessageDispatcher.Subscribe(await modelContainer.GetModelUpdateStream());
            MessageDispatcher.Register<ModelExecuteActionMessage<TModel>>(message =>
            {
                message.Execute(Model);
                return TaskDone.Done;
            });
        }

        protected override void SetupMessageDispatcher(StreamMessageDispatchReceiver dispatcher)
        {
            base.SetupMessageDispatcher(dispatcher);
            dispatcher.Register<ModelItemAddMessage<T>>(ProcessModelItemAddMessage);
            dispatcher.Register<ModelItemRemoveMessage<T>>(ProcessModelItemRemoveMessage);
        }

        private Task ProcessModelItemAddMessage(ModelItemAddMessage<T> message)
        {
            foreach (var item in message.Items)
            {
                var resultItem = item.Retrieve(ModelReceiveContext, ReceiveAction.Insert);
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
    }
}