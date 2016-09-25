using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrainBenchmark
{
    class Program
    {
        private static readonly Dictionary<string, double> masterTimes = new Dictionary<string, double>()
        {
            ["PosLength"] = 1915400,
            ["RouteSensor"] = 159142,
            ["SemaphoreNeighbor"] = 11903400,
            ["SwitchSensor"] = 251.786,
            ["SwitchSet"] = 33543.2
        };

        static void Main(string[] args)
        {
#if !DEBUG
            Benchmark();
            //new BenchmarkSwitcher(Assembly.GetExecutingAssembly()).Run();
#else
            Profile();
#endif
        }

        private static void Profile()
        {
            var bench = new SwitchSet();
            var watch = Stopwatch.StartNew();
            int i = 0;
            for (; (i & 0x0F) != 0 || watch.ElapsedMilliseconds < 5000; i++)
            {
                bench.Parallel();
            }
            Console.WriteLine("Iterations: " + i);
        }

        private static void Benchmark()
        {
            var posLength = BenchmarkRunner.Run<PosLength>();
            var routeSensor = BenchmarkRunner.Run<RouteSensor>();
            var semaphoreNeighbor = BenchmarkRunner.Run<SemaphoreNeighbor>();
            var switchSensor = BenchmarkRunner.Run<SwitchSensor>();
            var switchSet = BenchmarkRunner.Run<SwitchSet>();

            var markdown = CreateMarkdown();
            AppendBenchmarkResult(markdown, posLength);
            AppendBenchmarkResult(markdown, routeSensor);
            AppendBenchmarkResult(markdown, semaphoreNeighbor);
            AppendBenchmarkResult(markdown, switchSensor);
            AppendBenchmarkResult(markdown, switchSet);

            File.WriteAllText(@"..\..\..\..\..\BenchmarkResults.md", markdown.ToString());
        }

        private static void AppendBenchmarkResult(StringBuilder builder, Summary summary)
        {
            var master = masterTimes[summary.Title];
            var immediate = summary.Reports[0].ResultStatistics.Median;
            var transaction = summary.Reports[1].ResultStatistics.Median;
            var parallel = summary.Reports[2].ResultStatistics.Median;

            builder.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5:0.00}x|{6:0.00}x|{7:0.00}x|{8:0.00}x|{9:0.00}x|{10:0.00}x|",
                summary.Title, NormalizeNano(master), NormalizeNano(immediate), NormalizeNano(transaction), NormalizeNano(parallel),
                master / immediate, master / transaction, master / parallel,
                immediate / transaction, immediate / parallel, transaction / parallel).AppendLine();
        }

        private static StringBuilder CreateMarkdown()
        {
            return new StringBuilder()
                .AppendLine("# Results of the TTC 2015 Train Benchmark")
                .AppendLine()
                .AppendFormat("Last Run: {0}", DateTime.Now).AppendLine()
                .AppendLine()
                .AppendLine("Test Case|Master|Immediate|Transaction|Parallel|M→I|M→T|M→P|I→T|I→P|T→P|")
                .AppendLine("---------|------|---------|-----------|--------|---|---|---|---|---|---|");
        }

        private static string NormalizeNano(double nano)
        {
            const string format = "{0:0.00} {1}s";
            if (nano >= 1000000000)
                return string.Format(format, nano / 1000000000, "");
            else if (nano >= 1000000)
                return string.Format(format, nano / 1000000, "m");
            else if (nano >= 1000)
                return string.Format(format, nano / 1000, "u");
            else
                return string.Format(format, nano, "n");
        }
    }
}
