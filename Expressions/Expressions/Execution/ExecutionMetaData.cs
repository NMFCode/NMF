using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class ExecutionMetaData
    {
        internal int TotalVisits { get; set; }
        internal int RemainingVisits { get; set; }
        internal ShortList<INotificationResult> Sources { get; } = new ShortList<INotificationResult>();
    }
}
