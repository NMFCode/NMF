using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    internal class ObservableProxyExpression<T> : Expression, INotifyExpression<T>
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        protected INotifyValue<T> value;

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public ObservableProxyExpression(INotifyValue<T> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            this.value = value;
        }

        public bool CanBeConstant
        {
            get { return IsConstant; }
        }

        public bool IsParameterFree
        {
            get { return true; }
        }

        public T Value
        {
            get { return value.Value; }
        }

        public object ValueObject
        {
            get { return Value; }
        }

        public override bool CanReduce
        {
            get { return false; }
        }

        public override ExpressionType NodeType
        {
            get { return ExpressionType.Parameter; }
        }

        public bool IsConstant
        {
#pragma warning disable S3060 // "is" should not be used with "this"
            get { return this is ConstantValue<T>; }
#pragma warning restore S3060 // "is" should not be used with "this"
        }

        public IEnumerable<INotifiable> Dependencies => value.Dependencies;

        public ISuccessorList Successors => value.Successors;

        public ExecutionMetaData ExecutionMetaData => value.ExecutionMetaData;

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }
        
        public new INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Dispose()
        {
            Successors.UnsetAll();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            var result = value.Notify(sources);
            if (result.Changed && ValueChanged != null && result is IValueChangedNotificationResult valueChange)
            {
                ValueChanged?.Invoke(this, new ValueChangedEventArgs(valueChange.OldValue, valueChange.NewValue));
            }
            return result;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }
    }

    internal class ObservableProxyReversableExpression<T> : ObservableProxyExpression<T>, INotifyReversableExpression<T>
    {
        public ObservableProxyReversableExpression(INotifyReversableValue<T> value)
            : base(value) { }

        public new T Value
        {
            get
            {
                return this.value.Value;
            }
            set
            {
                var reversable = (INotifyReversableValue<T>)this.value;
                reversable.Value = value;
            }
        }

        public bool IsReversable
        {
            get
            {
                var reversable = (INotifyReversableValue<T>)this.value;
                return reversable.IsReversable;
            }
        }
    }

}
