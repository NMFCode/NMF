using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes a notification result that represents the fact that the value did not change
    /// </summary>
    public class UnchangedNotificationResult : INotificationResult
    {
        private static readonly UnchangedNotificationResult instance = new UnchangedNotificationResult();

        /// <summary>
        /// Gets the default instance
        /// </summary>
        public static UnchangedNotificationResult Instance => instance;

        /// <inheritdoc />
        public bool Changed => false;

        /// <inheritdoc />
        public INotifiable Source => null;

        private UnchangedNotificationResult() { }

        /// <inheritdoc />
        public void IncreaseReferences(int references) { }

        /// <inheritdoc />
        public void FreeReference() { }
    }
}
