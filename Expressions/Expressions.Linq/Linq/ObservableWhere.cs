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
        private Dictionary<T, TaggedObservableValue<bool, ItemMultiplicity>> lambdas = new Dictionary<T, TaggedObservableValue<bool, ItemMultiplicity>>();
        private int nulls;
        private INotifyValue<bool> nullCheck;
        private static bool isValueType = ReflectionHelper.IsValueType<T>();

        public ObservingFunc<T, bool> Lambda
        {
            get
            {
                return lambda;
            }
        }

        private INotifyEnumerable<T> Source
        {
            get
            {
                return source;
            }
        }


        public ObservableWhere(INotifyEnumerable<T> source, ObservingFunc<T, bool> lambda)
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
                    var removed = new List<T>();
                    foreach (T item in e.OldItems)
                    {
                        TaggedObservableValue<bool, ItemMultiplicity> lambdaResult;
                        if (isValueType || item != null)
                        {
                            if (lambdas.TryGetValue(item, out lambdaResult))
                            {
                                if (lambdaResult.Value)
                                {
                                    removed.Add(lambdaResult.Tag.Item);
                                }
                                lambdaResult.Tag = lambdaResult.Tag.Decrease();
                                if (lambdaResult.Tag.Multiplicity == 0)
                                {
                                    lambdaResult.ValueChanged -= LambdaChanged;
                                    lambdaResult.Detach();
                                    lambdas.Remove(item);
                                }
                            }
                            else
                            {
                                //throw new InvalidOperationException();
                            }
                        }
                        else
                        {
                            nulls--;
                            if (nulls == 0)
                            {
                                nullCheck.Detach();
                                nullCheck = null;
                            }
                            removed.Add(default(T));
                        }
                    }
                    OnRemoveItems(removed);
                }
                if (e.NewItems != null)
                {
                    var added = new List<T>();
                    foreach (T item in e.NewItems)
                    {
                        var lambdaResult = AttachItem(item);
                        if (lambdaResult.Value)
                        {
                            added.Add(item);
                        }
                    }
                    OnAddItems(added);
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
            foreach (var pair in lambdas)
            {
                pair.Value.Detach();
            }
            lambdas.Clear(); 
        }

        private INotifyValue<bool> AttachItem(T item)
        {
            if (isValueType || item != null)
            {
                TaggedObservableValue<bool, ItemMultiplicity> lambdaResult;
                if (!lambdas.TryGetValue(item, out lambdaResult))
                {
                    lambdaResult = Lambda.InvokeTagged(item, new ItemMultiplicity(item, 1));
                    lambdas.Add(item, lambdaResult);
                    lambdaResult.ValueChanged += LambdaChanged;
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
                    nullCheck.ValueChanged += NullCheckValueChanged;
                }
                return nullCheck;
            }
        }

        private void NullCheckValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (nullCheck.Value)
            {
                OnAddItems(SL.Repeat(default(T), nulls));
            }
            else
            {
                OnRemoveItems(SL.Repeat(default(T), nulls));
            }
        }

        private void LambdaChanged(object sender, ValueChangedEventArgs e)
        {
            var lambdaResult = sender as TaggedObservableValue<bool, List<T>>;
            if (lambdaResult.Value)
            {
                OnAddItems(lambdaResult.Tag);
            }
            else
            {
                OnRemoveItems(lambdaResult.Tag);
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

        public override IEnumerator<T> GetEnumerator()
        {
            return SL.Where(Source, item =>
            {
                if (isValueType || item != null)
                {
                    TaggedObservableValue<bool, ItemMultiplicity> node;
                    if (lambdas.TryGetValue(item, out node))
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
            if (lambdas.TryGetValue(item, out node))
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
                return SL.Sum(SL.Where(lambdas.Values, lambda => lambda.Value), node => node.Tag.Multiplicity);
            }
        }

        void ICollection<T>.Add(T item)
        {
            TaggedObservableValue<bool, ItemMultiplicity> stack;
            if (!lambdas.TryGetValue(item, out stack))
            {
                var sourceCollection = Source as INotifyCollection<T>;
                if (sourceCollection != null && !sourceCollection.IsReadOnly)
                {
                    sourceCollection.Add(item);
                }
                else
                {
                    throw new InvalidOperationException("Source is not a collection or is read-only");
                }
                if (!lambdas.TryGetValue(item, out stack))
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
            var coll = Source as INotifyCollection<T>;
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
                var collection = Source as INotifyCollection<T>;
                return !Lambda.IsReversable && (collection == null || collection.IsReadOnly);
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            var coll = Source as INotifyCollection<T>;
            if (coll != null && !coll.IsReadOnly)
            {
                return coll.Remove(item);
            }
            else
            {
                TaggedObservableValue<bool, ItemMultiplicity> stack;
                if (lambdas.TryGetValue(item, out stack))
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
    }

}
