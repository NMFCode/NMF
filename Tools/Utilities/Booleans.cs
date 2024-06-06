﻿using System.Collections.Generic;

namespace NMF.Utilities
{
    /// <summary>
    /// A helper class for configurations
    /// </summary>
    public static class Booleans
    {
        /// <summary>
        /// Gets a collection of the boolean values
        /// </summary>
        public static IEnumerable<bool> Values
        {
            get
            {
                yield return true;
                yield return false;
            }
        }
    }
}
