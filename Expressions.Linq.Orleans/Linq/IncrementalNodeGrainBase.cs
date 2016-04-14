using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Messages;
using Orleans.Collections.Observable;
using Orleans.Streams;
using Orleans.Streams.Linq.Nodes;

namespace NMF.Expressions.Linq.Orleans
{
    public abstract class IncrementalNodeGrainBase<TSource, TResult> : StreamProcessorNodeGrain<ContainerElement<TSource>, ContainerElement<TResult>>
    {
        protected Dictionary<ContainerElementReference<TSource>, TSource> InputList;
        protected DistributedPropertyChangedProcessor<ContainerElement<TSource>> PropertyChangedProcessor;
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

        public virtual async Task<Guid> EnumerateToSubscribers(Guid? transactionId = null)
        {
            var tId = TransactionGenerator.GenerateTransactionId(transactionId);
            await StreamSender.StartTransaction(tId);
            await StreamSender.SendItems(ResultElements.ToList());
            await StreamSender.EndTransaction(tId);
            return tId;
        }

        protected override void RegisterMessages()
        {
            base.RegisterMessages();
            StreamMessageDispatchReceiver.Register<ItemUpdateMessage<ContainerElement<TSource>>>(ProcessItemUpdateMessage);
            StreamMessageDispatchReceiver.Register<ItemRemoveMessage<ContainerElement<TSource>>>(ProcessItemRemoveMessage);
            StreamMessageDispatchReceiver.Register<ItemPropertyChangedMessage>(PropertyChangedProcessor.ProcessItemPropertyChangedMessage);
            StreamMessageDispatchReceiver.Register<ItemPropertyChangedMessage>(message => StreamSender.FlushQueue());
        }

        protected override async Task ProcessItemAddMessage(ItemAddMessage<ContainerElement<TSource>> itemMessage)
        {
            await PropertyChangedProcessor.ProcessItemAddMessage(itemMessage);
            foreach (var item in itemMessage.Items)
            {
                if (InputList.ContainsKey(item.Reference))
                {
                    throw new InvalidOperationException("Cannot add item that is already added.");
                }
                InputList.Add(item.Reference, item.Item);
                InputItemAdded(item);
            }

            await StreamSender.FlushQueue();
        }

        protected abstract void InputItemAdded(ContainerElement<TSource> hostedItem);

        protected abstract void InputItemDeleted(ContainerElement<TSource> hostedItem);

        private async Task ProcessItemUpdateMessage(ItemUpdateMessage<ContainerElement<TSource>> message)
        {
            foreach (var item in message.Items)
            {
                if (!InputList.ContainsKey(item.Reference))
                {
                    throw new InvalidOperationException("Cannot update unknown item.");
                }
                InputList[item.Reference] = item.Item;
            }

            await StreamSender.FlushQueue();
        }

        private async Task ProcessItemRemoveMessage(ItemRemoveMessage<ContainerElement<TSource>> message)
        {
            foreach (var item in message.Items)
            {
                if (!InputList.ContainsKey(item.Reference))
                {
                    throw new InvalidOperationException("Cannot remove unknown item.");
                }
                InputList.Remove(item.Reference);
                InputItemDeleted(item);
            }

            await StreamSender.FlushQueue();
        }
    }
}