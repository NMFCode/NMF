using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    class ObservableProxyExpression<T> : Expression, INotifyExpression<T>
    {
        protected INotifyValue<T> value;

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

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return this;
        }

        public void Refresh() { }

        public T Value
        {
            get { return value.Value; }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public override bool CanReduce
        {
            get
            {
                return false;
            }
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Parameter;
            }
        }

        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add
            {
                this.value.ValueChanged += value;
            }
            remove
            {
                this.value.ValueChanged -= value;
            }
        }

        public void Detach()
        {
            this.value.Detach();
        }

        public void Attach()
        {
            this.value.Attach();
        }

        public bool IsAttached
        {
            get { return this.value.IsAttached; }
        }


        public bool IsConstant
        {
            get { return this is ConstantValue<T>; }
        }


        public new INotifyExpression<T> Reduce()
        {
            return this;
        }
    }

    class ObservableProxyReversableExpression<T> : ObservableProxyExpression<T>, INotifyReversableExpression<T>
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
