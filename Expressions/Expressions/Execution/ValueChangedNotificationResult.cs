using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes the notification that a value was changed
    /// </summary>
    public interface IValueChangedNotificationResult : INotificationResult
    {
        /// <summary>
        /// Gets the old value
        /// </summary>
        object OldValue { get; }

        /// <summary>
        /// Gets the new value
        /// </summary>
        object NewValue { get; }
    }

    /// <summary>
    /// Denotes the notification that a value was changed
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    public interface IValueChangedNotificationResult<out T> : IValueChangedNotificationResult
    {
        /// <summary>
        /// Gets the old value
        /// </summary>
        new T OldValue { get; }

        /// <summary>
        /// Gets the new value
        /// </summary>
        new T NewValue { get; }
    }

    /// <summary>
    /// Denotes the default implementation of a value change result
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    public class ValueChangedNotificationResult<T> : IValueChangedNotificationResult<T>
    {
        private readonly INotifiable source;
        private readonly T oldValue;
        private readonly T newValue;

        /// <inheritdoc />
        public bool Changed { get { return true; } }

        /// <inheritdoc />
        public INotifiable Source { get { return source; } }

        /// <inheritdoc />
        public T OldValue { get { return oldValue; } }

        /// <inheritdoc />
        public T NewValue { get { return newValue; } }

        object IValueChangedNotificationResult.OldValue { get { return oldValue; } }

        object IValueChangedNotificationResult.NewValue { get { return newValue; } }

        /// <summary>
        /// Creates a new notification result
        /// </summary>
        /// <param name="source">The source DDG node</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        public ValueChangedNotificationResult(INotifiable source, T oldValue, T newValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            this.source = source;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        void INotificationResult.IncreaseReferences(int references) { }

        void INotificationResult.FreeReference() { }
    }
}
