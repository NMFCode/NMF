using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IdAttribute : Attribute
    {
        public bool IsId { get; set; }

        public IdAttribute(bool isId)
        {
            this.IsId = isId;
        }

        public IdAttribute() : this(true) { }
    }
}
