using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        static void Main(string[] args)
        {
#if !DEBUG
            new BenchmarkSwitcher(Assembly.GetExecutingAssembly()).Run();
#else
            Profile();
#endif
        }

        private static void Profile()
        {
            var bench = new SemaphoreNeighbor();
            var watch = Stopwatch.StartNew();
            for (int i = 0; ; i++)
            {
                bench.Immediate();
                if ((i & 0xFF) == 0 && watch.ElapsedMilliseconds > 5000)
                    break;
            }
        }
    }

    class TrainBenchmarkConfig : ManualConfig
    {
        public TrainBenchmarkConfig()
        {
            Add(Job.Default.WithTargetCount(30));
        }
    }
}
