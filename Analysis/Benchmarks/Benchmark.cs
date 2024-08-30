﻿using NMF.Expressions;
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
        /// Gets the name of the phase that is currently executed
        /// </summary>
        public string Phase { get; private set; }

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

        private readonly Stopwatch stopwatch = new Stopwatch();

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
            var result = CommandLine.Parser.Default.ParseArguments<BenchmarkOptions>(args);
            switch (result.Tag)
            {
                case CommandLine.ParserResultType.Parsed:
                    var options = (result as CommandLine.Parsed<BenchmarkOptions>).Value;
                    Run(options);
                    break;
                case CommandLine.ParserResultType.NotParsed:
                default:
                    PrintGreeting();
                    PrintWrongArgumentsHelp(result);
                    Environment.Exit(1);
                    break;
            }
        }

        /// <summary>
        /// Runs the benchmark with the given command line arguments
        /// </summary>
        /// <param name="options">The benchmark options</param>
        public void Run(BenchmarkOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

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

            if (Log != null)
            {
                Log.Close();
            }
            if (Reporting != null)
            {
                Reporting.Close();
            }
        }

        /// <summary>
        /// Remove analyzers that are not wanted in the current run
        /// </summary>
        /// <param name="options">The benchmark options to be used</param>
        /// <returns>False if there are analyzers wanted that do not exist</returns>
        private bool CheckAnalyzers(BenchmarkOptions options)
        {
            if (options.Analyzers != null && options.Analyzers.Count() > 0)
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
                if (options.Analyzers.Count() > 0)
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

            Phase = "Load";
            stopwatch.Start();
            LoadRoot(options);
            stopwatch.Stop();
            Report(null, "Time", stopwatch.Elapsed.TotalMilliseconds, options);

            foreach (var job in Analyzers)
            {
                Phase = "Initialize";
                job.Prepare(options);
                stopwatch.Restart();
                job.Initialize(options);
                stopwatch.Stop();
                Report(job.Name, "Time", stopwatch.Elapsed.TotalMilliseconds, options);

                if (i == 0 && options.Memory)
                {
                    Report(job.Name, "Memory", job.GetMemoryConsumption(), options);
                }

                Phase = "Validate";
                stopwatch.Restart();
                job.AnalyzeAndReport(options);
                stopwatch.Stop();
                Report(job.Name, "Time", stopwatch.Elapsed.TotalMilliseconds, options);
            }

            if (Modifier != null)
            {
                for (int j = 0; j < options.Iterations; j++)
                {
                    Iteration = j;

                    Phase = "Modify";
                    stopwatch.Restart();
                    Modifier(j);
                    stopwatch.Stop();
                    Report(null, "Time", stopwatch.Elapsed.TotalMilliseconds, options);

                    foreach (var job in Analyzers)
                    {
                        Phase = "Revalidate";
                        stopwatch.Restart();
                        var reportAction = job.AnalyzeAndReport(options);
                        stopwatch.Stop();
                        reportAction();
                        Report(job.Name, "Time", stopwatch.Elapsed.TotalMilliseconds, options);
                    }
                }
            }
        }

        /// <summary>
        /// Creates an entry in the reporting CSV
        /// </summary>
        /// <param name="job">The job that reports or Null if this reports is not related to a job</param>
        /// <param name="metricName">The metric that is to be measured</param>
        /// <param name="value">The value to report</param>
        /// <param name="options">The benchmark options to be used</param>
        public void Report(string job, string metricName, object value, BenchmarkOptions options)
        {
            Report(job, Phase, metricName, value, options);
        }

        /// <summary>
        /// Creates an entry in the reporting CSV
        /// </summary>
        /// <param name="job">The job that reports or Null if this reports is not related to a job</param>
        /// <param name="phase">The action that has been done or the phase that the job is in</param>
        /// <param name="metricName">The metric that is to be measured</param>
        /// <param name="value">The value to report</param>
        /// <param name="options">The benchmark options to be used</param>
        protected virtual void Report(string job, string phase, string metricName, object value, BenchmarkOptions options)
        {
            Reporting.WriteLine("{0};{1};{2};{3};{4};{5};{6};",
                options.Id,
                job,
                phase,
                RunIndex,
                Iteration,
                metricName,
                value);
        }

        /// <summary>
        /// Informs the user that used wrong arguments
        /// </summary>
        /// <param name="options">The benchmark options to be used</param>
        protected virtual void PrintWrongArgumentsHelp(CommandLine.ParserResult<BenchmarkOptions> options)
        {
            PrintErrorLog("You are using me wrongly.");
            Log.WriteLine("The correct usage is as follows:");
            var helpText = CommandLine.Text.HelpText.AutoBuild(options);
            Log.WriteLine(helpText.ToString());
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
            var descriptionAttribute = executingAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            var titleAttribute = executingAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            var title = titleAttribute != null && titleAttribute.Length > 0 ? (titleAttribute[0] as AssemblyTitleAttribute).Title : executingAssemblyName.Name;

            Log.WriteLine("This is {0} in version {1}.", title, executingAssemblyName.Version);
            if (descriptionAttribute != null && descriptionAttribute.Length > 0)
            {
                Log.WriteLine((descriptionAttribute[0] as AssemblyDescriptionAttribute).Description);
            }
            var copyrightAttribute = executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (copyrightAttribute != null && copyrightAttribute.Length > 0)
            {
                Log.WriteLine((copyrightAttribute[0] as AssemblyCopyrightAttribute).Copyright);
            }
        }
    }
}
