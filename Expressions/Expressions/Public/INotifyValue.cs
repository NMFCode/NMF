using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents a simple incremental value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INotifyValue<out T>
    {
        /// <summary>
        /// Gets the current value
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets fired when the value changed
        /// </summary>
        event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Attach a listener to this value
        /// </summary>
        void Detach();

        /// <summary>
        /// Detach a listener to this value
        /// </summary>
        void Attach();

        /// <summary>
        /// Returns whether this value listens for changes
        /// </summary>
        bool IsAttached { get; }
    }

    /// <summary>
    /// Represents a reversable incremental value
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    public interface INotifyReversableValue<T> : INotifyValue<T>
    {
        /// <summary>
        /// Gets or sets the current value
        /// </summary>
        new T Value { get; set; }

        /// <summary>
        /// Checks whether it is allowed to set values
        /// </summary>
        bool IsReversable { get; }
    }
}
