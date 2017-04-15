using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Meta;
using NMF.Models.Changes;
using NMF.Models.Repository;
using NMF.Models.Tests.Railway;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var railwayModel = repository.Resolve(new Uri(BaseUri), "railway.railway").Model;
            Assert.IsNotNull(railwayModel);
            railway = railwayModel.RootElements.Single() as RailwayContainer;
            Assert.IsNotNull(railway);
        }

        [TestMethod]
        public void ApplyListDeletionComposition()
        {
            var toDelete = railway.Routes.First();

            var change = new CompositionListDeletion
            {
                AffectedElement = railway,
                Index = 0,
                Feature = RailwayContainer.ClassInstance.LookupReference("routes")
            };

            change.Apply();

            Assert.AreNotEqual(toDelete, railway.Routes.FirstOrDefault());
            CollectionAssert.DoesNotContain(railway.Routes.ToList(), toDelete);
        }

        [TestMethod]
        public void ApplyListDeletionAssociation()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var toDelete = parent.ConnectsTo[0];

            var change = new AssociationListDeletion
            {
                AffectedElement = parent,
                Feature = TrackElement.ClassInstance.LookupReference("connectsTo"),
                Index = 0
            };

            change.Apply();

            Assert.AreNotEqual(toDelete, parent.ConnectsTo.FirstOrDefault());
            CollectionAssert.DoesNotContain(parent.ConnectsTo.ToList(), toDelete);
        }

        [TestMethod]
        public void ApplyCollectionDeletionComposition()
        {
            var toDelete = railway.Routes[0];

            var change = new CompositionListDeletion
            {
                AffectedElement = railway,
                Feature = RailwayContainer.ClassInstance.LookupReference("routes"),
                DeletedElement = toDelete
            };

            change.Apply();

            Assert.AreNotEqual(toDelete, railway.Routes.FirstOrDefault());
            CollectionAssert.DoesNotContain(railway.Routes.ToList(), toDelete);
        }

        [TestMethod]
        public void ApplyCollectionDeletionAssociation()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var toDelete = parent.ConnectsTo[0];

            var change = new AssociationCollectionDeletion
            {
                AffectedElement = parent,
                Feature = TrackElement.ClassInstance.LookupReference("connectsTo"),
                DeletedElement = toDelete
            };
            
            change.Apply();

            Assert.AreNotEqual(toDelete, parent.ConnectsTo.FirstOrDefault());
            CollectionAssert.DoesNotContain(parent.ConnectsTo.ToList(), toDelete);
        }

        [TestMethod]
        public void ApplyListClear()
        {
            var change = new CompositionCollectionReset
            {
                AffectedElement = railway,
                Feature = RailwayContainer.ClassInstance.LookupReference("semaphores")
            };

            change.Apply();

            Assert.AreEqual(0, railway.Semaphores.Count);
        }

        [TestMethod]
        public void ApplyListInsertionComposition()
        {
            var toInsert = new Route();

            var change = new CompositionListInsertion
            {
                AffectedElement = railway,
                Feature = RailwayContainer.ClassInstance.LookupReference("routes"),
                Index = 0,
                AddedElement = toInsert
            };

            change.Apply();

            Assert.AreEqual(toInsert, railway.Routes.First());
        }

        [TestMethod]
        public void ApplyListInsertionAssociation()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var toInsert = railway.Routes[0].DefinedBy[1].Elements[0];

            var change = new AssociationListInsertion
            {
                AffectedElement = parent,
                Feature = TrackElement.ClassInstance.LookupReference("connectsTo"),
                Index = 0,
                AddedElement = toInsert
            };
            change.Apply();

            Assert.AreSame(toInsert, parent.ConnectsTo[0]);
        }

        [TestMethod]
        public void ApplyCollectionInsertionComposition()
        {
            var toInsert = new Route();

            var change = new CompositionCollectionInsertion
            {
                AffectedElement = railway,
                Feature = RailwayContainer.ClassInstance.LookupReference("routes"),
                AddedElement = toInsert
            };
            change.Apply();

            Assert.AreEqual(toInsert, railway.Routes.Last());
        }

        [TestMethod]
        public void ApplyCollectionInsertionAssociation()
        {
            var parent = railway.Routes[0].DefinedBy[0].Elements[0];
            var toInsert = railway.Routes[0].DefinedBy[1].Elements[0];

            var change = new AssociationCollectionInsertion
            {
                AffectedElement = parent,
                Feature = TrackElement.ClassInstance.LookupReference("connectsTo"),
                AddedElement = toInsert
            };
            change.Apply();

            Assert.AreSame(toInsert, parent.ConnectsTo[parent.ConnectsTo.Count - 1]);
        }

        [TestMethod]
        public void ApplyPropertyChangeAttribute()
        {
            var parent = railway.Routes.First().Entry;
            var newValue = Signal.FAILURE;
            var oldValue = repository.Resolve(parent.AbsoluteUri).GetType().GetProperty("Signal").GetValue(parent, null);

            var change = new AttributeChange
            {
                AffectedElement = parent,
                Feature = Semaphore.ClassInstance.LookupAttribute("signal"),
                OldValue = oldValue.ToString(),
                NewValue = newValue.ToString()
            };
            change.Apply();

            Assert.AreEqual(newValue, parent.Signal);
        }

        [TestMethod]
        public void ApplyPropertyChangeReference()
        {
            var parent = railway.Routes[0];
            var newValue = railway.Semaphores[0];

            var change = new AssociationChange
            {
                AffectedElement = parent,
                Feature = Route.ClassInstance.LookupReference("entry"),
                NewValue = newValue
            };
            change.Apply();

            Assert.AreSame(newValue, parent.Entry);
        }
    }
}