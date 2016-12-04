using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;

namespace NMF.Models.Tests
{
    [TestClass]
    public class TransactionTests
    {
        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestMethod]
        public void RollbackTest()
        {
            var model = LoadRailwayModel(new ModelRepository());
            var referenceModel = LoadRailwayModel(new ModelRepository());

            using (var trans = new NMFTransaction(model))
            {
                var route = new Route() { Id = 42 };
                model.Routes.Add(route);
                model.Semaphores[0].Signal = Signal.FAILURE;
                model.Routes[0].DefinedBy.RemoveAt(0); //hier aufpassen
                model.Routes[0].DefinedBy[0].Elements.RemoveAt(0);

            }

            Assert.AreEqual(model.Routes.Count, referenceModel.Routes.Count);
            Assert.AreEqual(model.Routes[0].DefinedBy.Count, referenceModel.Routes[0].DefinedBy.Count);
            Assert.AreEqual(model.Routes[0].DefinedBy[0].Elements.Count, referenceModel.Routes[0].DefinedBy[0].Elements.Count);
            Assert.AreEqual(model.Routes[0].DefinedBy[0].Elements[0].ConnectsTo.Count, referenceModel.Routes[0].DefinedBy[0].Elements[0].ConnectsTo.Count);
            Assert.AreEqual(model.Semaphores[0].Signal, referenceModel.Semaphores[0].Signal);
        }

        [TestMethod]
        public void DemoTestCase()
        {
            var model = LoadRailwayModel(new ModelRepository());
            var referenceModel = LoadRailwayModel(new ModelRepository());

            using (var trans = new NMFTransaction(model))
            {
                model.Routes[0].DefinedBy.RemoveAt(0); 
            }

            var debug = model.Routes[0].DefinedBy[0].Elements;
            var debugReference = referenceModel.Routes[0].DefinedBy[0].Elements;
            Assert.AreEqual(debug.First().ConnectsTo.Count, debugReference.First().ConnectsTo.Count);
        }

        [TestMethod]
        public void CommitTest()
        {
            var model = LoadRailwayModel(new ModelRepository());
            var referenceModel = LoadRailwayModel(new ModelRepository());

            using (var trans = new NMFTransaction(model))
            {
                var route = new Route() { Id = 42 };
                model.Routes.Add(route);
                model.Routes[0].DefinedBy.RemoveAt(0);
                model.Semaphores[0].Signal = Signal.FAILURE;
                trans.Commit();
            }

            Assert.AreNotEqual(model.Routes.Count, referenceModel.Routes.Count);
            Assert.AreNotEqual(model.Routes[0].DefinedBy.Count, referenceModel.Routes[0].DefinedBy.Count);
            Assert.AreNotEqual(model.Semaphores[0].Signal, referenceModel.Semaphores[0].Signal);
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
