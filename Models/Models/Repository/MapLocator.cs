using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes an abstract locator that maps to preconfigured locations
    /// </summary>
    public abstract class MapLocator : IModelLocator
    {
        /// <summary>
        /// Gets the preconfiured mappings
        /// </summary>
        public IDictionary<Uri, string> Mappings { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public MapLocator()
        {
            Mappings = new Dictionary<Uri, string>();
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="mappings">The mappings</param>
        public MapLocator(IDictionary<Uri, string> mappings)
        {
            Mappings = mappings;
        }

        /// <inheritdoc />
        public bool CanLocate(Uri uri)
        {
            return Mappings != null && Mappings.ContainsKey(uri);
        }

        /// <inheritdoc />
        public Uri GetRepositoryUri(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
            {
                return uri;
            }
            // throw away the fragment
            return new Uri(uri.GetLeftPart(UriPartial.Query));
        }

        /// <inheritdoc />
        public abstract Stream Open(Uri repositoryId);
    }
}
