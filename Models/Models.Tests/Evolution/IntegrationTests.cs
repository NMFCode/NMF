using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Evolution;
using NMF.Models.Repository;
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
            var types = changes.TraverseFlat().Select(t => t.GetType()).Distinct();
            var serializer = new XmiSerializer(types);
            string xmi;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(changes, writer);
                xmi = writer.ToString();
            }

            //Deserialize the XMI
            ModelChangeCollection newChanges;
            using (var reader = new StringReader(xmi))
            {
                newChanges = serializer.Deserialize(reader) as ModelChangeCollection;
            }

            Assert.IsNotNull(newChanges);
            //Since model elements don't implement Equals() we can only test for property equality
            var newRoute = ((ListInsertionComposition<IRoute>)((ChangeTransaction)newChanges.Changes[0]).SourceChange).NewElements[0];
            Assert.AreEqual(route.Id, newRoute.Id);
            //We have to leave out the changes with model elements in them when tesing for equality
            CollectionAssert.AreEqual(changes.Changes.Skip(1).ToArray(), newChanges.Changes.Skip(1).ToArray());

            //Load second instance of the model
            var newRepository = new ModelRepository();
            var newModel = LoadRailwayModel(newRepository);

            //Apply changes to the new model
            newChanges.Apply(newRepository);

            Assert.AreEqual(model.Routes.Count, newModel.Routes.Count);
            Assert.AreEqual(model.Routes[0].DefinedBy.Count, newModel.Routes[0].DefinedBy.Count);
            Assert.AreEqual(model.Routes[0].DefinedBy[0].Elements.Count, newModel.Routes[0].DefinedBy[0].Elements.Count);
            Assert.AreEqual(model.Semaphores[0].Signal, newModel.Semaphores[0].Signal);
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
