using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public interface IChangeListener
    {
        INotifiable Node { get; }

        INotificationResult AggregateChanges();
    }
}
