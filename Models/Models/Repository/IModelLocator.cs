using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes an interface for a component that can locate models
    /// </summary>
    public interface IModelLocator
    {
        /// <summary>
        /// Determines whether the given locator is able to locate the given URI
        /// </summary>
        /// <param name="uri">the URI to locate</param>
        /// <returns>true, if the URI can be located, otherwise false</returns>
        bool CanLocate(Uri uri);

        /// <summary>
        /// Calculates the repository URI for the given URI
        /// </summary>
        /// <param name="uri">the input URI</param>
        /// <returns>the URI as used for a repository</returns>
        Uri GetRepositoryUri(Uri uri);

        /// <summary>
        /// Opens the given URI
        /// </summary>
        /// <param name="repositoryId">the URI as used in the repository</param>
        /// <returns>A stream</returns>
        Stream Open(Uri repositoryId);
    }
}
