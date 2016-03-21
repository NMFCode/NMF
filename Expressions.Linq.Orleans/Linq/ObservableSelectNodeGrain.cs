using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Observable;
using Orleans.Streams;
using Orleans.Streams.Linq.Nodes;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Orleans
{
    internal sealed class ObservableSelectNodeGrain<TSource, TResult> : StreamProcessorNodeGrain<ContainerHostedElement<TSource>, ContainerHostedElement<TResult>>, IObservableSelectNodeGrain<TSource, TResult>
    {
        
        private ObservingFunc<TSource, TResult> lambda;

        public ObservingFunc<TSource, TResult> Lambda
        {
            get
            {
                return lambda;
            }
        }

        private Dictionary<TSource, TaggedObservableValue<TResult, int>> lambdas = new Dictionary<TSource, TaggedObservableValue<TResult, int>>();
        private Dictionary<int, TSource> items = new Dictionary<int, TSource>();

        private List<ContainerHostedElement<TResult>> OutputMessages = new List<ContainerHostedElement<TResult>>();

        private void DetachItem(TSource item, INotifyValue<TResult> lambdaValue)
        {
            lambdaValue.Detach();
        }

        protected override async Task ItemArrived(IEnumerable<ContainerHostedElement<TSource>> items)
        {
            foreach (var item in items)
            {
                ProcessArrivedItem(item);
            }

            await ProcessOutputMessages();
        }

        private async Task ProcessOutputMessages()
        {
            await StreamProvider.SendItems(OutputMessages, false);
            OutputMessages.Clear();
        }

        private void ProcessArrivedItem(ContainerHostedElement<TSource> hostedItem)
        {
            var itemKnown = items.ContainsKey(hostedItem.Reference.Offset);
            
            // remove
            if (itemKnown && !hostedItem.Reference.Exists)
            {
                TaggedObservableValue<TResult, int> lambdaResult;
                var item = items[hostedItem.Reference.Offset];
                if (lambdas.TryGetValue(item, out lambdaResult))
                {
                        lambdas.Remove(item);
                        lambdaResult.ValueChanged -= LambdaChanged;
                        OutputMessages.Add(CreateElementReference(hostedItem.Reference.Offset, false));
                        //removed.Add(lambdaResult.Value);
                }
            }

            // Insert
            else if (!itemKnown)
            {
                items[hostedItem.Reference.Offset] = hostedItem.Item;
                var added = new List<TResult>();
                var lambdaResult = AttachItem(hostedItem.Item, hostedItem.Reference.Offset);
                added.Add(lambdaResult.Value);
                OutputMessages.Add(CreateElementReference(hostedItem.Reference.Offset, false));
                //OnAddItems(added, e.NewStartingIndex);
            }

            // Update
            else
            {
                var newItem = hostedItem.Item;
                var oldItem = items[hostedItem.Reference.Offset];

                // if type of INotifyPropertyChanged then find and merge differences.

                items[hostedItem.Reference.Offset] = hostedItem.Item;
                // INotifyExpression.ApplyParameters() without recreating?
            }
        }

        private ContainerHostedElement<TResult> CreateElementReference(int offset, bool exists = true)
        {
            TaggedObservableValue<TResult, int> lambdaResult;
            if (lambdas.TryGetValue(items[offset], out lambdaResult))
            {
                return new ContainerHostedElement<TResult>(
                    new ContainerElementReference<TResult>(this.GetPrimaryKey(), offset, null, null, exists),
                    lambdaResult.Value);
            }
            else
            {
                return null;
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

        private TaggedObservableValue<TResult, int> AttachItem(TSource item, int offset)
        {
            TaggedObservableValue<TResult, int> lambdaResult;
            if (!lambdas.TryGetValue(item, out lambdaResult))
            {
                lambdaResult = lambda.InvokeTagged<int>(item, offset);
                lambdaResult.ValueChanged += LambdaChanged;
                lambdas.Add(item, lambdaResult);
            }
            //lambdaResult.Tag++;
            return lambdaResult;
        }

        private void LambdaChanged(object sender, ValueChangedEventArgs e)
        {
            OnLambdaValueChanged(sender as TaggedObservableValue<TResult, int>, e);
        }

        private void OnLambdaValueChanged(TaggedObservableValue<TResult, int> value, ValueChangedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");
            TResult result = (TResult)e.NewValue;
            TResult oldResult = (TResult)e.OldValue;

            // Use tag to store item offset
            // TODO discuss with Georg
            OutputMessages.Add(CreateElementReference(value.Tag));
            // why multiple times?
            //for (int i = 0; i < value.Tag; i++)
            //{
            //    OnUpdateItem(result, oldResult);
            //}
        }

        public Task SetObservingFunc(Func<TSource, TResult> observingFunc)
        {
            Expression<Func<TSource, TResult>> observingExpression = x => observingFunc(x);
            lambda = new ObservingFunc<TSource, TResult>(observingExpression);
            return TaskDone.Done;
        }
    }
}
