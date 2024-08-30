using System;

namespace NMF.Models
{
    /// <summary>
    /// This exception is thrown if a change was attempted but the model element is locked
    /// </summary>
    public class LockedException : Exception
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public LockedException() : base("The model element could not be unlocked.") { }
    }
}
