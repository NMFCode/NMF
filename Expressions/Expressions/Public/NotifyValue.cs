using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// Abstract base implementation of notify value
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public abstract class NotifyValue<T> : INotifyValue<T>, ISuccessorList
    {
        private readonly ExecutionMetaData metadata = new ExecutionMetaData();

        /// <inheritdoc />
        public abstract T Value { get; }

        /// <inheritdoc />
        public ISuccessorList Successors => this;

        /// <inheritdoc />
        public abstract IEnumerable<INotifiable> Dependencies { get; }

        /// <inheritdoc />
        public ExecutionMetaData ExecutionMetaData {  get { return metadata; } }

        /// <inheritdoc />
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Gets called when the value changed
        /// </summary>
        /// <param name="e">event args</param>
        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the ValueChanged event
        /// </summary>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        protected void OnValueChanged(T oldValue, T newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Detach();
        }

        /// <inheritdoc />
        protected virtual void Attach() { }

        /// <inheritdoc />
        protected virtual void Detach() { }

        /// <inheritdoc />
        public abstract INotificationResult Notify(IList<INotificationResult> sources);


        #region SuccessorList


        private bool isDummySet = false;
        private readonly List<INotifiable> successors = new List<INotifiable>();

        /// <inheritdoc />
        public bool HasSuccessors => !isDummySet && successors.Count > 0;

        /// <inheritdoc />
        public bool IsAttached => isDummySet || successors.Count > 0;

        /// <inheritdoc />
        public int Count => successors.Count;

        /// <inheritdoc />
        public IEnumerable<INotifiable> AllSuccessors => successors;


        /// <inheritdoc />
        public void Set(INotifiable node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            successors.Add(node);
            if (isDummySet)
            {
                isDummySet = false;
            }
            else
            {
                if (successors.Count == 1)
                {
                    Attach();
                }
            }
        }


        /// <inheritdoc />
        public void SetDummy()
        {
            if (successors.Count == 0 && !isDummySet)
            {
                isDummySet = true;
                Attach();
            }
        }


        /// <inheritdoc />
        public void Unset(INotifiable node, bool leaveDummy = false)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!successors.Remove(node))
            {
                throw new InvalidOperationException("The specified node is not registered as the successor.");
            }
            if (!(isDummySet = leaveDummy))
            {
                Detach();
            }
        }


        /// <inheritdoc />
        public void UnsetAll()
        {
            if (IsAttached)
            {
                isDummySet = false;
                successors.Clear();
                Detach();
            }
        }

        /// <inheritdoc />
        public INotifiable GetSuccessor(int index)
        {
            return successors[index];
        }

        #endregion
    }

    /// <summary>
    /// base implementation of a reversable notify value
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class NotifyReversableValue<T> : INotifyReversableValue<T>, INotifyPropertyChanged, ISuccessorList
    {
        internal INotifyReversableExpression<T> Expression { get; private set; }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;


        /// <inheritdoc />
        public ISuccessorList Successors => this;

        /// <inheritdoc />
        public IEnumerable<INotifiable> Dependencies { get { yield return Expression; } }

        /// <inheritdoc />
        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="expression">The underlying expression</param>
        /// <param name="parameterMappings">Parameter mappings</param>
        public NotifyReversableValue(Expression<Func<T>> expression, IDictionary<string, object> parameterMappings = null)
            : this(NotifySystem.CreateReversableExpression<T>(expression.Body, null, parameterMappings)) { }

        internal NotifyReversableValue(INotifyReversableExpression<T> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            Expression = expression;
        }

        /// <inheritdoc />
        public bool IsReversable
        {
            get { return Expression.IsReversable; }
        }

        /// <summary>
        /// Gets called when the value changed
        /// </summary>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        /// <summary>
        /// Raises PropertyChanged
        /// </summary>
        /// <param name="propertyName">the name of the property</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public virtual INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var oldValue = ((IValueChangedNotificationResult<T>)sources[0]).OldValue;
                OnValueChanged(oldValue, Value);
                OnPropertyChanged("Value");
                return new ValueChangedNotificationResult<T>(this, oldValue, Value);
            }
            return UnchangedNotificationResult.Instance;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Successors.UnsetAll();
        }
        

        private void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);
            OnAttach();
        }

        private void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
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

        #region SuccessorList


        private bool isDummySet = false;
        private readonly List<INotifiable> successors = new List<INotifiable>();

        /// <inheritdoc />
        public bool HasSuccessors => !isDummySet && successors.Count > 0;

        /// <inheritdoc />
        public bool IsAttached => isDummySet || successors.Count > 0;

        /// <inheritdoc />
        public int Count => successors.Count;

        /// <inheritdoc />
        public IEnumerable<INotifiable> AllSuccessors => successors;


        /// <inheritdoc />
        public void Set(INotifiable node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            successors.Add(node);
            if (isDummySet)
            {
                isDummySet = false;
            }
            else
            {
                if (successors.Count == 1)
                {
                    Attach();
                }
            }
        }


        /// <inheritdoc />
        public void SetDummy()
        {
            if (successors.Count == 0 && !isDummySet)
            {
                isDummySet = true;
                Attach();
            }
        }


        /// <inheritdoc />
        public void Unset(INotifiable node, bool leaveDummy = false)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!successors.Remove(node))
            {
                throw new InvalidOperationException("The specified node is not registered as the successor.");
            }
            if (!(isDummySet = leaveDummy))
            {
                Detach();
            }
        }


        /// <inheritdoc />
        public void UnsetAll()
        {
            if (IsAttached)
            {
                isDummySet = false;
                successors.Clear();
                Detach();
            }
        }

        /// <inheritdoc />
        public INotifiable GetSuccessor(int index)
        {
            return successors[index];
        }

        #endregion
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
                        inner.Successors.Unset(this);
                    if (value != null)
                        value.Successors.Set(this);
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

        public MultiSuccessorList Successors { get; } = new MultiSuccessorList();

        ISuccessorList INotifiable.Successors => Successors;

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

            Successors.Attached += (obj, e) => Attach();
            Successors.Detached += (obj, e) => Detach();
        }

        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        public virtual INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var oldValue = ((IValueChangedNotificationResult<T>)sources[0]).OldValue;
                OnValueChanged(oldValue, Value);
                return new ValueChangedNotificationResult<T>(this, oldValue, Value);
            }
            return UnchangedNotificationResult.Instance;
        }

        public void Dispose()
        {
            Successors.UnsetAll();
        }

        private void Attach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Set(this);
            OnAttach();
        }

        private void Detach()
        {
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
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

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ReversableProxyExpression<T>(Inner.ApplyParameters(parameters, trace), UpdateHandler);
        }

        public INotifyExpression<T> Reduce()
        {
            Inner = Inner.Reduce();
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }
    }
}
