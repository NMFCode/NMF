using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a constant value
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    public sealed class ConstantValue<T> : INotifyValue<T>
    {
        private readonly T value;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="value">The value</param>
        public ConstantValue(T value)
        {
            this.value = value;
        }

        /// <inheritdoc/>
        public IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        /// <inheritdoc/>
        public event EventHandler<ValueChangedEventArgs> ValueChanged { add { } remove { } }

        /// <inheritdoc/>
        public ISuccessorList Successors { get; } = SingletonSuccessorList.Instance;

        /// <inheritdoc/>
        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();


        /// <inheritdoc/>
        public T Value
        {
            get { return value; }
        }

        /// <inheritdoc/>
        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            throw new InvalidOperationException("A constant cannot have a dependency and therefore cannot be notified of a dependency change.");
        }

        /// <inheritdoc/>
        public void Dispose() { }
    }
}
