using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    /// <summary>
    /// Denotes metadata for change propagation
    /// </summary>
    public class ExecutionMetaData
    {
        internal int TotalVisits;
        internal int RemainingVisits;

        /// <summary>
        /// Gets the last results
        /// </summary>
#pragma warning disable S3887 // Mutable, non-private fields should not be "readonly"
        public readonly NotificationResultCollection Results = new NotificationResultCollection();
#pragma warning restore S3887 // Mutable, non-private fields should not be "readonly"
    }
}
