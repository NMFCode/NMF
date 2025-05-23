﻿using System;
using System.Collections.Generic;
using SL = System.Linq.Enumerable;
using System.Diagnostics;
using System.Collections;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableWhere<T> : ObservableEnumerable<T>, INotifyCollection<T>
    {
        public override string ToString()
        {
            return "[Where]";
        }

        internal struct ItemMultiplicity
        {
            public ItemMultiplicity(T item, int multiplicity) : this()
            {
                Multiplicity = multiplicity;
                Item = item;
            }

            public int Multiplicity { get; private set; }

            public T Item { get; private set; }

            public ItemMultiplicity Increase()
            {
                return new ItemMultiplicity(Item, Multiplicity + 1);
            }

            public ItemMultiplicity Decrease()
            {
                return new ItemMultiplicity(Item, Multiplicity - 1);
            }
        }

        private readonly INotifyEnumerable<T> source;
        private readonly ObservingFunc<T, bool> lambda;
        private readonly Dictionary<T, TaggedObservableValue<bool, ItemMultiplicity>> lambdaInstances = new Dictionary<T, TaggedObservableValue<bool, ItemMultiplicity>>();
        private int nulls;
        private INotifyValue<bool> nullCheck;
        private static readonly bool isValueType = ReflectionHelper.IsValueType<T>();

        public ObservingFunc<T, bool> Lambda { get { return lambda; } }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return source;
                if (nullCheck != null)
                    yield return nullCheck;
                foreach (var item in lambdaInstances.Values)
                    yield return item;
            }
        }

        public ObservableWhere(INotifyEnumerable<T> source, ObservingFunc<T, bool> lambda)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (lambda == null) throw new ArgumentNullException(nameof(lambda));

            this.source = source;
            this.lambda = lambda;
        }

        private INotifyValue<bool> AttachItem(T item)
        {
            if (isValueType || item != null)
            {
                TaggedObservableValue<bool, ItemMultiplicity> lambdaResult;
                if (!lambdaInstances.TryGetValue(item, out lambdaResult))
                {
                    lambdaResult = Lambda.InvokeTagged(item, new ItemMultiplicity(item, 1));
                    lambdaInstances.Add(item, lambdaResult);
                    lambdaResult.Successors.Set(this);
                }
                else
                {
                    lambdaResult.Tag = lambdaResult.Tag.Increase();
                }
                return lambdaResult;
            }
            else
            {
                nulls++;
                if (nullCheck == null)
                {
                    nullCheck = Lambda.Observe(default(T));
                    nullCheck.Successors.Set(this);
                }
                return nullCheck;
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

        public override IEnumerator<T> GetEnumerator()
        {
            if (ObservableExtensions.KeepOrder)
            {
                return SL.Where(source, item =>
                {
                    if (isValueType || item != null)
                    {
                        TaggedObservableValue<bool, ItemMultiplicity> node;
                        if (lambdaInstances.TryGetValue(item, out node))
                        {
                            return node.Value;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return nullCheck != null && nullCheck.Value;
                    }
                }).GetEnumerator();
            }
            else
            {
                return ItemsUnordered.GetEnumerator();
            }
        }

        private IEnumerable<T> ItemsUnordered
        {
            get
            {
                foreach (var item in lambdaInstances.Values)
                {
                    if (item.Value)
                    {
                        for (int i = 0; i < item.Tag.Multiplicity; i++)
                        {
                            yield return item.Tag.Item;
                        }
                    }
                }
                if (nullCheck != null && nullCheck.Value)
                {
                    for (int i = 0; i < nulls; i++)
                    {
                        yield return default(T);
                    }
                }
            }
        }

        public override void Add(T item)
        {
            TaggedObservableValue<bool, ItemMultiplicity> stack;
            if (!lambdaInstances.TryGetValue(item, out stack))
            {
                if (source is INotifyCollection<T> sourceCollection && !sourceCollection.IsReadOnly)
                {
                    sourceCollection.Add(item);
                    stack = (TaggedObservableValue<bool, ItemMultiplicity>)AttachItem(item);
                }
                else
                {
                    throw new InvalidOperationException("Source is not a collection or is read-only");
                }
            }
            if (!stack.Value)
            {
                if (stack != null && stack.IsReversable)
                {
                    stack.Value = true;
                    return;
                }
                else
                {
                    Debug.WriteLine("Could not set predicate.");
                }
            }
        }

        public override void Clear()
        {
            if (source is not INotifyCollection<T> coll || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
            var list = new List<T>(this);
            if (list.Count == coll.Count)
            {
                coll.Clear();
            }
            else
            {
                foreach (var item in list)
                {
                    coll.Remove(item);
                }
            }
            OnDetach();
            OnAttach();
        }

        public override bool Remove(T item)
        {
            if (source is INotifyCollection<T> coll && !coll.IsReadOnly)
            {
                return coll.Remove(item);
            }
            else
            {
                TaggedObservableValue<bool, ItemMultiplicity> stack;
                if (lambdaInstances.TryGetValue(item, out stack))
                {
                    if (stack.Tag.Multiplicity > 1)
                    {
                        throw new InvalidOperationException("Could not remove the requested item. Changing the predicate would result in multiple elements to be removed.");
                    }
                    else if (stack.IsReversable)
                    {
                        stack.Value = false;
                    }
                }
            }
            return false;
        }

        public override bool Contains(T item)
        {
            TaggedObservableValue<bool, ItemMultiplicity> node;
            if (lambdaInstances.TryGetValue(item, out node))
            {
                return node.Value;
            }
            else
            {
                return false;
            }
        }

        public override int Count
        {
            get
            {
                return SL.Sum(SL.Where(lambdaInstances.Values, lambda => lambda.Value), node => node.Tag.Multiplicity);
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                var collection = source as INotifyCollection<T>;
                return !Lambda.IsReversable && (collection == null || collection.IsReadOnly);
            }
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var notification = CollectionChangedNotificationResult<T>.Create(this);
            var added = notification.AddedItems;
            var removed = notification.RemovedItems;
            var moved = notification.MovedItems;

            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var sourceChange = (ICollectionChangedNotificationResult<T>)change;
                    if (sourceChange.IsReset)
                    {
                        return MakeReset(notification);
                    }
                    else
                    {
                        NotifySource(sourceChange, added, removed, moved);
                    }
                }
                else if (nullCheck != null && change.Source == nullCheck)
                {
                    AddOrRemoveNulls(added, removed);
                }
                else
                {
                    NotifyFilterChange(added, removed, change);
                }
            }

            OnRemoveItems(removed);
            OnAddItems(added);

            return notification;
        }

        private INotificationResult MakeReset(CollectionChangedNotificationResult<T> notification)
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

        private static void NotifyFilterChange(List<T> added, List<T> removed, INotificationResult change)
        {
            var lambdaResult = (TaggedObservableValue<bool, ItemMultiplicity>)change.Source;
            if (lambdaResult.Value)
                added.AddRange(SL.Repeat(lambdaResult.Tag.Item, lambdaResult.Tag.Multiplicity));
            else
                removed.AddRange(SL.Repeat(lambdaResult.Tag.Item, lambdaResult.Tag.Multiplicity));
        }

        private void AddOrRemoveNulls(List<T> added, List<T> removed)
        {
            if (nullCheck.Value)
            {
                added.AddRange(SL.Repeat(default(T), nulls));
            }
            else
            {
                removed.AddRange(SL.Repeat(default(T), nulls));
            }
        }

        private void NotifySource(ICollectionChangedNotificationResult<T> sourceChange, List<T> added, List<T> removed, List<T> moved)
        {
            if (sourceChange.RemovedItems != null && sourceChange.RemovedItems.Count > 0)
            {
                foreach (var item in sourceChange.RemovedItems)
                {
                    NotifyItemRemoved(removed, item);
                }
            }

            if (sourceChange.AddedItems != null && sourceChange.AddedItems.Count > 0)
            {
                foreach (var item in sourceChange.AddedItems)
                {
                    var lambdaResult = AttachItem(item);
                    if (lambdaResult.Value)
                    {
                        added.Add(item);
                    }
                }
            }

            if (sourceChange.MovedItems != null && ObservableExtensions.KeepOrder && sourceChange.MovedItems.Count > 0)
            {
                foreach (var item in sourceChange.MovedItems)
                {
                    if (isValueType || item != null)
                    {
                        TaggedObservableValue<bool, ItemMultiplicity> node;
                        if (lambdaInstances.TryGetValue(item, out node) && node.Value)
                        {
                            moved.Add(item);
                        }
                    }
                    else if (nullCheck != null && nullCheck.Value)
                    {
                        moved.Add(item);
                    }
                }
            }
        }

        private void NotifyItemRemoved(List<T> removed, T item)
        {
            if (isValueType || item != null)
            {
                TaggedObservableValue<bool, ItemMultiplicity> lambdaResult;
                if (lambdaInstances.TryGetValue(item, out lambdaResult))
                {
                    if (lambdaResult.Value)
                    {
                        removed.Add(lambdaResult.Tag.Item);
                    }
                    lambdaResult.Tag = lambdaResult.Tag.Decrease();
                    if (lambdaResult.Tag.Multiplicity == 0)
                    {
                        lambdaResult.Successors.Unset(this);
                        lambdaInstances.Remove(item);
                    }
                }
            }
            else
            {
                nulls--;
                if (nulls == 0)
                {
                    nullCheck.Successors.Unset(this);
                    nullCheck = null;
                }
                removed.Add(default(T));
            }
        }
    }

}
