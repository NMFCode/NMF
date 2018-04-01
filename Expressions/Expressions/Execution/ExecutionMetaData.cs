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
        public NotificationResultCollection Results = new NotificationResultCollection();
    }
}
