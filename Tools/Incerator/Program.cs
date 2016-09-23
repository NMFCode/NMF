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
            InceratorConfiguration options = new InceratorConfiguration();
            if (Parser.Default.ParseArguments(args, options))
            {
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
            }
            else
            {
                Console.WriteLine("You are using me wrongly!");
                Console.WriteLine("Usage: Incerator optimize [options]");
                Console.WriteLine(options.GetHelp());
            }
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

            var assembly = Assembly.LoadFile(Path.GetFullPath(options.Assembly));
            if (assembly == null) throw new InvalidOperationException("The specified assembly could not be loaded.");

            var type = assembly.GetType(options.Type);
            if (type == null) throw new InvalidOperationException("The specified type does not exist.");
            if (!typeof(Analysis).IsAssignableFrom(type)) throw new InvalidOperationException("The specified type is not an analysis.");

            var analysis = (Analysis)Activator.CreateInstance(type);
            analysis.Run(repository);

            Console.SetOut(sysOut);
        }

        protected virtual IBenchmark<Configuration> CreateBenchmark()
        {
            return new Benchmark(options, repository);
        }

        private void OptimizeAnalysis(Configuration baseConfiguration)
        {
            var measurements = new List<MeasuredConfiguration<Configuration>>();
            var benchmark = new Benchmark(options, repository);
            var repBenchmark = new RepeatAverageBenchmark<Configuration>(benchmark, options.Repetitions);
            switch (options.Method)
            {
                case OptimizationAlgorithm.Full:
                    var counter = 1;
                    foreach (var config in baseConfiguration.GetAllPossibilities())
                    {
                        Console.Write("Running configuration {0}", counter);
                        var values = repBenchmark.MeasureConfiguration(config);
                        Console.WriteLine();
                        if (values != null)
                        {
                            measurements.Add(new MeasuredConfiguration<Configuration>(config, values));
                            Console.WriteLine("Configuration completed in {0}ms and took {1}bytes.", values["Time"], values["Memory"]);
                        }
                        else
                        {
                            Console.WriteLine("The configuration [{0}] threw an exception and is therefore discarded.", config.Describe());
                        }
                        counter++;
                    }
                    break;
                case OptimizationAlgorithm.NSGAII:
                    throw new NotImplementedException();
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
                    csv.WriteLine("Configuration;Time;Memory;VirtualMemory;PagedMemory");
                    var index = 1;
                    var baseName = Path.ChangeExtension(options.Configuration, null);
                    foreach (var config in measurements)
                    {
                        var fileName = string.Format("{0}_{1}.xmi", baseName, index);
                        var time = config.Measurements["Time"];
                        var memory = config.Measurements["Memory"];
                        var virtualMemory = config.Measurements["VirtualMemory"];
                        var pagedMemory = config.Measurements["PagedMemory"];
                        csv.WriteLine("{0};{1};{2};{3};{4}", 
                            Path.GetFileNameWithoutExtension(fileName),
                            time, 
                            memory, 
                            virtualMemory, 
                            pagedMemory);
                        repository.Save(config.Configuration, fileName);
                        index++;
                    }
                }
            }
        }
    }

    public class Benchmark : IBenchmark<Configuration>
    {
        public InceratorConfiguration Options { get; private set; }
        public ModelRepository Repository { get; private set; }

        public Benchmark(InceratorConfiguration options, ModelRepository repository)
        {
            Options = options;
            Repository = repository;
        }

        public IDictionary<string, double> MeasureConfiguration(Configuration configuration)
        {
            var tmpFile = Path.GetTempFileName();
            Repository.Save(configuration, tmpFile);

            var processTemplate = "{0} -c {1} -a {2} -t {3}";
            var executingAssemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            var processCmd = string.Format(processTemplate, nameof(OperationMode.Record), tmpFile, Options.Assembly, Options.Type);

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
            results.Add("Memory", peakWorkingSet);
            results.Add("PagedMemory", peakPagedMem);
            results.Add("VirtualMemory", peakVirtualMem);

            return results;
        }
    }
}
