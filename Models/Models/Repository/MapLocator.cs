using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    public abstract class MapLocator : IModelLocator
    {
        public IDictionary<Uri, string> Mappings { get; private set; }

        public MapLocator()
        {
            Mappings = new Dictionary<Uri, string>();
        }

        public MapLocator(IDictionary<Uri, string> mappings)
        {
            Mappings = mappings;
        }

        public bool CanLocate(Uri uri)
        {
            return Mappings != null && Mappings.ContainsKey(uri);
        }

        public Uri GetRepositoryUri(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
            {
                return uri;
            }
            // throw away the fragment
            return new Uri(uri.GetLeftPart(UriPartial.Query));
        }

        public abstract Stream Open(Uri repositoryId);
    }
}
