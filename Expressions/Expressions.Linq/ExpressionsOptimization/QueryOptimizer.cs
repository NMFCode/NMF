using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NMF.Expressions
{
    /// <summary>
    /// This class encapsulates a default query optimizer
    /// </summary>
    static class QueryOptimizer
    {
        static QueryOptimizer()
        {
            defaultQueryOptimizer = new ProjectionMergeQueryOptimizer();
        }

        private static IQueryOptimizer defaultQueryOptimizer;

        /// <summary>
        /// Gets or sets the incremental computation system to be used by default
        /// </summary>
        /// <remarks>This property can never be set to a null value</remarks>
        public static IQueryOptimizer DefaultQueryOptimizer
        {
            get
            {
                return defaultQueryOptimizer;
            }
            set
            {
                defaultQueryOptimizer = value ?? defaultQueryOptimizer;
            }
        }
    }
}