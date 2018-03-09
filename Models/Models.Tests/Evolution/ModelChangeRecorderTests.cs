using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Changes;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            //ListDeletionAssociation will be removed
            /*var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            parent.ConnectsTo.RemoveAt(0);
            var expected = new List<IModelChange>()
            {
                new ListDeletionAssociation<ITrackElement>(parent.AbsoluteUri, "ConnectsTo", 0, 1)
            };
            var actual = rec.GetModelChanges().Changes;
            CollectionAssert.AreEqual(expected, actual);*/
        }

        [TestMethod]
        public void RecordListClear()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            parent.ConnectsTo.Clear();

            //var expected = new CollectionResetAssociation<ITrackElement>(parent.AbsoluteUri, "ConnectsTo", new List<Uri>());
            //var actual = rec.GetModelChanges().Changes[0];
            //Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RecordListInsertionComposition()
        {
            var semaphore = new Semaphore { Signal = Signal.STOP };
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            railway.Semaphores.Insert(0, semaphore);

            //var expected = new ChangeTransaction()
            //{
            //    SourceChange = new ListInsertionComposition<ISemaphore>(railway.AbsoluteUri, "Semaphores", 0, new List<ISemaphore>() { semaphore }),
            //    NestedChanges = new List<IModelChange>() { new ElementCreation(semaphore) }
            //};
            //var actual = rec.GetModelChanges().Changes[0];
            //Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RecordListInsertionAssociation()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var newItem = railway.Routes[0].DefinedBy[1].Elements[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            parent.ConnectsTo.Insert(0, newItem);

            //var expected = new List<IModelChange>()
            //{
            //    new ListInsertionAssociation<ITrackElement>(parent.AbsoluteUri, "ConnectsTo", 0, new List<Uri>() { newItem.AbsoluteUri })
            //};
            //var actual = rec.GetModelChanges().Changes;
            //CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RecordElementDeletion()
        {
            var toDelete = railway.Routes[0];
            var rec = new ModelChangeRecorder();
            rec.Start(railway);

            toDelete.Delete();

            //var actual = ((ChangeTransaction)rec.GetModelChanges().Changes[0]).SourceChange;
            //Assert.AreEqual(expected, actual);
        }
    }
}
