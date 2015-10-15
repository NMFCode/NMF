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

        public bool CanLocate(Uri uri)
        {
            return Mappings.ContainsKey(uri);
        }

        public Uri GetRepositoryUri(Uri uri)
        {
            return uri;
        }

        public abstract Stream Open(Uri repositoryId);
    }
}
