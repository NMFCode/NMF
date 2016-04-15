using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq.Orleans;
using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Endpoints;
using Orleans.Collections.Observable;
using Orleans.Collections.Utilities;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.Streams.Linq;
using Orleans.TestingHost;

namespace Expressions.Linq.Orleans.Test
{
    [TestClass]
    public class LinqTest : TestingSiloHost
    {
        private IStreamProvider _provider;
        private IncrementalStreamProcessorAggregateFactory _factory;

        [TestInitialize]
        public void TestInitialize()
        {
            _provider = GrainClient.GetStreamProvider("CollectionStreamProvider");
            _factory = new IncrementalStreamProcessorAggregateFactory(GrainFactory);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Optional. 
            // By default, the next test class which uses TestignSiloHost will
            // cause a fresh Orleans silo environment to be created.
            StopAllSilosIfRunning();
        }


        [TestMethod]
        public async Task TestOneLevelSelectDataPass()
        {
            var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());

            var resultConsumer = await collection.SelectIncremental(i => i.Item.ToString(), _factory).ToListConsumer();

            var items = new List<int>() { 1, 2, 5 };
            await collection.BatchAdd(items);

            CollectionAssert.AreEqual(items.Select(i => i.ToString()).ToList(), resultConsumer.Items.Select(e => e.Item).ToList());
        }


        [TestMethod]
        public async Task TestOneLevelWhereDataFilter()
        {
            var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());

            var resultConsumer = await collection.WhereIncremental(i => i.Item >= 42, _factory).ToListConsumer();

            var itemsSatisfyingCriteria = new List<int> { 42, 232130, 123, 58 };
            var itemsViolatingCriteria = new List<int> {1, -500, 23 };

            await collection.BatchAdd(itemsSatisfyingCriteria.Concat(itemsViolatingCriteria).ToList());

            CollectionAssert.AreEqual(itemsSatisfyingCriteria, resultConsumer.Items.Select(e => e.Item).ToList());
        }

    }
}