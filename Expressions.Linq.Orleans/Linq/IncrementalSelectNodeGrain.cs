using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Orleans
{
    internal sealed class IncrementalSelectNodeGrain<TSource, TResult> : IncrementalNodeGrainBase<ContainerElement<TSource>, ContainerElement<TResult>, TSource, TResult>,
        IIncrementalSelectNodeGrain<TSource, TResult>
    {
        private readonly Dictionary<ContainerElement<TSource>, TaggedObservableValue<TResult, ContainerElementReference<TResult>>> lambdas = new Dictionary<ContainerElement<TSource>, TaggedObservableValue<TResult, ContainerElementReference<TResult>>>();

        private ObservingFunc<ContainerElement<TSource>, TResult> _lambda;

        public Task SetObservingFunc(SerializableFunc<ContainerElement<TSource>, TResult> observingFunc)
        {
            _lambda = new ObservingFunc<ContainerElement<TSource>, TResult>(observingFunc.Value);
            return TaskDone.Done;
        }

        public override Task TearDown()
        {
            DetachSource();
            return base.TearDown();
        }

        protected override void InputItemAdded(ContainerElement<TSource> hostedItem)
        {
            var lambdaResult = AttachItem(hostedItem);
            StreamSender.EnqueueAddItems(new ContainerElement<TResult>(lambdaResult.Tag, lambdaResult.Value).SingleValueToList());
        }

        protected override void InputItemDeleted(ContainerElement<TSource> hostedItem)
        {
            TaggedObservableValue<TResult, ContainerElementReference<TResult>> lambdaResult;
            InputList.Remove(hostedItem.Reference);
            if (lambdas.TryGetValue(hostedItem, out lambdaResult))
            {
                lambdas.Remove(hostedItem);
                lambdaResult.ValueChanged -= LambdaChanged;
                ResultElements.Remove(lambdaResult.Value);
                StreamSender.EnqueueRemoveItems(ResultElements[lambdaResult.Tag].SingleValueToList());
            }
        }

        private void DetachItem(ContainerElement<TSource> item, INotifyValue<TResult> lambdaValue)
        {
            lambdaValue.Detach();
        }

        private void DetachSource()
        {
            foreach (var pair in lambdas)
            {
                DetachItem(pair.Key, pair.Value);
            }
            lambdas.Clear();
        }

        private TaggedObservableValue<TResult, ContainerElementReference<TResult>> AttachItem(ContainerElement<TSource> hostedItem)
        {
            TaggedObservableValue<TResult, ContainerElementReference<TResult>> lambdaResult;
            if (!lambdas.TryGetValue(hostedItem, out lambdaResult))
            {
                lambdaResult = _lambda.InvokeTagged<ContainerElementReference<TResult>>(hostedItem);
                var resultReference = ResultElements.Add(lambdaResult.Value);
                lambdaResult.Tag = resultReference;
                lambdaResult.ValueChanged += LambdaChanged;
                lambdas.Add(hostedItem, lambdaResult);
            }

            return lambdaResult;
        }

        private void LambdaChanged(object sender, ValueChangedEventArgs e)
        {
            OnLambdaValueChanged(sender as TaggedObservableValue<TResult, ContainerElementReference<TResult>>, e);
        }

        private void OnLambdaValueChanged(TaggedObservableValue<TResult, ContainerElementReference<TResult>> value, ValueChangedEventArgs e)
        {
            ResultElements.SetElement(value.Tag, (TResult) e.NewValue);
            StreamSender.EnqueueUpdateItems(ResultElements[value.Tag].SingleValueToList());
        }
    }
}