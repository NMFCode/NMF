using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Observable;
using Orleans.Collections.Utilities;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.TestingHost;
using TestGrains;

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
            StopAllSilosIfRunning();
        }

        [TestMethod]
        public async Task TestObservableSelectNodeGrainAddNewItemsViaInputStream()
        {
            var sender = new StreamMessageSender(_provider);
            var provider = new SingleStreamTransactionSender<int>(sender);

            var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());

            await collection.SetInput(new List<StreamIdentity>() { await sender.GetStreamIdentity() });

            var selectNodeGrain = GrainClient.GrainFactory.GetGrain<IObservableSelectNodeGrain<int, string>>(Guid.NewGuid());
            await selectNodeGrain.SetObservingFunc(new SerializableFunc<int, string>(i => i.ToString()));

            await selectNodeGrain.SetInput((await collection.GetStreamIdentities()).First());

            var consumer = new MultiStreamListConsumer<ContainerElement<string>>(_provider);
            await consumer.SetInput(new List<StreamIdentity> { await selectNodeGrain.GetStreamIdentity() });

            var items = new List<int>() { 1, 2, 5 };
            await provider.SendItems(items);

            Assert.AreEqual(items.Count, consumer.Items.Count);
            CollectionAssert.AreEquivalent(items, consumer.Items.Select(x => int.Parse(x.Item)).ToList());
        }

        [TestMethod]
        public async Task TestObservableSelectNodeGrainRemoveExisting()
        {
            var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());

            var selectNodeGrain = GrainClient.GrainFactory.GetGrain<IObservableSelectNodeGrain<int, string>>(Guid.NewGuid());
            await selectNodeGrain.SetObservingFunc(new SerializableFunc<int, string>(i => i.ToString()));

            await selectNodeGrain.SetInput((await collection.GetStreamIdentities()).First());

            var clientConsumer = new MultiStreamListConsumer<ContainerElement<string>>(_provider);
            await clientConsumer.SetInput(new List<StreamIdentity> { await selectNodeGrain.GetStreamIdentity() });

            var items = new List<int>() { 1, 2, 5 }.ToList();
            var elementReferences = await collection.BatchAdd(items);

            Assert.AreEqual(items.Count, clientConsumer.Items.Count);
            CollectionAssert.AreEquivalent(items, clientConsumer.Items.Select(s => int.Parse(s.Item)).ToList());

            var removedReference = elementReferences.First();
            await collection.Remove(removedReference);
            clientConsumer.Items.Clear();

            await selectNodeGrain.EnumerateToSubscribers();

            Assert.AreEqual(2, clientConsumer.Items.Count);
            CollectionAssert.AreEquivalent(items.Skip(1).ToList(), clientConsumer.Items.Select(s => int.Parse(s.Item)).ToList());
        }

        [TestMethod]
        public async Task TestObservableSelectNodeGrainUpdateExistingWithPropertyChanged()
        {
            var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<TestObjectWithPropertyChange>>(Guid.NewGuid());

            var selectNodeGrain = GrainClient.GrainFactory.GetGrain<IObservableSelectNodeGrain<TestObjectWithPropertyChange, int>>(Guid.NewGuid());
            await selectNodeGrain.SetInput((await collection.GetStreamIdentities()).First());
            await selectNodeGrain.SetObservingFunc(new SerializableFunc<TestObjectWithPropertyChange, int>(o => o.Value));

            var clientConsumer = new MultiStreamListConsumer<ContainerElement<int>>(_provider, 
                (containerElement1, containerElement2) => containerElement1.Reference.Equals(containerElement2.Reference));
            await clientConsumer.SetInput(new List<StreamIdentity> { await selectNodeGrain.GetStreamIdentity() });

            var items = new List<int> { 1, 5, 10 }.Select(i => new TestObjectWithPropertyChange(i)).ToList();
            var elementReferences = await collection.BatchAdd(items);

            Assert.AreEqual(items.Count, clientConsumer.Items.Count);
            CollectionAssert.AreEquivalent(items.Select(o => o.Value).ToList(), clientConsumer.Items.Select(x => x.Item).ToList());

            // Update value
            var updatedReference = elementReferences.First();
            await collection.ExecuteSync(o => { o.Value = 42; }, updatedReference);
            Assert.AreEqual(items.Count, clientConsumer.Items.Count);
            CollectionAssert.AreEquivalent(new List<int> { 42, 5, 10 }, clientConsumer.Items.Select(i => i.Item).ToList());

            clientConsumer.Items.Clear();
            await selectNodeGrain.EnumerateToSubscribers();

            Assert.AreEqual(items.Count, clientConsumer.Items.Count);
            CollectionAssert.AreEquivalent(new List<int> { 42, 5, 10 }, clientConsumer.Items.Select(i => i.Item).ToList());
        }

        [TestMethod]
        public async Task TestObservableSelectAggregateGrainAddNewItemsViaInputStream()
        {
            var provider = new MultiStreamProvider<int>(_provider, 4);

            var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());
            await collection.SetNumberOfNodes(4);

            await collection.SetInput(await provider.GetStreamIdentities());

            var selectAggregateGrain = GrainClient.GrainFactory.GetGrain<IObservableSelectAggregateGrain<int, string>>(Guid.NewGuid());
            await selectAggregateGrain.SetObservingFunc(new SerializableFunc<int, string>(i => i.ToString()));

            await selectAggregateGrain.SetInput(await collection.GetStreamIdentities());

            var consumer = new MultiStreamListConsumer<ContainerElement<string>>(_provider);
            await consumer.SetInput(await selectAggregateGrain.GetStreamIdentities());

            var items = Enumerable.Range(0, 10000).ToList();
            await provider.SendItems(items);

            Assert.AreEqual(items.Count, consumer.Items.Count);
            CollectionAssert.AreEquivalent(items, consumer.Items.Select(x => int.Parse(x.Item)).ToList());
        }
    }
}
