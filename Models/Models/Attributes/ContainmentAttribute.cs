using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContainmentAttribute : Attribute
    {
        public bool IsContainment { get; set; }

        public ContainmentAttribute(bool isContainment)
        {
            this.IsContainment = true;
        }

        public ContainmentAttribute() : this(true) { }
    }
}
