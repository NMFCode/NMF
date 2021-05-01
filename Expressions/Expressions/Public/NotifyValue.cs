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
    public abstract class NotifyValue<T> : INotifyValue<T>, ISuccessorList
    {
        private readonly ExecutionMetaData metadata = new ExecutionMetaData();

        public abstract T Value { get; }

        public ISuccessorList Successors => this;

        public abstract IEnumerable<INotifiable> Dependencies { get; }

        public ExecutionMetaData ExecutionMetaData {  get { return metadata; } }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        protected void OnValueChanged(T oldValue, T newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        public void Dispose()
        {
            Detach();
        }

        protected virtual void Attach() { }

        protected virtual void Detach() { }

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

        public INotifiable GetSuccessor(int index)
        {
            return successors[index];
        }

        #endregion
    }

    public class NotifyReversableValue<T> : INotifyReversableValue<T>, INotifyPropertyChanged, ISuccessorList
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


        public ISuccessorList Successors => this;

        public IEnumerable<INotifiable> Dependencies { get { yield return Expression; } }

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public NotifyReversableValue(Expression<Func<T>> expression, IDictionary<string, object> parameterMappings = null)
            : this(NotifySystem.CreateReversableExpression<T>(expression.Body, null, parameterMappings)) { }

        internal NotifyReversableValue(INotifyReversableExpression<T> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            Expression = expression;
        }

        public bool IsReversable
        {
            get { return Expression.IsReversable; }
        }

        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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
