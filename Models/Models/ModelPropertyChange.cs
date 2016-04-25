using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Expressions;

namespace NMF.Models.Expressions
{
    /// <summary>
    /// The base class for simple property access proxies targeting a specific change event instead of the generic property changed event
    /// </summary>
    /// <typeparam name="TClass">The member type for the property</typeparam>
    /// <typeparam name="TProperty">The property type</typeparam>
    public abstract class ModelPropertyChange<TClass, TProperty> : INotifyReversableExpression<TProperty>
    {
        public TClass ModelElement { get; private set; }
        private int attachedCount = 0;

        /// <summary>
        /// Creates a proxy for the given model instance
        /// </summary>
        /// <param name="modelElement"></param>
        protected ModelPropertyChange(TClass modelElement)
        {
            ModelElement = modelElement;
        }

        /// <summary>
        /// Registers the given event handler to a property changed event
        /// </summary>
        /// <param name="handler">The event handler that shall be registered</param>
        protected abstract void RegisterChangeEventHandler(EventHandler<ValueChangedEventArgs> handler);

        /// <summary>
        /// Unregisters the given handler from a property changed event
        /// </summary>
        /// <param name="handler">The event handler</param>
        protected abstract void UnregisterChangeEventHandler(EventHandler<ValueChangedEventArgs> handler);

        /// <summary>
        /// Determines whether the expression can be replaced by a constant expression
        /// </summary>
        public bool CanBeConstant
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns whether this value listens for changes
        /// </summary>
        public bool IsAttached
        {
            get
            {
                return attachedCount > 0;
            }
        }

        /// <summary>
        /// Determines whether the current expression is a constant
        /// </summary>
        public bool IsConstant
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the current expression contains parameters
        /// </summary>
        public bool IsParameterFree
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the current value
        /// </summary>
        public abstract TProperty Value
        {
            get;
            set;
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
        /// Checks whether it is allowed to set values
        /// </summary>
        public bool IsReversable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets fired when the value changed
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Applies the given set of parameters to the expression
        /// </summary>
        /// <param name="parameters">A set of parameter values</param>
        /// <returns>A new expression with all parameter placeholders replaced with the parameter values</returns>
        /// <remarks>In case that the current expression is parameter free, it simply returns itself</remarks>
        public INotifyExpression<TProperty> ApplyParameters(IDictionary<string, object> parameters)
        {
            return this;
        }

        /// <summary>
        /// Detach a listener to this value
        /// </summary>
        public void Attach()
        {
            attachedCount++;
            if (attachedCount == 1)
            {
                RegisterChangeEventHandler(ForwardValueChange);
            }
        }

        private void ForwardValueChange(object sender, ValueChangedEventArgs e)
        {
            var handler = ValueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Attach a listener to this value
        /// </summary>
        public void Detach()
        {
            attachedCount--;
            if (attachedCount == 0)
            {
                UnregisterChangeEventHandler(ForwardValueChange);
            }
        }

        /// <summary>
        /// Simplifies the current expression
        /// </summary>
        /// <returns>A simpler expression repüresenting the same incremental value (e.g. a constant if this expression can be constant), otherwise itself</returns>
        public INotifyExpression<TProperty> Reduce()
        {
            return this;
        }

        /// <summary>
        /// Refreshes the current value of the current expression
        /// </summary>
        public void Refresh()
        {
        }
    }
}
