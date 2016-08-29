using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public interface IValueChangedNotificationResult : INotificationResult
    {
        object OldValue { get; }

        object NewValue { get; }
    }

    public class ValueChangedNotificationResult<T> : IValueChangedNotificationResult
    {
        public bool Changed { get { return true; } }

        public INotifiable Source { get; private set; }

        public T OldValue { get; private set; }

        public T NewValue { get; private set; }

        object IValueChangedNotificationResult.OldValue { get { return OldValue; } }

        object IValueChangedNotificationResult.NewValue { get { return NewValue; } }

        public ValueChangedNotificationResult(INotifiable source, T oldValue, T newValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            Source = source;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
