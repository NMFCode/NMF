using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NMF.Expressions
{
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    internal class ObservableConstant<T> : Expression, INotifyExpression<T>, ISuccessorList
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        private readonly T _value;

#pragma warning disable S3237 // "value" contextual keyword should be used
#pragma warning disable S108 // Nested blocks of code should not be left empty
#pragma warning disable CA1822 // mark as static
        public event EventHandler Attached { add { } remove { } }
        public event EventHandler Detached { add { } remove { } }
#pragma warning restore CA1822 // mark as static
#pragma warning restore S108 // Nested blocks of code should not be left empty
#pragma warning restore S3237 // "value" contextual keyword should be used

        public event EventHandler<ValueChangedEventArgs> ValueChanged { add { } remove { } }

        public override string ToString()
        {
            return "[Constant " + ToDiagnosticString() + "]";
        }

        private string ToDiagnosticString()
        {
            var val = (Value != null ? Value.ToString() : "(null)");
            var index = val.IndexOfAny(new[] { '<', '>', '"', '´', '`' });
            if (index >= 0)
            {
                return val.Substring(0, index) + "...";
            }
            else
            {
                return val;
            }
        }

        public ObservableConstant(T value)
        {
            _value = value;
        }

        public override ExpressionType NodeType { get { return ExpressionType.Constant; } }

        public override bool CanReduce { get { return false; } }

        public bool CanBeConstant { get { return true; } }

        public bool IsConstant { get { return true; } }

        public bool IsParameterFree { get { return true; } }

        public IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public T Value => _value;

        public object ValueObject => _value;

        public ISuccessorList Successors => this;

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public int Count => 0;

        public bool HasSuccessors => true;

        public bool IsAttached => true;

        public IEnumerable<INotifiable> AllSuccessors => Enumerable.Empty<INotifiable>();

        public new INotifyExpression<T> Reduce()
        {
            return this;
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            throw new InvalidOperationException("A constant cannot have a dependency and therefore cannot be notified of a dependency change.");
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        public void Dispose()
        {
        }

        public void Set(INotifiable node)
        {
        }

        public void SetDummy()
        {
        }

        public void Unset(INotifiable node, bool leaveDummy = false)
        {
        }

        public void UnsetAll()
        {
        }

        public INotifiable GetSuccessor(int index)
        {
            throw new NotSupportedException();
        }
    }
}
