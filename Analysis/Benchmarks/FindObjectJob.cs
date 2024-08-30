using NMF.Expressions;
using NMF.Models;
using System;
using System.Linq.Expressions;

namespace NMF.Benchmarks
{
    /// <summary>
    /// Defines a job to find an object in a model
    /// </summary>
    /// <typeparam name="TRoot">The root model elements type</typeparam>
    /// <typeparam name="TObject">The target model element type</typeparam>
    public class FindObjectJob<TRoot, TObject> : BenchmarkJob<TRoot>
        where TRoot : IModelElement
    {
        /// <summary>
        /// Gets the query to find the object in a model
        /// </summary>
        public Func<TRoot, Expression<Func<TObject>>> Query { get; private set; }

        /// <summary>
        /// Gets the member used for reporting
        /// </summary>
        public Func<TObject, string> ReportingMember { get; set; }

        private Func<TObject> functionCompiled;
        private INotifyValue<TObject> incrementalValue;

        /// <summary>
        /// Creates a new job to find an object
        /// </summary>
        /// <param name="name">The jobs name</param>
        /// <param name="benchmark">The parent benchmark</param>
        /// <param name="query">The query to find the object</param>
        /// <param name="reportingMember">The member used for reporting</param>
        public FindObjectJob(string name, Benchmark<TRoot> benchmark, Func<TRoot, Expression<Func<TObject>>> query, Func<TObject, string> reportingMember) : base(name, benchmark)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            ReportingMember = reportingMember ?? (o => o != null ? o.ToString() : "<null>");
            Query = query;
        }

        /// <summary>
        /// Analyzes the data and returns a reporting action
        /// </summary>
        /// <param name="options">The benchmark options</param>
        /// <returns>An action that will write the data to the reports. This is then no longer measured</returns>
        public override Action AnalyzeAndReport(BenchmarkOptions options)
        {
            TObject obj;
            if (incrementalValue != null)
            {
                obj = incrementalValue.Value;
            }
            else
            {
                obj = functionCompiled();
            }
            return () => Benchmark.Report(Name, "Value", obj, options);
        }

        /// <summary>
        /// Initializes the job. The time taken here is measured.
        /// </summary>
        /// <param name="options">The benchmark options</param>
        public override void Initialize(BenchmarkOptions options)
        {
            if (options.Incremental)
            {
                incrementalValue = Observable.Expression(Query(Benchmark.Root));
            }
        }

        /// <summary>
        /// Prepares the job before initialization. The time taken here is not measured
        /// </summary>
        /// <param name="options">The benchmark options</param>
        public override void Prepare(BenchmarkOptions options)
        {
            functionCompiled = Query(Benchmark.Root).Compile();
        }

        /// <summary>
        /// Gets the memory consumption needed by the current job
        /// </summary>
        /// <returns>The number of bytes consumed</returns>
        public override long GetMemoryConsumption()
        {
            if (incrementalValue != null)
            {
                return MemoryMeter.ComputeMemoryConsumption(incrementalValue);
            }
            else
            {
                return MemoryMeter.ComputeMemoryConsumption(functionCompiled);
            }
        }
    }
}
