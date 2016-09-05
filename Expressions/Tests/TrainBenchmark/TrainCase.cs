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

namespace TrainBenchmark
{
    public abstract class TrainCase<TResult, TInject>
    {
        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";
        private const double RepairPortion = 0.5;
        private const double InjectPortion = 0.2;

        public abstract Func<RailwayContainer, INotifyEnumerable<TResult>> Query { get; }

        public abstract Action<TResult> Repair { get; }

        public abstract Func<RailwayContainer, INotifyEnumerable<TInject>> InjectSelector { get; }

        public abstract Action<TInject> Inject { get; }

        public RunData ImmediateRunData { get; }
        public RunData TransactionRunData { get; }

        private readonly XorShift128Plus rnd = new XorShift128Plus(42);

        public TrainCase()
        {
            ImmediateRunData = new RunData(this);
            TransactionRunData = new RunData(this);
        }

        [Benchmark]
        public void Immediate()
        {
            DoRepair(ImmediateRunData.QueryResults.ToList());
            DoInject(ImmediateRunData.InjectResults.ToList());
        }

        [Benchmark(Baseline = true)]
        public void Transaction()
        {
            ExecutionEngine.Current.BeginTransaction();
            DoRepair(TransactionRunData.QueryResults);
            ExecutionEngine.Current.CommitTransaction();

            ExecutionEngine.Current.BeginTransaction();
            DoInject(TransactionRunData.InjectResults);
            ExecutionEngine.Current.CommitTransaction();
        }

        private void DoRepair(List<TResult> errors)
        {
            int take = Math.Min(1, (int)(RepairPortion * errors.Count));
            int skip = errors.Count <= take ? 0 : rnd.Next(errors.Count - take);
            foreach (var error in errors.Skip(skip).Take(take))
                Repair(error);
        }

        private void DoInject(List<TInject> injects)
        {
            int take = Math.Min(1, (int)(InjectPortion * injects.Count));
            int skip = injects.Count <= take ? 0 : rnd.Next(injects.Count - take);
            foreach (var injectTarget in injects.Skip(skip).Take(take))
                Inject(injectTarget);
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
}
