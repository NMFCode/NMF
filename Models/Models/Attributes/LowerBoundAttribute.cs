using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// Denotes the lower bound for a given collection
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.ReturnValue | AttributeTargets.Parameter)]
    public class LowerBoundAttribute : Attribute
    {
        /// <summary>
        /// Gets the assigned lower bound
        /// </summary>
        public int LowerBound { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="lowerBound">the lower bound</param>
        public LowerBoundAttribute(int lowerBound)
        {
            this.LowerBound = lowerBound;
        }
    }
}
