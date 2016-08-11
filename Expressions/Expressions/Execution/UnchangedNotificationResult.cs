using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class UnchangedNotificationResult : INotificationResult
    {
        public bool Changed { get { return false; } }

        public INotifiable Source { get; private set; }

        public UnchangedNotificationResult(INotifiable source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            Source = source;
        }
    }
}
