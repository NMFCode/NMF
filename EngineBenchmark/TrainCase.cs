extern alias branch;
using BenchmarkDotNet.Attributes;
using NMF.Expressions;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineBenchmark
{
    public abstract class TrainCase<TResult, TInject>
    {
        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        public abstract Func<RailwayContainer, INotifyEnumerable<TResult>> Query { get; }

        public abstract Action<TResult> Repair { get; }

        public abstract Func<RailwayContainer, INotifyEnumerable<TInject>> InjectSelector { get; }

        public abstract Action<TInject> Inject { get; }

        private readonly RunData immediate;
        private readonly RunData transaction;

        private readonly XorShift128Plus rnd = new XorShift128Plus();

        public TrainCase()
        {
            immediate = new RunData(this);
            transaction = new RunData(this);
        }

        [Benchmark]
        public void Immediate()
        {
            DoRepair(immediate.QueryResults.ToList());
            DoInject(immediate.InjectResults.ToList());
        }

        [Benchmark(Baseline = true)]
        public void Transaction()
        {
            branch::NMF.Expressions.ExecutionEngine.Current.BeginTransaction();
            DoRepair(transaction.QueryResults);
            branch::NMF.Expressions.ExecutionEngine.Current.CommitTransaction();

            branch::NMF.Expressions.ExecutionEngine.Current.BeginTransaction();
            DoInject(transaction.InjectResults);
            branch::NMF.Expressions.ExecutionEngine.Current.CommitTransaction();
        }

        private void DoRepair(List<TResult> errors)
        {
            foreach (var error in errors)
                Repair(error);
        }

        private void DoInject(List<TInject> injects)
        {
            int skip = injects.Count <= 2 ? 0 : rnd.Next(injects.Count / 2);
            int take = rnd.Next(injects.Count - skip);
            foreach (var injectTarget in injects.Skip(skip).Take(take))
                Inject(injectTarget);
        }

        private class RunData
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
}
