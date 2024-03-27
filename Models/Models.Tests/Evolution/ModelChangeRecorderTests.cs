using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Changes;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using System;
using System.Linq;

namespace NMF.Models.Tests
{
    [TestClass]
    public class ModelChangeRecorderTests
    {
        private ModelRepository repository;
        private RailwayContainer railway;

        private const string BaseUri = "http://github.com/NMFCode/NMF/Models/Models.Test/railway.railway";

        [TestInitialize]
        public void LoadRailwayModel()
        {
            repository = new ModelRepository();
            var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
        }

        [TestMethod]
        public void RecordPropertyChangeAttribute()
        {
            var semaphore = railway.Semaphores[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            var oldValue = semaphore.Signal;
            semaphore.Signal = Signal.FAILURE;

            var actual = rec.GetModelChanges().Changes[0];

            Assert.IsInstanceOfType(actual, typeof(AttributePropertyChange));
            var change = actual as AttributePropertyChange;
            Assert.AreSame(semaphore, change.AffectedElement);
            Assert.AreEqual("signal", change.Feature.Name);
            Assert.AreEqual(oldValue.ToString(), change.OldValue);
            Assert.AreEqual(Signal.FAILURE.ToString(), change.NewValue);
        }

        [TestMethod]
        public void RecordPropertyChangeReference()
        {
            var parent = railway.Routes[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            parent.Entry = railway.Semaphores[0];

            var actual = rec.GetModelChanges().Changes[0];

            Assert.IsInstanceOfType(actual, typeof(AssociationPropertyChange));
            var change = actual as AssociationPropertyChange;
            Assert.AreSame(parent, change.AffectedElement);
            Assert.AreEqual("entry", change.Feature.Name);
            Assert.AreEqual(railway.Semaphores[0], change.NewValue);
        }

        [TestMethod]
        public void RecordListDeletionComposition()
        {
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            railway.Semaphores.RemoveAt(0);
            
            var actual = rec.GetModelChanges().Changes[0];

            Assert.IsInstanceOfType(actual, typeof(ChangeTransaction));
            var transaction = actual as ChangeTransaction;
            Assert.IsInstanceOfType(transaction.SourceChange, typeof(CompositionListDeletion));
        }

        [TestMethod]
        public void RecordListDeletionAssociation()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            parent.ConnectsTo.RemoveAt(0);
            var actual = rec.GetModelChanges().Changes;
            Assert.AreEqual(1, actual.Count);
            Assert.IsInstanceOfType(actual.Single(), typeof(AssociationListDeletion));
        }

        [TestMethod]
        public void RecordListClear()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            parent.ConnectsTo.Clear();

            var actual = rec.GetModelChanges().Changes.Single();
            Assert.IsInstanceOfType(actual, typeof(AssociationCollectionReset));
        }

        [TestMethod]
        public void RecordListInsertionComposition()
        {
            var semaphore = new Semaphore { Signal = Signal.STOP };
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            railway.Semaphores.Insert(0, semaphore);

            var actual = rec.GetModelChanges().Changes.Single();
            Assert.IsInstanceOfType(actual, typeof(CompositionListInsertion));
        }

        [TestMethod]
        public void RecordListInsertionAssociation()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var newItem = railway.Routes[0].DefinedBy[1].Elements[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            parent.ConnectsTo.Insert(0, newItem);

            var actual = rec.GetModelChanges().Changes.Single();
            Assert.IsInstanceOfType(actual, typeof(AssociationListInsertion));
        }

        [TestMethod]
        public void RecordElementDeletion()
        {
            var toDelete = railway.Routes[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            toDelete.Delete();

            // deleting a route means that a range of elements are deleted, so we see several changes that an association was deleted
            // only the last one is the actual deletion from the composition
            var actual = rec.GetModelChanges().Changes.Last();
            Assert.IsInstanceOfType(actual, typeof(CompositionListDeletion));
        }
    }
}
