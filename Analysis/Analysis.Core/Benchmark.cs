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
        /// Creates the default benchmark options
        /// </summary>
        /// <returns>An object with the benchmark options</returns>
        protected virtual BenchmarkOptions CreateBenchmarkOptions()
        {
            return null;
        }

        /// <summary>
        /// Public constructor
        /// </summary>
        public Benchmark()
        {
            Repository = new ModelRepository();
            Analyzers = new List<BenchmarkJob<TRoot>>();
            Options = CreateBenchmarkOptions() ?? new BenchmarkOptions();
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
        /// Gets the options used for configuration
        /// </summary>
        public BenchmarkOptions Options { get; private set; }

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
        /// <param name="args">The command line arguments</param>
        public void Run(string[] args)
        {
            if (!CommandLine.Parser.Default.ParseArguments(args, Options))
            {
                PrintGreeting();
                PrintWrongArgumentsHelp();
                Environment.Exit(1);
                return;
            }

            if (Options.LogFile != null)
            {
                Log = File.AppendText(Options.LogFile);
            }

            if (Options.Target != null)
            {
                Reporting = File.AppendText(Options.Target);
            }

            PrintGreeting();

            if (!CheckAnalyzers())
            {
                Environment.Exit(2);
                return;
            }

            for (int i = 0; i < Options.Runs; i++)
            {
                RunBenchmark(i);
            }
        }

        /// <summary>
        /// Remove analyzers that are not wanted in the current run
        /// </summary>
        /// <returns>False if there are analyzers wanted that do not exist</returns>
        private bool CheckAnalyzers()
        {
            if (Options.Analyzers != null && Options.Analyzers.Count > 0)
            {
                for (int i = Analyzers.Count - 1; i >= 0; i--)
                {
                    if (!Options.Analyzers.Contains(Analyzers[i].Name))
                    {
                        Analyzers.RemoveAt(i);
                    }
                    else
                    {
                        Options.Analyzers.Remove(Analyzers[i].Name);
                    }
                }
                if (Options.Analyzers.Count > 0)
                {
                    PrintWrongAnalyzersHelp();
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
        protected virtual void PrintWrongAnalyzersHelp()
        {
            PrintErrorLog(string.Format("The analyzers {0} could not be found.", string.Join(", ", Options.Analyzers)));
        }

        /// <summary>
        /// Runs the benchmark at the given run index
        /// </summary>
        /// <param name="i">Run index</param>
        private void RunBenchmark(int i)
        {
            RunIndex = i;

            stopwatch.Start();
            LoadRoot();
            stopwatch.Stop();
            Report(null, "Load", stopwatch.ElapsedMilliseconds);

            foreach (var job in Analyzers)
            {
                job.Prepare();
                stopwatch.Restart();
                job.Initialize();
                stopwatch.Stop();
                Report(job.Name, "Initialize", stopwatch.ElapsedMilliseconds);

                if (i == 0 && Options.Memory)
                {
                    Report(job.Name, "Memory", job.GetMemoryConsumption());
                }

                stopwatch.Restart();
                job.AnalyzeAndReport();
                stopwatch.Stop();
                Report(job.Name, "Validate", stopwatch.ElapsedMilliseconds);
            }

            if (Modifier != null)
            {
                for (int j = 0; j < Options.Iterations; j++)
                {
                    Iteration = j;

                    stopwatch.Restart();
                    Modifier(j);
                    stopwatch.Stop();
                    Report(null, "Modify", stopwatch.ElapsedMilliseconds);

                    foreach (var job in Analyzers)
                    {
                        stopwatch.Restart();
                        var reportAction = job.AnalyzeAndReport();
                        stopwatch.Stop();
                        reportAction();
                        Report(job.Name, "Revalidate", stopwatch.ElapsedMilliseconds);
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
        public virtual void Report(string job, string phase, object value)
        {
            Reporting.WriteLine("{0};{1};{2};{3};{4};{5};",
                Options.Id,
                job,
                phase,
                RunIndex,
                Iteration,
                value);
        }

        /// <summary>
        /// Informs the user that used wrong arguments
        /// </summary>
        protected virtual void PrintWrongArgumentsHelp()
        {
            PrintErrorLog("You are using me wrongly.");
            Log.WriteLine("The correct usage is as follows:");
            var helpText = CommandLine.Text.HelpText.AutoBuild(Options);
            Log.WriteLine(helpText.RenderParsingErrorsText(Options, 4));
        }

        /// <summary>
        /// Loads the root model element
        /// </summary>
        protected virtual void LoadRoot()
        {
            Root = Repository.Resolve(Options.ModelPath).RootElements.OfType<TRoot>().Single();
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
