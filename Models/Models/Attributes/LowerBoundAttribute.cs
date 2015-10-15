using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.ReturnValue | AttributeTargets.Parameter)]
    public class LowerBoundAttribute : Attribute
    {
        public int LowerBound { get; set; }

        public LowerBoundAttribute(int lowerBound)
        {
            this.LowerBound = lowerBound;
        }
    }
}
