using System.Collections.Generic;
using CommandLine;
using System.Reflection;

namespace NMF.Benchmarks
{
    /// <summary>
    /// A base class for options to run the benchmarks
    /// </summary>
    public class BenchmarkOptions
    {
        /// <summary>
        /// Measure Memory consumption
        /// </summary>
        [Option('m', "memory", Required = false, Default = false, HelpText = "Measure Memory consumption")]
        public bool Memory { get; set; }

        /// <summary>
        /// The file to the initial model path
        /// </summary>
        [Option('s', "source", Required = true, HelpText = "The file to the initial model path")]
        public string ModelPath { get; set; }

        /// <summary>
        /// Defines the number of iterations the benchmark should be repeated
        /// </summary>
        [Option('r', "runs", Required = false, Default = 5, HelpText = "Defines the number of iterations the benchmark should be repeated")]
        public int Runs { get; set; }

        /// <summary>
        /// The number of times a modification-analysis loop should be repeated
        /// </summary>
        [Option('x', "iterations", Required = false, Default = 10, HelpText = "The number of times a modification-analysis loop should be repeated")]
        public int Iterations { get; set; }

        /// <summary>
        /// The analyzers. If no analyzer is provided, all available analyzers will be selected.
        /// </summary>
        [Option('a', "analyzers", Min = 0)]
        public List<string> Analyzers { get; set; }

        /// <summary>
        /// Determines whether the analysis benchmark should run in batch mode
        /// </summary>
        [Option('b', "batch", Required = false, HelpText = "If set, the analysis runs in batch mode")]
        public bool Batch
        {
            get
            {
                return !Incremental;
            }
            set
            {
                Incremental = !value;
            }
        }

        /// <summary>
        /// Determines whether the analysis benchmark should run in incremental mode
        /// </summary>
        [Option('i', "incremental", Required = false, HelpText = "If set, the analysis runs in incremental mode")]
        public bool Incremental { get; set; }

        /// <summary>
        /// The name of the benchmark run (used for reporting)
        /// </summary>
        [Option('n', "name", Required = false, HelpText = "The reporting id for this analysis run")]
        public string Id { get; set; }

        /// <summary>
        /// A file where the benchmark results shall be stored.
        /// </summary>
        [Option('t', "target", Required = true, HelpText = "A file where the benchmark results shall be stored.")]
        public string Target { get; set; }

        /// <summary>
        /// A file where the benchmark logs shall be stored.
        /// </summary>
        [Option('l', "log", Required = false, HelpText = "A file where the benchmark logs shall be stored.")]
        public string LogFile { get; set; }

        /// <summary>
        /// Creates new benchmark options
        /// </summary>
        public BenchmarkOptions()
        {
            Incremental = true;
            Runs = 5;
            Id = Assembly.GetExecutingAssembly().GetName().Name;
        }
    }
}
