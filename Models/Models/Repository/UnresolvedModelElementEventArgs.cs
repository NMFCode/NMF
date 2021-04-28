using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public UnresolvedModelElementEventArgs(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");

            Uri = uri;
        }

        /// <summary>
        /// The Uri that could not be resolved
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets or sets the model element that was identified during the event
        /// </summary>
        public IModelElement ModelElement { get; set; }
    }
}
