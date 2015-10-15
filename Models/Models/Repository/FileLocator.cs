using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    public class FileLocator : IModelLocator
    {
        private static FileLocator instance = new FileLocator();

        public static FileLocator Instance { get { return instance; } }

        public bool CanLocate(Uri uri)
        {
            return uri != null && uri.IsAbsoluteUri && uri.IsFile;
        }

        public Uri GetRepositoryUri(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            return new Uri(uri.GetLeftPart(UriPartial.Query), UriKind.Absolute);
        }

        public Stream Open(Uri repositoryId)
        {
            if (repositoryId == null) throw new ArgumentNullException("uri");
            return new FileStream(repositoryId.LocalPath, FileMode.Open, FileAccess.Read);
        }
    }
}
