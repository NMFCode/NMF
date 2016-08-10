using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class ExecutionMetaData
    {
        public int TotalVisits { get; set; }
        public int RemainingVisits { get; set; }
        public List<INotificationResult> Sources { get; private set; }

        public ExecutionMetaData()
        {
            Sources = new List<INotificationResult>();
        }
    }
}
