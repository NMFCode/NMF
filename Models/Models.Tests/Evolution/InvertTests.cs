using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Evolution;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;

namespace NMF.Models.Tests.Evolution
{
    /// <summary>
    /// Summary description for InvertTests
    /// </summary>
    [TestClass]
    public class InvertTests
    {
        private ModelRepository repository1;
        private ModelRepository repository2;
        private RailwayContainer railway1;
        private RailwayContainer railway2;

        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";
        
        private RailwayContainer LoadRailwayModel(ModelRepository repository)
        {
            var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            var railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
            return railway;
        }

        [TestInitialize]
        public void Setup()
        {
            repository1 = new ModelRepository();
            repository2 = new ModelRepository();
            railway1 = LoadRailwayModel(repository1);
            railway2 = LoadRailwayModel(repository2);
        }

        [TestMethod]
        public void InvertPropertyChangeAttribute()
        {
            var parent = railway1.Routes.First().Entry;
            var newValue = Signal.FAILURE;
            var oldValue = repository1.Resolve(parent.AbsoluteUri).GetType().GetProperty("Signal").GetValue(parent, null);

            var change = new PropertyChangeAttribute<Signal>(parent.AbsoluteUri, "Signal", (Signal)oldValue, newValue);

            change.Apply(repository1);
            change.Invert(repository1);

            Assert.AreEqual(oldValue, parent.Signal);
        }

        [TestMethod]
        public void InvertPropertyChangeReference()
        {
            var parent = railway1.Routes[0];
            var newValue = railway1.Semaphores[0];
            var oldValue = parent.Entry;

            var change = new PropertyChangeReference<Semaphore>(parent.AbsoluteUri, "Entry", parent.Entry.AbsoluteUri, newValue.AbsoluteUri);

            change.Apply(repository1);
            change.Invert(repository1);
            Assert.AreSame(oldValue, parent.Entry);
        }

        [TestMethod]
        public void InvertListInsertionComposition()
        {
            var toInsert = new Route();
            var change = new ListInsertionComposition<IRoute>(railway1.AbsoluteUri, "Routes", 0, new List<IRoute>() { toInsert });

            change.Apply(repository1);
            change.Invert(repository1);
            Assert.AreEqual(railway1.Routes.Count, railway2.Routes.Count);
            CollectionAssert.DoesNotContain(railway1.Routes.ToList(), toInsert);
        }

        [TestMethod]
        public void InvertListInsertionAssociation()
        {
            var parent1 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var parent2 = railway2.Routes[0].DefinedBy[0].Elements[0];
            var toInsert = railway1.Routes[0].DefinedBy[1].Elements[0];
            var change = new ListInsertionAssociation<ITrackElement>(parent1.AbsoluteUri, "ConnectsTo", 0, new List<Uri>() { toInsert.AbsoluteUri });

            change.Apply(repository1);
            change.Invert(repository1);
            Assert.AreNotSame(toInsert, parent1.ConnectsTo[0]);
            Assert.AreEqual(parent1.ConnectsTo.Count, parent2.ConnectsTo.Count);
        }

        [TestMethod]
        public void InvertCollectionInsertionComposition()
        {
            var toInsert = new Route();
            var change = new CollectionInsertionComposition<IRoute>(railway1.AbsoluteUri, "Routes", new Collection<IRoute>() { toInsert });

            change.Apply(repository1);
            change.Invert(repository1);

            CollectionAssert.DoesNotContain(railway1.Routes.ToList(), toInsert);
            Assert.AreEqual(railway1.Routes.Count, railway2.Routes.Count);
        }

        [TestMethod]
        public void InvertCollectionInsertionAssociation()
        {
            var parent1 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var parent2 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var toInsert = railway1.Routes[0].DefinedBy[1].Elements[0];
            var change = new CollectionInsertionAssociation<ITrackElement>(parent1.AbsoluteUri, "ConnectsTo", new Collection<Uri>() { toInsert.AbsoluteUri });

            change.Apply(repository1);
            change.Invert(repository1);

            Assert.AreNotSame(toInsert, parent1.ConnectsTo[parent1.ConnectsTo.Count - 1]);
            Assert.AreEqual(parent2.ConnectsTo.Count, parent1.ConnectsTo.Count);
        }


        [TestMethod]
        public void InvertListDeletionComposition()
        {
            var toDelete = railway1.Routes.Take(1).ToList();
            var change = new ListDeletionComposition<IRoute>(railway1.AbsoluteUri, "Routes", 0, 1, toDelete, railway1, railway1.Parent);

            change.Apply(repository1);
            change.Invert(repository1);

            CollectionAssert.IsSubsetOf(toDelete, railway1.Routes.ToList());
            Assert.AreEqual(railway1.Routes.Count, railway2.Routes.Count);
        }

        [TestMethod]
        public void InvertListDeletionAssociation()
        {
            var parent1 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var parent2 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var toDelete = parent1.ConnectsTo[0];
            var change = new ListDeletionAssociation<ITrackElement>(parent1.AbsoluteUri, "ConnectsTo", 0, 1, new List<Uri>() { parent1.ConnectsTo[0].AbsoluteUri });

            change.Apply(repository1);
            change.Invert(repository1);

            Assert.AreEqual(parent1.ConnectsTo.Count, parent2.ConnectsTo.Count);
            CollectionAssert.Contains(parent1.ConnectsTo.ToList(), toDelete);
        }

        [TestMethod]
        public void InvertCollectionReset()
        {
            var rec = new ModelChangeRecorder(true);
            rec.Start(railway1);

            //railway1.Semaphores.Clear();
            railway1.Routes[0].DefinedBy.Clear();
            var change = (rec.GetModelChanges().Changes[0]);
            change.Invert(repository1);

            Assert.AreEqual(railway1.Semaphores.Count, railway2.Semaphores.Count);
            Assert.AreEqual(railway1.Routes[0].DefinedBy.Count, railway2.Routes[0].DefinedBy.Count);
        }

        

        [TestMethod]
        public void InvertCollectionDeletionComposition()
        {
            var toDelete = railway1.Routes.Take(1).ToList();
            var change = new CollectionDeletionComposition<IRoute>(railway1.AbsoluteUri, "Routes", new Collection<IRoute>() { toDelete[0] }, railway1, railway1.Parent);

            change.Apply(repository1);
            change.Invert(repository1);

            CollectionAssert.IsSubsetOf(toDelete, railway1.Routes.ToList());
            Assert.AreEqual(railway1.Routes.Count, railway2.Routes.Count); 
        }

        [TestMethod]
        public void InvertCollectionDeletionAssociation()
        {
            var parent1 = railway1.Routes[0].DefinedBy[0].Elements[0];
            var parent2 = railway2.Routes[0].DefinedBy[0].Elements[0];
            var toDelete = parent1.ConnectsTo[0];
            var change = new CollectionDeletionAssociation<ITrackElement>(parent1.AbsoluteUri, "ConnectsTo", new Collection<Uri>() { parent1.ConnectsTo[0].AbsoluteUri });

            change.Apply(repository1);
            change.Invert(repository1);

            CollectionAssert.Contains(parent1.ConnectsTo.ToList(), toDelete);
            Assert.AreEqual(parent1.ConnectsTo.Count, parent2.ConnectsTo.Count);
        }

        [TestMethod]
        public void InvertChangeTransaction()
        {
            var rec = new ModelChangeRecorder(true);
            rec.Start(railway1);

            railway1.Semaphores.RemoveAt(0);
            var changes = rec.GetModelChanges().Changes[0];
            changes.Invert(repository1);

            Assert.AreEqual(railway1.Semaphores.Count, railway2.Semaphores.Count);
        }
    }
}
