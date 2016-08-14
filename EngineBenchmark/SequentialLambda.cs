extern alias master;
extern alias branch;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EngineBenchmark
{
    public class SequentialLambda
    {
        private Dummy<int> newDummy;
        private Dummy<int> oldDummy;
        private branch::NMF.Expressions.INotifyValue<bool> newGraph;
        private master::NMF.Expressions.INotifyValue<bool> oldGraph;

        public SequentialLambda()
        {
            newDummy = new Dummy<int>(42);
            oldDummy = new Dummy<int>(42);
            newGraph = branch::NMF.Expressions.Observable.Expression(() => newDummy.Item == newDummy.Item && (AppDomain.CurrentDomain.IsFullyTrusted && newDummy.Item == 42));
            oldGraph = master::NMF.Expressions.Observable.Expression(() => oldDummy.Item == oldDummy.Item && (AppDomain.CurrentDomain.IsFullyTrusted && oldDummy.Item == 42));
        }

        [Benchmark(Baseline = true)]
        public void Master()
        {
            oldDummy.Item = 42 - oldDummy.Item;
        }
        
        [Benchmark]
        public void Immediate()
        {
            newDummy.Item = 42 - newDummy.Item;
        }

        [Benchmark]
        public void Transaction()
        {
            branch::NMF.Expressions.ExecutionEngine.Current.BeginTransaction();
            newDummy.Item = 42 - newDummy.Item;
            branch::NMF.Expressions.ExecutionEngine.Current.CommitTransaction();
        }
    }
}
