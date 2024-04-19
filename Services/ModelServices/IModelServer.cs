using System;

namespace NMF.Models.Services
{
    /// <summary>
    /// Denotes an abstract interface for a model server
    /// </summary>
    public interface IModelServer
    {
        /// <summary>
        /// Gets or creates a session for the given model uri
        /// </summary>
        /// <param name="path">The path to the model on the file system</param>
        /// <returns>The session created for this model element</returns>
        IModelSession GetOrCreateSession(string path);

        /// <summary>
        /// Gets or creates a session for the given model uri
        /// </summary>
        /// <param name="uri">The path to the model on the file system</param>
        /// <returns>The session created for this model element</returns>
        IModelSession GetOrCreateSession(Uri uri);
    }
}