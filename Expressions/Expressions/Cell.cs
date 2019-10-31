using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class Cell<T> : INotifyReversableExpression<T>
    {
        private T _value;

        public bool CanBeConstant => false;

        public bool IsConstant => false;

        public bool IsParameterFree => true;

        public object ValueObject => Value;

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

        protected virtual void OnValueChanging(ValueChangedEventArgs e)
        {
            ValueChanging?.Invoke(this, e);
        }

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        public bool IsReversable => true;

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public IEnumerable<INotifiable> Dependencies => Enumerable.Empty<INotifiable>();

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        T INotifyValue<T>.Value => Value;

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public event EventHandler<ValueChangedEventArgs> ValueChanging;

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        public void Dispose()
        {
            Successors.UnsetAll();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return UnchangedNotificationResult.Instance;
        }

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
