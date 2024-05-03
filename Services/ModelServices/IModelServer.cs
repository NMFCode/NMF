using NMF.Expressions;
using NMF.Models.Repository;
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
        /// <param name="uri">The path to the model on the file system</param>
        /// <param name="path">The path to the model on the file system</param>
        /// <returns>The session created for this model element</returns>
        IModelSession GetOrCreateSession(Uri uri, string path);

        /// <summary>
        /// Gets or creates a session for the given model uri
        /// </summary>
        /// <param name="uri">The path to the model on the file system</param>
        /// <returns>The session created for this model element</returns>
        IModelSession GetOrCreateSession(Uri uri);

        /// <summary>
        /// Gets the selected element from this server
        /// </summary>
        IModelElement SelectedElement { get; }

        /// <summary>
        /// Gets raised when the selected element changed
        /// </summary>
        event EventHandler SelectedElementChanged;

        /// <summary>
        /// Gets the model repository for this server
        /// </summary>
        IModelRepository Repository { get; }
    }
}