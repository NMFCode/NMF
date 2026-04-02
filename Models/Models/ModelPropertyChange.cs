using System;
using System.Collections.Generic;
using System.Linq;
using NMF.Expressions;
using System.ComponentModel;

namespace NMF.Models.Expressions
{
    /// <summary>
    /// The base class for simple property access proxies targeting a specific change event instead of the generic property changed event
    /// </summary>
    /// <typeparam name="TClass">The member type for the property</typeparam>
    /// <typeparam name="TProperty">The property type</typeparam>
    public abstract class ModelPropertyChange<TClass, TProperty> : NotifyExpression<TProperty>, INotifyReversableExpression<TProperty> where TClass : INotifyPropertyChanged
    {
        private readonly PropertyChangeListener listener;
        
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
            listener = new PropertyChangeListener(this, propertyName);
        }

        /// <summary>
        /// Gets or sets the current value
        /// </summary>
        public new abstract TProperty Value
        {
            get;
            set;
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
        protected override void OnAttach()
        {
            listener.Subscribe(ModelElement);
        }

        /// <inheritdoc />
        protected override void OnDetach()
        {
            listener.Unsubscribe();
        }

        /// <inheritdoc />
        public override bool IsParameterFree => true;

        /// <inheritdoc />
        public override IEnumerable<INotifiable> Dependencies => Enumerable.Empty<INotifiable>();

        /// <inheritdoc />
        protected override TProperty GetValue() => Value;

        /// <inheritdoc />
        protected override INotifyExpression<TProperty> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }
    }
}
