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

        [Option('a', "assembly", HelpText = "The assembly that should be optimized", Required = true)]
        public string Assembly { get; set; }

        [Option('t', "type", HelpText = "The analysis type that should be optímized", Required = true)]
        public string Type { get; set; }

        [Option('m', "method", HelpText = "The method how the analysis should be optimized. Allowed values are Full or NSGAII.", Required = false, DefaultValue = OptimizationAlgorithm.Full)]
        public OptimizationAlgorithm Method { get; set; }

        [Option('n', "repetitions", HelpText = "A number specifying how often each measurement should be repeated", Required = false, DefaultValue = 5)]
        public int Repetitions { get; set; }

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
