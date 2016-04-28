using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Expressions.Linq.Orleans.Test.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq.Orleans;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Models;
using NMF.Models.Tests.Railway;
using Orleans;
using Orleans.Streams;
using Orleans.Streams.Linq;
using Orleans.TestingHost;

namespace Expressions.Linq.Orleans.Test
{
    [TestClass]
    public class SqoTest : TestingSiloHost
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
        public async Task SelectTest()
        {
            var localModel = ModelTestUtil.ModelLoadingFunc(ModelTestUtil.ModelPath);
            var modelContainer = await ModelTestUtil.LoadModelContainer(GrainFactory);
            var factory = new IncrementalNmfModelStreamProcessorAggregateFactory(GrainFactory, modelContainer);

            var query = await modelContainer.Select(x => x, factory);
            var resultConsumer = await query.ToNmfModelConsumer();

            Assert.AreEqual(0, resultConsumer.Items.Count);

            await modelContainer.EnumerateToSubscribers();

            ModelTestUtil.AssertXmlEquals(localModel.SingleValueToList(), resultConsumer.Items);
        }

        [TestMethod]
        public async Task SelectManyTest()
        {
            var localModel = ModelTestUtil.ModelLoadingFunc(ModelTestUtil.ModelPath);
            var modelContainer = await ModelTestUtil.LoadModelContainer(GrainFactory);
            var factory = new IncrementalNmfModelStreamProcessorAggregateFactory(GrainFactory, modelContainer);

            var query = await modelContainer.SimpleSelectMany(model => model.Semaphores, factory);
            var resultConsumer = await query.ToNmfModelConsumer();

            Assert.AreEqual(0, resultConsumer.Items.Count);

            await modelContainer.EnumerateToSubscribers();

            var localResults = localModel.Semaphores; 
            ModelTestUtil.AssertXmlEquals(localResults, resultConsumer.Items);
        }

        [TestMethod]
        public async Task WhereSelectManyTest()
        {
            var localModel = ModelTestUtil.ModelLoadingFunc(ModelTestUtil.ModelPath);
            var modelContainer = await ModelTestUtil.LoadModelContainer(GrainFactory);
            var factory = new IncrementalNmfModelStreamProcessorAggregateFactory(GrainFactory, modelContainer);

            var query =
                modelContainer.SimpleSelectMany(model => model.Semaphores, factory)
                    .Where(s => s.Signal == Signal.GO);
            var resultConsumer = await query.ToNmfModelConsumer();

            Assert.AreEqual(0, resultConsumer.Items.Count);

            await modelContainer.EnumerateToSubscribers();

            var localResults = localModel.Semaphores.Where(s => s.Signal == Signal.GO);
            ModelTestUtil.AssertXmlEquals(localResults, resultConsumer.Items);
        }
        
    }
}