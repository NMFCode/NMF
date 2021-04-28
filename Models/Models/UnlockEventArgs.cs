using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// Denotes the event that an unlock of a model element was requested
    /// </summary>
    public class UnlockEventArgs : EventArgs
    {
        /// <summary>
        /// The model element for which the unlock was requested
        /// </summary>
        public IModelElement Element { get; }

        /// <summary>
        /// Gets or sets whether the model element may be unlocked
        /// </summary>
        public bool MayUnlock { get; set; } = true;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="element">The element that was requested to get unlocked</param>
        public UnlockEventArgs(IModelElement element)
        {
            Element = element;
        }
    }
}
