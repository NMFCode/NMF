using System.Collections.Generic;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Streams;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Orleans
{
    internal sealed class IncrementalSelectNodeGrain<TSource, TResult> : IncrementalNodeGrainBase<TSource, TResult>,
        IIncrementalSelectNodeGrain<TSource, TResult>
    {
        private readonly Dictionary<TSource, TaggedObservableValue<TResult, ContainerElementReference<TResult>>> lambdas =
            new Dictionary<TSource, TaggedObservableValue<TResult, ContainerElementReference<TResult>>>();

        private ObservingFunc<TSource, TResult> _lambda;

        public Task SetObservingFunc(SerializableFunc<TSource, TResult> observingFunc)
        {
            _lambda = new ObservingFunc<TSource, TResult>(observingFunc.Value);
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
            var item = hostedItem.Item;
            InputList.Remove(hostedItem.Reference);
            if (lambdas.TryGetValue(item, out lambdaResult))
            {
                lambdas.Remove(item);
                lambdaResult.ValueChanged -= LambdaChanged;
                ResultElements.Remove(lambdaResult.Value);
                StreamSender.EnqueueRemoveItems(ResultElements[lambdaResult.Tag].SingleValueToList());
            }
        }

        private void DetachItem(TSource item, INotifyValue<TResult> lambdaValue)
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

        private TaggedObservableValue<TResult, ContainerElementReference<TResult>> AttachItem(ContainerElement<TSource> element)
        {
            TaggedObservableValue<TResult, ContainerElementReference<TResult>> lambdaResult;
            if (!lambdas.TryGetValue(element.Item, out lambdaResult))
            {
                lambdaResult = _lambda.InvokeTagged<ContainerElementReference<TResult>>(element.Item);
                var resultReference = ResultElements.Add(lambdaResult.Value);
                lambdaResult.Tag = resultReference;
                lambdaResult.ValueChanged += LambdaChanged;
                lambdas.Add(element.Item, lambdaResult);
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