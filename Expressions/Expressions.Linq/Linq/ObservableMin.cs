using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq
{
    internal static class ObservableComparisons
    {
        public static INotifyValue<T> Max<T>(INotifyEnumerable<T> source)
        {
            return MaxWithComparer(source, null);
        }

        public static INotifyValue<T> MaxWithComparer<T>(INotifyEnumerable<T> source, IComparer<T> comparer)
        {
            return MinWithComparer(source, new ReverseComparer<T>(comparer));
        }

        public static INotifyValue<TResult> LambdaMax<TSource, TResult>(INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return LambdaMaxWithComparer(source, selector, null);
        }

        public static INotifyValue<TResult> LambdaMaxWithComparer<TSource, TResult>(INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector, IComparer<TResult> comparer)
        {
            return MinWithComparer(source.Select(selector), new ReverseComparer<TResult>(comparer));
        }

        public static INotifyValue<T?> NullableMax<T>(INotifyEnumerable<T?> source) where T : struct
        {
            return NullableMaxWithComparer(source, null);
        }

        public static INotifyValue<T?> NullableMaxWithComparer<T>(INotifyEnumerable<T?> source, IComparer<T> comparer) where T : struct
        {
            return NullableMinWithComparer(source, new ReverseComparer<T>(comparer));
        }

        public static INotifyValue<T?> LambdaNullableMax<TSource, T>(INotifyEnumerable<TSource> source, Expression<Func<TSource, T?>> selector) where T : struct
        {
            return LambdaNullableMaxWithComparer(source, selector, null);
        }

        public static INotifyValue<T?> LambdaNullableMaxWithComparer<TSource, T>(INotifyEnumerable<TSource> source, Expression<Func<TSource, T?>> selector, IComparer<T> comparer) where T : struct
        {
            return NullableMinWithComparer(source.Select(selector), new ReverseComparer<T>(comparer));
        }

        public static INotifyValue<T> Min<T>(INotifyEnumerable<T> source)
        {
            return MinWithComparer(source, null);
        }

        public static INotifyValue<T> MinWithComparer<T>(INotifyEnumerable<T> source, IComparer<T> comparer)
        {
            var observable = new ObservableMin<T>(source, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        public static INotifyValue<TResult> LambdaMin<TSource, TResult>(INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return LambdaMinWithComparer(source, selector, null);
        }

        public static INotifyValue<TResult> LambdaMinWithComparer<TSource, TResult>(INotifyEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector, IComparer<TResult> comparer)
        {
            return MinWithComparer(source.Select(selector), comparer);
        }

        public static INotifyValue<T?> NullableMin<T>(INotifyEnumerable<T?> source) where T : struct
        {
            return NullableMinWithComparer(source, null);
        }

        public static INotifyValue<T?> NullableMinWithComparer<T>(INotifyEnumerable<T?> source, IComparer<T> comparer) where T : struct
        {
            var observable = new ObservableNullableMin<T>(source, comparer);
            observable.Successors.SetDummy();
            return observable;
        }

        public static INotifyValue<T?> LambdaNullableMin<TSource, T>(INotifyEnumerable<TSource> source, Expression<Func<TSource, T?>> selector) where T : struct
        {
            return LambdaNullableMinWithComparer(source, selector, null);
        }

        public static INotifyValue<T?> LambdaNullableMinWithComparer<TSource, T>(INotifyEnumerable<TSource> source, Expression<Func<TSource, T?>> selector, IComparer<T> comparer) where T : struct
        {
            return NullableMinWithComparer(source.Select(selector), comparer);
        }

        public static INotifyValue<T> MaxExpression<T>(IEnumerableExpression<T> source)
        {
            return MaxWithComparer(source.AsNotifiable(), null);
        }

        public static INotifyValue<T> MaxExpressionWithComparer<T>(IEnumerableExpression<T> source, IComparer<T> comparer)
        {
            return MinWithComparer(source.AsNotifiable(), new ReverseComparer<T>(comparer));
        }

        public static INotifyValue<TResult> LambdaMaxExpression<TSource, TResult>(IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return LambdaMaxWithComparer(source.AsNotifiable(), selector, null);
        }

        public static INotifyValue<TResult> LambdaMaxExpressionWithComparer<TSource, TResult>(IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, IComparer<TResult> comparer)
        {
            return MinWithComparer(source.AsNotifiable().Select(selector), new ReverseComparer<TResult>(comparer));
        }

        public static INotifyValue<T?> NullableMaxExpression<T>(IEnumerableExpression<T?> source) where T : struct
        {
            return NullableMaxWithComparer(source.AsNotifiable(), null);
        }

        public static INotifyValue<T?> NullableMaxExpressionWithComparer<T>(IEnumerableExpression<T?> source, IComparer<T> comparer) where T : struct
        {
            return NullableMinWithComparer(source.AsNotifiable(), new ReverseComparer<T>(comparer));
        }

        public static INotifyValue<T?> LambdaNullableMaxExpression<TSource, T>(IEnumerableExpression<TSource> source, Expression<Func<TSource, T?>> selector) where T : struct
        {
            return LambdaNullableMaxWithComparer(source.AsNotifiable(), selector, null);
        }

        public static INotifyValue<T?> LambdaNullableMaxExpressionWithComparer<TSource, T>(IEnumerableExpression<TSource> source, Expression<Func<TSource, T?>> selector, IComparer<T> comparer) where T : struct
        {
            return NullableMinWithComparer(source.AsNotifiable().Select(selector), new ReverseComparer<T>(comparer));
        }

        public static INotifyValue<T> MinExpression<T>(INotifyEnumerable<T> source)
        {
            return MinWithComparer(source, null);
        }

        public static INotifyValue<T> MinExpressionWithComparer<T>(IEnumerableExpression<T> source, IComparer<T> comparer)
        {
            return MinWithComparer(source.AsNotifiable(), comparer);
        }

        public static INotifyValue<TResult> LambdaMinExpression<TSource, TResult>(IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return LambdaMinWithComparer(source.AsNotifiable(), selector, null);
        }

        public static INotifyValue<TResult> LambdaMinExpressionWithComparer<TSource, TResult>(IEnumerableExpression<TSource> source, Expression<Func<TSource, TResult>> selector, IComparer<TResult> comparer)
        {
            return MinWithComparer(source.AsNotifiable().Select(selector), comparer);
        }

        public static INotifyValue<T?> NullableMinExpression<T>(IEnumerableExpression<T?> source) where T : struct
        {
            return NullableMinWithComparer(source.AsNotifiable(), null);
        }

        public static INotifyValue<T?> NullableMinExpressionWithComparer<T>(IEnumerableExpression<T?> source, IComparer<T> comparer) where T : struct
        {
            return NullableMinWithComparer(source.AsNotifiable(), comparer);
        }

        public static INotifyValue<T?> LambdaNullableMinExpression<TSource, T>(IEnumerableExpression<TSource> source, Expression<Func<TSource, T?>> selector) where T : struct
        {
            return LambdaNullableMinWithComparer(source.AsNotifiable(), selector, null);
        }

        public static INotifyValue<T?> LambdaNullableMinExpressionWithComparer<TSource, T>(IEnumerableExpression<TSource> source, Expression<Func<TSource, T?>> selector, IComparer<T> comparer) where T : struct
        {
            return NullableMinWithComparer(source.AsNotifiable().Select(selector), comparer);
        }
    }

    internal class ObservableMin<T> : INotifyValue<T>
    {
        
        private INotifyEnumerable<T> source;
        private IComparer<T> comparer;
        private bool hasValue;
        private T current;

        public ObservableMin(INotifyEnumerable<T> source) : this(source, null) { }

        public ObservableMin(INotifyEnumerable<T> source, IComparer<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this.source = source;
            this.comparer = comparer ?? Comparer<T>.Default;

            Successors.Attached += (obj, e) => Attach();
            Successors.Detached += (obj, e) => Detach();
        }

        public T Value
        {
            get { return current; }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public IEnumerable<INotifiable> Dependencies { get { yield return source; } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);
            
            hasValue = false;
            foreach (var item in source)
            {
                AddItem(item);
            }
        }

        public void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        private void AddItem(T item)
        {
            if (!hasValue || comparer.Compare(current, item) > 0)
            {
                current = item;
                hasValue = true;
            }
        }

        private void OnValueChanged(T value, T oldValue)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new ValueChangedEventArgs(oldValue, value));
            }
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            var change = (CollectionChangedNotificationResult<T>)sources[0];
            var oldValue = Value;
            var reset = false;

            if (!change.IsReset)
            {
                if (change.RemovedItems != null)
                {
                    foreach (var item in change.RemovedItems)
                    {
                        if (comparer.Compare(current, item) == 0)
                        {
                            reset = true;
                            break;
                        }
                    }
                }
                if (change.AddedItems != null && !reset)
                {
                    foreach (var item in change.AddedItems)
                    {
                        AddItem(item);
                    }
                }
            }

            if (change.IsReset || reset)
            {
                current = default(T);
                hasValue = false;
                foreach (var item in source)
                {
                    AddItem(item);
                }
            }

            if (!EqualityComparer<T>.Default.Equals(current, oldValue))
            {
                OnValueChanged(current, oldValue);
                return new ValueChangedNotificationResult<T>(this, oldValue, current);
            }

            return UnchangedNotificationResult.Instance;
        }

        public void Dispose()
        {
            Detach();
        }
    }

    internal class ObservableNullableMin<T> : INotifyValue<T?>
        where T : struct
    {
        
        private INotifyEnumerable<T?> source;
        private IComparer<T> comparer;
        private T? current;

        public ObservableNullableMin(INotifyEnumerable<T?> source) : this(source, null) { }

        public ObservableNullableMin(INotifyEnumerable<T?> source, IComparer<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this.source = source;
            this.comparer = comparer ?? Comparer<T>.Default;

            Successors.Attached += (obj, e) => Attach();
            Successors.Detached += (obj, e) => Detach();
        }

        public T? Value
        {
            get { return current; }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public IEnumerable<INotifiable> Dependencies { get { yield return source; } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);
            
            foreach (var item in source)
            {
                AddItem(item);
            }
        }

        public void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        private void AddItem(T? item)
        {
            if (!item.HasValue)
                return;

            if (!current.HasValue || comparer.Compare(current.Value, item.Value) > 0)
                current = item;
        }

        private void OnValueChanged(T? value, T? oldValue)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new ValueChangedEventArgs(oldValue, value));
            }
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            var change = (CollectionChangedNotificationResult<T?>)sources[0];
            var oldValue = Value;
            var reset = false;

            if (!change.IsReset)
            {
                foreach (var item in change.AllRemovedItems)
                {
                    if (item.HasValue && comparer.Compare(current.Value, item.Value) == 0)
                    {
                        reset = true;
                        break;
                    }
                }

                if (!reset)
                {
                    foreach (var item in change.AllAddedItems)
                    {
                        AddItem(item);
                    }
                }
            }

            if (change.IsReset || reset)
            {
                current = null;
                foreach (var item in source)
                {
                    AddItem(item);
                }
            }

            if (!EqualityComparer<T>.Default.Equals(current.Value, oldValue.Value))
            {
                OnValueChanged(current, oldValue);
                return new ValueChangedNotificationResult<T?>(this, oldValue, current);
            }

            return UnchangedNotificationResult.Instance;
        }

        public void Dispose()
        {
            Detach();
        }
    }
}
