using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UpperBoundAttribute : Attribute
    {
        public int UpperBound { get; set; }

        public UpperBoundAttribute(int upperBound)
        {
            this.UpperBound = upperBound;
        }
    }
}
