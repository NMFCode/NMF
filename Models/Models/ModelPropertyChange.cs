using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Expressions;
using System.Collections.Specialized;
using System.ComponentModel;

namespace NMF.Models.Expressions
{
    /// <summary>
    /// The base class for simple property access proxies targeting a specific change event instead of the generic property changed event
    /// </summary>
    /// <typeparam name="TClass">The member type for the property</typeparam>
    /// <typeparam name="TProperty">The property type</typeparam>
    public abstract class ModelPropertyChange<TClass, TProperty> : INotifyReversableExpression<TProperty> where TClass : INotifyPropertyChanged
    {
        private readonly SuccessorList successors = new SuccessorList();
        private readonly IExecutionContext context = ExecutionEngine.Current.Context;
        private readonly string propertyName;
        
        public TClass ModelElement { get; private set; }

        /// <summary>
        /// Creates a proxy for the given model instance
        /// </summary>
        /// <param name="modelElement"></param>
        protected ModelPropertyChange(TClass modelElement, string propertyName)
        {
            ModelElement = modelElement;
            this.propertyName = propertyName;

            successors.CollectionChanged += (obj, e) =>
            {
                if (successors.Count == 0)
                    Detach();
                else if (e.Action == NotifyCollectionChangedAction.Add && successors.Count == 1)
                    Attach();
            };
        }

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

        public IList<INotifiable> Successors
        {
            get
            {
                return successors;
            }
        }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                return Enumerable.Empty<INotifiable>();
            }
        }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

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
            context.AddChangeListener(this, ModelElement, propertyName);
        }

        /// <summary>
        /// Attach a listener to this value
        /// </summary>
        public void Detach()
        {
            context.RemoveChangeListener(this, ModelElement, propertyName);
        }

        /// <summary>
        /// Simplifies the current expression
        /// </summary>
        /// <returns>A simpler expression repüresenting the same incremental value (e.g. a constant if this expression can be constant), otherwise itself</returns>
        public INotifyExpression<TProperty> Reduce()
        {
            return this;
        }

        public void Dispose()
        {
            Detach();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<TProperty>(this, Value, Value);
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters)
        {
            return ApplyParameters(parameters);
        }
    }
}
