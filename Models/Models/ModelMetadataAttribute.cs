using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class ModelMetadataAttribute : Attribute
    {
        public Uri ModelUri { get; private set; }

        public string ResourceName { get; private set; }

        public ModelMetadataAttribute(string modelUri, string resourceName)
        {
            Uri parsedModelUri;
            if (Uri.TryCreate(modelUri, UriKind.Absolute, out parsedModelUri))
            {
                ModelUri = parsedModelUri;
            }
            else
            {
                ModelUri = new Uri(modelUri, UriKind.Relative);
            }
            ResourceName = resourceName;
        }
    }
}
