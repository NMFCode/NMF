using System;
using System.IO;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes a locator based on the file system
    /// </summary>
    public class FileLocator : IModelLocator
    {
        private static readonly FileLocator instance = new FileLocator();

        /// <summary>
        /// Gets the singleton instance of the file locator
        /// </summary>
        public static FileLocator Instance { get { return instance; } }

        /// <inheritdoc />
        public bool CanLocate(Uri uri)
        {
            return uri != null && ((uri.IsAbsoluteUri && uri.IsFile && File.Exists(uri.LocalPath)) || (!uri.IsAbsoluteUri && File.Exists(uri.OriginalString)));
        }

        /// <inheritdoc />
        public Uri GetRepositoryUri(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (uri.IsAbsoluteUri)
            {
                return new Uri(uri.GetLeftPart(UriPartial.Query), UriKind.Absolute);
            }
            else
            {
                return new Uri(Path.GetFullPath(uri.OriginalString));
            }
        }

        /// <inheritdoc />
        public Stream Open(Uri repositoryId)
        {
            if (repositoryId == null) throw new ArgumentNullException(nameof(repositoryId));
            if (repositoryId.IsAbsoluteUri)
            {
                return new FileStream(repositoryId.LocalPath, FileMode.Open, FileAccess.Read);
            }
            else
            {
                return new FileStream(repositoryId.OriginalString, FileMode.Open, FileAccess.Read);
            }
        }
    }
}
