using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class UnchangedNotificationResult : INotificationResult
    {
        private static readonly UnchangedNotificationResult instance = new UnchangedNotificationResult();
        public static UnchangedNotificationResult Instance => instance;

        public bool Changed => false;

        public INotifiable Source => null;

        private UnchangedNotificationResult() { }
    }
}
