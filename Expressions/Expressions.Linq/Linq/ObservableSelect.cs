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
        public override string ToString()
        {
            return "[Select]";
        }

        private readonly INotifyEnumerable<TSource> source;
        private readonly ObservingFunc<TSource, TResult> lambda;
        private TaggedObservableValue<TResult, int> nullLambda;
        private readonly Dictionary<TSource, TaggedObservableValue<TResult, int>> lambdaInstances = new Dictionary<TSource, TaggedObservableValue<TResult, int>>();

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

        public IEnumerable<INotifiable> LambdaInstances
        {
            get
            {
                if (nullLambda != null)
                {
                    yield return nullLambda;
                }
                foreach(var child in lambdaInstances.Values)
                {
                    yield return child;
                }
            }
        }

        private TaggedObservableValue<TResult, int> AttachItem(TSource item)
        {
            if (item != null)
            {
                TaggedObservableValue<TResult, int> lambdaResult;
                if (!lambdaInstances.TryGetValue(item, out lambdaResult))
                {
                    lambdaResult = lambda.InvokeTagged(item, 0);
                    lambdaResult.Successors.Set(this);
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
                    nullLambda.Successors.Set(this);
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
                item.Successors.Unset(this);
            lambdaInstances.Clear();
        }

        public override IEnumerator<TResult> GetEnumerator()
        {
            if (ObservableExtensions.KeepOrder)
            {
                return ItemsInternal.GetEnumerator();
            }
            else
            {
                return ItemsUnordered.GetEnumerator();
            }
        }

        private IEnumerable<TResult> ItemsUnordered
        {
            get
            {
                foreach (var item in lambdaInstances.Values)
                {
                    for (int i = 0; i < item.Tag; i++)
                    {
                        yield return item.Value;
                    }
                }
                if (nullLambda != null)
                {
                    for (int i = 0; i < nullLambda.Tag; i++)
                    {
                        yield return nullLambda.Value;
                    }
                }
            }
        }

        private IEnumerable<TResult> ItemsInternal
        {
            get
            {
                foreach (var item in source)
                {
                    if (item == null)
                    {
                        yield return nullLambda.Value;
                    }
                    else
                    {
                        TaggedObservableValue<TResult, int> lambdaResult;
                        if (lambdaInstances.TryGetValue(item, out lambdaResult))
                        {
                            yield return lambdaResult.Value;
                        }
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
            var notification = CollectionChangedNotificationResult<TResult>.Create(this);
            var added = notification.AddedItems;
            var removed = notification.RemovedItems;

            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var sourceChange = (ICollectionChangedNotificationResult<TSource>)change;
                    if (sourceChange.IsReset)
                    {
                        foreach (var item in lambdaInstances.Values)
                            item.Successors.Unset(this);
                        lambdaInstances.Clear();
                        foreach (var item in source)
                            AttachItem(item);
                        OnCleared();
                        notification.TurnIntoReset();
                        return notification;
                    }
                    else
                    {
                        NotifySource(sourceChange, added, removed, sources.Count > 1);
                    }
                }
                else
                {
                    var itemChange = (IValueChangedNotificationResult<TResult>)change;
                    if (sources.Count > 1)
                    {
                        // if there are multiple changes, we need to check whether the change belongs to items
                        // we just added or removed
                        var addIndex = added.IndexOf(itemChange.OldValue);
                        var removeIndex = removed.IndexOf(itemChange.NewValue);
                        if (addIndex == -1)
                        {
                            if (removeIndex == -1)
                            {
                                // the changed item was not added or removed
                                removed.Add(itemChange.OldValue);
                                added.Add(itemChange.NewValue);
                            }
                            else
                            {
                                // if the value is removed, remove the old value only
                                removed[removeIndex] = itemChange.OldValue;
                            }
                        }
                        else
                        {
                            if (removeIndex == -1)
                            {
                                // an added item changed
                                added[addIndex] = itemChange.NewValue;
                            }
                            else
                            {
                                // an added item turns out to be the same as a removed item, so erase the change
                                added.RemoveAt(addIndex);
                                removed.RemoveAt(removeIndex);
                            }
                        }
                    }
                    else
                    {
                        removed.Add(itemChange.OldValue);
                        added.Add(itemChange.NewValue);
                    }
                }
            }
            RaiseEvents(added, removed, null);
            return notification;
        }

        private void NotifySource(ICollectionChangedNotificationResult<TSource> sourceChange, List<TResult> added, List<TResult> removed, bool checkConflicts)
        {
            if (sourceChange.RemovedItems != null)
            {
                foreach (var item in sourceChange.RemovedItems)
                {
                    if (item != null)
                    {
                        var lambdaResult = lambdaInstances[item];
                        lambdaResult.Tag--;
                        if (checkConflicts)
                        {
                            var addIndex = added.IndexOf(lambdaResult.Value);
                            if (addIndex != -1)
                            {
                                added.RemoveAt(addIndex);
                            }
                            else
                            {
                                removed.Add(lambdaResult.Value);
                            }
                        }
                        else
                        {
                            removed.Add(lambdaResult.Value);
                        }
                        if (lambdaResult.Tag == 0)
                        {
                            lambdaInstances.Remove(item);
                            lambdaResult.Successors.Unset(this);
                        }
                    }
                    else if (nullLambda != null)
                    {
                        nullLambda.Tag--;
                        if (nullLambda.Tag == 0)
                        {
                            nullLambda.Successors.Unset(this);
                            nullLambda = null;
                        }
                    }
                }
            }

            if (sourceChange.AddedItems != null)
            {
                foreach (var item in sourceChange.AddedItems)
                {
                    var lambdaResult = AttachItem(item);
                    if (checkConflicts || removed.Count > 0)
                    {
                        var removeIndex = removed.IndexOf(lambdaResult.Value);
                        if (removeIndex != -1)
                        {
                            removed.RemoveAt(removeIndex);
                        }
                        else
                        {
                            added.Add(lambdaResult.Value);
                        }
                    }
                    else
                    {
                        added.Add(lambdaResult.Value);
                    }
                }
            }
        }
    }
}
