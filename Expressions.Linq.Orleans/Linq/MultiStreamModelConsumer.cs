using System.Linq;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans;
using Orleans.Streams;
using Orleans.Streams.Endpoints;

namespace NMF.Expressions.Linq.Orleans
{
    public class MultiStreamModelConsumer<T, TModel> : MultiStreamListConsumer<T> where TModel : IResolvableModel
    {
        protected TModel Model;

        public LocalResolveContext ResolveContext { get; }

        public MultiStreamModelConsumer(IStreamProvider streamProvider, TModel model) : base(streamProvider)
        {
            Model = model;
            ResolveContext = new LocalResolveContext(model);
            ModelElement.EnforceModels = true;
        }

        public async Task SetModelContainer(IModelContainerGrain<TModel> modelContainer)
        {
            var dispatcher = MessageDispatchers.First();
            await dispatcher.Subscribe(await modelContainer.GetModelUpdateStream());
            dispatcher.Register<ModelExecuteActionMessage<TModel>>(message =>
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
                var resultItem = item.Retrieve(ResolveContext);
                Items.Add(resultItem);
            }

            return TaskDone.Done;
        }

        private Task ProcessModelItemRemoveMessage(ModelItemRemoveMessage<T> message)
        {
            foreach (var item in message.Items)
            {
                var resultItem = item.Retrieve(ResolveContext);
                Items.Remove(resultItem);
            }

            return TaskDone.Done;
        }
    }
}