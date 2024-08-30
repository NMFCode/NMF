using NMF.Expressions;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Benchmarks
{
    /// <summary>
    /// Creates a new job to find patterns in a model and report their occurence
    /// </summary>
    /// <typeparam name="TRoot">The model root type</typeparam>
    /// <typeparam name="TPattern">The pattern element type</typeparam>
    public class FindPatternJob<TRoot, TPattern> : BenchmarkJob<TRoot>
        where TRoot : IModelElement
    {
        /// <summary>
        /// Gets the pattern used to find the pattern occurences
        /// </summary>
        public Func<TRoot, IEnumerableExpression<TPattern>> Pattern { get; private set; }

        private IEnumerable<TPattern> currentPattern;

        /// <summary>
        /// Creates a new job to find patterns
        /// </summary>
        /// <param name="name">The name of the job</param>
        /// <param name="benchmark">The parent benchmark</param>
        /// <param name="pattern">The pattern</param>
        public FindPatternJob(string name, Benchmark<TRoot> benchmark, Func<TRoot, IEnumerableExpression<TPattern>> pattern) : base(name, benchmark)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            Pattern = pattern;
        }

        /// <summary>
        /// Initializes the job. The time taken here is measured.
        /// </summary>
        /// <param name="options">The benchmark options</param>
        public override void Initialize(BenchmarkOptions options)
        {
            if (options.Incremental)
            {
                currentPattern = Pattern(Benchmark.Root).AsNotifiable();
            }
            else
            {
                currentPattern = Pattern(Benchmark.Root);
            }
        }

        /// <summary>
        /// Analyzes the data and returns a reporting action
        /// </summary>
        /// <param name="options">The benchmark options</param>
        /// <returns>An action that will write the data to the reports. This is then no longer measured</returns>
        public override Action AnalyzeAndReport(BenchmarkOptions options)
        {
            var count = currentPattern.Count();

            return () => Benchmark.Report(Name, "Count", count, options);
        }

        /// <summary>
        /// Gets the memory consumption needed by the current job
        /// </summary>
        /// <returns>The number of bytes consumed</returns>
        public override long GetMemoryConsumption()
        {
            return MemoryMeter.ComputeMemoryConsumption(currentPattern);
        }
    }
}
