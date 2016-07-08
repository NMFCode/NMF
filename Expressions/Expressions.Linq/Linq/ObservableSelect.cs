using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Linq.Expressions;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableSelect<TSource, TResult> : ObservableEnumerable<TResult>
    {
        
        private INotifyEnumerable<TSource> source;
        private ObservingFunc<TSource, TResult> lambda;
        private TaggedObservableValue<TResult, int> nullLambda;

        public ObservingFunc<TSource, TResult> Lambda
        {
            get
            {
                return lambda;
            }
        }

        private INotifyEnumerable<TSource> Source
        {
            get
            {
                return source;
            }
        }

        private Dictionary<TSource, TaggedObservableValue<TResult, int>> lambdas = new Dictionary<TSource, TaggedObservableValue<TResult, int>>();

        public ObservableSelect(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TResult> lambda)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (lambda == null) throw new ArgumentNullException("lambda");

            this.source = source;
            this.lambda = lambda;

            Attach();
        }

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
                        if (item != null)
                        {
                            TaggedObservableValue<TResult, int> lambdaResult;
                            if (lambdas.TryGetValue(item, out lambdaResult))
                            {
                                lambdaResult.Tag--;
                                if (lambdaResult.Tag == 0)
                                {
                                    lambdas.Remove(item);
                                    lambdaResult.ValueChanged -= LambdaChanged;
                                    lambdaResult.Detach();
                                }
                                removed.Add(lambdaResult.Value);
                            }
                            else
                            {
                                throw new InvalidOperationException();
                            }
                        }
                        else if (nullLambda != null)
                        {
                            nullLambda.Tag--;
                            if (nullLambda.Tag == 0)
                            {
                                nullLambda.ValueChanged -= LambdaChanged;
                                nullLambda.Detach();
                                nullLambda = null;
                            }
                        }
                    }
                    OnRemoveItems(removed, e.OldStartingIndex);
                }
                if (e.NewItems != null)
                {
                    var added = new List<TResult>();
                    foreach (TSource item in e.NewItems)
                    {
                        var lambdaResult = AttachItem(item);
                        added.Add(lambdaResult.Value);
                    }
                    OnAddItems(added, e.NewStartingIndex);
                }
            }
            else
            {
                DetachSource();
                OnCleared();
            }
        }

        private void DetachSource()
        {
            foreach (var value in lambdas.Values)
            {
                value.Detach();
            }
            lambdas.Clear(); 
        }

        private TaggedObservableValue<TResult, int> AttachItem(TSource item)
        {
            if (item != null)
            {
                TaggedObservableValue<TResult, int> lambdaResult;
                if (!lambdas.TryGetValue(item, out lambdaResult))
                {
                    lambdaResult = lambda.InvokeTagged<int>(item, 0);
                    lambdaResult.ValueChanged += LambdaChanged;
                    lambdas.Add(item, lambdaResult);
                }
                lambdaResult.Tag++;
                return lambdaResult;
            }
            else
            {
                if (nullLambda == null)
                {
                    nullLambda = lambda.InvokeTagged(item, 0);
                    nullLambda.ValueChanged += LambdaChanged;
                }
                nullLambda.Tag++;
                return nullLambda;
            }
        }

        private void LambdaChanged(object sender, ValueChangedEventArgs e)
        {
            OnLambdaValueChanged(sender as TaggedObservableValue<TResult, int>, e);
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

        private void OnLambdaValueChanged(TaggedObservableValue<TResult, int> value, ValueChangedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");
            TResult result = (TResult)e.NewValue;
            TResult oldResult = (TResult)e.OldValue;
            for (int i = 0; i < value.Tag; i++)
            {
                OnUpdateItem(result, oldResult);
            }
        }

        public override IEnumerator<TResult> GetEnumerator()
        {
            return ItemsInternal.GetEnumerator();
        }

        private IEnumerable<TResult> ItemsInternal
        {
            get
            {
                foreach (var item in Source)
                {
                    TaggedObservableValue<TResult, int> lambdaResult;
                    if (lambdas.TryGetValue(item, out lambdaResult))
                    {
                        yield return lambdaResult.Value;
                    }
                }
            }
        }

        public override int Count
        {
            get
            {
                return SL.Sum(lambdas.Values, item => item.Tag);
            }
        }
    }
}
