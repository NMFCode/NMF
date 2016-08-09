using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class ValueChangedNotificationResult<T> : INotificationResult
    {
        public bool Changed { get { return true; } }

        public INotifiable Source { get; private set; }

        public T OldValue { get; private set; }

        public T NewValue { get; private set; }

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
