using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Observable;
using Orleans.Streams;
using Orleans.Streams.Linq.Nodes;
using Orleans.Streams.Messages;
using SL = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Orleans
{
    internal sealed class ObservableSelectNodeGrain<TSource, TResult> : StreamProcessorNodeGrain<ContainerElement<TSource>, ContainerElement<TResult>>, IObservableSelectNodeGrain<TSource, TResult>
    {
        
        private ObservingFunc<TSource, TResult> lambda;

        public ObservingFunc<TSource, TResult> Lambda
        {
            get
            {
                return lambda;
            }
        }

        private DistributedPropertyChangedProcessor<ContainerElement<TSource>> _propertyChangedProcessor;
        private Dictionary<ContainerElementReference<TSource>,  TSource> _inputList;
        private ContainerElementList<TResult> _resultElements; 
        private Dictionary<TSource, TaggedObservableValue<TResult, ContainerElementReference<TResult>>> lambdas = new Dictionary<TSource, TaggedObservableValue<TResult, ContainerElementReference<TResult>>>();
        private Queue<IStreamMessage> _outputMessages;
        //private Dictionary<int, TSource> items = new Dictionary<int, TSource>();

        public override async Task OnActivateAsync()
        {
            _propertyChangedProcessor = new DistributedPropertyChangedProcessor<ContainerElement<TSource>>();
            _inputList = new Dictionary<ContainerElementReference<TSource>, TSource>();
            _resultElements = new ContainerElementList<TResult>(this.GetPrimaryKey(), null, null);
            _outputMessages = new Queue<IStreamMessage>();
            await base.OnActivateAsync();
        }

        private void DetachItem(TSource item, INotifyValue<TResult> lambdaValue)
        {
            lambdaValue.Detach();
        }

        protected override void RegisterMessages()
        {
            base.RegisterMessages();
            StreamMessageDispatchReceiver.Register<ItemPropertyChangedMessage>(_propertyChangedProcessor.ProcessItemPropertyChangedMessage);
        }

        protected override async Task ProcessItemMessage(ItemMessage<ContainerElement<TSource>> itemMessage)
        {
            await _propertyChangedProcessor.ProcessItemMessage(itemMessage);
            foreach (var item in itemMessage.Items)
            {
                await CalculateResult(item);
            }

            while (_outputMessages.Count > 0)
            {
                await StreamMessageSender.SendMessage(_outputMessages.Dequeue());
            }
        }



        private async Task CalculateResult(ContainerElement<TSource> hostedItem)
        {
            var itemKnown = _inputList.ContainsKey(hostedItem.Reference);
            
            // remove
            if (itemKnown && !hostedItem.Reference.Exists)
            {
                TaggedObservableValue<TResult, ContainerElementReference<TResult>> lambdaResult;
                var item = hostedItem.Item;
                _inputList.Remove(hostedItem.Reference);
                if (lambdas.TryGetValue(item, out lambdaResult))
                {
                        lambdas.Remove(item);
                        lambdaResult.PropertyChanged -= PropertyChanged;
                        var removedReference = _resultElements.Remove(lambdaResult.Value);
                        await StreamTransactionSender.SendItem(new ContainerElement<TResult>(removedReference, lambdaResult.Value));
                }
            }

            // Insert
            else if (!itemKnown)
            {
                var added = new List<TResult>();
                _inputList.Add(hostedItem.Reference, hostedItem.Item);
                var lambdaResult = AttachItem(hostedItem);
                //lambdaResult.ValueChanged += LambdaChanged;
                lambdaResult.PropertyChanged += PropertyChanged;
                //var elementReference = (await _resultElements.AddRange(new List<TResult> { lambdaResult.Value })).First();
                added.Add(lambdaResult.Value);
                await StreamTransactionSender.SendItem(new ContainerElement<TResult>(lambdaResult.Tag, lambdaResult.Value));
                //OnAddItems(added, e.NewStartingIndex);
            }

            else
            {
                _inputList[hostedItem.Reference] = hostedItem.Item;
            }
            // Update
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(sender as IContainerElementNotifyPropertyChanged, e); ;
        }

        private void OnPropertyChanged(IContainerElementNotifyPropertyChanged value, PropertyChangedEventArgs args)
        {
            var changeArgs = new ContainerElementPropertyChangedEventArgs(args.PropertyName, 42, value.Identifier);
            _outputMessages.Enqueue(new ItemPropertyChangedMessage(changeArgs));
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
                lambdaResult = lambda.InvokeTagged<ContainerElementReference<TResult>>(element.Item);
                var resultReference = _resultElements.AddRange(new List<TResult> {lambdaResult.Value}).First();
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
            //TResult oldResult = (TResult)e.OldValue;

            // Use tag to store item offset
            // TODO send property changed
            var changedResult = result as IContainerElementNotifyPropertyChanged;
//            _outputMessages.Enqueue(new ItemPropertyChangedMessage(new ContainerElementPropertyChangedEventArgs(e.)));
            //StreamTransactionSender.SendItem(new ContainerElement<TResult>(value.Tag, _resultElements[value.Tag]));
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

        public Task<Guid> EnumerateToStream(StreamIdentity streamIdentity, Guid transactionId)
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task<Guid> EnumerateToSubscribers(Guid? transactionId = null)
        {
            return await StreamTransactionSender.SendItems(_resultElements.ToList(), true, transactionId);
        }
    }
}
