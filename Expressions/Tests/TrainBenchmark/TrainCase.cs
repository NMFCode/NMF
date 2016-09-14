using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using NMF.Expressions;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainBenchmark
{
    [Config(typeof(TrainBenchmarkConfig))]
    public abstract class TrainCase<TResult, TInject>
    {
        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";
        private const double RepairPortion = 0.2;
        private const double InjectPortion = 0.2;

        public abstract Func<RailwayContainer, INotifyEnumerable<TResult>> Query { get; }

        public abstract Action<TResult> Repair { get; }

        public abstract Func<RailwayContainer, INotifyEnumerable<TInject>> InjectSelector { get; }

        public abstract Action<TInject> Inject { get; }

        public RunData ImmediateRunData { get; }
        public RunData TransactionRunData { get; }
        public RunData ParallelRunData { get; }

        private readonly ExecutionEngine defaultEngine;
        private readonly ExecutionEngine parallelEngine;
        private readonly Random rnd = new Random(42);

        public TrainCase()
        {
            defaultEngine = ExecutionEngine.Current;
            parallelEngine = new ParallelArrayExecutionEngine();

            ImmediateRunData = new RunData(this);
            TransactionRunData = new RunData(this);
            ExecutionEngine.Current = parallelEngine;
            ParallelRunData = new RunData(this);
            ExecutionEngine.Current = defaultEngine;
        }

        [Benchmark]
        public void Immediate()
        {
            DoRepair(ImmediateRunData.QueryResults.ToList());
            DoInject(ImmediateRunData.InjectResults.ToList());
        }

        [Benchmark]
        public void Transaction()
        {
            ExecutionEngine.Current.BeginTransaction();
            DoRepair(TransactionRunData.QueryResults);
            ExecutionEngine.Current.CommitTransaction();

            ExecutionEngine.Current.BeginTransaction();
            DoInject(TransactionRunData.InjectResults);
            ExecutionEngine.Current.CommitTransaction();
        }

        [Benchmark(Baseline = true)]
        public void Parallel()
        {
            ExecutionEngine.Current = parallelEngine;

            ExecutionEngine.Current.BeginTransaction();
            DoRepair(ParallelRunData.QueryResults);
            ExecutionEngine.Current.CommitTransaction();

            ExecutionEngine.Current.BeginTransaction();
            DoInject(ParallelRunData.InjectResults);
            ExecutionEngine.Current.CommitTransaction();

            ExecutionEngine.Current = defaultEngine;
        }

        private void DoRepair(List<TResult> errors)
        {
            ExecuteRandom(errors, Repair, RepairPortion);
        }

        private void DoInject(List<TInject> injects)
        {
            ExecuteRandom(injects, Inject, InjectPortion);
        }

        private void ExecuteRandom<T>(List<T> candidates, Action<T> action, double percentage)
        {
            if (candidates.Count == 0)
                return;

            int take = (int)(percentage * candidates.Count);
            if (take <= 0)
                take = 1;
            int skip = candidates.Count == take ? 0 : rnd.Next(candidates.Count - take);

            for (int i = skip; i < skip + take; i++)
                action(candidates[i]);
        }

        public class RunData
        {
            public RailwayContainer Model { get; } = LoadRailwayModel();
            
            public INotifyEnumerable<TResult> Query { get; }

            public List<TResult> QueryResults { get; }

            public INotifyEnumerable<TInject> Inject { get; }

            public List<TInject> InjectResults { get; }

            public RunData(TrainCase<TResult, TInject> test)
            {
                Query = test.Query(Model);
                QueryResults = Query.ToList();
                Query.CollectionChanged += (obj, e) => UpdateList(e, QueryResults);

                Inject = test.InjectSelector(Model);
                InjectResults = Inject.ToList();
                Inject.CollectionChanged += (obj, e) => UpdateList(e, InjectResults);
            }

            private static void UpdateList<T>(NotifyCollectionChangedEventArgs e, List<T> list)
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                    throw new NotImplementedException();

                if (e.OldItems != null)
                {
                    foreach (T result in e.OldItems)
                        list.Remove(result);
                }
                if (e.NewItems != null)
                    list.AddRange(e.NewItems.Cast<T>());
            }

            private static RailwayContainer LoadRailwayModel()
            {
                var repository = new ModelRepository();
                var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
                return railwayModel.RootElements.Single() as RailwayContainer;
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
