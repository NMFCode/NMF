using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Observable;
using Orleans.Streams;
using Orleans.Streams.Linq.Nodes;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans
{
    public abstract class ObservableNodeGrainBase<TSource, TResult> : StreamProcessorNodeGrain<ContainerElement<TSource>, ContainerElement<TResult>>
    {
        protected DistributedPropertyChangedProcessor<ContainerElement<TSource>> PropertyChangedProcessor;
        protected Dictionary<ContainerElementReference<TSource>, TSource> InputList;
        protected ContainerElementList<TResult> ResultElements;

        public override async Task OnActivateAsync()
        {
            PropertyChangedProcessor = new DistributedPropertyChangedProcessor<ContainerElement<TSource>>();
            InputList = new Dictionary<ContainerElementReference<TSource>, TSource>();
            ResultElements = new ContainerElementList<TResult>(this.GetPrimaryKey(), null, null);
            await base.OnActivateAsync();
        }

        public Task<Guid> EnumerateToStream(StreamIdentity streamIdentity, Guid transactionId)
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task<Guid> EnumerateToSubscribers(Guid? transactionId = null)
        {
            return await StreamTransactionSender.SendItems(ResultElements.ToList(), true, transactionId);
        }

        protected override void RegisterMessages()
        {
            base.RegisterMessages();
            StreamMessageDispatchReceiver.Register<ItemPropertyChangedMessage>(PropertyChangedProcessor.ProcessItemPropertyChangedMessage);
            StreamMessageDispatchReceiver.Register<ItemPropertyChangedMessage>(message => StreamMessageSender.SendMessagesFromQueue());
        }

        private void ProcessItem(ContainerElement<TSource> hostedItem)
        {
            var itemKnown = InputList.ContainsKey(hostedItem.Reference);

            // remove
            if (itemKnown && !hostedItem.Reference.Exists)
            {
                InputList.Remove(hostedItem.Reference);
                ItemDeleted(hostedItem);
            }

            // Insert
            else if (!itemKnown)
            {
                InputList.Add(hostedItem.Reference, hostedItem.Item);
                ItemAdded(hostedItem);
            }

            // Update
            else
            {
                InputList[hostedItem.Reference] = hostedItem.Item;
            }
        }

        protected abstract void ItemAdded(ContainerElement<TSource> hostedItem);

        protected abstract void ItemDeleted(ContainerElement<TSource> hostedItem);

        protected override async Task ProcessItemMessage(ItemMessage<ContainerElement<TSource>> itemMessage)
        {
            await PropertyChangedProcessor.ProcessItemMessage(itemMessage);
            foreach (var item in itemMessage.Items)
            {
                ProcessItem(item);
            }

            await StreamMessageSender.SendMessagesFromQueue();
        }
    }
}