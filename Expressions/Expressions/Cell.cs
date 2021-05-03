using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an atomic mutable value
    /// </summary>
    /// <typeparam name="T">The type of the cell</typeparam>
    public class Cell<T> : INotifyReversableExpression<T>
    {
        private T _value;

        /// <inheritdoc />
        public bool CanBeConstant => false;

        /// <inheritdoc />
        public bool IsConstant => false;

        /// <inheritdoc />
        public bool IsParameterFree => true;

        /// <inheritdoc />
        public object ValueObject => Value;

        /// <inheritdoc />
        public T Value
        {
            get => _value; set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    var e = new ValueChangedEventArgs(_value, value);
                    OnValueChanging(e);
                    _value = value;
                    ExecutionEngine.Current.InvalidateNode(this);
                    OnValueChanged(e);
                }
            }
        }

        /// <summary>
        /// Gets called when the value of this cell is about to change
        /// </summary>
        /// <param name="e">the event data</param>
        protected virtual void OnValueChanging(ValueChangedEventArgs e)
        {
            ValueChanging?.Invoke(this, e);
        }

        /// <summary>
        /// Gets called when the value of this cell changed
        /// </summary>
        /// <param name="e">the event data</param>
        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        /// <inheritdoc />
        public bool IsReversable => true;

        /// <inheritdoc />
        public ISuccessorList Successors { get; } = new MultiSuccessorList();

        /// <inheritdoc />
        public IEnumerable<INotifiable> Dependencies => Enumerable.Empty<INotifiable>();

        /// <inheritdoc />
        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        T INotifyValue<T>.Value => Value;

        /// <inheritdoc />
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <inheritdoc />
        public event EventHandler<ValueChangedEventArgs> ValueChanging;

        /// <inheritdoc />
        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Successors.UnsetAll();
        }

        /// <inheritdoc />
        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return UnchangedNotificationResult.Instance;
        }

        /// <inheritdoc />
        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }
    }
}
