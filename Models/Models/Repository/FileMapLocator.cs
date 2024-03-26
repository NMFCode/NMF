using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes a locator that locates URI through a mapping to files
    /// </summary>
    public class FileMapLocator : MapLocator
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public FileMapLocator() : base() { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="mappings">the mappings</param>
        public FileMapLocator(IDictionary<Uri, string> mappings) : base(mappings) { }

        /// <inheritdoc />
        public override Stream Open(Uri repositoryId)
        {
            return new FileStream(Mappings[repositoryId], FileMode.Open, FileAccess.Read);
        }
    }
}
