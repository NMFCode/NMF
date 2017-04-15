using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Changes;
using NMF.Models.Repository;
using NMF.Models.Repository.Serialization;
using NMF.Models.Tests.Railway;
using NMF.Serialization.Xmi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NMF.Models.Tests.Evolution
{
    [TestClass]
    public class IntegrationTests
    {
        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestMethod]
        public void SerializationIntegration()
        {
            //Load the model
            var repository = new ModelRepository();
            var model = LoadRailwayModel(repository);

            //Create the recorder
            var recorder = new ModelChangeRecorder();
            recorder.Start(model);

            //Change the model
            var route = new Route() { Id = 42 };
            model.Routes.Add(route);
            model.Routes[0].DefinedBy.RemoveAt(0);
            model.Routes[0].DefinedBy[0].Elements.RemoveAt(0);
            model.Semaphores[0].Signal = Signal.FAILURE;

            //Parse the changes
            var changes = recorder.GetModelChanges();

            //Serialize the changes
            var file = System.IO.Path.GetTempFileName();
            try
            {
                repository.Save(changes, file);

                var xmi = File.ReadAllText(file);

                //Load second instance of the model
                var newRepository = new ModelRepository();
                var newModel = LoadRailwayModel(newRepository);

                //Deserialize the XMI
                var newChangesModel = newRepository.Resolve(file);
                var newChanges = newChangesModel.RootElements[0] as ModelChangeSet;

                Assert.IsNotNull(newChanges);


                //Apply changes to the new model
                newChanges.Apply();

                Assert.AreEqual(model.Routes.Count, newModel.Routes.Count);
                Assert.AreEqual(model.Routes[0].DefinedBy.Count, newModel.Routes[0].DefinedBy.Count);
                Assert.AreEqual(model.Routes[0].DefinedBy[0].Elements.Count, newModel.Routes[0].DefinedBy[0].Elements.Count);
                Assert.AreEqual(model.Semaphores[0].Signal, newModel.Semaphores[0].Signal);
            }
            finally
            {
                File.Delete(file);
            }
        }

        private RailwayContainer LoadRailwayModel(ModelRepository repository)
        {
            var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            var railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
            return railway;
        }
    }
}
