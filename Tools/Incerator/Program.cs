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
using System.Text;

namespace NMF.Incerator
{
    class Incerator
    {
        private readonly ModelRepository repository;
        private readonly InceratorConfiguration options;

        public Incerator(InceratorConfiguration options, ModelRepository repository)
        {
            this.options = options;
            this.repository = repository;
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var result = CommandLine.Parser.Default.ParseArguments<InceratorConfiguration>(args);
            switch (result.Tag)
            {
                case ParserResultType.Parsed:
                    var options = (result as Parsed<InceratorConfiguration>).Value;

                    var incerator = new Incerator(options, new ModelRepository());
#if DEBUG
                    incerator.Run();
#else
                  try
                    {
                        incerator.Run();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
#endif
                    break;
                case ParserResultType.NotParsed:
                default:
                    Console.WriteLine("You are using me wrongly!");
                    Console.WriteLine("Usage: Incerator optimize [options]");
                    Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result).ToString());
                    break;
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assName = new AssemblyName(args.Name);
            if (File.Exists(assName.Name))
            {
                return Assembly.LoadFile(Path.GetFullPath(assName.Name));
            }
            else if (File.Exists(assName.Name + ".dll"))
            {
                return Assembly.LoadFile(Path.GetFullPath(assName.Name + ".dll"));
            }
            else if (File.Exists(assName.Name + ".exe"))
            {
                return Assembly.LoadFile(Path.GetFullPath(assName.Name + ".exe"));
            }
            return null;
        }

        public void Run()
        {
            switch (options.Mode)
            {
                case OperationMode.Record:
                    LoadNotifySystem(options);
                    Console.Write(".");
                    RunAnalysis(options);
                    break;
                case OperationMode.Optimize:
                    var recorder = new RecordingNotifySystem(NotifySystem.DefaultSystem);
                    NotifySystem.DefaultSystem = recorder;
                    Console.WriteLine("Recording variation points...");
                    RunAnalysis(options);
                    Console.WriteLine("Found {0} variation points. ", recorder.Configuration.MethodConfigurations.Select(c => c.AllowedStrategies.Count).Aggregate(1, (a, b) => a * b));
                    OptimizeAnalysis(recorder.Configuration);
                    break;
                default:
                    Console.WriteLine("Operation mode {0} is unknown.", options.Mode);
                    Environment.Exit(1);
                    break;
            }
        }

        private void LoadNotifySystem(InceratorConfiguration options)
        {
            var configModel = repository.Resolve(options.Configuration);
            var configuration = configModel.RootElements[0] as Configuration;
            NotifySystem.DefaultSystem = new ConfiguredNotifySystem(repository, configuration);
        }

        private void RunAnalysis(InceratorConfiguration options)
        {
            var sysOut = Console.Out;
            Console.SetOut(TextWriter.Null);

            var type = Type.GetType(options.Type);
            if (type == null) throw new InvalidOperationException("The specified analysis type does not exist.");
            if (!typeof(Analysis).IsAssignableFrom(type)) throw new InvalidOperationException("The specified analysis type is not an analysis.");

            var analysis = (Analysis)Activator.CreateInstance(type);
            analysis.Run(repository);

            Console.SetOut(sysOut);
        }

        private Assembly LoadAssembly(AssemblyName arg)
        {
            if (File.Exists(arg.Name))
            {
                return Assembly.LoadFile(Path.GetFullPath(arg.Name));
            }
            else
            {
                Console.WriteLine("No assembly could be found for the file name {0}.", arg.Name);
                return null;
            }
        }

        protected virtual IBenchmark<Configuration> CreateBenchmark()
        {
            if (options.Benchmark != null)
            {
                var benchmarkType = Type.GetType(options.Benchmark, LoadAssembly, null);
                if (benchmarkType == null) throw new InvalidOperationException("The specified benchmark type does not exist.");
                if (!typeof(IBenchmark<Configuration>).IsAssignableFrom(benchmarkType)) throw new InvalidOperationException("The specified benchmark type is not a benchmark.");

                return (IBenchmark<Configuration>)Activator.CreateInstance(benchmarkType);
            }
            return new Benchmark(options, repository);
        }

        private void OptimizeAnalysis(Configuration baseConfiguration)
        {
            IList<MeasuredConfiguration<Configuration>> measurements;
            var benchmark = CreateBenchmark();
            if (options.Repetitions > 1)
            {
                benchmark = new RepeatAverageBenchmark<Configuration>(benchmark, options.Repetitions);
            }
            switch (options.Method)
            {
                case OptimizationAlgorithm.Full:
                    measurements = PerformFullExploration(baseConfiguration, benchmark);
                    break;
                case OptimizationAlgorithm.Genetic:
                    measurements = PerformGeneticAlgorithm(baseConfiguration, benchmark, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("options", "The chosen method is not supported");
            }

            if (!options.All)
            {
                Console.WriteLine("Starting Pareto filter for dimensions Time and Memory");
                var paretoDimensions = new Dictionary<string, DimensionRating>();
                paretoDimensions.Add("Time", DimensionRating.SmallerIsBetter);
                paretoDimensions.Add("Memory", DimensionRating.SmallerIsBetter);
                var pareto = new ParetoFilter<Configuration>(paretoDimensions);

                measurements = pareto.Filter(measurements).ToList();
            }
            else
            {
                Console.WriteLine("Skipping Pareto filter");
            }

            Console.WriteLine("Saving results to disk.");
            if (measurements.Count == 1)
            {
                repository.Save(measurements[0].Configuration, options.Configuration);
            }
            else
            {
                using (var csv = new StreamWriter(Path.ChangeExtension(options.Configuration, "csv"), false))
                {
                    csv.Write("Configuration");
                    foreach (var metric in benchmark.Metrics)
                    {
                        csv.Write(";" + metric);
                    }
                    csv.WriteLine();
                    var index = 1;
                    var baseName = Path.ChangeExtension(options.Configuration, null);
                    foreach (var config in measurements)
                    {
                        var fileName = string.Format("{0}_{1}.xmi", baseName, index);
                        csv.Write(Path.GetFileNameWithoutExtension(fileName));
                        foreach (var metric in benchmark.Metrics)
                        {
                            csv.Write(";");
                            csv.Write(config.Measurements[metric]);
                        }
                        csv.WriteLine();
                        repository.Save(config.Configuration, fileName);
                        index++;
                    }
                }
            }
        }

        private static IList<MeasuredConfiguration<Configuration>> PerformGeneticAlgorithm(Configuration baseConfiguration, IBenchmark<Configuration> benchmark, InceratorConfiguration configuration)
        {
            var geneticSearch = new GeneticOptimization(benchmark, baseConfiguration);
            return geneticSearch.Optimize(configuration.Generations, configuration.PopulationSize).ToList();
        }

        private static IList<MeasuredConfiguration<Configuration>> PerformFullExploration(Configuration baseConfiguration, IBenchmark<Configuration> repBenchmark)
        {
            var counter = 1;
            var measurements = new List<MeasuredConfiguration<Configuration>>();
            var keepLastLine = true;
            foreach (var config in baseConfiguration.GetAllPossibilities())
            {
                if (!keepLastLine)
                {
                    Console.CursorTop--;
                }
                Console.Write("Running configuration {0}", counter);
                var values = repBenchmark.MeasureConfiguration(config);
                Console.WriteLine();
                if (values == null || !repBenchmark.Metrics.All(m => values[m] != double.NaN))
                {
                    Console.WriteLine("The configuration [{0}] threw an exception and is therefore discarded.", config.Describe());
                    keepLastLine = true;
                }
                else
                {
                    measurements.Add(new MeasuredConfiguration<Configuration>(config, values));
                    keepLastLine = false;
                }
                counter++;
            }
            return measurements;
        }
    }

    public class Benchmark : IBenchmark<Configuration>
    {
        public InceratorConfiguration Options { get; private set; }
        public ModelRepository Repository { get; private set; }

        public IEnumerable<string> Metrics
        {
            get
            {
                yield return "Time";
                yield return "WorkingSet";
                yield return "PagedMemory";
                yield return "VirtualMemory";
            }
        }

        public Benchmark(InceratorConfiguration options, ModelRepository repository)
        {
            Options = options;
            Repository = repository;
        }

        public IDictionary<string, double> MeasureConfiguration(Configuration configuration)
        {
            var tmpFile = Path.GetTempFileName();
            Repository.Save(configuration, tmpFile);

            var processTemplate = "{0} -c {1} -a \"{2}\"";
            var executingAssemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            var processCmd = string.Format(processTemplate, nameof(OperationMode.Record), tmpFile, Options.Type);

            var stopwatch = new Stopwatch();

            // Define variables to track the peak
            // memory usage of the process.
            long peakPagedMem = 0,
                peakWorkingSet = 0,
                peakVirtualMem = 0;

            Process recordProcess = null;

            var startInfo = new ProcessStartInfo(executingAssemblyPath, processCmd);
            startInfo.CreateNoWindow = true;
            startInfo.ErrorDialog = false;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;

            stopwatch.Start();
            try
            {
                // Start the process.
                recordProcess = Process.Start(executingAssemblyPath, processCmd);

                do
                {
                    if (!recordProcess.HasExited)
                    {
                        try
                        {
                            // Refresh the current process property values.
                            recordProcess.Refresh();

                            // Update the values for the overall peak memory statistics.
                            peakPagedMem = recordProcess.PeakPagedMemorySize64;
                            peakVirtualMem = recordProcess.PeakVirtualMemorySize64;
                            peakWorkingSet = recordProcess.PeakWorkingSet64;
                        }
                        catch (InvalidOperationException)
                        {
                        }
                    }
                }
                // refresh process statistics every 100ms
                while (!recordProcess.WaitForExit(100));

                stopwatch.Stop();

                Console.Write(".");

                if (recordProcess.ExitCode != 0)
                {
                    Console.Error.WriteLine("Recorder for configuration {0} quit with exit code {1}.", configuration.Describe(), recordProcess.ExitCode);
                    var error = recordProcess.StandardError.ReadToEnd();
                    Console.Error.WriteLine(error);
                    return null;
                }
            }
            finally
            {
                if (recordProcess != null)
                {
                    recordProcess.Close();
                }
            }

            var results = new Dictionary<string, double>();
            results.Add("Time", stopwatch.ElapsedMilliseconds);
            results.Add("WorkingSet", peakWorkingSet);
            results.Add("PagedMemory", peakPagedMem);
            results.Add("VirtualMemory", peakVirtualMem);

            return results;
        }
    }
}
