using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;

namespace NMF.Expressions.Linq.Orleans
{
    public class IncrementalWhereNodeGrain<TSource> : IncrementalNodeGrainBase<TSource, TSource>, IIncrementalWhereNodeGrain<TSource>
    {
        private SerializableFunc<TSource, bool> _observingFunc;

        private Dictionary<ContainerElementReference<TSource>, TaggedObservableValue<bool, ContainerElement<TSource>>> lambdas = new Dictionary<ContainerElementReference<TSource>, TaggedObservableValue<bool, ContainerElement<TSource>>>();
        private ObservingFunc<TSource, bool> _lambda;

        protected override void InputItemAdded(ContainerElement<TSource> hostedItem)
        {
            AttachItem(hostedItem);
        }

        protected override void InputItemDeleted(ContainerElement<TSource> hostedItem)
        {
            TaggedObservableValue<bool, ContainerElement<TSource>> lambdaResult;
            var item = hostedItem.Item;
            InputList.Remove(hostedItem.Reference);
            if (lambdas.TryGetValue(hostedItem.Reference, out lambdaResult))
            {
                lambdas.Remove(hostedItem.Reference);
                lambdaResult.ValueChanged -= LambdaChanged;
                lambdaResult.Detach(); // TODO test and copy to select node grain
                if (lambdaResult.Value)
                {
                    hostedItem.Reference.Exists = false;
                    StreamTransactionSender.EnqueueItemsForSending(hostedItem);
                }
            }
        }

        private void DetachItem(ContainerElementReference<TSource> item, INotifyValue<bool> lambdaValue)
        {
            lambdaValue.Detach();
        }

        public override Task TearDown()
        {
            DetachSource();
            return base.TearDown();
        }

        private void DetachSource()
        {
            foreach (var pair in lambdas)
            {
                DetachItem(pair.Key, pair.Value);
            }
            lambdas.Clear();
        }

        private TaggedObservableValue<bool, ContainerElement<TSource>> AttachItem(ContainerElement<TSource> element)
        {
            TaggedObservableValue<bool, ContainerElement<TSource>> lambdaResult;
            if (!lambdas.TryGetValue(element.Reference, out lambdaResult))
            {
                lambdaResult = _lambda.InvokeTagged<ContainerElement<TSource>>(element.Item, element);
                if (lambdaResult.Value)
                {
                    StreamTransactionSender.EnqueueItemsForSending(lambdaResult.Tag);
                }
                lambdaResult.ValueChanged += LambdaChanged;
                lambdas.Add(element.Reference, lambdaResult);
            }

            return lambdaResult;
        }

        public override Task<Guid> EnumerateToSubscribers(Guid? transactionId = null)
        {
            var itemsToSend = lambdas.Values.Where(i => i.Value).Select(i => i.Tag).ToList();
            return StreamTransactionSender.SendItems(itemsToSend, true, transactionId);
        }

        private void LambdaChanged(object sender, ValueChangedEventArgs e)
        {
            OnLambdaValueChanged(sender as TaggedObservableValue<bool, ContainerElement<TSource>>, e);
        }

        private void OnLambdaValueChanged(TaggedObservableValue<bool, ContainerElement<TSource>> value, ValueChangedEventArgs e)
        {
            if (value.Value)
            {
                StreamTransactionSender.EnqueueItemsForSending(value.Tag);
            }
            else
            {
                value.Tag.Reference.Exists = false;
                StreamTransactionSender.EnqueueItemsForSending(value.Tag);
                value.Tag.Reference.Exists = true;
            }
        }

        public Task SetObservingFunc(SerializableFunc<TSource, bool> observingFunc)
        {
            _lambda = new ObservingFunc<TSource, bool>(observingFunc.Value);
            return TaskDone.Done;
        }
    }
}