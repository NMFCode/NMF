using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using SL = System.Linq.Enumerable;
using System.Text;
using System.Diagnostics;

namespace NMF.Expressions.Linq
{
    internal sealed class ObservableWhere<T> : ObservableEnumerable<T>, INotifyCollection<T>
    {
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

        private INotifyEnumerable<T> source;
        private ObservingFunc<T, bool> lambda;
        private Dictionary<T, TaggedObservableValue<bool, ItemMultiplicity>> lambdaInstances = new Dictionary<T, TaggedObservableValue<bool, ItemMultiplicity>>();
        private int nulls;
        private INotifyValue<bool> nullCheck;
        private static bool isValueType = ReflectionHelper.IsValueType<T>();

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
            if (source == null) throw new ArgumentNullException("source");
            if (lambda == null) throw new ArgumentNullException("lambda");

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
                    lambdaResult.Successors.Add(this);
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
                    nullCheck.Successors.Add(this);
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
                item.Successors.Remove(this);
            lambdaInstances.Clear();
        }

        public override IEnumerator<T> GetEnumerator()
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
        
        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var added = new List<T>();
            var removed = new List<T>();

            foreach (var change in sources)
            {
                if (change.Source == source)
                {
                    var sourceChange = (CollectionChangedNotificationResult<T>)change;
                    if (sourceChange.IsReset)
                    {
                        foreach (var item in lambdaInstances.Values)
                            item.Successors.Remove(this);
                        lambdaInstances.Clear();
                        foreach (var item in source)
                            AttachItem(item);
                        OnCleared();
                        return new CollectionChangedNotificationResult<T>(this);
                    }
                    else
                    {
                        NotifySource(sourceChange, added, removed);
                    }
                }
                else if (nullCheck != null && change.Source == nullCheck)
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
                else
                {
                    var lambdaResult = (TaggedObservableValue<bool, ItemMultiplicity>)change.Source;
                    if (lambdaResult.Value)
                        added.AddRange(SL.Repeat(lambdaResult.Tag.Item, lambdaResult.Tag.Multiplicity));
                    else
                        removed.AddRange(SL.Repeat(lambdaResult.Tag.Item, lambdaResult.Tag.Multiplicity));
                }
            }

            OnRemoveItems(removed);
            OnAddItems(added);
            return new CollectionChangedNotificationResult<T>(this, added, removed);
        }

        private void NotifySource(CollectionChangedNotificationResult<T> sourceChange, List<T> added, List<T> removed)
        {
            foreach (var item in sourceChange.AllRemovedItems)
            {
                if (isValueType || item != null)
                {
                    var lambdaResult = lambdaInstances[item];
                    if (lambdaResult.Value)
                    {
                        removed.Add(lambdaResult.Tag.Item);
                    }
                    lambdaResult.Tag = lambdaResult.Tag.Decrease();
                    if (lambdaResult.Tag.Multiplicity == 0)
                    {
                        lambdaResult.Successors.Remove(this);
                        lambdaInstances.Remove(item);
                    }
                }
                else
                {
                    nulls--;
                    if (nulls == 0)
                    {
                        nullCheck.Successors.Remove(this);
                        nullCheck = null;
                    }
                    removed.Add(default(T));
                }
            }
            
            foreach (var item in sourceChange.AllAddedItems)
            {
                var lambdaResult = AttachItem(item);
                if (lambdaResult.Value)
                {
                    added.Add(item);
                }
            }
        }

        #region ICollection methods

        void ICollection<T>.Add(T item)
        {
            TaggedObservableValue<bool, ItemMultiplicity> stack;
            if (!lambdaInstances.TryGetValue(item, out stack))
            {
                var sourceCollection = source as INotifyCollection<T>;
                if (sourceCollection != null && !sourceCollection.IsReadOnly)
                {
                    sourceCollection.Add(item);
                }
                else
                {
                    throw new InvalidOperationException("Source is not a collection or is read-only");
                }
                if (!lambdaInstances.TryGetValue(item, out stack))
                {
                    throw new InvalidOperationException("Something is wrong with the event hookup.");
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

        void ICollection<T>.Clear()
        {
            var coll = source as INotifyCollection<T>;
            if (coll == null || coll.IsReadOnly) throw new InvalidOperationException("Source is not a collection or is read-only");
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
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                var collection = source as INotifyCollection<T>;
                return !Lambda.IsReversable && (collection == null || collection.IsReadOnly);
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            var coll = source as INotifyCollection<T>;
            if (coll != null && !coll.IsReadOnly)
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

        #endregion
    }

}
