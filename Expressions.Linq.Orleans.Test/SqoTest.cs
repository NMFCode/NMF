using System.Linq;
using System.Threading.Tasks;
using Expressions.Linq.Orleans.Test.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq.Orleans;
using NMF.Expressions.Linq.Orleans.Model;
using Orleans;
using Orleans.Streams;
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
            
            var localModel = ModelTestUtil.ModelLoadingFunc();
            var modelContainer = await ModelTestUtil.LoadModelContainer(GrainFactory);
            var factory = new IncrementalStreamProcessorAggregateFactory<NMF.Models.Model>(GrainFactory, modelContainer);

            // TODO implement here
            var query = await modelContainer.SelectIncremental(x => x, factory);
            var resultConsumer = await query.ToModelConsumer(ModelTestUtil.ModelLoadingFunc);

            Assert.AreEqual(0, resultConsumer.Items.Count);

            await modelContainer.EnumerateToSubscribers();

            Assert.AreEqual(localModel.ToXmlString(), resultConsumer.Items.First().ToXmlString());
        }
    }
}