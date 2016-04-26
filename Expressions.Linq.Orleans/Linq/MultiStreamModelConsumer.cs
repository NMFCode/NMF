using System;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Models;
using Orleans;
using Orleans.Streams;
using Orleans.Streams.Endpoints;

namespace NMF.Expressions.Linq.Orleans
{
    public class MultiStreamModelConsumer<T> : MultiStreamListConsumer<T> where T : IModelElement
    {
        protected Models.Model Model;

        public MultiStreamModelConsumer(IStreamProvider streamProvider, Func<Models.Model> modelLoadingFunc) : base(streamProvider)
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