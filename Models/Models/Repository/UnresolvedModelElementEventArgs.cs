﻿using System;

namespace NMF.Models.Repository
{
    /// <summary>
    /// Denotes the event data that a model element could not be resolved
    /// </summary>
    public class UnresolvedModelElementEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="uri">The uri that could not be resolved</param>
        public UnresolvedModelElementEventArgs(Uri uri) : this(uri, null) { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="uri">The uri that could not be resolved</param>
        /// <param name="hintPath">A hint path, if available</param>
        public UnresolvedModelElementEventArgs(Uri uri, string hintPath)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            Uri = uri;
            HintPath = hintPath;
        }


        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="uri">The uri that could not be resolved</param>
        /// <param name="hintPath">A hint path, if available</param>
        /// <param name="loadException">Exception that happened during model loading, if any</param>
        public UnresolvedModelElementEventArgs(Uri uri, string hintPath, Exception loadException)
            : this(uri, hintPath)
        {
            LoadException = loadException;
        }

        /// <summary>
        /// The Uri that could not be resolved
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// A hint path
        /// </summary>
        public string HintPath { get; }

        /// <summary>
        /// Exception while model was loaded, if any
        /// </summary>
        public Exception LoadException { get; }

        /// <summary>
        /// Gets or sets the model element that was identified during the event
        /// </summary>
        public IModelElement ModelElement { get; set; }
    }
}
