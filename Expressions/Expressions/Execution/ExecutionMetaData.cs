using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class ExecutionMetaData
    {
        internal int TotalVisits;
        internal int RemainingVisits;
        internal List<INotificationResult> Sources { get; } = new List<INotificationResult>();
        internal INotificationResult Result;
    }
}
