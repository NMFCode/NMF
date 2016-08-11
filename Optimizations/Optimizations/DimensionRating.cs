using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Optimizations
{
    public enum DimensionRating
    {
        BiggerIsBetter,
        SmallerIsBetter
    }

    public static class DimensionRatingExtensions
    {
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
