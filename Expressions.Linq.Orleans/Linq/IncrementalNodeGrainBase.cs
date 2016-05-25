using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans.Streams;
using Orleans.Streams.Messages;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans
{
    /// <summary>
    /// Abstract base class for SQOs with incremental operations.
    /// </summary>
    /// <typeparam name="TSource">Type of data to deal with.</typeparam>
    /// <typeparam name="TResult">Type of result data.</typeparam>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    public abstract class IncrementalNodeGrainBase<TSource, TResult, TModel> : ModelProcessingNodeGrain<TSource, TResult, TModel>, IElementEnumeratorNode<TResult> where TModel : IResolvableModel
    {
        protected NotifyCollection<TSource> InputList;
        protected INotifyEnumerable<TResult> ResultEnumerable;

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            InputList = new NotifyCollection<TSource>();
            StreamConsumer = new TransactionalStreamModelConsumer<TSource, TModel>(GetStreamProvider(StreamProviderNamespace), TearDown, InputList);

            StreamConsumer.MessageDispatcher.Register<FlushMessage>(ProcessFlushMessage);
            StreamConsumer.MessageDispatcher.Register<TransactionMessage>(ProcessTransactionMessage);
        }


        protected void AttachToResult()
        {
            ResultEnumerable.CollectionChanged += ResultEnumerable_CollectionChanged;
            // TODO maybe add detach method
        }

        private void ResultEnumerable_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var items = new List<IObjectRemoteValue<TResult>>();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        items.Add(ModelRemoteValueFactory.CreateModelRemoteValue<TResult>((TResult) newItem, SendContext));
                    }
                    StreamSender.EnqueueAddRemoteItems(items);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var removeItem in e.OldItems)
                    {
                        items.Add(ModelRemoteValueFactory.CreateModelRemoteValue<TResult>((TResult)removeItem, SendContext));
                    }
                    StreamSender.EnqueueRemoveRemoteItems(items);
                    break;
                default:
                    throw new NotImplementedException();

            }
        }

        public Task<Guid> EnumerateToStream(StreamIdentity streamIdentity, Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<Guid> EnumerateToSubscribers(Guid? transactionId = null)
        {
            //var tId = TransactionGenerator.GenerateTransactionId(transactionId);
            //await StreamSender.StartTransaction(tId);
            //await StreamSender.SendItems(ResultEnumerable);
            //await StreamSender.EndTransaction(tId);
            //return tId;
            throw new NotImplementedException();
        }

        protected async Task ProcessTransactionMessage(TransactionMessage transactionMessage)
        {
            if(transactionMessage.State == TransactionState.End)
                await StreamSender.AwaitSendingComplete();

            await StreamSender.SendMessageBroadcast(transactionMessage);
        }

        private async Task ProcessFlushMessage(FlushMessage message)
        {
            await StreamSender.AwaitSendingComplete();
            await StreamSender.SendMessageBroadcast(message);
        }

    }
}