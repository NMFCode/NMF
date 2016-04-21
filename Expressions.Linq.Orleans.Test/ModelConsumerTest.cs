using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Linq.Orleans;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Expressions.Linq.Orleans.Model;
using NMF.Expressions.Linq.Orleans.TestGrains;
using NMF.Models;
using NMF.Models.Repository;
using Orleans;
using Orleans.Streams;
using Orleans.Streams.Endpoints;
using Orleans.Streams.Messages;
using Orleans.TestingHost;
using TTC2015.TrainBenchmark.Railway;

namespace Expressions.Linq.Orleans.Test
{
    [TestClass]
    public class ModelConsumerTest : TestingSiloHost
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
        public async Task LoadModelSucceeded()
        {
            //var repository = new ModelRepository();
            //var train = repository.Resolve(new Uri(new FileInfo("models/railway-1.xmi").FullName));
            //var railwayContainer = train.Model.RootElements.Single() as RailwayContainer;

            var grain = GrainFactory.GetGrain<IModelContainerGrain<NMF.Models.Model>>(Guid.NewGuid());
            await grain.LoadModel(() =>
            {
                var repository = new ModelRepository();
                var train = repository.Resolve(new Uri(new FileInfo("models/railway-1.xmi").FullName));
                var railwayContainer = train.Model.RootElements.Single() as RailwayContainer;

                return train.Model;
            });
        }

        [TestMethod]
        public async Task ModelUpdateSucceeded()
        {
            //var repository = new ModelRepository();
            //var train = repository.Resolve(new Uri(new FileInfo("models/railway-1.xmi").FullName));
            //var railwayContainer = train.Model.RootElements.Single() as RailwayContainer;

            Func<NMF.Models.Model> modelLoadingFunc = new Func<Model>(() =>
            {
                var repository = new ModelRepository();
                var train = repository.Resolve(new Uri(new FileInfo("models/railway-1.xmi").FullName));
                var railwayContainer = train.Model.RootElements.Single() as RailwayContainer;

                return train.Model;
            });

            var modelContainerGrain = GrainFactory.GetGrain<IModelContainerGrain<NMF.Models.Model>>(Guid.NewGuid());
            await modelContainerGrain.LoadModel(modelLoadingFunc);

            var consumerGrain = GrainFactory.GetGrain<ITestModelProcessingNodeGrain<NMF.Models.Model, int>>(Guid.NewGuid());
            await consumerGrain.LoadModel(modelLoadingFunc);
            await consumerGrain.SetModelContainer(modelContainerGrain);

            StreamMessageDispatchReceiver receiver = new StreamMessageDispatchReceiver(_provider);
            await receiver.Subscribe(await modelContainerGrain.GetStreamIdentity());

            Func<Model, IModelElement> modelSelectorFunc = new Func<Model, IModelElement>((model) =>
            {
                var railwayContainer = model.RootElements.Single() as RailwayContainer;
                return railwayContainer.Routes.First();
            });

            Assert.IsTrue(await CurrentModelsMatch(modelSelectorFunc, modelContainerGrain, consumerGrain));

            receiver.Register<ModelPropertyChangedMessage>(message =>
            {
                Console.WriteLine(message);
                return TaskDone.Done;
            });

            await modelContainerGrain.ExecuteSync((model) =>
            {
                var railwayContainer = model.RootElements.Single() as RailwayContainer;
                railwayContainer.Routes.First().Entry = null;
            });

            Assert.IsTrue(await CurrentModelsMatch(modelSelectorFunc, modelContainerGrain, consumerGrain));

            var afterChangeString = await modelContainerGrain.ModelToString((model) =>
            {
                var railwayContainer = model.RootElements.Single() as RailwayContainer;
                return railwayContainer.Routes.First();
            });


        }

        private async Task<bool> CurrentModelsMatch<T>(Func<Model, IModelElement> elementSelectorFunc, IModelLoader<T> loader1, IModelLoader<T> loader2) where T : Model
        {
            var s1 = await loader1.ModelToString(elementSelectorFunc);
            var s2 = await loader2.ModelToString(elementSelectorFunc);

            return s1.Equals(s2);
        }

    }
}