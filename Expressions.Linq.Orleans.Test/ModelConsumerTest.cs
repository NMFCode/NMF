using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Expressions.Linq.Orleans.Test.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq;
using NMF.Expressions.Linq.Orleans;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Expressions.Linq.Orleans.TestGrains;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using Orleans;
using Orleans.Streams;
using Orleans.TestingHost;

namespace Expressions.Linq.Orleans.Test
{
    [TestClass]
    public class ModelConsumerTest : TestingSiloHost
    {
        private IncrementalStreamProcessorAggregateFactory _factory;

        private IStreamProvider _provider;

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
        public async Task LoadModelSucceeded()
        {
            await ModelTestUtil.LoadModelContainer(GrainFactory);
        }

        [TestMethod]
        public async Task ModelPropertyChange()
        {
            var modelContainerGrain = await ModelTestUtil.LoadModelContainer(GrainFactory);

            var consumerGrain = await LoadAndAttachModelTestConsumer(modelContainerGrain);



            Assert.IsTrue(await ModelTestUtil.CurrentModelsMatch(modelContainerGrain, consumerGrain));

            // Property Changed test to null
            await modelContainerGrain.ExecuteSync(model =>
            {
                var railwayContainer = model.RootElements.Single() as RailwayContainer;
                railwayContainer.Routes.First().Entry = null;
            });
            Assert.IsTrue(await ModelTestUtil.CurrentModelsMatch(modelContainerGrain, consumerGrain));

            // Property Changed test to known object
            await modelContainerGrain.ExecuteSync(model =>
            {
                var railwayContainer = model.RootElements.Single() as RailwayContainer;
                railwayContainer.Routes.First().Entry = railwayContainer.Semaphores[2];
            });
            Assert.IsTrue(await ModelTestUtil.CurrentModelsMatch(modelContainerGrain, consumerGrain));

            // Property Changed test to native type
            await modelContainerGrain.ExecuteSync(model =>
            {
                var railwayContainer = model.RootElements.Single() as RailwayContainer;
                var segmentToModify = railwayContainer.Descendants().OfType<Segment>().First();
                segmentToModify.Length = 42;
            });
            Assert.IsTrue(await ModelTestUtil.CurrentModelsMatch(modelContainerGrain, consumerGrain));
        }

        [TestMethod]
        public async Task ModelCollectionChange()
        {
            var repository = new ModelRepository();
            var train = repository.Resolve(new Uri(new FileInfo("models/railway-1.xmi").FullName));
            var rc = train.Model.RootElements.Single() as RailwayContainer;

            train.Model.BubbledChange += (sender, args) =>
            {
                Console.WriteLine("foo");
            };

            var routeToChange = rc.Routes.First().DefinedBy;

            var otherRoute = rc.Descendants().OfType<Route>().Where(r => !r.Equals(routeToChange)).First();
            var foo = routeToChange.AsNotifiable();
            foo.CollectionChanged += (sender, args) =>
            {
                Console.WriteLine("Asdasdasdas");
            };
            var sensorToAdd = otherRoute.DefinedBy.First();

            routeToChange.Add(sensorToAdd); // Does not fire BubbledChange

            rc.Routes.First().Entry = rc.Semaphores[2]; // fires BubbledChange



            // END

            //var modelContainerGrain = await LoadModelContainer();

            //var consumerGrain = await LoadAndAttachModelTestConsumer(modelContainerGrain);

            //Func<Model, IModelElement> modelSelectorFunc = model =>
            //{
            //    var railwayContainer = model.RootElements.Single() as RailwayContainer;
            //    return railwayContainer;
            //};

            //Assert.IsTrue(await CurrentModelsMatch(modelSelectorFunc, modelContainerGrain, consumerGrain));

            //// Collection changed add known object
            //await modelContainerGrain.ExecuteSync(model =>
            //{
            //    var railwayContainer = model.RootElements.Single() as RailwayContainer;
            //    var routeToChange = railwayContainer.Routes.First().DefinedBy;

            //    var otherRoute = railwayContainer.Descendants().OfType<Route>().Where(r => !r.Equals(routeToChange)).First();
            //    var sensorToAdd = otherRoute.DefinedBy.First();

            //    routeToChange.Add(sensorToAdd);
            //});
            //Assert.IsTrue(await CurrentModelsMatch(modelSelectorFunc, modelContainerGrain, consumerGrain));

            // TODO
            //// Property Changed test to native type
            //await modelContainerGrain.ExecuteSync(model =>
            //{
            //    var railwayContainer = model.RootElements.Single() as RailwayContainer;
            //    var segmentToModify = railwayContainer.Descendants().OfType<Segment>().First();
            //    segmentToModify.Length = 42;
            //});
            //Assert.IsTrue(await CurrentModelsMatch(modelSelectorFunc, modelContainerGrain, consumerGrain));
        }

        private async Task<ITestModelProcessingNodeGrain<Model, int>> LoadAndAttachModelTestConsumer(IModelContainerGrain<Model> modelContainerGrain)
        {
            var consumerGrain = GrainFactory.GetGrain<ITestModelProcessingNodeGrain<Model, int>>(Guid.NewGuid());
            await consumerGrain.LoadModel(ModelTestUtil.ModelLoadingFunc);
            await consumerGrain.SetModelContainer(modelContainerGrain);

            return consumerGrain;
        }


    }
}