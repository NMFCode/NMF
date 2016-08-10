using CommandLine;
using NMF.Analyses;
using NMF.Expressions;
using NMF.Expressions.IncrementalizationConfiguration;
using NMF.Models.Repository;
using NMF.Optimizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NMF.Incerator
{
    class Incerator
    {
        internal static readonly ModelRepository repository = new ModelRepository();

        static void Main(string[] args)
        {
            InceratorConfiguration options = new InceratorConfiguration();
            if (Parser.Default.ParseArguments(args, options))
            {
#if DEBUG
                RunIncerator(options);
#else
                try
                {
                    RunIncerator(options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
#endif
            }
            else
            {
                Console.WriteLine("You are using me wrongly!");
                Console.WriteLine("Usage: Incerator optimize [options]");
                Console.WriteLine(options.GetHelp());
            }
        }

        private static void RunIncerator(InceratorConfiguration options)
        {
            switch (options.Mode)
            {
                case OperationMode.Record:
                    LoadNotifySystem(options);
                    RunAnalysis(options);
                    break;
                case OperationMode.Optimize:
                    var recorder = new RecordingNotifySystem(NotifySystem.DefaultSystem);
                    NotifySystem.DefaultSystem = recorder;
                    RunAnalysis(options);
                    OptimizeAnalysis(options, recorder.Configuration);
                    break;
                default:
                    Console.WriteLine("Operation mode {0} is unknown.", options.Mode);
                    Environment.Exit(1);
                    break;
            }
        }

        private static void LoadNotifySystem(InceratorConfiguration options)
        {
            var configModel = repository.Resolve(options.Configuration);
            var configuration = configModel.RootElements[0] as Configuration;
            NotifySystem.DefaultSystem = new ConfiguredNotifySystem(repository, configuration);
        }

        private static void RunAnalysis(InceratorConfiguration options)
        {
            var assembly = Assembly.LoadFile(options.Assembly);
            if (assembly == null) throw new InvalidOperationException("The specified assembly could not be loaded.");

            var type = assembly.GetType(options.Type);
            if (type == null) throw new InvalidOperationException("The specified type does not exist.");
            if (!typeof(Analysis).IsAssignableFrom(type)) throw new InvalidOperationException("The specified type is not an analysis.");

            var analysis = (Analysis)Activator.CreateInstance(type);
            analysis.Run(repository);
        }

        private static void OptimizeAnalysis(InceratorConfiguration options, Configuration baseConfiguration)
        {
            var measurements = new List<MeasuredConfiguration<Configuration>>();
            var benchmark = new Benchmark(options);
            var repBenchmark = new RepeatAverageBenchmark<Configuration>(benchmark, options.Repetitions);
            switch (options.Method)
            {
                case OptimizationAlgorithm.Full:
                    foreach (var config in baseConfiguration.GetAllPossibilities())
                    {
                        var values = repBenchmark.MeasureConfiguration(config);
                        if (values != null)
                        {
                            measurements.Add(new MeasuredConfiguration<Configuration>(config, values));
                        }
                    }
                    break;
                case OptimizationAlgorithm.NSGAII:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException("options", "The chosen method is not supported");
            }

            var paretoDimensions = new Dictionary<string, DimensionRating>();
            paretoDimensions.Add("Time", DimensionRating.SmallerIsBetter);
            paretoDimensions.Add("Memory", DimensionRating.SmallerIsBetter);
            var pareto = new ParetoFilter<Configuration>(paretoDimensions);

            var results = pareto.Filter(measurements).ToList();

            if (results.Count == 1)
            {

            }
            else
            {

            }
        }
    }

    public class Benchmark : IBenchmark<Configuration>
    {
        public InceratorConfiguration Options { get; private set; }

        public Benchmark(InceratorConfiguration options)
        {
            Options = options;
        }

        public IDictionary<string, double> MeasureConfiguration(Configuration configuration)
        {
            var tmpFile = Path.GetTempFileName();
            Incerator.repository.Save(configuration, tmpFile);

            var processTemplate = "{0} -c {1} -a {2} -t {3}";
            var executingAssemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            var processCmd = string.Format(processTemplate, nameof(OperationMode.Record), tmpFile, Options.Assembly, Options.Type);
            
            var process = Process.Start(executingAssemblyPath, processCmd);
            var memory = process.VirtualMemorySize64;
            process.WaitForExit();

            if (process.ExitCode != 0) return null;

            var results = new Dictionary<string, double>();
            results.Add("Time", (process.ExitTime - process.StartTime).TotalMilliseconds);
            results.Add("Memory", process.PeakWorkingSet64);
            results.Add("VirtualMemory", process.PeakVirtualMemorySize64);
            results.Add("PagedMemory", process.PeakPagedMemorySize64);
            results.Add("TotalTime", process.TotalProcessorTime.TotalMilliseconds);
            results.Add("UserTime", process.UserProcessorTime.TotalMilliseconds);
            results.Add("PrivelegedTime", process.PrivilegedProcessorTime.TotalMilliseconds);
            return results;
        }
    }
}
