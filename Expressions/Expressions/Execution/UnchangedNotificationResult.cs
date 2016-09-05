using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class UnchangedNotificationResult : INotificationResult
    {
        private static UnchangedNotificationResult instance = new UnchangedNotificationResult();
        public static UnchangedNotificationResult Instance { get { return instance; } }

        public bool Changed { get { return false; } }

        public INotifiable Source { get { return null; } }

        private UnchangedNotificationResult() { }
    }
}
