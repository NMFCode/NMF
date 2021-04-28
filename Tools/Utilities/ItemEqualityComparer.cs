using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Utilities
{
    /// <summary>
    /// Represents a comparison of arrays based on their items
    /// </summary>
    /// <typeparam name="T">The type of the array items</typeparam>
    public class ItemEqualityComparer<T> : IEqualityComparer<T[]>
    {
        private static readonly ItemEqualityComparer<T> instance = new ItemEqualityComparer<T>();

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static ItemEqualityComparer<T> Instance { get { return instance; } }

        private ItemEqualityComparer() { }

        /// <summary>
        /// Determines whether the given arrays contain the same elements
        /// </summary>
        /// <param name="x">The first array</param>
        /// <param name="y">The second array</param>
        /// <returns>True, if both arrays contain the same elements, otherwise false</returns>
        public bool Equals(T[] x, T[] y)
        {
            if (x != null)
            {
                if (y != null)
                {
                    if (x.Length != y.Length) return false;
                    for (int i = 0; i < x.Length; i++)
                    {
                        if (!object.Equals(x[i], y[i])) return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return y == null;
            }
        }

        /// <summary>
        /// Gets a hash code of the given array
        /// </summary>
        /// <param name="obj">The given array</param>
        /// <returns>A hash code with hopefully uniform distribution</returns>
        public int GetHashCode(T[] obj)
        {
            int i = 0;
            if (obj != null)
            {
                unchecked
                {
                    for (int j = 0; j < obj.Length; j++)
                    {
                        object target = obj[j];
                        if (target != null) i ^= target.GetHashCode() + j << j;
                    }
                }
            }
            return i;
        }
    }
}
