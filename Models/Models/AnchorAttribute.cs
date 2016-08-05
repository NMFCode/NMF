using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AnchorAttribute : Attribute
    {
        public Type AnchorType { get; private set; }

        public AnchorAttribute(Type anchorType)
        {
            AnchorType = anchorType;
        }
    }
}
