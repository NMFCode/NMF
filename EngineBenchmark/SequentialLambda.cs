using BenchmarkDotNet.Attributes;
using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineBenchmark
{
    public class SequentialLambda
    {
        private Dummy<int> dummy;
        private INotifyValue<bool> test;

        public SequentialLambda()
        {
            dummy = new Dummy<int>(42);
            test = Observable.Expression(() => dummy.Item == dummy.Item && (AppDomain.CurrentDomain.IsFullyTrusted && dummy.Item == 42));
        }

        [Benchmark(Baseline = true)]
        public void Immediate()
        {
            dummy.Item = 42 - dummy.Item;
        }

        [Benchmark]
        public void Transaction()
        {
            ExecutionEngine.Current.BeginTransaction();
            dummy.Item = 42 - dummy.Item;
            ExecutionEngine.Current.CommitTransaction();
        }
    }
}
