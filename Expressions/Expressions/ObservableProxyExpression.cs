using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableProxyExpression<T> : Expression, INotifyExpression<T>
    {
        protected INotifyValue<T> value;

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public ObservableProxyExpression(INotifyValue<T> value)
        {
            if (value == null) throw new ArgumentNullException("value");

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
            get { return this is ConstantValue<T>; }
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
            return value.Notify(sources);
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
