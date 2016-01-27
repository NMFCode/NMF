using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using SL = System.Linq.Enumerable;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal abstract class ObservableItemExpressionEnumerable<TSource, TLambdaResult, TResult> : ObservableEnumerable<TResult>
    {
        private INotifyEnumerable<TSource> source;
        private ObservingFunc<TSource, TLambdaResult> lambda;

        public ObservingFunc<TSource, TLambdaResult> Lambda
        {
            get
            {
                return lambda;
            }
        }

        protected INotifyEnumerable<TSource> Source
        {
            get
            {
                return source;
            }
        }

        private Dictionary<TSource, Stack<TaggedObservableValue<TLambdaResult, TResult>>> lambdas = new Dictionary<TSource, Stack<TaggedObservableValue<TLambdaResult, TResult>>>();

        public ObservableItemExpressionEnumerable(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TLambdaResult> lambda)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (lambda == null) throw new ArgumentNullException("lambda");

            this.source = source;
            this.lambda = lambda;
        }

        protected virtual void DetachItem(TSource item, INotifyValue<TLambdaResult> lambdaValue)
        {
            lambdaValue.Detach();
        }

        protected virtual bool ResetChangeIndex { get { return true; } }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move) return;
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                {
                    var removed = new List<TResult>();
                    foreach (TSource item in e.OldItems)
                    {
                        Stack<TaggedObservableValue<TLambdaResult, TResult>> lambdasForItem;
                        if (lambdas.TryGetValue(item, out lambdasForItem))
                        {
                            var lambdaResult = lambdasForItem.Pop();
                            if (lambdasForItem.Count == 0) lambdas.Remove(item);
                            lambdaResult.ValueChanged -= LambdaChanged;
                            DetachItem(item, lambdaResult);
                            if (RemoveItem(item, lambdaResult))
                            {
                                removed.Add(lambdaResult.Tag);
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    OnRemoveItems(removed, ResetChangeIndex ? 0 : e.OldStartingIndex);
                }
                if (e.NewItems != null)
                {
                    var added = new List<TResult>();
                    foreach (TSource item in e.NewItems)
                    {
                        var lambdaResult = AttachItem(item);
                        if (AddItem(item, lambdaResult))
                        {
                            added.Add(lambdaResult.Tag);
                        }
                    }
                    OnAddItems(added, ResetChangeIndex ? 0 : e.NewStartingIndex);
                }
            }
            else
            {
                DetachSource();
                OnCleared();
            }
        }

        protected abstract bool RemoveItem(TSource sourceItem, TaggedObservableValue<TLambdaResult, TResult> lambdaValue);

        protected abstract bool AddItem(TSource sourceItem, TaggedObservableValue<TLambdaResult, TResult> lambdaValue);

        private void DetachSource()
        {
            foreach (var pair in lambdas)
            {
                foreach (var value in pair.Value)
                {
                    DetachItem(pair.Key, value);
                }
            }
            lambdas.Clear(); 
        }

        private TaggedObservableValue<TLambdaResult, TResult> AttachItem(TSource item)
        {
            var lambdaResult = lambda.InvokeTagged<TResult>(item);
            lambdaResult.ValueChanged += LambdaChanged;
            Stack<TaggedObservableValue<TLambdaResult, TResult>> lambdaStack;
            if (!lambdas.TryGetValue(item, out lambdaStack))
            {
                lambdaStack = new Stack<TaggedObservableValue<TLambdaResult, TResult>>();
                lambdas.Add(item, lambdaStack);
            }
            lambdaStack.Push(lambdaResult);
            lambdaResult.Tag = GetResult(item, lambdaResult.Value);
            AttachItem(item, lambdaResult);
            return lambdaResult;
        }

        protected virtual void AttachItem(TSource sourceItem, TaggedObservableValue<TLambdaResult, TResult> lambdaValue) { }

        private void LambdaChanged(object sender, ValueChangedEventArgs e)
        {
            OnLambdaValueChanged(sender as TaggedObservableValue<TLambdaResult, TResult>, e);
        }

        protected abstract void OnLambdaValueChanged(TaggedObservableValue<TLambdaResult, TResult> value, ValueChangedEventArgs e);

        protected abstract TResult GetResult(TSource sourceItem, TLambdaResult lambdaResult);

        protected IDictionary<TSource, Stack<TaggedObservableValue<TLambdaResult, TResult>>> Lambdas
        {
            get
            {
                return lambdas;
            }
        }

        protected override void AttachCore()
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    AttachItem(item);
                }
                source.CollectionChanged += SourceCollectionChanged;
            }
        }

        protected override void DetachCore()
        {
            DetachSource();
            Source.CollectionChanged -= SourceCollectionChanged;
        }
    }
}
