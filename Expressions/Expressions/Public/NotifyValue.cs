using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    public class NotifyValue<T> : INotifyValue<T>, INotifyPropertyChanged
    {
        public NotifyValue(Expression<Func<T>> expression, IDictionary<string, object> parameterMappings = null)
            : this(NotifySystem.CreateExpression<T>(expression.Body, parameterMappings: parameterMappings)) { }

        internal INotifyExpression<T> Expression { get; private set; }

        internal NotifyValue(INotifyExpression<T> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression = expression;
            if (!expression.IsAttached) expression.Attach();
            expression.ValueChanged += ExpressionValueChanged;
        }

        private void ExpressionValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
            OnPropertyChanged("Value");
        }

        public T Value
        {
            get
            {
                return Expression.Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public void Detach()
        {
            Expression.Detach();
        }

        public void Attach()
        {
            Expression.Attach();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;



        public bool IsAttached
        {
            get { return Expression.IsAttached; }
        }
    }

    public class NotifyReversableValue<T> : INotifyReversableValue<T>, INotifyPropertyChanged
    {
        internal INotifyReversableExpression<T> Expression { get; private set; }

        public NotifyReversableValue(Expression<Func<T>> expression, IDictionary<string, object> parameterMappings = null)
            : this(NotifySystem.CreateReversableExpression<T>(expression.Body, parameterMappings)) { }

        internal NotifyReversableValue(INotifyReversableExpression<T> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression = expression;
            if (!expression.IsAttached) expression.Attach();
            expression.ValueChanged += ExpressionValueChanged;
        }

        private void ExpressionValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
            OnPropertyChanged("Value");
        }

        public T Value
        {
            get
            {
                return Expression.Value;
            }
            set
            {
                Expression.Value = value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public void Detach()
        {
            Expression.Detach();
        }

        public void Attach()
        {
            Expression.Attach();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public bool IsAttached
        {
            get { return Expression.IsAttached; }
        }


        public bool IsReversable
        {
            get { return Expression.IsReversable; }
        }
    }

    internal class ReversableProxyValue<T, TExpression> : INotifyReversableValue<T> where TExpression : class, INotifyValue<T>
    {
        private TExpression inner;
        public TExpression Inner
        {
            get
            {
                return inner;
            }
            protected set
            {
                if (inner != value)
                {
                    if (inner != null) inner.ValueChanged -= Inner_ValueChanged;
                    if (value != null) value.ValueChanged += Inner_ValueChanged;
                    inner = value;
                }
            }
        }

        public Action<T> UpdateHandler { get; private set; }

        public ReversableProxyValue(TExpression inner, Action<T> updateHandler)
        {
            if (inner == null) throw new ArgumentNullException("inner");
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            Inner = inner;
            UpdateHandler = updateHandler;

            Inner.ValueChanged += Inner_ValueChanged;
        }

        void Inner_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }

        public T Value
        {
            get
            {
                return Inner.Value;
            }
            set
            {
                UpdateHandler(value);
            }
        }

        public bool IsReversable
        {
            get { return true; }
        }


        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public void Detach()
        {
            Inner.Detach();
        }

        public void Attach()
        {
            Inner.Attach();
        }

        public bool IsAttached
        {
            get { return Inner.IsAttached; }
        }
    }

    internal class ReversableProxyExpression<T> : ReversableProxyValue<T, INotifyExpression<T>>, INotifyReversableExpression<T>
    {
        public ReversableProxyExpression(INotifyExpression<T> inner, Action<T> changeAction) : base(inner, changeAction) { }

        public bool CanBeConstant
        {
            get { return Inner.CanBeConstant; }
        }

        public bool IsConstant
        {
            get { return Inner.IsConstant; }
        }

        public bool IsParameterFree
        {
            get { return Inner.IsParameterFree; }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ReversableProxyExpression<T>(Inner.ApplyParameters(parameters), UpdateHandler);
        }

        public void Refresh()
        {
            Inner.Refresh();
        }

        public INotifyExpression<T> Reduce()
        {
            Inner = Inner.Reduce();
            return this;
        }
    }

}
