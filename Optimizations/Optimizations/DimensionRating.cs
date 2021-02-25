using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Optimizations
{
    /// <summary>
    /// Denotes a rating how to cope with dimensions
    /// </summary>
    public enum DimensionRating
    {
        /// <summary>
        /// Denotes that larger values in this dimensions are better
        /// </summary>
        BiggerIsBetter,
        /// <summary>
        /// Denotes that smaller values in this dimension are better
        /// </summary>
        SmallerIsBetter
    }

    /// <summary>
    /// Denotes extensions to dimension ratings
    /// </summary>
    public static class DimensionRatingExtensions
    {
        /// <summary>
        /// Determines whether the provided actual value is better than the given reference
        /// </summary>
        /// <param name="rating">The rating of the dimension</param>
        /// <param name="actual">The actual value</param>
        /// <param name="reference">The reference value</param>
        /// <returns>True, if the actual value is better, otherwise false</returns>
        public static bool IsBetter(this DimensionRating rating, double actual, double reference)
        {
            switch (rating)
            {
                case DimensionRating.BiggerIsBetter:
                    return actual > reference;
                case DimensionRating.SmallerIsBetter:
                    return actual < reference;
                default:
                    throw new ArgumentOutOfRangeException("rating");
            }
        }
    }
}
