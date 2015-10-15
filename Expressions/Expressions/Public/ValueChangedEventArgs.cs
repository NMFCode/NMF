using System;

namespace NMF.Expressions
{
    /// <summary>
    /// Represents the event data when the value of an incremental expression has changed
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a valuechanged event for the given old value and new value
        /// </summary>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        public ValueChangedEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// The old value
        /// </summary>
        public object OldValue { get; private set; }

        /// <summary>
        /// The new value
        /// </summary>
        public object NewValue { get; private set; }
    }
}
