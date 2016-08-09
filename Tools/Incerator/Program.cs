using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Incerator
{
    class Program
    {
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
                    break;
                case OperationMode.Optimize:
                    break;
                default:
                    Console.WriteLine("Operation mode {0} is unknown.", options.Mode);
                    Environment.Exit(1);
                    break;
            }
        }
    }
}
