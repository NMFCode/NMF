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

        public abstract Func<RailwayContainer, IEnumerable<TInject>> InjectSelector { get; }

        public abstract Action<TInject> Inject { get; }

        private readonly RailwayContainer immediateModel;
        private readonly INotifyEnumerable<TResult> immediateTest;
        private readonly List<TResult> immediateResult;

        private readonly RailwayContainer transactionModel;
        private readonly INotifyEnumerable<TResult> transactionTest;
        private readonly List<TResult> transactionResult = new List<TResult>();

        private readonly Random rnd = new Random();

        public TrainCase()
        {
            immediateModel = LoadRailwayModel();
            immediateTest = Query(immediateModel);
            immediateResult = immediateTest.ToList();
            immediateTest.CollectionChanged += (obj, e) => UpdateResults(e, immediateResult);

            transactionModel = LoadRailwayModel();
            transactionTest = Query(transactionModel);
            transactionResult = transactionTest.ToList();
            transactionTest.CollectionChanged += (obj, e) => UpdateResults(e, transactionResult);
        }

        private RailwayContainer LoadRailwayModel()
        {
            var repository = new ModelRepository();
            var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
            return railwayModel.RootElements.Single() as RailwayContainer;
        }

        private void UpdateResults(NotifyCollectionChangedEventArgs e, List<TResult> results)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
                throw new NotImplementedException();

            if (e.OldItems != null)
            {
                foreach (TResult result in e.OldItems)
                    results.Remove(result);
            }
            if (e.NewItems != null)
                results.AddRange(e.NewItems.Cast<TResult>());
        }

        [Benchmark]
        public void Immediate()
        {
            foreach (var error in immediateResult.ToArray())
                Repair(error);

            foreach (var injectTarget in InjectSelector(immediateModel).Where(i => rnd.Next(10) == 0))
                Inject(injectTarget);
        }

        [Benchmark(Baseline = true)]
        public void Transaction()
        {
            branch::NMF.Expressions.ExecutionEngine.Current.BeginTransaction();
            foreach (var error in transactionResult)
                Repair(error);
            branch::NMF.Expressions.ExecutionEngine.Current.CommitTransaction();

            branch::NMF.Expressions.ExecutionEngine.Current.BeginTransaction();
            foreach (var injectTarget in InjectSelector(transactionModel).Where(i => rnd.Next(10) == 0))
                Inject(injectTarget);
            branch::NMF.Expressions.ExecutionEngine.Current.CommitTransaction();
        }
    }
}
