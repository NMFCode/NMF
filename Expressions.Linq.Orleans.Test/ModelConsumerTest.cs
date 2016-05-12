using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Expressions.Linq.Orleans.Test.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Expressions.Linq.Orleans.TestGrains;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using NMF.Utilities;
using Orleans;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.Streams.Stateful;
using Orleans.TestingHost;

namespace Expressions.Linq.Orleans.Test
{
    [TestClass]
    public class ModelConsumerTest : TestingSiloHost
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
            await modelContainerGrain.ExecuteSync(model => { ((RailwayContainer) model.RootElements.Single()).Routes.First().Entry = null; });
            Assert.IsTrue(await ModelTestUtil.CurrentModelsMatch(modelContainerGrain, consumerGrain));

            // Property Changed test to known object
            await modelContainerGrain.ExecuteSync(model => { ((RailwayContainer)model.RootElements.Single()).Routes.First().Entry = ((RailwayContainer)model.RootElements.Single()).Semaphores[2]; });
            Assert.IsTrue(await ModelTestUtil.CurrentModelsMatch(modelContainerGrain, consumerGrain));

            // Property Changed test to native type
            await modelContainerGrain.ExecuteSync(model =>
            {
                var segmentToModify = model.Descendants().OfType<Segment>().First();
                segmentToModify.Length = 42;
            });
            Assert.IsTrue(await ModelTestUtil.CurrentModelsMatch(modelContainerGrain, consumerGrain));
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task ModelNewModelElementCreatedNotForwarded()
        {
            var modelContainerGrain = await ModelTestUtil.LoadModelContainer(GrainFactory);

            var consumerGrain = await LoadAndAttachModelTestConsumer(modelContainerGrain);

            ModelElement.EnforceModels = true;
            await modelContainerGrain.ExecuteSync(container =>
            {
                var switchToUpdate = container.Descendants().OfType<ISwitch>().First(sw => sw.Sensor == null);
                switchToUpdate.Sensor = new Sensor();
            }, false);
        }

        [TestMethod]
        public async Task ModelNewModelElementCreatedForwarded()
        {
            var modelContainerGrain = await ModelTestUtil.LoadModelContainer(GrainFactory);

            var consumerGrain = await LoadAndAttachModelTestConsumer(modelContainerGrain);

            ModelElement.EnforceModels = true;
            await modelContainerGrain.ExecuteSync(container =>
            {
                var switchToUpdate = container.RootElements.Single().As<RailwayContainer>().Descendants().OfType<ISwitch>().First(sw => sw.Sensor == null);
                switchToUpdate.Sensor = new Sensor();
            }, true);

            Assert.IsTrue(await ModelTestUtil.CurrentModelsMatch(modelContainerGrain, consumerGrain));
        }

        [TestMethod]
        public async Task ModelCollectionChange()
        {
            var repository = new ModelRepository();
            var train = repository.Resolve(new Uri(new FileInfo("models/railway-1.xmi").FullName));
            var rc = train.Model.RootElements.Single() as RailwayContainer;

            train.Model.BubbledChange += (sender, args) => { Console.WriteLine("foo"); };

            var routeToChange = rc.Routes.First().DefinedBy;

            var otherRoute = rc.Descendants().OfType<Route>().Where(r => !r.Equals(routeToChange)).First();
            var foo = routeToChange.AsNotifiable();
            foo.CollectionChanged += (sender, args) => { Console.WriteLine("Asdasdasdas"); };
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

        private async Task<ITestModelProcessingNodeGrain<NMF.Models.Model, int>> LoadAndAttachModelTestConsumer(
            IModelContainerGrain<NMF.Models.Model> modelContainerGrain)
        {
            var consumerGrain = GrainFactory.GetGrain<ITestModelProcessingNodeGrain<NMF.Models.Model, int>>(Guid.NewGuid());
            await consumerGrain.Setup(modelContainerGrain);

            return consumerGrain;
        }
    }
}