using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableProxyExpression<T> : Expression, INotifyExpression<T>, ISuccessorList
    {
        protected INotifyValue<T> value;

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public ObservableProxyExpression(INotifyValue<T> value)
        {
            if (value == null) throw new ArgumentNullException("value");

            this.value = value;
        }

        private void Detach()
        {
            foreach (var dep in Dependencies)
            {
                dep.Successors.Unset(this);
            }
        }

        private void Attach()
        {
            foreach (var dep in Dependencies)
            {
                dep.Successors.Set(this);
            }
        }

        public bool CanBeConstant
        {
            get { return IsConstant; }
        }

        public bool IsParameterFree
        {
            get { return true; }
        }

        public T Value
        {
            get { return value.Value; }
        }

        public object ValueObject
        {
            get { return Value; }
        }

        public override bool CanReduce
        {
            get { return false; }
        }

        public override ExpressionType NodeType
        {
            get { return ExpressionType.Parameter; }
        }

        public override Type Type
        {
            get { return typeof(T); }
        }

        public bool IsConstant
        {
            get { return this is ConstantValue<T>; }
        }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return value;
            }
        }

        public ISuccessorList Successors => this;

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }
        
        public new INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Dispose()
        {
            Successors.UnsetAll();
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                var change = (IValueChangedNotificationResult<T>)sources[0];
                ValueChanged?.Invoke(this, new ValueChangedEventArgs(change.OldValue, Value));
                return new ValueChangedNotificationResult<T>(this, change.OldValue, Value);
            }
            return new ValueChangedNotificationResult<T>(this, Value, Value);
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }


        #region SuccessorList


        private bool isDummySet = false;
        private readonly List<INotifiable> successors = new List<INotifiable>();

        /// <inheritdoc />
        public INotifiable this[int index] { get { return successors[index]; } }

        /// <inheritdoc />
        public bool HasSuccessors => !isDummySet && successors.Count > 0;

        /// <inheritdoc />
        public bool IsAttached => isDummySet || successors.Count > 0;

        /// <inheritdoc />
        public int Count => successors.Count;

        /// <inheritdoc />
        public event EventHandler Attached;


        /// <inheritdoc />
        public event EventHandler Detached;

        /// <inheritdoc />
        public IEnumerator<INotifiable> GetEnumerator()
        {
            return successors.GetEnumerator();
        }


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
                    Attached?.Invoke(this, EventArgs.Empty);
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
                Attached?.Invoke(this, EventArgs.Empty);
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
                Detached?.Invoke(this, EventArgs.Empty);
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
                Detached?.Invoke(this, EventArgs.Empty);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    internal class ObservableProxyReversableExpression<T> : ObservableProxyExpression<T>, INotifyReversableExpression<T>
    {
        public ObservableProxyReversableExpression(INotifyReversableValue<T> value)
            : base(value) { }

        public new T Value
        {
            get
            {
                return this.value.Value;
            }
            set
            {
                var reversable = (INotifyReversableValue<T>)this.value;
                reversable.Value = value;
            }
        }

        public bool IsReversable
        {
            get
            {
                var reversable = (INotifyReversableValue<T>)this.value;
                return reversable.IsReversable;
            }
        }
    }

}
