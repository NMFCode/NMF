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
        private Dictionary<TSource, TaggedObservableValue<TResult, int>> lambdaInstances = new Dictionary<TSource, TaggedObservableValue<TResult, int>>();

        public ObservingFunc<TSource, TResult> Lambda { get { return lambda; } }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                if (nullLambda != null)
                    yield return nullLambda;
                foreach (var child in lambdaInstances.Values)
                    yield return child;
            }
        }

        public ObservableSelect(INotifyEnumerable<TSource> source, ObservingFunc<TSource, TResult> lambda)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (lambda == null) throw new ArgumentNullException("lambda");

            this.source = source;
            this.lambda = lambda;
        }

        private TaggedObservableValue<TResult, int> AttachItem(TSource item)
        {
            if (item != null)
            {
                TaggedObservableValue<TResult, int> lambdaResult;
                if (!lambdaInstances.TryGetValue(item, out lambdaResult))
                {
                    lambdaResult = lambda.InvokeTagged(item, 0);
                    lambdaResult.Successors.Add(this);
                    lambdaInstances.Add(item, lambdaResult);
                }
                lambdaResult.Tag++;
                return lambdaResult;
            }
            else
            {
                if (nullLambda == null)
                {
                    nullLambda = lambda.InvokeTagged(item, 0);
                    nullLambda.Successors.Add(this);
                }
                nullLambda.Tag++;
                return nullLambda;
            }
        }

        protected override void OnAttach()
        {
            foreach (var item in source)
            {
                AttachItem(item);
            }
        }

        protected override void OnDetach()
        {
            foreach (var item in lambdaInstances.Values)
                item.Successors.Remove(this);
            lambdaInstances.Clear();
        }
        
        public override IEnumerator<TResult> GetEnumerator()
        {
            return ItemsInternal.GetEnumerator();
        }

        private IEnumerable<TResult> ItemsInternal
        {
            get
            {
                foreach (var item in source)
                {
                    TaggedObservableValue<TResult, int> lambdaResult;
                    if (lambdaInstances.TryGetValue(item, out lambdaResult))
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
                return SL.Sum(lambdaInstances.Values, item => item.Tag);
            }
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var added = new List<TResult>();
            var removed = new List<TResult>();
            var replaceAdded = new List<TResult>();
            var replaceRemoved = new List<TResult>();

            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var sourceChange = (CollectionChangedNotificationResult<TSource>)change;
                    if (sourceChange.IsReset)
                    {
                        foreach (var item in lambdaInstances.Values)
                            item.Successors.Remove(this);
                        lambdaInstances.Clear();
                        foreach (var item in source)
                            AttachItem(item);
                        OnCleared();
                        return new CollectionChangedNotificationResult<TResult>(this);
                    }
                    else
                    {
                        NotifySource(sourceChange, added, removed);
                    }
                }
                else
                {
                    var itemChange = (ValueChangedNotificationResult<TResult>)change;
                    replaceRemoved.Add(itemChange.OldValue);
                    replaceAdded.Add(itemChange.NewValue);
                }
            }

            OnRemoveItems(removed);
            OnAddItems(added);
            OnReplaceItems(replaceRemoved, replaceAdded);
            return new CollectionChangedNotificationResult<TResult>(this, added, removed, replaceAdded, replaceRemoved);
        }

        private void NotifySource(CollectionChangedNotificationResult<TSource> sourceChange, List<TResult> added, List<TResult> removed)
        {
            foreach (var item in sourceChange.AllRemovedItems)
            {
                if (item != null)
                {
                    var lambdaResult = lambdaInstances[item];
                    lambdaResult.Tag--;
                    if (lambdaResult.Tag == 0)
                    {
                        lambdaInstances.Remove(item);
                        lambdaResult.Successors.Remove(this);
                    }
                    removed.Add(lambdaResult.Value);
                }
                else if (nullLambda != null)
                {
                    nullLambda.Tag--;
                    if (nullLambda.Tag == 0)
                    {
                        nullLambda.Successors.Remove(this);
                        nullLambda = null;
                    }
                }
            }
                
            foreach (var item in sourceChange.AllAddedItems)
            {
                var lambdaResult = AttachItem(item);
                added.Add(lambdaResult.Value);
            }
        }
    }
}
