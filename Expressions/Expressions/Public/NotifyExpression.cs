using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// The common base class for incremental expressions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NotifyExpression<T> : NotifyExpressionBase, INotifyExpression<T>
    {
        /// <summary>
        /// Creates a new incremental expression
        /// </summary>
        protected NotifyExpression() { }

        /// <summary>
        /// Creates a new incremental expression with the given initial value
        /// </summary>
        /// <param name="value">The initial value</param>
        protected NotifyExpression(T value)
        {
            this.value = value;
        }

        private T value;

        /// <summary>
        /// Gets the current value of this expression
        /// </summary>
        public T Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Gets the current value as object
        /// </summary>
        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        /// <summary>
        /// Gets the type of this incremental expression
        /// </summary>
        public sealed override Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Gets fired when the current value of this expression changes
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Refreshes the current value
        /// </summary>
        public virtual void Refresh()
        {
            var newVal = GetValue();
            if (!EqualityComparer<T>.Default.Equals(value, newVal))
            {
                var oldVal = value;
                value = newVal;
                OnValueChanged(oldVal, newVal);
            }
        }

        /// <summary>
        /// Determines whether this expression can be reduced
        /// </summary>
        public override bool CanReduce
        {
            get
            {
                return CanBeConstant;
            }
        }

        /// <summary>
        /// Simplifies the current expression
        /// </summary>
        /// <returns>A simpler expression repüresenting the same incremental value (e.g. a constant if this expression can be constant), otherwise itself</returns>
        public new virtual INotifyExpression<T> Reduce()
        {
            Attach();
            if (CanBeConstant)
            {
                return new ObservableConstant<T>(Value);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Simplifies the current expression
        /// </summary>
        /// <returns>A simpler expression repüresenting the same incremental value (e.g. a constant if this expression can be constant), otherwise itself</returns>
        protected override Expression BaseReduce()
        {
            Attach();
            if (CanBeConstant)
            {
                return new ObservableConstant<T>(Value);
            }
            else
            {
                return this;
            }            
        }

        /// <summary>
        /// Gets called when the value of the current expression changes
        /// </summary>
        /// <param name="oldVal">The old value</param>
        /// <param name="newVal">The new value</param>
        protected void OnValueChanged(T oldVal, T newVal)
        {
            OnValueChanged(new ValueChangedEventArgs(oldVal, newVal));
        }


        /// <summary>
        /// Gets called when the value of the current expression changes
        /// </summary>
        /// <param name="e">The event data</param>
        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            var handler = ValueChanged;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Returns whether the current expression can be constant
        /// </summary>
        public virtual bool CanBeConstant { get { return false; } }

        /// <summary>
        /// Gets the value of the current incremental expression
        /// </summary>
        /// <returns>The current value</returns>
        protected abstract T GetValue();

        /// <summary>
        /// Returns whether the expression is currently olistening to changes
        /// </summary>
        public bool IsAttached { get; private set; }

        /// <summary>
        /// Detaches a client from the incremental expression
        /// </summary>
        public void Detach()
        {
            if (IsAttached)
            {
                IsAttached = false;
                DetachCore();
            }
        }

        /// <summary>
        /// Attaches a client to the incremental expression
        /// </summary>
        public void Attach()
        {
            if (!IsAttached)
            {
                AttachCore();
                IsAttached = true;
                Refresh();
            }
        }

        /// <summary>
        /// Detach this incremental expression from listening to changes
        /// </summary>
        protected abstract void DetachCore();

        /// <summary>
        /// Attach this incremental client to listening to changes
        /// </summary>
        protected abstract void AttachCore();

        /// <summary>
        /// Returns whether this expression is parameter free
        /// </summary>
        public abstract bool IsParameterFree
        {
            get;
        }

        /// <summary>
        /// Applies the given set of parameters to the expression
        /// </summary>
        /// <param name="parameters">A set of parameter values</param>
        /// <returns>A new expression with all parameter placeholders replaced with the parameter values</returns>
        /// <remarks>In case that the current expression is parameter free, it simply returns itself</remarks>
        public abstract INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters);

        /// <summary>
        /// Returns whether this expression is a constant value
        /// </summary>
        public virtual bool IsConstant
        {
            get { return false; }
        }
    }
}
