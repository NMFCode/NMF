using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace NMF.Expressions
{
    /// <summary>
    /// The common base class for incremental expressions
    /// </summary>
    /// <typeparam name="T">The element type of the expression</typeparam>
    public abstract class NotifyExpression<T> : NotifyExpressionBase, INotifyExpression<T>, IValueChangedNotificationResult<T>, ISuccessorList
    {
        /// <summary>
        /// Creates a new instance
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
        private T oldValue;

        /// <inheritdoc />
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Gets the current value of this expression
        /// </summary>
        public T Value { get { return value; } }

        /// <summary>
        /// Gets the current value as object
        /// </summary>
        public object ValueObject { get { return Value; } }

        /// <summary>
        /// Gets the type of this incremental expression
        /// </summary>
        public sealed override Type Type { get { return typeof(T); } }

        /// <summary>
        /// Determines whether this expression can be reduced
        /// </summary>
        public override bool CanReduce { get { return CanBeConstant; } }

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
        /// Returns whether this expression is parameter free
        /// </summary>
        public abstract bool IsParameterFree { get; }

        /// <summary>
        /// Returns whether this expression is a constant value
        /// </summary>
        public virtual bool IsConstant
        {
            get { return false; }
        }

        /// <inheritdoc />
        public ISuccessorList Successors => this;

        /// <inheritdoc />
        public abstract IEnumerable<INotifiable> Dependencies { get; }

        /// <inheritdoc />
        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        /// <inheritdoc />
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
            value = GetValue();
        }

        private void Detach()
        {
            OnDetach();
            foreach (var dep in Dependencies)
                dep.Successors.Unset(this);
        }

        /// <summary>
        /// Occurs when this node gets (re)attached to another node for the first time
        /// </summary>
        protected virtual void OnAttach() { }

        /// <summary>
        /// Occurs when the last successor of this node gets removed
        /// </summary>
        protected virtual void OnDetach() { }

        /// <summary>
        /// Simplifies the current expression
        /// </summary>
        /// <returns>A simpler expression representing the same incremental value (e.g. a constant if this expression can be constant), otherwise itself</returns>
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
        /// <returns>A simpler expression representing the same incremental value (e.g. a constant if this expression can be constant), otherwise itself</returns>
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
            var handler = ValueChanged;
            if (handler != null)
            {
                handler.Invoke(this, new ValueChangedEventArgs(oldVal, newVal));
            }
        }

        /// <summary>
        /// Refreshes the current value
        /// </summary>
        public virtual INotificationResult Notify(IList<INotificationResult> sources)
        {
            var newVal = GetValue();
            if (!EqualityComparer<T>.Default.Equals(value, newVal))
            {
                oldValue = value;
                value = newVal;
                OnValueChanged(oldValue, newVal);
                return this;
            }
            return UnchangedNotificationResult.Instance;
        }

        public override string ToString()
        {
            try
            {
                return "[" + NodeType.ToString() + "]";
            }
            catch (InvalidOperationException)
            {
                return "[" + GetType().Name + "]";
            }
        }

        /// <summary>
        /// Applies the given set of parameters to the expression
        /// </summary>
        /// <param name="parameters">A set of parameter values</param>
        /// <param name="trace">A trace to make sure parameters are only applied once for every DDG node</param>
        /// <returns>A new expression with all parameter placeholders replaced with the parameter values</returns>
        /// <remarks>In case that the current expression is parameter free, it simply returns itself</remarks>
        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            INotifiable traced;
            if (!trace.TryGetValue(this, out traced))
            {
                traced = ApplyParametersCore(parameters, trace);
                trace.Add(this, traced);
            }
            return traced as INotifyExpression<T>;
        }

        /// <summary>
        /// Applies the given set of parameters to the expression
        /// </summary>
        /// <param name="parameters">A set of parameter values</param>
        /// <param name="trace">A trace to make sure parameters are only applied once for every DDG node</param>
        /// <returns>A new expression with all parameter placeholders replaced with the parameter values</returns>
        /// <remarks>In case that the current expression is parameter free, it simply returns itself</remarks>
        protected abstract INotifyExpression<T> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace);

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        #region value change notification

        T IValueChangedNotificationResult<T>.OldValue { get { return oldValue; } }

        T IValueChangedNotificationResult<T>.NewValue { get { return value; } }

        object IValueChangedNotificationResult.OldValue { get { return oldValue; } }

        object IValueChangedNotificationResult.NewValue { get { return value; } }

        INotifiable INotificationResult.Source { get { return this; } }

        bool INotificationResult.Changed { get { return true; } }

        void INotificationResult.IncreaseReferences(int references) { }

        void INotificationResult.FreeReference() { }
        #endregion

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
}
