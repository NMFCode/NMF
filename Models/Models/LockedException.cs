using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// This exception is thrown if a change was attempted but the model element is locked
    /// </summary>
    public class LockedException : Exception
    {
        public LockedException() : base("The model element could not be unlocked.") { }
    }
}
