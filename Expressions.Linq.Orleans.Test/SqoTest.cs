using System.Threading.Tasks;
using Expressions.Linq.Orleans.Test.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq.Orleans;
using Orleans;
using Orleans.Streams;
using Orleans.TestingHost;

namespace Expressions.Linq.Orleans.Test
{
    [TestClass]
    public class SqoTest : TestingSiloHost
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
        public async Task SelectTest()
        {
            var modelContainer = await ModelTestUtil.LoadModelContainer(GrainFactory);

            // TODO implement here
            //var query = await modelContainer.SelectIncremental(x => x, _factory).ToListConsumer();

        }
    }
}