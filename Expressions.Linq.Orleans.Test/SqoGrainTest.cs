using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expressions.Linq.Orleans.Test.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions;
using NMF.Expressions.Linq.Orleans;
using NMF.Expressions.Linq.Orleans.Interfaces;
using NMF.Expressions.Linq.Orleans.Linq.Interfaces;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using NMF.Models.Tests.Railway;
using Orleans;
using Orleans.Collections;
using Orleans.Collections.Endpoints;
using Orleans.Collections.Utilities;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.TestingHost;
using TestGrains;

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
        public async Task TestIncrementalSelectAggregateGrain()
        {
            var modelGrain = await ModelTestUtil.LoadModelContainer(GrainFactory);

            var selectGrain = GrainFactory.GetGrain<IIncrementalSelectAggregateGrain<Model, Model>>(Guid.NewGuid());
            await selectGrain.SetModelContainer(modelGrain);
            await selectGrain.SetObservingFunc(new SerializableFunc<Model, Model>(_ => _));
            await selectGrain.SetInput(new List<StreamIdentity> {await modelGrain.GetStreamIdentity()});
            await selectGrain.LoadModel(ModelTestUtil.ModelLoadingFunc);

            var consumer = new MultiStreamModelConsumer<Model>(_provider, ModelTestUtil.ModelLoadingFunc);
            await consumer.SetInput(await selectGrain.GetStreamIdentities());

            await modelGrain.EnumerateToSubscribers(Guid.NewGuid());

            Assert.AreEqual(1, consumer.Items.Count);
            var model1 = await modelGrain.ModelToString(model => model);
            var model2 = consumer.Items[0].ToXmlString();

            Assert.AreEqual(model1, model2);
        }

        [TestMethod]
        public async Task TestIncrementalSelectNodeGrain()
        {
            var modelGrain = await ModelTestUtil.LoadModelContainer(GrainFactory);

            var selectNodeGrain = GrainFactory.GetGrain<IIncrementalSelectNodeGrain<Model, Model>>(Guid.NewGuid());
            await selectNodeGrain.SetModelContainer(modelGrain);
            await selectNodeGrain.SetInput(await modelGrain.GetStreamIdentity());
            await selectNodeGrain.SetObservingFunc(new SerializableFunc<Model, Model>(_ => _));
            await selectNodeGrain.LoadModel(ModelTestUtil.ModelLoadingFunc);

            var consumer = new MultiStreamModelConsumer<Model>(_provider, ModelTestUtil.ModelLoadingFunc);
            await consumer.SetInput(new List<StreamIdentity> { await selectNodeGrain.GetStreamIdentity() });

            await modelGrain.EnumerateToSubscribers(Guid.NewGuid());

            Assert.AreEqual(1, consumer.Items.Count);
            var model1 = await modelGrain.ModelToString(model => model);
            var model2 = consumer.Items[0].ToXmlString();

            Assert.AreEqual(model1, model2);
        }

        [TestMethod]
        public async Task TestObservableSimpleSelectManyNodeGrainRetrieveItems()
        {
            var modelGrain = await ModelTestUtil.LoadModelContainer(GrainFactory);

            var localModel = ModelTestUtil.ModelLoadingFunc();

            var selectNodeGrain = GrainFactory.GetGrain<IIncrementalSimpleSelectManyNodeGrain<Model, ISemaphore>>(Guid.NewGuid());
            await selectNodeGrain.SetModelContainer(modelGrain);
            await selectNodeGrain.SetSelector(new SerializableFunc<Model, IEnumerable<ISemaphore>>(model => (model.RootElements.Single() as RailwayContainer).Semaphores));
            await selectNodeGrain.LoadModel(ModelTestUtil.ModelLoadingFunc);
            await selectNodeGrain.SetInput(await modelGrain.GetStreamIdentity());

            var consumer = new MultiStreamModelConsumer<ISemaphore>(_provider, ModelTestUtil.ModelLoadingFunc);
            await consumer.SetInput(new List<StreamIdentity> { await selectNodeGrain.GetStreamIdentity() });

            await modelGrain.EnumerateToSubscribers(Guid.NewGuid());

            var localSemaphores = (localModel.RootElements.Single() as RailwayContainer).Semaphores;

            Assert.AreEqual(localSemaphores.Count, consumer.Items.Count);

            var localXmlString = localSemaphores.Select(r => r.ToXmlString()).OrderBy(s => s).ToList();
            var processedXmlstring = consumer.Items.Select(r => r.ToXmlString()).OrderBy(s => s).ToList(); 

            CollectionAssert.AreEqual(localXmlString, processedXmlstring);

        }

  
    }
}
