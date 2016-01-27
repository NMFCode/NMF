using NMF.Expressions;
using NMF.Models;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NMF.Benchmarks
{
    /// <summary>
    /// A generic class to supports benchmarks of model analyzes
    /// </summary>
    /// <typeparam name="TRoot">The root model element type</typeparam>
    public class Benchmark<TRoot>
        where TRoot : IModelElement
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public Benchmark()
        {
            Repository = new ModelRepository();
            Analyzers = new List<BenchmarkJob<TRoot>>();
            Log = Console.Out;
            Reporting = Console.Out;
        }

        /// <summary>
        /// Gets the index of the current benchmark run
        /// </summary>
        public int RunIndex { get; private set; }

        /// <summary>
        /// Gets the index of the current iteration within a benchmark run if modifiers are in action
        /// </summary>
        public int Iteration { get; private set; }

        /// <summary>
        /// Gets or sets a function that modifies the model under benchmark
        /// </summary>
        public Action<int> Modifier { get; set; }

        /// <summary>
        /// Gets a list of analyzers registered for the benchmark
        /// </summary>
        public List<BenchmarkJob<TRoot>> Analyzers { get; private set; }

        /// <summary>
        /// Gets the model repository used by the benchmark
        /// </summary>
        public ModelRepository Repository { get; private set; }

        /// <summary>
        /// Gets the root model element
        /// </summary>
        public TRoot Root { get; private set; }

        /// <summary>
        /// Gets or sets the Text writer used for status logs
        /// </summary>
        public TextWriter Log { get; set; }

        /// <summary>
        /// Gets or sets the text writer used to publish benchmark results
        /// </summary>
        public TextWriter Reporting { get; set; }

        private Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Loads the model with the given path as new model root
        /// </summary>
        /// <param name="modelPath">The path to the root model</param>
        public virtual void Load(string modelPath)
        {
            Root = Repository.Resolve(modelPath).RootElements.OfType<TRoot>().Single();
        }

        /// <summary>
        /// Creates a new analzer that finds a pattern and reports the number of occurences
        /// </summary>
        /// <typeparam name="TAnalysis">The element type of the pattern</typeparam>
        /// <param name="name">The name of the analyzer</param>
        /// <param name="pattern">The analyzer pattern</param>
        public void CreateFindPatternJob<TAnalysis>(string name, Func<TRoot, IEnumerableExpression<TAnalysis>> pattern)
        {
            Analyzers.Add(new FindPatternJob<TRoot, TAnalysis>(name, this, pattern));
        }

        /// <summary>
        /// Creates a new analyzer that finds an object and reports a member
        /// </summary>
        /// <typeparam name="TAnalysis">The type of the object to find</typeparam>
        /// <param name="name">The name of the analyzer</param>
        /// <param name="objectToFind">A query that finds the object</param>
        /// <param name="reportingMember">The member that should be used for publishing the results. If set to none, ToString() is used.</param>
        public void CreateFindObjectJob<TAnalysis>(string name, Func<TRoot, Expression<Func<TAnalysis>>> objectToFind, Func<TAnalysis, string> reportingMember = null)
        {
            Analyzers.Add(new FindObjectJob<TRoot, TAnalysis>(name, this, objectToFind, reportingMember));
        }


        /// <summary>
        /// Runs the benchmark with the given command line arguments
        /// </summary>
        /// <param name="args">The commandline arguments</param>
        public void Run(string[] args)
        {
            var options = new BenchmarkOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                PrintGreeting();
                PrintWrongArgumentsHelp(options);
                Environment.Exit(1);
                return;
            }

            Run(options);
        }

        /// <summary>
        /// Runs the benchmark with the given command line arguments
        /// </summary>
        /// <param name="options">The benchmark options</param>
        public void Run(BenchmarkOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");

            if (options.LogFile != null)
            {
                Log = File.AppendText(options.LogFile);
            }

            if (options.Target != null)
            {
                Reporting = File.AppendText(options.Target);
            }

            PrintGreeting();

            if (!CheckAnalyzers(options))
            {
                Environment.Exit(2);
                return;
            }

            for (int i = 0; i < options.Runs; i++)
            {
                RunBenchmark(i, options);
            }
        }

        /// <summary>
        /// Remove analyzers that are not wanted in the current run
        /// </summary>
        /// <param name="options">The benchmark options to be used</param>
        /// <returns>False if there are analyzers wanted that do not exist</returns>
        private bool CheckAnalyzers(BenchmarkOptions options)
        {
            if (options.Analyzers != null && options.Analyzers.Count > 0)
            {
                for (int i = Analyzers.Count - 1; i >= 0; i--)
                {
                    if (!options.Analyzers.Contains(Analyzers[i].Name))
                    {
                        Analyzers.RemoveAt(i);
                    }
                    else
                    {
                        options.Analyzers.Remove(Analyzers[i].Name);
                    }
                }
                if (options.Analyzers.Count > 0)
                {
                    PrintWrongAnalyzersHelp(options);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Prints the given message as error
        /// </summary>
        /// <param name="message">Message to print</param>
        protected virtual void PrintErrorLog(string message)
        {
            if (Log == Console.Out)
            {
                Console.Error.WriteLine(message);
            }
            else
            {
                Log.WriteLine(message);
            }
        }

        /// <summary>
        /// Prints a message that some analyzers have been selected that do not exist
        /// </summary>
        /// <param name="options">The benchmark options to be used</param>
        protected virtual void PrintWrongAnalyzersHelp(BenchmarkOptions options)
        {
            PrintErrorLog(string.Format("The analyzers {0} could not be found.", string.Join(", ", options.Analyzers)));
        }

        /// <summary>
        /// Runs the benchmark at the given run index
        /// </summary>
        /// <param name="options">The benchmark options to be used</param>
        /// <param name="i">Run index</param>
        private void RunBenchmark(int i, BenchmarkOptions options)
        {
            RunIndex = i;

            stopwatch.Start();
            LoadRoot(options);
            stopwatch.Stop();
            Report(null, "Load", stopwatch.ElapsedMilliseconds, options);

            foreach (var job in Analyzers)
            {
                job.Prepare();
                stopwatch.Restart();
                job.Initialize();
                stopwatch.Stop();
                Report(job.Name, "Initialize", stopwatch.ElapsedMilliseconds, options);

                if (i == 0 && options.Memory)
                {
                    Report(job.Name, "Memory", job.GetMemoryConsumption(), options);
                }

                stopwatch.Restart();
                job.AnalyzeAndReport();
                stopwatch.Stop();
                Report(job.Name, "Validate", stopwatch.ElapsedMilliseconds, options);
            }

            if (Modifier != null)
            {
                for (int j = 0; j < options.Iterations; j++)
                {
                    Iteration = j;

                    stopwatch.Restart();
                    Modifier(j);
                    stopwatch.Stop();
                    Report(null, "Modify", stopwatch.ElapsedMilliseconds, options);

                    foreach (var job in Analyzers)
                    {
                        stopwatch.Restart();
                        var reportAction = job.AnalyzeAndReport();
                        stopwatch.Stop();
                        reportAction();
                        Report(job.Name, "Revalidate", stopwatch.ElapsedMilliseconds, options);
                    }
                }
            }
        }

        /// <summary>
        /// Creates an entry in the reporting CSV
        /// </summary>
        /// <param name="job">The job that reports or Null if this reports is not related to a job</param>
        /// <param name="phase">The action that has been done or the phase that the job is in</param>
        /// <param name="value">The value to report</param>
        /// <param name="options">The benchmark options to be used</param>
        public virtual void Report(string job, string phase, object value, BenchmarkOptions options)
        {
            Reporting.WriteLine("{0};{1};{2};{3};{4};{5};",
                options.Id,
                job,
                phase,
                RunIndex,
                Iteration,
                value);
        }

        /// <summary>
        /// Informs the user that used wrong arguments
        /// </summary>
        /// <param name="options">The benchmark options to be used</param>
        protected virtual void PrintWrongArgumentsHelp(BenchmarkOptions options)
        {
            PrintErrorLog("You are using me wrongly.");
            Log.WriteLine("The correct usage is as follows:");
            var helpText = CommandLine.Text.HelpText.AutoBuild(options);
            Log.WriteLine(helpText.RenderParsingErrorsText(options, 4));
        }

        /// <summary>
        /// Loads the root model element
        /// </summary>
        /// <param name="options">The benchmark options to be used</param>
        protected virtual void LoadRoot(BenchmarkOptions options)
        {
            Root = Repository.Resolve(options.ModelPath).RootElements.OfType<TRoot>().Single();
        }

        /// <summary>
        /// Greets the user with version information etc.
        /// </summary>
        protected virtual void PrintGreeting()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var executingAssemblyName = executingAssembly.GetName();
            var descriptionAttribute = executingAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            var titleAttribute = executingAssembly.GetCustomAttribute<AssemblyTitleAttribute>();

            var title = titleAttribute != null ? titleAttribute.Title : executingAssemblyName.Name;

            Log.WriteLine("This is {0} in version {1}.", title, executingAssemblyName.Version);
            if (descriptionAttribute != null)
            {
                Log.WriteLine(descriptionAttribute.Description);
            }
            var copyrightAttribute = executingAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            if (copyrightAttribute != null)
            {
                Log.WriteLine(copyrightAttribute.Copyright);
            }
        }
    }
}
