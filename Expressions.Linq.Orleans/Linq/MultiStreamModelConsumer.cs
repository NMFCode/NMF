using System;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Models;
using Orleans;
using Orleans.Streams;
using Orleans.Streams.Endpoints;

namespace NMF.Expressions.Linq.Orleans
{
    public class MultiStreamModelConsumer<T, TModel> : MultiStreamListConsumer<T> where TModel : IResolvableModel
    {
        protected TModel Model;

        public MultiStreamModelConsumer(IStreamProvider streamProvider, Func<TModel> modelLoadingFunc) : base(streamProvider)
        {
            Model = modelLoadingFunc();
        }

        protected override void SetupMessageDispatcher(StreamMessageDispatchReceiver dispatcher)
        {
            base.SetupMessageDispatcher(dispatcher);
            dispatcher.Register<ModelItemAddMessage<T>>(ProcessModelItemAddMessage);
        }

        private Task ProcessModelItemAddMessage(ModelItemAddMessage<T> message)
        {
            foreach (var item in message.Items)
            {
                var resultItem = item.Retrieve(Model);
                Items.Add(resultItem);
            }

            return TaskDone.Done;
        }
    }
}