using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Messages;
using Orleans.Streams;
using Orleans.Streams.Linq.Nodes;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans
{
    /// <summary>
    /// Abstract base class for SQOs with incremental operations.
    /// </summary>
    /// <typeparam name="TSource">Type of data to deal with. Assume data is wrapped in ContainerElement.</typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public abstract class IncrementalNodeGrainBase<TSource, TResult, TModel> : ModelProcessingNodeGrain<TSource, TResult, TModel>, IElementEnumeratorNode<TResult> where TModel : IResolvableModel
    {
        protected NotifyCollection<TSource> InputList;
        protected INotifyEnumerable<TResult> ResultEnumerable;

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            InputList = new NotifyCollection<TSource>();
        }

        protected void AttachToResult()
        {
            ResultEnumerable.CollectionChanged += ResultEnumerable_CollectionChanged;
        }

        private void ResultEnumerable_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var items = new List<IModelRemoteValue<TResult>>();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        items.Add(ModelRemoteValueFactory.CreateModelChangeValue<TResult>((TResult) newItem));
                    }
                    StreamSender.EnqueueAddModelItems(items);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var removeItem in e.OldItems)
                    {
                        items.Add(ModelRemoteValueFactory.CreateModelChangeValue<TResult>((TResult)removeItem));
                    }
                    StreamSender.EnqueueRemoveModelItems(items);
                    break;
                default:
                    throw new NotImplementedException();

            }
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
            await StreamSender.SendItems(ResultEnumerable);
            await StreamSender.EndTransaction(tId);
            return tId;
        }

        protected override void RegisterMessages()
        {
            base.RegisterMessages();
            StreamConsumer.MessageDispatcher.Register<ModelItemAddMessage<TSource>>(ProcessItemAddMessage);
            StreamConsumer.MessageDispatcher.Register<ModelItemRemoveMessage<TSource>>(ProcessItemRemoveMessage);
        }

        protected async Task ProcessItemAddMessage(ModelItemAddMessage<TSource> itemMessage)
        {
            foreach (var item in itemMessage.Items)
            {
                var localItem = item.Retrieve(ResolveContext);

                if (item.GlobalIdentifier != null && !ResolveContext.ObjectLookup.ContainsKey(item.GlobalIdentifier))
                    ResolveContext.ObjectLookup.Add(item.GlobalIdentifier, localItem);

                InputList.Add(localItem);
            }

            await StreamSender.FlushQueue();
        }

        private async Task ProcessItemRemoveMessage(ModelItemRemoveMessage<TSource> message)
        {
            foreach (var item in message.Items)
            {
                var localItem = item.Retrieve(ResolveContext);

                if (item.GlobalIdentifier != null && ResolveContext.ObjectLookup.ContainsKey(item.GlobalIdentifier))
                    ResolveContext.ObjectLookup.Remove(item.GlobalIdentifier);

                InputList.Remove(localItem);
            }

            await StreamSender.FlushQueue();
        }
    }
}