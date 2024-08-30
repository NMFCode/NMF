using NMF.Models;
using System;

namespace NMF.Benchmarks
{
    /// <summary>
    /// Defines an abstract benchmark analyzer job
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BenchmarkJob<T>
        where T : IModelElement
    {
        /// <summary>
        /// Creates a new benchmark job
        /// </summary>
        /// <param name="name">The jobs name</param>
        /// <param name="benchmark">The parent benchmark</param>
        public BenchmarkJob(string name, Benchmark<T> benchmark)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (benchmark == null) throw new ArgumentNullException(nameof(benchmark));

            Name = name;
            Benchmark = benchmark;
        }

        /// <summary>
        /// Gets the name of the job
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The parent benchmark
        /// </summary>
        public Benchmark<T> Benchmark { get; private set; }

        /// <summary>
        /// Initializes the job. The time taken here is measured.
        /// </summary>
        /// <param name="options">The benchmark options</param>
        public abstract void Initialize(BenchmarkOptions options);

        /// <summary>
        /// Analyzes the data and returns a reporting action
        /// </summary>
        /// <param name="options">The benchmark options</param>
        /// <returns>An action that will write the data to the reports. This is then no longer measured</returns>
        public abstract Action AnalyzeAndReport(BenchmarkOptions options);

        /// <summary>
        /// Prepares the job before initialization. The time taken here is not measured
        /// </summary>
        /// <param name="options">The benchmark options</param>
        public virtual void Prepare(BenchmarkOptions options) { }

        /// <summary>
        /// Gets the memory consumption needed by the current job
        /// </summary>
        /// <returns>The number of bytes consumed</returns>
        public abstract long GetMemoryConsumption();
    }
}
