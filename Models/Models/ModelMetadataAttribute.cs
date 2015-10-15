using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class ModelMetadataAttribute : Attribute
    {
        public string ModelUri { get; private set; }

        public string ResourceName { get; private set; }

        public ModelMetadataAttribute(string modelUri, string resourceName)
        {
            ModelUri = modelUri;
            ResourceName = resourceName;
        }
    }
}
