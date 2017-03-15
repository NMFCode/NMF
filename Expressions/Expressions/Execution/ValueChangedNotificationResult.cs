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
        private readonly INotifiable source;
        private readonly T oldValue;
        private readonly T newValue;

        public bool Changed { get { return true; } }

        public INotifiable Source { get { return source; } }

        public T OldValue { get { return oldValue; } }

        public T NewValue { get { return newValue; } }

        object IValueChangedNotificationResult.OldValue { get { return oldValue; } }

        object IValueChangedNotificationResult.NewValue { get { return newValue; } }

        public ValueChangedNotificationResult(INotifiable source, T oldValue, T newValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            this.source = source;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }
}
