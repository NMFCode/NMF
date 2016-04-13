using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Observable;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.Streams.Linq.Nodes;
using Orleans.Streams.Messages;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Orleans
{
    internal sealed class ObservableSelectNodeGrain<TSource, TResult> : ObservableNodeGrainBase<TSource, TResult>, IObservableSelectNodeGrain<TSource, TResult>
    {

        private ObservingFunc<TSource, TResult> _lambda;
        public ObservingFunc<TSource, TResult> Lambda => _lambda;

        private Dictionary<TSource, TaggedObservableValue<TResult, ContainerElementReference<TResult>>> lambdas = new Dictionary<TSource, TaggedObservableValue<TResult, ContainerElementReference<TResult>>>();


        private void DetachItem(TSource item, INotifyValue<TResult> lambdaValue)
        {
            lambdaValue.Detach();
        }

        protected override void ItemAdded(ContainerElement<TSource> hostedItem)
        {
            var lambdaResult = AttachItem(hostedItem);
            StreamTransactionSender.EnqueueItemsForSending(new ContainerElement<TResult>(lambdaResult.Tag, lambdaResult.Value));
        }

        protected override void ItemDeleted(ContainerElement<TSource> hostedItem)
        {
            TaggedObservableValue<TResult, ContainerElementReference<TResult>> lambdaResult;
            var item = hostedItem.Item;
            InputList.Remove(hostedItem.Reference);
            if (lambdas.TryGetValue(item, out lambdaResult))
            {
                lambdas.Remove(item);
                lambdaResult.ValueChanged -= LambdaChanged;
                var removedReference = ResultElements.Remove(lambdaResult.Value);
                StreamTransactionSender.EnqueueItemsForSending(new ContainerElement<TResult>(removedReference, lambdaResult.Value));
            }
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

        private TaggedObservableValue<TResult, ContainerElementReference<TResult>> AttachItem(ContainerElement<TSource> element)
        {
            TaggedObservableValue<TResult, ContainerElementReference<TResult>> lambdaResult;
            if (!lambdas.TryGetValue(element.Item, out lambdaResult))
            {
                lambdaResult = _lambda.InvokeTagged<ContainerElementReference<TResult>>(element.Item);
                var resultReference = ResultElements.AddRange(new List<TResult> { lambdaResult.Value }).First();
                lambdaResult.Tag = resultReference;
                lambdaResult.ValueChanged += LambdaChanged;
                lambdas.Add(element.Item, lambdaResult);
            }
            //lambdaResult.Tag++;
            return lambdaResult;
        }

        private void LambdaChanged(object sender, ValueChangedEventArgs e)
        {
            OnLambdaValueChanged(sender as TaggedObservableValue<TResult, ContainerElementReference<TResult>>, e);
        }

        private void OnLambdaValueChanged(TaggedObservableValue<TResult, ContainerElementReference<TResult>> value, ValueChangedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");
            TResult result = (TResult)e.NewValue;
            ResultElements.SetElement(value.Tag, (TResult) e.NewValue);
            StreamTransactionSender.EnqueueItemsForSending(ResultElements[value.Tag]);
        }

        public Task SetObservingFunc(SerializableFunc<TSource, TResult> observingFunc)
        {
            _lambda = new ObservingFunc<TSource, TResult>(observingFunc.Value);
            return TaskDone.Done;
        }

    }
}
