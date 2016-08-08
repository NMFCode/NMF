using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents a simple incremental value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INotifyValue<out T> : INotifiable
    {
        /// <summary>
        /// Gets the current value
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets fired when the value changed
        /// </summary>
        event EventHandler<ValueChangedEventArgs> ValueChanged;
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
