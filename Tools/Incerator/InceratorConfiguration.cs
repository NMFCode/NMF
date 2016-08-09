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
        [Option('m', "model", HelpText = "The model on which incerator should operate", Required = true)]
        public string Model { get; set; }

        [Option('s', "change-sequence", HelpText = "The change sequence for which the analysis should be optimized", Required = true)]
        public string ChangeSequence { get; set; }

        [Option('c', "configuration", HelpText = "The change configuration", Required = true)]
        public string Configuration { get; set; }

        [Option('a', "assembly", HelpText = "The assembly that should be optimized", Required = true)]
        public string Assembly { get; set; }

        [Option('t', "type", HelpText = "The analysis type that should be optímized", Required = true)]
        public string Type { get; set; }

        [ValueOption(0)]
        public OperationMode Mode { get; set; }


        public string GetHelp()
        {
            return HelpText.AutoBuild(this);
        }
    }

    public enum OperationMode
    {
        Record,
        Optimize
    }
}
