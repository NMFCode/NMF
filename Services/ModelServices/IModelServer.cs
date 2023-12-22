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
        /// <typeparam name="T">The expected type of root element</typeparam>
        /// <param name="path">The path to the model on the file system</param>
        /// <returns>The session created for this model element</returns>
        IModelSession<T> GetOrCreateSession<T>(string path) where T : class, IModelElement;

        /// <summary>
        /// Gets or creates a session for the given model uri
        /// </summary>
        /// <typeparam name="T">The expected type of root element</typeparam>
        /// <param name="uri">The path to the model on the file system</param>
        /// <returns>The session created for this model element</returns>
        IModelSession<T> GetOrCreateSession<T>(Uri uri) where T : class, IModelElement;
    }
}