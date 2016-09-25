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
using System.Collections;

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

        public RunData Data { get; }

        private readonly ExecutionEngine defaultEngine;
        private readonly ExecutionEngine parallelEngine;
        private readonly Random rnd = new Random(42);

        public TrainCase()
        {
            defaultEngine = ExecutionEngine.Current;
            parallelEngine = new ParallelArrayExecutionEngine();
            Data = new RunData(this);
        }

        [Benchmark]
        public void Immediate()
        {
            DoRepair(Data.QueryResults.ToList());
            DoInject(Data.InjectResults.ToList());
        }

        [Benchmark]
        public void Transaction()
        {
            ExecutionEngine.Current.BeginTransaction();
            DoRepair(Data.QueryResults);
            ExecutionEngine.Current.CommitTransaction();

            ExecutionEngine.Current.BeginTransaction();
            DoInject(Data.InjectResults);
            ExecutionEngine.Current.CommitTransaction();
        }

        [Benchmark(Baseline = true)]
        public void Parallel()
        {
            ExecutionEngine.Current = parallelEngine;

            ExecutionEngine.Current.BeginTransaction();
            DoRepair(Data.QueryResults);
            ExecutionEngine.Current.CommitTransaction();

            ExecutionEngine.Current.BeginTransaction();
            DoInject(Data.InjectResults);
            ExecutionEngine.Current.CommitTransaction();

            ExecutionEngine.Current = defaultEngine;
        }

        private void DoRepair(IList<TResult> errors)
        {
            ExecuteRandom(errors, Repair, RepairPortion);
        }

        private void DoInject(IList<TInject> injects)
        {
            ExecuteRandom(injects, Inject, InjectPortion);
        }

        private void ExecuteRandom<T>(IList<T> candidates, Action<T> action, double percentage)
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

            public SortedList<int, TResult> QueryResults { get; }

            public INotifyEnumerable<TInject> Inject { get; }

            public SortedList<int, TInject> InjectResults { get; }

            public RunData(TrainCase<TResult, TInject> test)
            {
                Query = test.Query(Model);
                QueryResults = new SortedList<int, TResult>(r => r.GetHashCode(), Query);
                Query.CollectionChanged += (obj, e) => UpdateList(e, QueryResults);

                Inject = test.InjectSelector(Model);
                InjectResults = new SortedList<int, TInject>(r => r.GetHashCode(), Inject);
                Inject.CollectionChanged += (obj, e) => UpdateList(e, InjectResults);
            }

            private static void UpdateList<T>(NotifyCollectionChangedEventArgs e, SortedList<int, T> list)
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                    throw new NotImplementedException();

                if (e.OldItems != null)
                {
                    foreach (T result in e.OldItems)
                        list.Remove(result);
                }
                if (e.NewItems != null)
                {
                    foreach (T result in e.NewItems)
                        list.Add(result);
                }
            }

            private static RailwayContainer LoadRailwayModel()
            {
                var repository = new ModelRepository();
                var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
                return railwayModel.RootElements.Single() as RailwayContainer;
            }
        }
    }

    public class SortedList<TKey, TItem> : IList<TItem>
    {
        private readonly Func<TItem, TKey> keySelector;
        private readonly List<TKey> keys = new List<TKey>();
        private readonly List<TItem> items = new List<TItem>();

        public int Count { get { return keys.Count; } }

        public bool IsReadOnly { get { return false; } }

        public TItem this[int index]
        {
            get
            {
                return items[index];
            }

            set
            {
                throw new InvalidOperationException();
            }
        }

        public SortedList(Func<TItem, TKey> keySelector)
        {
            this.keySelector = keySelector;
        }

        public SortedList(Func<TItem, TKey> keySelector, IEnumerable<TItem> items) : this(keySelector)
        {
            foreach (var item in items)
                Add(item);
        }

        public int IndexOf(TItem item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, TItem item)
        {
            throw new InvalidOperationException();
        }

        public void RemoveAt(int index)
        {
            keys.RemoveAt(index);
            items.RemoveAt(index);
        }

        public void Add(TItem item)
        {
            var key = keySelector(item);
            var index = keys.BinarySearch(key);
            if (index >= 0)
                throw new ArgumentException("Duplicate Key!");
            keys.Insert(~index, key);
            items.Insert(~index, item);
        }

        public void Clear()
        {
            keys.Clear();
            items.Clear();
        }

        public bool Contains(TItem item)
        {
            return items.Contains(item);
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(TItem item)
        {
            var index = items.IndexOf(item);
            if (index < 0)
                return false;
            items.RemoveAt(index);
            keys.RemoveAt(index);
            return true;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
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
