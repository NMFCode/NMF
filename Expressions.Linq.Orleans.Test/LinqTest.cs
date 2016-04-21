//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NMF.Expressions.Linq.Orleans;
//using Orleans;
//using Orleans.Collections.Observable;
//using Orleans.Collections.Utilities;
//using Orleans.Streams;
//using Orleans.TestingHost;
//using TestGrains;

//namespace Expressions.Linq.Orleans.Test
//{
//    [TestClass]
//    public class LinqTest : TestingSiloHost
//    {
//        private IncrementalStreamProcessorAggregateFactory _factory;
//        private IStreamProvider _provider;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            _provider = GrainClient.GetStreamProvider("CollectionStreamProvider");
//            _factory = new IncrementalStreamProcessorAggregateFactory(GrainFactory);
//        }

//        [ClassCleanup]
//        public static void ClassCleanup()
//        {
//            // Optional. 
//            // By default, the next test class which uses TestignSiloHost will
//            // cause a fresh Orleans silo environment to be created.
//            StopAllSilosIfRunning();
//        }


//        [TestMethod]
//        public async Task TestOneLevelSelectDataPass()
//        {
//            var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());

//            var resultConsumer = await collection.SelectIncremental(i => i.Item.ToString(), _factory).ToListConsumer();

//            var items = new List<int> {1, 2, 5};
//            await collection.BatchAdd(items);

//            CollectionAssert.AreEqual(items.Select(i => i.ToString()).ToList(), resultConsumer.Items.Select(e => e.Item).ToList());
//        }


//        [TestMethod]
//        public async Task TestOneLevelWhereDataFilter()
//        {
//            var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<TestObjectWithPropertyChange>>(Guid.NewGuid());

//            var resultConsumer = await collection.WhereIncremental(i => i.Item.Value >= 42, _factory).ToListConsumer();

//            var itemsSatisfyingCriteria = new List<int> {42, 232130, 123, 58};
//            var itemsViolatingCriteria = new List<int> {1, -500, 23};

//            var hostedItems =
//                await
//                    collection.BatchAddReturnDictionary(
//                        itemsSatisfyingCriteria.Concat(itemsViolatingCriteria).Select(x => new TestObjectWithPropertyChange(x)).ToList());

//            CollectionAssert.AreEqual(itemsSatisfyingCriteria, resultConsumer.Items.Select(e => e.Item.Value).ToList());

//            var itemSatisfyingCrit = hostedItems.First(i => i.Value.Value >= 42);
//            var itemViolatingCrit = hostedItems.First(i => i.Value.Value < 42);

//            // Make satisfying item violate
//            await collection.ExecuteSync(i => i.Value = 2, itemSatisfyingCrit.Key);
//            itemsSatisfyingCriteria.Remove(itemSatisfyingCrit.Value.Value);
//            CollectionAssert.AreEqual(itemsSatisfyingCriteria, resultConsumer.Items.Select(e => e.Item.Value).ToList());

//            // Make violating item satisfy
//            await collection.ExecuteSync(i => i.Value = 109, itemViolatingCrit.Key);
//            itemsSatisfyingCriteria.Add(109);
//            CollectionAssert.AreEqual(itemsSatisfyingCriteria, resultConsumer.Items.Select(e => e.Item.Value).ToList());
//        }
//    }
//}