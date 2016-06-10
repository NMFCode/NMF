using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Evolution;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Tests.Evolution
{
    [TestClass]
    public class ChangeApplyTests
    {
        private ModelRepository repository;
        private RailwayContainer railway;

        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestInitialize]
        public void LoadRailwayModel()
        {
            repository = new ModelRepository();
            var railwayModel = repository.Resolve(new Uri(BaseUri), "..\\..\\railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
        }

        [TestMethod]
        public void ApplyListDeletion()
        {
            var toDelete = railway.Routes.Take(1).ToList();
            var change = new ListDeletion(railway.AbsoluteUri, "Routes", 0, 1);

            change.Apply(repository);
            
            Assert.AreNotEqual(toDelete, railway.Routes.FirstOrDefault());
            CollectionAssert.DoesNotContain(railway.Routes.ToList(), toDelete);
        }

        [TestMethod]
        public void ApplyListInsertion()
        {
            var toInsert = new Route();
            var change = new ListInsertion<IRoute>(railway.AbsoluteUri, "Routes", 0, new[] { toInsert });

            change.Apply(repository);

            Assert.AreEqual(toInsert, railway.Routes.First());
        }

        [TestMethod]
        public void ApplyPropertyChange()
        {
            var parent = railway.Routes.First().Entry;
            var newValue = Signal.FAILURE;
            var change = new PropertyChange<Signal>(parent.AbsoluteUri, "Signal", parent.Signal, newValue);

            change.Apply(repository);

            Assert.AreEqual(newValue, parent.Signal);
        }
    }
}
