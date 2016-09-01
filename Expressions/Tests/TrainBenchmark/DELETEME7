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
                bench.Transaction();
                if ((i & 0xFF) == 0 && watch.ElapsedMilliseconds > 5000)
                    break;
            }
        }
    }

    public class XorShift128Plus
    {
        private int s0;
        private int s1;

        public XorShift128Plus(long seed)
        {
            s0 = (int)(seed >> 32);
            s1 = (int)(seed & 0xFFFFFFFF);
        }

        public XorShift128Plus() : this(Stopwatch.GetTimestamp()) { }

        public int Next()
        {
            int x = s0;
            int y = s1;
            s0 = y;
            x ^= x << 11; // a
            s1 = x ^ y ^ (x >> 8) ^ (y >> 19); // b, c
            return s1 + y;
        }

        public int Next(int exclusiveUpperBound)
        {
            return Next() % exclusiveUpperBound;
        }
    }
}
