using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace NMF.Incerator
{
    public class InceratorConfiguration
    {
        public InceratorConfiguration()
        {
            Method = OptimizationAlgorithm.Full;
            Repetitions = 5;
            Mode = OperationMode.Record;
        }

        [Option('c', "configuration", HelpText = "The change configuration", Required = true)]
        public string Configuration { get; set; }

        [Option("all", DefaultValue = false, HelpText = "Print results for all configurations (skip Pareto-filter)", Required = false)]
        public bool All { get; set; }

        [Option('a', "analysis", HelpText = "The implementation of the analysis as an assembly-qualified type. The assembly path may be a relative path.", Required = true)]
        public string Type { get; set; }

        [Option('m', "method", HelpText = "The method how the analysis should be optimized. Allowed values are Full or NSGAII.", Required = false, DefaultValue = OptimizationAlgorithm.Full)]
        public OptimizationAlgorithm Method { get; set; }

        [Option('n', "repetitions", HelpText = "A number specifying how often each measurement should be repeated", Required = false, DefaultValue = 5)]
        public int Repetitions { get; set; }

        [Option('b', "benchmark", HelpText = "The assembly-qualified name of a benchmark class. The assembly may be specified in a relative path. If not provided, a default benchmark implementation is chosen.", Required = false)]
        public string Benchmark { get; set; }

        [ValueOption(0)]
        public OperationMode Mode { get; set; }


        public string GetHelp()
        {
            return HelpText.AutoBuild(this);
        }
    }

    public enum OperationMode
    {
        Record = 0,
        Optimize
    }

    public enum OptimizationAlgorithm
    {
        Full = 0,
        NSGAII
    }
}
