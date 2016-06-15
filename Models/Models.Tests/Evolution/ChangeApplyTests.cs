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
        public void ApplyListClear()
        {
            var change = new ListDeletion(railway.AbsoluteUri, "Semaphores", 0, int.MaxValue);

            change.Apply(repository);

            Assert.AreEqual(0, railway.Semaphores.Count);
        }

        [TestMethod]
        public void ApplyListInsertionContainment()
        {
            var toInsert = new Route();
            var change = new ListInsertionContainment<IRoute>(railway.AbsoluteUri, "Routes", 0, new List<IRoute>() { toInsert });

            change.Apply(repository);

            Assert.AreEqual(toInsert, railway.Routes.First());
        }

        [TestMethod]
        public void ApplyListInsertionReference()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var toInsert = railway.Routes[0].DefinedBy[1].Elements[0];
            var change = new ListInsertionReference<ITrackElement>(parent.AbsoluteUri, "ConnectsTo", 0, new List<Uri>() { toInsert.AbsoluteUri });

            change.Apply(repository);

            Assert.AreSame(toInsert, parent.ConnectsTo[0]);
        }

        [TestMethod]
        public void ApplyPropertyChangeAttribute()
        {
            var parent = railway.Routes.First().Entry;
            var newValue = Signal.FAILURE;
            var change = new PropertyChangeAttribute<Signal>(parent.AbsoluteUri, "Signal", newValue);

            change.Apply(repository);

            Assert.AreEqual(newValue, parent.Signal);
        }

        [TestMethod]
        public void ApplyPropertyChangeReference()
        {
            var parent = railway.Routes[0];
            var newValue = railway.Semaphores[0];
            var change = new PropertyChangeReference<Semaphore>(parent.AbsoluteUri, "Entry", newValue.AbsoluteUri);

            change.Apply(repository);

            Assert.AreSame(newValue, parent.Entry);
        }
    }
}
