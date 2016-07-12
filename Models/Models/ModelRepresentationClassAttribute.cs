using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum)]
    public class ModelRepresentationClassAttribute : Attribute
    {
        public ModelRepresentationClassAttribute(string uriString)
        {
            UriString = uriString;
        }

        public string UriString { get; private set; }
    }
}
