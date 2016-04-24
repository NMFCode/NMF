using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expressions.Linq.Orleans.Test.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Endpoints;
using Orleans.Collections.Observable;
using Orleans.Collections.Utilities;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.TestingHost;
using TestGrains;
using TTC2015.TrainBenchmark.Railway;

namespace Expressions.Linq.Orleans.Test
{
    [TestClass]
    public class SqoGrainTest : TestingSiloHost
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
        public async Task TestObservableSelectNodeGrainRetrieveItems()
        {
            var modelGrain = await ModelTestUtil.LoadModelContainer(GrainFactory);

            var selectNodeGrain = GrainFactory.GetGrain<IIncrementalSelectNodeGrain<Model, Model>>(Guid.NewGuid());
            await selectNodeGrain.SetInput(await modelGrain.GetStreamIdentity());
            await selectNodeGrain.SetModelContainer(modelGrain);
            await selectNodeGrain.SetObservingFunc(new SerializableFunc<Model, Model>(_ => _));
            await selectNodeGrain.LoadModel(ModelTestUtil.ModelLoadingFunc);

            var consumer = new MultiStreamListConsumer<Model>(_provider);
            await consumer.SetInput(new List<StreamIdentity> { await selectNodeGrain.GetStreamIdentity() });

            await modelGrain.EnumerateToSubscribers(Guid.NewGuid());

            Assert.AreEqual(1, consumer.Items.Count);
            var model1 = await modelGrain.ModelToString(model => model);
            var model2 = consumer.Items[0].ToXmlString();

            Assert.AreEqual(model1, model2);

        }

        //[TestMethod]
        //public async Task TestObservableSelectNodeGrainRemoveExisting()
        //{
        //    var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());

        //    var selectNodeGrain = GrainClient.GrainFactory.GetGrain<IIncrementalSelectNodeGrain<int, string>>(Guid.NewGuid());
        //    await selectNodeGrain.SetObservingFunc(new SerializableFunc<ContainerElement<int>, string>(element => element.Item.ToString()));

        //    await selectNodeGrain.SetInput((await collection.GetStreamIdentities()).First());

        //    var clientConsumer = new ContainerElementListConsumer<string>(_provider);
        //    await clientConsumer.SetInput(new List<StreamIdentity> { await selectNodeGrain.GetStreamIdentity() });

        //    var items = new List<int>() { 1, 2, 5 }.ToList();
        //    var elementReferences = await collection.BatchAdd(items);

        //    Assert.AreEqual(items.Count, clientConsumer.Items.Count);
        //    CollectionAssert.AreEquivalent(items, clientConsumer.Items.Select(s => int.Parse(s.Item)).ToList());

        //    var removedReference = elementReferences.First();
        //    await collection.Remove(removedReference);
        //    clientConsumer.Items.Clear();

        //    await selectNodeGrain.EnumerateToSubscribers();

        //    Assert.AreEqual(2, clientConsumer.Items.Count);
        //    CollectionAssert.AreEquivalent(items.Skip(1).ToList(), clientConsumer.Items.Select(s => int.Parse(s.Item)).ToList());
        //}

        //[TestMethod]
        //public async Task TestObservableSelectNodeGrainUpdateExistingWithPropertyChanged()
        //{
        //    var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<TestObjectWithPropertyChange>>(Guid.NewGuid());

        //    var selectNodeGrain = GrainClient.GrainFactory.GetGrain<IIncrementalSelectNodeGrain<TestObjectWithPropertyChange, int>>(Guid.NewGuid());
        //    await selectNodeGrain.SetInput((await collection.GetStreamIdentities()).First());
        //    await selectNodeGrain.SetObservingFunc(new SerializableFunc<ContainerElement<TestObjectWithPropertyChange>, int>(o => o.Item.Value));

        //    var clientConsumer = new ContainerElementListConsumer<int>(_provider);
        //    await clientConsumer.SetInput(new List<StreamIdentity> { await selectNodeGrain.GetStreamIdentity() });

        //    var items = new List<int> { 1, 5, 10 }.Select(i => new TestObjectWithPropertyChange(i)).ToList();
        //    var elementReferences = await collection.BatchAdd(items);

        //    Assert.AreEqual(items.Count, clientConsumer.Items.Count);
        //    CollectionAssert.AreEquivalent(items.Select(o => o.Value).ToList(), clientConsumer.Items.Select(x => x.Item).ToList());

        //    // Update value
        //    var updatedReference = elementReferences.First();
        //    await collection.ExecuteSync(o => { o.Value = 42; }, updatedReference);
        //    Assert.AreEqual(items.Count, clientConsumer.Items.Count);
        //    CollectionAssert.AreEquivalent(new List<int> { 42, 5, 10 }, clientConsumer.Items.Select(i => i.Item).ToList());

        //    clientConsumer.Items.Clear();
        //    await selectNodeGrain.EnumerateToSubscribers();

        //    Assert.AreEqual(items.Count, clientConsumer.Items.Count);
        //    CollectionAssert.AreEquivalent(new List<int> { 42, 5, 10 }, clientConsumer.Items.Select(i => i.Item).ToList());
        //}

        //[TestMethod]
        //public async Task TestObservableSelectAggregateGrainAddNewItemsViaInputStream()
        //{
        //    var provider = new MultiStreamProvider<int>(_provider, 4);

        //    var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());
        //    await collection.SetNumberOfNodes(4);

        //    await collection.SetInput(await provider.GetStreamIdentities());

        //    var selectAggregateGrain = GrainClient.GrainFactory.GetGrain<IIncrementalSelectAggregateGrain<int, string>>(Guid.NewGuid());
        //    await selectAggregateGrain.SetObservingFunc(new SerializableFunc<ContainerElement<int>, string>(element => element.Item.ToString()));

        //    await selectAggregateGrain.SetInput(await collection.GetStreamIdentities());

        //    var consumer = new ContainerElementListConsumer<string>(_provider);
        //    await consumer.SetInput(await selectAggregateGrain.GetStreamIdentities());

        //    var items = Enumerable.Range(0, 10000).ToList();
        //    await provider.SendItems(items);

        //    Assert.AreEqual(items.Count, consumer.Items.Count);
        //    CollectionAssert.AreEquivalent(items, consumer.Items.Select(x => int.Parse(x.Item)).ToList());
        //}

        //#region WHERE

        //[TestMethod]
        //public async Task TestObservableWhereNodeGrainAddRemove()
        //{
        //    var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<TestObjectWithPropertyChange>>(Guid.NewGuid());

        //    var whereNodeGrain = GrainClient.GrainFactory.GetGrain<IIncrementalWhereNodeGrain<TestObjectWithPropertyChange>>(Guid.NewGuid());
        //    await whereNodeGrain.SetInput((await collection.GetStreamIdentities()).First());
        //    await whereNodeGrain.SetObservingFunc(new SerializableFunc<ContainerElement<TestObjectWithPropertyChange>, bool>(i => i.Item.Value >= 42));

        //    var clientConsumer = new ContainerElementListConsumer<TestObjectWithPropertyChange>(_provider);
        //    await clientConsumer.SetInput((await whereNodeGrain.GetStreamIdentity()).SingleValueToList());

        //    var itemsMeetingCondition = new List<int> { 42, 57 };
        //    var itemsViolatingCondition = new List<int> { 1, 5, 10, 41 };
        //    var allItems = itemsViolatingCondition.Concat(itemsMeetingCondition).Select(i => new TestObjectWithPropertyChange(i)).ToList();
        //    var elementReferences = await collection.BatchAdd(allItems);

        //    Assert.AreEqual(itemsMeetingCondition.Count, clientConsumer.Items.Count);
        //    CollectionAssert.AreEquivalent(itemsMeetingCondition, clientConsumer.Items.Select(x => x.Item.Value).ToList());

        //    // Update value
        //    var updatedReference = elementReferences.First();
        //    await collection.ExecuteSync(o => { o.Value = 50; }, updatedReference);
        //    itemsMeetingCondition.Add(50);
        //    Assert.AreEqual(itemsMeetingCondition.Count, clientConsumer.Items.Count);
        //    CollectionAssert.AreEquivalent(itemsMeetingCondition, clientConsumer.Items.Select(i => i.Item.Value).ToList());

        //    // Remove value
        //    await collection.Remove(updatedReference);
        //    itemsMeetingCondition.Remove(50);
        //    clientConsumer.Items.Clear();

        //    await whereNodeGrain.EnumerateToSubscribers();

        //    Assert.AreEqual(itemsMeetingCondition.Count, clientConsumer.Items.Count);
        //    CollectionAssert.AreEquivalent(itemsMeetingCondition, clientConsumer.Items.Select(i => i.Item.Value).ToList());
        //}

        //[TestMethod]
        //public async Task TestObservableSelectWhereAggregateGrainAddNewItemsViaInputStream()
        //{
        //    var provider = new MultiStreamProvider<int>(_provider, 4);

        //    var collection = GrainClient.GrainFactory.GetGrain<IObservableContainerGrain<int>>(Guid.NewGuid());
        //    await collection.SetNumberOfNodes(4);

        //    await collection.SetInput(await provider.GetStreamIdentities());

        //    var selectAggregateGrain = GrainClient.GrainFactory.GetGrain<IIncrementalWhereAggregateGrain<int>>(Guid.NewGuid());
        //    await selectAggregateGrain.SetObservingFunc(new SerializableFunc<ContainerElement<int>, bool>(i => i.Item <= 42));

        //    await selectAggregateGrain.SetInput(await collection.GetStreamIdentities());

        //    var consumer = new ContainerElementListConsumer<int>(_provider);
        //    await consumer.SetInput(await selectAggregateGrain.GetStreamIdentities());

        //    var items = Enumerable.Range(0, 10000).ToList();
        //    await provider.SendItems(items);

        //    CollectionAssert.AreEquivalent(items.Where(i => i <= 42).ToList(), consumer.Items.Select(x => x.Item).ToList());

        //    #endregion

        //}
    }
}
