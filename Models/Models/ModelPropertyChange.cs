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
        private readonly PropertyChangeListener listener;
        private readonly string propertyName;
        
        /// <summary>
        /// The model element
        /// </summary>
        public TClass ModelElement { get; private set; }

        /// <summary>
        /// Creates a proxy for the given model instance
        /// </summary>
        /// <param name="modelElement">the model element</param>
        /// <param name="propertyName">The property name</param>
        protected ModelPropertyChange(TClass modelElement, string propertyName)
        {
            ModelElement = modelElement;
            this.propertyName = propertyName;
            this.listener = new PropertyChangeListener(this);

            var successors = new MultiSuccessorList();
            successors.Attached += (obj, e) => Attach();
            successors.Detached += (obj, e) => Detach();
            Successors = successors;
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

        /// <inheritdoc />
        public ISuccessorList Successors { get; }

        /// <inheritdoc />
        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                return Enumerable.Empty<INotifiable>();
            }
        }

        /// <inheritdoc />
        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        /// <summary>
        /// Gets fired when the value changed
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <inheritdoc />
        public INotifyExpression<TProperty> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        /// <summary>
        /// Detach a listener to this value
        /// </summary>
        public void Attach()
        {
            listener.Subscribe(ModelElement, propertyName);
        }

        /// <summary>
        /// Attach a listener to this value
        /// </summary>
        public void Detach()
        {
            listener.Unsubscribe();
        }

        /// <summary>
        /// Simplifies the current expression
        /// </summary>
        /// <returns>A simpler expression repüresenting the same incremental value (e.g. a constant if this expression can be constant), otherwise itself</returns>
        public INotifyExpression<TProperty> Reduce()
        {
            return this;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        /// <param name="disposing">True, if managed resources should disposed, otherwise false</param>
        protected virtual void Dispose(bool disposing)
        {
            Successors.UnsetAll();
        }

        /// <inheritdoc />
        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<TProperty>(this, Value, Value);
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }
    }
}
