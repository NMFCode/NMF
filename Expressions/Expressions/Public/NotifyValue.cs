using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    public class NotifyValue<T> : INotifyValue<T>, INotifyPropertyChanged
    {
        internal INotifyExpression<T> Expression { get; private set; }

        public T Value { get { return Expression.Value; } }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ShortList<INotifiable> successors = new ShortList<INotifiable>();
        public IList<INotifiable> Successors { get { return successors; } }

        public virtual IEnumerable<INotifiable> Dependencies { get { yield return Expression; } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public NotifyValue(Expression<Func<T>> expression, IDictionary<string, object> parameterMappings = null)
            : this(NotifySystem.CreateExpression<T>(expression.Body, null, parameterMappings: parameterMappings)) { }
        
        internal NotifyValue(INotifyExpression<T> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression = expression;

            successors.CollectionChanged += (obj, e) =>
            {
                if (successors.Count == 0)
                    Detach();
                else if (e.Action == NotifyCollectionChangedAction.Add && successors.Count == 1)
                    Attach();
            };
        }

        public virtual INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var oldValue = ((ValueChangedNotificationResult<T>)sources[0]).OldValue;
                OnValueChanged(oldValue, Value);
                OnPropertyChanged("Value");
                return new ValueChangedNotificationResult<T>(this, oldValue, Value);
            }
            return new UnchangedNotificationResult(this);
        }

        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            if (ValueChanged != null)
                ValueChanged(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Attach()
        {
            OnAttach();
            foreach (var dep in Dependencies)
                dep.Successors.Add(this);
        }

        private void Detach()
        {
            OnDetach();
            foreach (var dep in Dependencies)
                dep.Successors.Remove(this);
        }

        /// <summary>
        /// Occurs when this node gets (re)attached to another node for the first time
        /// </summary>
        protected virtual void OnAttach() { }

        /// <summary>
        /// Occurs when the last successor of this node gets removed
        /// </summary>
        protected virtual void OnDetach() { }
    }

    public class NotifyReversableValue<T> : INotifyReversableValue<T>, INotifyPropertyChanged
    {
        internal INotifyReversableExpression<T> Expression { get; private set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ShortList<INotifiable> successors = new ShortList<INotifiable>();
        public IList<INotifiable> Successors { get { return successors; } }

        public IEnumerable<INotifiable> Dependencies { get { yield return Expression; } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public NotifyReversableValue(Expression<Func<T>> expression, IDictionary<string, object> parameterMappings = null)
            : this(NotifySystem.CreateReversableExpression<T>(expression.Body, null, parameterMappings)) { }

        internal NotifyReversableValue(INotifyReversableExpression<T> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression = expression;

            successors.CollectionChanged += (obj, e) =>
            {
                if (successors.Count == 0)
                    Detach();
                else if (e.Action == NotifyCollectionChangedAction.Add && successors.Count == 1)
                    Attach();
            };
        }

        public bool IsReversable
        {
            get { return Expression.IsReversable; }
        }

        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            if (ValueChanged != null)
                ValueChanged(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var oldValue = ((ValueChangedNotificationResult<T>)sources[0]).OldValue;
                OnValueChanged(oldValue, Value);
                OnPropertyChanged("Value");
                return new ValueChangedNotificationResult<T>(this, oldValue, Value);
            }
            return new UnchangedNotificationResult(this);
        }

        private void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Add(this);
            OnAttach();
        }

        private void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Remove(this);
            OnDetach();
        }

        /// <summary>
        /// Occurs when this node gets (re)attached to another node for the first time
        /// </summary>
        protected virtual void OnAttach() { }

        /// <summary>
        /// Occurs when the last successor of this node gets removed
        /// </summary>
        protected virtual void OnDetach() { }
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
                    if (inner != null)
                        inner.Successors.Remove(this);
                    if (value != null)
                        value.Successors.Add(this);
                    inner = value;
                }
            }
        }

        public T Value
        {
            get { return Inner.Value; }
            set { UpdateHandler(value); }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public Action<T> UpdateHandler { get; private set; }

        private readonly ShortList<INotifiable> successors = new ShortList<INotifiable>();

        public IList<INotifiable> Successors { get { return successors; } }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (inner != null)
                    yield return inner;
            }
        }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public bool IsReversable
        {
            get { return true; }
        }

        public ReversableProxyValue(TExpression inner, Action<T> updateHandler)
        {
            if (inner == null) throw new ArgumentNullException("inner");
            if (updateHandler == null) throw new ArgumentNullException("updateHandler");

            Inner = inner;
            UpdateHandler = updateHandler;

            successors.CollectionChanged += (obj, e) =>
            {
                if (successors.Count == 0)
                    Detach();
                else if (e.Action == NotifyCollectionChangedAction.Add && successors.Count == 1)
                    Attach();
            };
        }

        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            if (ValueChanged != null)
                ValueChanged(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        public virtual INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var oldValue = ((ValueChangedNotificationResult<T>)sources[0]).OldValue;
                OnValueChanged(oldValue, Value);
                return new ValueChangedNotificationResult<T>(this, oldValue, Value);
            }
            return new UnchangedNotificationResult(this);
        }

        private void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Add(this);
            OnAttach();
        }

        private void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Remove(this);
            OnDetach();
        }

        /// <summary>
        /// Occurs when this node gets (re)attached to another node for the first time
        /// </summary>
        protected virtual void OnAttach() { }

        /// <summary>
        /// Occurs when the last successor of this node gets removed
        /// </summary>
        protected virtual void OnDetach() { }
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

        public INotifyExpression<T> Reduce()
        {
            Inner = Inner.Reduce();
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters)
        {
            return ApplyParameters(parameters);
        }
    }
}
