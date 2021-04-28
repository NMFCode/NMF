using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// Denotes that the underlying collection has an upper bound
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UpperBoundAttribute : Attribute
    {
        /// <summary>
        /// Gets the upper bound
        /// </summary>
        public int UpperBound { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="upperBound">the upper bound</param>
        public UpperBoundAttribute(int upperBound)
        {
            this.UpperBound = upperBound;
        }
    }
}
