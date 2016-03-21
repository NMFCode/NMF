using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions;
using NMF.Expressions.Linq.Orleans.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Observable;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.TestingHost;

namespace Expressions.Linq.Orleans.Test
{
    [TestClass]
    public class LinqTest : TestingSiloHost
    {
        private IStreamProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _provider = GrainClient.GetStreamProvider("CollectionStreamProvider");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Optional. 
            // By default, the next test class which uses TestignSiloHost will
            // cause a fresh Orleans silo environment to be created.
            StopAllSilos();
        }

        [TestMethod]
        public async Task TestObservableSelectNodeGrain()
        {
            var provider = new SingleStreamProvider<int>(_provider);

            var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());

            await collection.SetInput(new List<TransactionalStreamIdentity<int>>() { await provider.GetStreamIdentity() });

            var selectNodeGrain = GrainClient.GrainFactory.GetGrain<IObservableSelectNodeGrain<int, string>>(Guid.NewGuid());
            await selectNodeGrain.SetObservingFunc(i => i.ToString());

            await selectNodeGrain.SetInput((await collection.GetStreamIdentities()).First());

            var consumer = new MultiStreamListConsumer<ContainerHostedElement<string>>(_provider);
            await consumer.SetInput(new List<TransactionalStreamIdentity<ContainerHostedElement<string>>> { await selectNodeGrain.GetStreamIdentity() });

            var items = new List<int>() {1, 2, 5};
            await provider.SendItems(items);

            Assert.AreEqual(items.Count, consumer.Items.Count);
            CollectionAssert.AreEquivalent(items, consumer.Items.Select(x => int.Parse(x.Item)).ToList());
        }
    }
}
